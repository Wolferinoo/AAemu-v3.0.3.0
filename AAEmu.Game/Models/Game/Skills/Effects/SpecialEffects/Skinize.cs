﻿using System;
using System.Collections.Generic;

using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Items;
using AAEmu.Game.Models.Game.Items.Actions;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.Skills.Effects.SpecialEffects
{
    public class Skinize : SpecialEffectAction
    {
        protected override SpecialType SpecialEffectActionType => SpecialType.Skinize;

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
            // TODO ...
            if (caster is Character) { _log.Debug("Special effects: Skinize value1 {0}, value2 {1}, value3 {2}, value4 {3}", value1, value2, value3, value4); }

            if ((!(caster is Character character)) || (character == null))
            {
                return;
            }

            if ((!(targetObj is SkillCastItemTarget itemTarget)) || (itemTarget == null))
            {
                return;
            }

            var itemToImage = character.Inventory.GetItemById(itemTarget.Id);
            if (itemToImage == null)
            {
                return;
            }

            if (itemToImage.HasFlag(ItemFlag.Skinized))
            {
                // Already a image item
                return;
            }

            if ((!(casterObj is SkillItem powderSkillItem)) || (powderSkillItem == null))
            {
                return;
            }

            var powderItem = character.Inventory.GetItemById(powderSkillItem.ItemId);
            if (powderItem == null)
            {
                return;
            }

            if (powderItem.Count < 1)
            {
                return;
            }

            itemToImage.SetFlag(ItemFlag.Skinized);
            character.SendPacket(new SCItemTaskSuccessPacket(ItemTaskType.Sknize, new List<ItemTask>() { new ItemUpdateBits(itemToImage) }, new List<ulong>()));
            powderItem._holdingContainer.ConsumeItem(ItemTaskType.Sknize, powderItem.TemplateId, 1, powderItem);
        }
    }
}
