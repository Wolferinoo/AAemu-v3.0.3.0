﻿using System;

using AAEmu.Game.Core.Packets;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Skills.Templates;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.Skills.Effects
{
    public class AcceptQuestEffect : EffectTemplate
    {
        public uint QuestId { get; set; }

        public override bool OnActionTime => false;

        public override void Apply(BaseUnit caster, SkillCaster casterObj, BaseUnit target, SkillCastTarget targetObj, CastAction castObj,
            EffectSource source, SkillObject skillObject, DateTime time, CompressedGamePackets packetBuilder = null)
        {
            _log.Trace("AcceptQuestEffect");

            if (target is Character character)
            {
                character.Quests.Add(QuestId);
            }
        }
    }
}
