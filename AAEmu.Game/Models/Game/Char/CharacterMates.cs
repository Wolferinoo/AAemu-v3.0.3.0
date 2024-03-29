﻿using System;
using System.Collections.Generic;

using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Managers.Id;
using AAEmu.Game.Core.Managers.UnitManagers;
using AAEmu.Game.Models.Game.Items.Templates;
using AAEmu.Game.Models.Game.NPChar;
using AAEmu.Game.Models.Game.Skills;
using AAEmu.Game.Models.Game.Skills.Effects;
using AAEmu.Game.Models.Game.Units.Static;

using MySql.Data.MySqlClient;

namespace AAEmu.Game.Models.Game.Char
{
    public class CharacterMates
    {
        /*
         * TODO:
         * EQUIPMENT CHANGE
         * FINISH ATTRIBUTES
         * NAME FROM LOCALIZED TABLE
         */

        public Character Owner { get; set; }

        private readonly Dictionary<ulong, MateDb> _mates; // itemId, MountDb
        private readonly List<uint> _removedMates;

        public CharacterMates(Character owner)
        {
            Owner = owner;
            _mates = new Dictionary<ulong, MateDb>();
            _removedMates = new List<uint>();
        }

        public MateDb GetMateInfo(ulong itemId)
        {
            return _mates.ContainsKey(itemId) ? _mates[itemId] : null;
        }

        private MateDb CreateNewMate(ulong itemId, NpcTemplate npcTemplate)
        {
            if (_mates.ContainsKey(itemId))
            {
                return null;
            }

            var template = new MateDb();
            template.Id = MateIdManager.Instance.GetNextId();
            template.ItemId = itemId;
            template.Level = npcTemplate.Level;
            template.Name = LocalizationManager.Instance.Get("npcs", "name", npcTemplate.Id, npcTemplate.Name);
            template.Owner = Owner.Id;
            template.Mileage = 0;
            template.Xp = ExpirienceManager.Instance.GetExpForLevel(npcTemplate.Level, true);
            template.Hp = 9999;
            template.Mp = 9999;
            template.UpdatedAt = DateTime.UtcNow;
            template.CreatedAt = DateTime.UtcNow;

            _mates.Add(template.ItemId, template);

            return template;
        }

        public void SpawnMount(SkillItem skillData)
        {
            var item = Owner.Inventory.GetItemById(skillData.ItemId);
            if (item == null)
            {
                return;
            }

            var itemTemplate = (SummonMateTemplate)ItemManager.Instance.GetTemplate(item.TemplateId);
            var npcId = itemTemplate.NpcId;
            var template = NpcManager.Instance.GetTemplate(npcId);
            var tlId = (ushort)TlIdManager.Instance.GetNextId();
            var objId = ObjectIdManager.Instance.GetNextId();

            // check if there already is such an object or if there is an object of this type
            var oldMates = MateManager.Instance.GetActiveMates(Owner.ObjId);
            if (oldMates != null)
            {
                foreach (var oldMate in oldMates)
                {
                    var mateDb = GetMateInfo(skillData.ItemId);
                    if (mateDb != null && oldMate.DbInfo.Id == mateDb.Id)
                    {
                        DespawnMate(oldMate.TlId); // such an object already exists
                        return;
                    }

                    if (oldMate.MateType == (MateType)(item.Template.Category_Id == 92 ? 1 : 2)) // 92 - Mount, 95 - Battle)
                    {
                        DespawnMate(oldMate.TlId); // there is an object of this type
                    }
                }
            }

            var mateDbInfo = GetMateInfo(skillData.ItemId) ?? CreateNewMate(skillData.ItemId, template);


            var mount = new Units.Mate();
            mount.ObjId = objId;
            mount.TlId = tlId;
            mount.OwnerId = Owner.Id;
            mount.Name = mateDbInfo.Name;
            mount.TemplateId = template.Id;
            mount.Template = template;
            mount.ModelId = template.ModelId;
            mount.Faction = Owner.Faction;
            mount.Level = (byte)mateDbInfo.Level;
            mount.Hp = mount.MaxHp;
            mount.Mp = mount.MaxMp;
            mount.OwnerObjId = Owner.ObjId;
            mount.Id = mateDbInfo.Id;
            mount.ItemId = mateDbInfo.ItemId;
            mount.UserState = 1; // TODO
            mount.Experience = mateDbInfo.Xp;
            mount.Mileage = mateDbInfo.Mileage;
            mount.SpawnDelayTime = 0; // TODO
            mount.DbInfo = mateDbInfo;
            mount.MateType = (MateType)(item.Template.Category_Id == 92 ? 1 : 2); // 92 - Mount, 95 - Battle

            mount.Transform = Owner.Transform.CloneDetached(mount);

            foreach (var skill in MateManager.Instance.GetMateSkills(npcId))
            {
                mount.Skills.Add(skill);
            }

            foreach (var buffId in template.Buffs)
            {
                var buff = SkillManager.Instance.GetBuffTemplate(buffId);
                if (buff == null)
                {
                    continue;
                }

                var obj = new SkillCasterUnit(mount.ObjId);
                buff.Apply(mount, obj, mount, null, null, new EffectSource(), null, DateTime.UtcNow);
            }

            // TODO: Load Pet Gear

            // Cap stats to their max
            mount.Hp = Math.Min(mount.Hp, mount.MaxHp);
            mount.Mp = Math.Min(mount.Mp, mount.MaxMp);

            mount.Transform.Local.AddDistanceToFront(3f);
            MateManager.Instance.AddActiveMateAndSpawn(Owner, mount, item);
        }

