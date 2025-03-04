﻿using System;

using AAEmu.Commons.Utils;
using AAEmu.Game.Core.Packets;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Faction;
using AAEmu.Game.Models.Game.Skills.Templates;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.Skills.Effects
{
    public class BuffEffect : EffectTemplate
    {
        public int Chance { get; set; }
        public int Stack { get; set; }
        public int AbLevel { get; set; }
        public BuffTemplate Buff { get; set; }
        public override uint BuffId => Buff.Id;
        public override bool OnActionTime => Buff.Tick > 0;

        public override void Apply(BaseUnit caster, SkillCaster casterObj, BaseUnit target, SkillCastTarget targetObj, CastAction castObj,
            EffectSource source, SkillObject skillObject, DateTime time, CompressedGamePackets packetBuilder = null)
        {
            var unit = (Unit)caster;
            if (target is Unit trg)
            {
                var hitType = SkillHitType.Invalid;
                if ((source.Skill?.HitTypes.TryGetValue(trg.ObjId, out hitType) ?? false)
                    && (source.Skill?.SkillMissed(trg.ObjId) ?? false))
                {
                    return;
                }
            }
            if (Rand.Next(0, 101) > Chance)
            {
                unit.ConditionChance = false;
                return;
            }
            else
            {
                unit.ConditionChance = true;
            }

            if (Buff.RequireBuffId > 0 && !target.Buffs.CheckBuff(Buff.RequireBuffId))
            {
                return; // TODO send error?
            }

            if (target.Buffs.CheckBuffImmune(Buff.Id))
            {
                return; // TODO send error of immune?
            }

            ushort abLevel = 1;
            if (caster is Character character)
            {
                _log.Debug("BuffEffect");
                if (source.Skill != null)
                {
                    var template = source.Skill.Template;
                    var abilityLevel = character.GetAbLevel((AbilityType)source.Skill.Template.AbilityId);
                    if (template.LevelStep != 0)
                    {
                        abLevel = (ushort)((abilityLevel / template.LevelStep) * template.LevelStep);
                    }
                    else
                    {
                        abLevel = (ushort)template.AbilityLevel;
                    }

                    //Dont allow lower than minimum ablevel for skill or infinite debuffs can happen
                    abLevel = (ushort)Math.Max(template.AbilityLevel, (int)abLevel);
                }
                else if (source.Buff != null)
                {
                    //not sure?
                }
            }
            else
            {
                if (source.Skill != null)
                {
                    abLevel = (ushort)source.Skill.Template.AbilityLevel;
                }
            }

            // TODO Doesn't let the quest work Id=2488 "A Mother's Tale", 13, "Lilyut Hills", "Nuian Main"
            ////Safeguard to prevent accidental flagging
            //if (Buff.Kind == BuffKind.Bad && !caster.CanAttack(target) && caster != target)
            //    return;
            target.Buffs.AddBuff(new Buff(target, unit, casterObj, Buff, source.Skill, time) { AbLevel = abLevel });

            if (Buff.Kind == BuffKind.Bad && unit.GetRelationStateTo(target) == RelationState.Friendly
                && unit != target && !target.Buffs.CheckBuff((uint)BuffConstants.Retribution))
            {
                unit.SetCriminalState(true);
            }
        }
    }
}
