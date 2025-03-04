﻿using System;
using System.Collections.Generic;
using System.Linq;

using AAEmu.Game.Core.Managers;
using AAEmu.Game.Models.Game.Skills.Effects;
using AAEmu.Game.Models.Game.Skills.Plots.Tree;
using AAEmu.Game.Models.Game.Skills.Templates;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Utils;

namespace AAEmu.Game.Models.Game.Skills.Plots
{
    public class PlotNextEvent
    {
        public uint Id { get; set; }
        public PlotEventTemplate Event { get; set; }
        public int Position { get; set; }
        public bool PerTarget { get; set; }
        public bool Casting { get; set; }
        public int Delay { get; set; }
        public int Speed { get; set; }
        public bool Channeling { get; set; }
        public int CastingInc { get; set; }
        public bool AddAnimCsTime { get; set; }
        public bool CastingDelayable { get; set; }
        public bool CastingCancelable { get; set; }
        public bool CancelOnBigHit { get; set; }
        public bool UseExeTime { get; set; }
        public bool Fail { get; set; }

        private int GetAnimDelay(IEnumerable<PlotEventEffect> effects)
        {
            if (!AddAnimCsTime)
            {
                return 0;
            }

            foreach (var effect in effects)
            {
                var template = SkillManager.Instance.GetEffectTemplate(effect.ActualId, effect.ActualType);
                if (!(template is SpecialEffect specialEffect))
                {
                    continue;
                }

                if (specialEffect.SpecialEffectTypeId != SpecialType.Anim)
                {
                    continue;
                }

                var anim = AnimationManager.Instance.GetAnimation((uint)specialEffect.Value1);
                return anim.CombatSyncTime;
            }

            return 0;
        }

        private int GetProjectileDelay(BaseUnit caster, BaseUnit target)
        {
            if (Speed <= 0)
            {
                return 0;
            }

            var dist = MathUtil.CalculateDistance(caster.Transform.World.Position, target.Transform.World.Position, true);
            //We want damage to be applied when the projectile hits target.
            return (int)Math.Round((dist / Speed) * 1000.0f);

        }

        private int GetSkillControllerDelay(PlotNode node)
        {
            //Value3=delay, Value5=useDelay?
            return node.Event.Effects.Where(o => o.ActualType == "SkillController")
                .Select(o => SkillManager.Instance.GetEffectTemplate(o.ActualId, o.ActualType) as SkillControllerTemplate)
                .Where(o => o != null && o.Value[4] == 1)
                .Max(o => (int?)o.Value[2]) ?? 0;
        }

        public int GetDelay(PlotState state, PlotTargetInfo eventInstance, PlotNode node)
        {
            var animTime = (int)(GetAnimDelay(node.Event.Effects) * (state.Caster.GlobalCooldownMul / 100f));
            var projectileTime = GetProjectileDelay(eventInstance.Source, eventInstance.Target);
            var skillCtrlTime = GetSkillControllerDelay(node);
            var delay = animTime + projectileTime + skillCtrlTime;
            if (Casting)
            {
                delay += (int)(state.Caster.ApplySkillModifiers(state.ActiveSkill, Static.SkillAttribute.CastTime,
                    Delay) * state.Caster.CastTimeMul);
            }
            else
            {
                delay += Delay;
            }

            return Math.Clamp(delay, 0, int.MaxValue);
        }
    }
}
