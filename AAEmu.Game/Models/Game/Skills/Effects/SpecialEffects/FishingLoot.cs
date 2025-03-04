﻿using System;
using System.Linq;

using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Managers.World;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Items;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.Skills.Effects.SpecialEffects
{
    class FishingLoot : SpecialEffectAction
    {
        protected override SpecialType SpecialEffectActionType => SpecialType.FishingLoot;

        public override void Execute(BaseUnit caster,
            SkillCaster casterObj,
            BaseUnit target,
            SkillCastTarget targetObj,
            CastAction castObj,
            Skill skill,
            SkillObject skillObject,
            DateTime time,
            int value1,
            int value2,
            int value3,
            int value4)
        {
            if (caster is Character) { _log.Debug("Special effects: FishingLoot value1 {0}, value2 {1}, value3 {2}, value4 {3}", value1, value2, value3, value4); }

            var lootTableId = new uint();
            var zoneId = ZoneManager.Instance.GetZoneByKey(target.Transform.ZoneId).GroupId;

            if (target.Transform.World.Position.Z > 101)
            {
                lootTableId = ZoneManager.Instance.GetZoneGroupById(zoneId).FishingLandLootPackId;
            }
            else
            {
                lootTableId = ZoneManager.Instance.GetZoneGroupById(zoneId).FishingSeaLootPackId;
            }

            var lootPacks = ItemManager.Instance.GetLootPacks(lootTableId);

            if (lootPacks != null)
            {
                var totalDropRate = (int)lootPacks.Sum(c => c.DropRate); //Adds the total drop rate of all possible items from a skill
                var rand = new Random();
                var randChoice = rand.Next(totalDropRate);

                LootPacks lootpack = null;

                foreach (var item in lootPacks) //Picks item based on a weighted system. The higher the droprate the more likely you are to get that item. 
                {
                    if (randChoice < item.DropRate)
                    {
                        lootpack = item;
                        break;
                    }
                    else
                    {
                        randChoice -= (int)item.DropRate;
                    }
                }

                var player = (Character)caster;

                if (player != null && lootpack != null)
                {
                    player.Inventory.Bag.AcquireDefaultItem(Items.Actions.ItemTaskType.Fishing, lootpack.ItemId, 1);
                }
            }
        }
    }
}
