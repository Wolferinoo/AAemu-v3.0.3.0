﻿using System;

using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.Skills.Effects.SpecialEffects
{
    public class Cooldown : SpecialEffectAction
    {
        protected override SpecialType SpecialEffectActionType => SpecialType.Cooldown;

        public override void Execute(BaseUnit caster,
            SkillCaster casterObj,
            BaseUnit target,
            SkillCastTarget targetObj,
            CastAction castObj,
            Skill skill,
            SkillObject skillObject,
            DateTime time,
            int cooldownTime,
            int value2,
            int value3,
            int value4)
        {
            // TODO only for server
            if (caster is Character) { _log.Debug("Special effects: Cooldown cooldownTime {0}, value2 {1}, value3 {2}, value4 {3}", cooldownTime, value2, value3, value4); }

            var unit = (Unit)caster;
            unit?.Cooldowns.AddCooldown(skill.Template.Id, (uint)cooldownTime);
        }
    }
}