        public void DespawnMate(uint tlId)
        {
            var mateInfo = MateManager.Instance.GetActiveMateByTlId(Owner.ObjId, tlId);
            if (mateInfo != null)
            {
                var mateDbInfo = GetMateInfo(mateInfo.ItemId);
                if (mateDbInfo != null)
                {
                    mateDbInfo.Hp = mateInfo.Hp;
                    mateDbInfo.Mp = mateInfo.Mp;
                    mateDbInfo.Level = mateInfo.Level;
                    mateDbInfo.Xp = mateInfo.Experience;
                    mateDbInfo.Mileage = mateInfo.Mileage;
                    mateDbInfo.Name = mateInfo.Name;
                    mateDbInfo.UpdatedAt = DateTime.UtcNow;
                }
            }

            MateManager.Instance.RemoveActiveMateAndDespawn(Owner, tlId);
        }

        public void Load(MySqlConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM mates WHERE `owner` = @owner";
                command.Parameters.AddWithValue("@owner", Owner.Id);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var template = new MateDb
                        {
                            Id = reader.GetUInt32("id"),
                            ItemId = reader.GetUInt64("item_id"),
                            Name = reader.GetString("name"),
                            Xp = reader.GetInt32("xp"),
                            Level = reader.GetUInt16("level"),
                            Mileage = reader.GetInt32("mileage"),
                            Hp = reader.GetInt32("hp"),
                            Mp = reader.GetInt32("mp"),
                            Owner = reader.GetUInt32("owner"),
                            UpdatedAt = reader.GetDateTime("updated_at"),
                            CreatedAt = reader.GetDateTime("created_at")
                        };
                        _mates.Add(template.ItemId, template);
                    }
                }
            }
        }

        public void Save(MySqlConnection connection, MySqlTransaction transaction)
        {
            if (_removedMates.Count > 0)
            {
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.Transaction = transaction;

                    command.CommandText = "DELETE FROM mates WHERE owner = @owner AND id IN(" + string.Join(",", _removedMates) + ")";
                    command.Parameters.AddWithValue("@owner", Owner.Id);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    _removedMates.Clear();
                }
            }

            foreach (var (_, value) in _mates)
            {
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.Transaction = transaction;

                    command.CommandText =
                        "REPLACE INTO mates(`id`,`item_id`,`name`,`xp`,`level`,`mileage`,`hp`,`mp`,`owner`,`updated_at`,`created_at`) " +
                        "VALUES (@id, @item_id, @name, @xp, @level, @mileage, @hp, @mp, @owner, @updated_at, @created_at)";
                    command.Parameters.AddWithValue("@id", value.Id);
                    command.Parameters.AddWithValue("@item_id", value.ItemId);
                    command.Parameters.AddWithValue("@name", value.Name);
                    command.Parameters.AddWithValue("@xp", value.Xp);
                    command.Parameters.AddWithValue("@level", value.Level);
                    command.Parameters.AddWithValue("@mileage", value.Mileage);
                    command.Parameters.AddWithValue("@hp", value.Hp);
                    command.Parameters.AddWithValue("@mp", value.Mp);
                    command.Parameters.AddWithValue("@owner", value.Owner);
                    command.Parameters.AddWithValue("@updated_at", value.UpdatedAt);
                    command.Parameters.AddWithValue("@created_at", value.CreatedAt);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public class MateDb
    {
        public uint Id { get; set; }
        public ulong ItemId { get; set; }
        public string Name { get; set; }
        public int Xp { get; set; }
        public ushort Level { get; set; }
        public int Mileage { get; set; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public uint Owner { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
