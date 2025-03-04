﻿using System;

using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Packets;
using AAEmu.Game.Models.Game.Skills.Templates;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.Skills.Effects
{
    public class DispelEffect : EffectTemplate
    {
        public int DispelCount { get; set; }
        public int CureCount { get; set; }
        public uint BuffTagId { get; set; }

        public override bool OnActionTime => false;

        public override void Apply(BaseUnit caster, SkillCaster casterObj, BaseUnit target, SkillCastTarget targetObj, CastAction castObj,
            EffectSource source, SkillObject skillObject, DateTime time, CompressedGamePackets packetBuilder = null)
        {
            _log.Trace("DispelEffect {0}", Id);

            if (BuffTagId > 0 && !target.Buffs.CheckBuffs(SkillManager.Instance.GetBuffsByTagId(BuffTagId)))
            {
                return;
            }

            if (DispelCount > 0 && caster.CanAttack(target))
            {
                target.Buffs.RemoveBuffs(BuffKind.Good, DispelCount, BuffTagId); //TODO ....
            }

            if (CureCount > 0 && !caster.CanAttack(target))
            {
                target.Buffs.RemoveBuffs(BuffKind.Bad, CureCount, BuffTagId);
            }
        }
    }
}
