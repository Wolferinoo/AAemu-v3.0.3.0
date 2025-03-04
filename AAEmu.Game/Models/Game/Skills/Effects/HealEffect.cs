﻿using System;

using AAEmu.Commons.Utils;
using AAEmu.Game.Core.Packets;
using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.Skills.Templates;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.Skills.Effects
{
    public class HealEffect : EffectTemplate
    {
        public bool UseFixedHeal { get; set; }
        public int FixedMin { get; set; }
        public int FixedMax { get; set; }
        public bool UseLevelHeal { get; set; }
        public float LevelMd { get; set; }
        public int LevelVaStart { get; set; }
        public int LevelVaEnd { get; set; }
        public bool Percent { get; set; }
        public bool UseChargedBuff { get; set; }
        public uint ChargedBuffId { get; set; }
        public float ChargedMul { get; set; }
        public bool SlaveApplicable { get; set; }
        public bool IgnoreHealAggro { get; set; }
        public float DpsMultiplier { get; set; }
        public uint ActabilityGroupId { get; set; }
        public int ActabilityStep { get; set; }
        public float ActabilityMul { get; set; }
        public float ActabilityAdd { get; set; }

        public override bool OnActionTime => false;

        public override void Apply(BaseUnit caster, SkillCaster casterObj, BaseUnit target, SkillCastTarget targetObj, CastAction castObj,
            EffectSource source, SkillObject skillObject, DateTime time, CompressedGamePackets packetBuilder = null)
        {
            _log.Trace("HealEffect {0}", Id);

            var unit = (Unit)caster;
            if (target is not Unit trg)
            {
                return;
            }

            if (trg.Hp <= 0)
            {
                return;
            }

            var min = 0.0f;
            var max = 0.0f;

            var levelMin = 0.0f;
            var levelMax = 0.0f;

            if (UseLevelHeal)
            {
                var lvlMd = unit.LevelDps * LevelMd;
                var levelModifier = (((source.Skill?.Level ?? 1) - 1) / 49 * (LevelVaEnd - LevelVaStart) + LevelVaStart) * 0.01f;

                levelMin += (lvlMd - levelModifier * lvlMd) + 0.5f;
                levelMax += (levelModifier + 1) * lvlMd + 0.5f;
            }

            max += ((unit.HDps * 0.001f) * DpsMultiplier);

            var minCastBonus = 1000f;
            // Hack null-check on skill
            var castTimeMod = source.Skill?.Template.CastingTime ?? 0; // This mod depends on casting_inc too!
            if (castTimeMod <= 1000)
            {
                minCastBonus = min > 0 ? min : minCastBonus;
            }
            else
            {
                minCastBonus = castTimeMod;
            }

            var variableDamage = (max * minCastBonus * 0.001f);
            min = variableDamage + levelMin;
            max = variableDamage + levelMax;

            var tickModifier = 1.0f;
            if (source.Buff?.TickEffects.Count > 0)
            {
                tickModifier = (float)(source.Buff.Tick / source.Buff.Duration);
            }

            min *= tickModifier;
            max *= tickModifier;

            if (UseChargedBuff)
            {
                var effect = unit.Buffs.GetEffectFromBuffId(ChargedBuffId);
                if (effect != null)
                {
                    min += ChargedMul * effect.Charge;
                    max += ChargedMul * effect.Charge;
                    effect.Exit();
                }
            }

            var criticalHeal = Rand.Next(0f, 100f) < unit.HealCritical;

            var value = (int)Rand.Next(min, max);

            if (criticalHeal)
            {
                value = (int)(value * (1 + (unit.HealCriticalBonus / 100)));
                unit.CombatBuffs.TriggerCombatBuffs(unit, trg, SkillHitType.SpellCritical, true);
            }

            value = (int)(value * trg.IncomingHealMul);

            if (UseFixedHeal)
            {
                value = Rand.Next(FixedMin, FixedMax);
                if (source.Buff != null && source.IsTrigger)
                {
                    value = (int)((value / 1000.0f) * source.Amount);
                }
                else
                {
                    value = (int)(value * tickModifier);
                }
            }

            value = (int)(value * unit.HealMul);

            var healHitType = criticalHeal ? SkillHitType.RangedCritical : SkillHitType.SpellHit;

            var packet = new SCUnitHealedPacket(castObj, casterObj, trg.ObjId, HealType.Health, healHitType, value);
            if (packetBuilder != null)
            {
                packetBuilder.AddPacket(packet);
            }
            else
            {
                trg.BroadcastPacket(packet, true);
            }

            trg.Hp += value;
            trg.Hp = Math.Min(trg.Hp, trg.MaxHp);
            trg.BroadcastPacket(new SCUnitPointsPacket(trg.ObjId, trg.Hp, trg.Mp, trg.HighAbilityRsc), true);

            trg.Events.OnHealed(this, new OnHealedArgs { Healer = unit, HealAmount = value });
        }
    }
}
