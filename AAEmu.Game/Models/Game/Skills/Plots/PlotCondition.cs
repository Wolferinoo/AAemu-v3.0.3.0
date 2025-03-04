﻿using System;

using AAEmu.Commons.Utils;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Items;
using AAEmu.Game.Models.Game.Items.Templates;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Utils;

using NLog;

namespace AAEmu.Game.Models.Game.Skills.Plots
{
    public class PlotCondition
    {
        protected static Logger _log = LogManager.GetCurrentClassLogger();
        public uint Id { get; set; }
        public bool NotCondition { get; set; }
        public PlotConditionType Kind { get; set; }
        public int Param1 { get; set; }
        public int Param2 { get; set; }
        public int Param3 { get; set; }

        public bool Check(BaseUnit caster, SkillCaster casterCaster, BaseUnit target, SkillCastTarget targetCaster, SkillObject skillObject, PlotEventCondition eventCondition, Skill skill)
        {
            var res = true;
            switch (Kind)
            {
                case PlotConditionType.Level:
                    {
                        res = ConditionLevel(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2, Param3);
                        break;
                    }
                case PlotConditionType.Relation:
                    {
                        res = ConditionRelation(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2, Param3);
                        break;
                    }
                case PlotConditionType.Direction:
                    {
                        res = ConditionDirection(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2, Param3, eventCondition);
                        break;
                    }
                case PlotConditionType.BuffTag:
                    {
                        res = ConditionBuffTag(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2, Param3, eventCondition);
                        break;
                    }
                case PlotConditionType.WeaponEquipStatus:
                    {
                        res = ConditionWeaponEquipStatus(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2, Param3);
                        break;
                    }
                case PlotConditionType.Chance:
                    {
                        res = ConditionChance(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2,
                            Param3);
                        break;
                    }
                case PlotConditionType.Dead:
                    {
                        res = ConditionDead(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2,
                            Param3);
                        break;
                    }
                case PlotConditionType.CombatDiceResult:
                    {
                        res = ConditionCombatDiceResult(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2,
                            Param3, skill); // Every CombatDiceResult is a NotCondition -> false makes it true. 
                        break;
                    }
                case PlotConditionType.InstrumentType:
                    {
                        res = ConditionInstrumentType(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2,
                            Param3);
                        break;
                    }
                case PlotConditionType.Range:
                    {
                        res = ConditionRange(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2,
                            Param3);
                        break;
                    }
                case PlotConditionType.Variable:
                    {
                        res = ConditionVariable(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2, Param3);
                        break;
                    }
                case PlotConditionType.UnitAttrib:
                    {
                        res = ConditionUnitAttrib(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2,
                            Param3);
                        break;
                    }
                case PlotConditionType.Actability:
                    {
                        res = ConditionActability(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2,
                            Param3);
                        break;
                    }
                case PlotConditionType.Stealth:
                    {
                        res = ConditionStealth(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2,
                            Param3);
                        break;
                    }
                case PlotConditionType.Visible:
                    {
                        res = ConditionVisible(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2,
                            Param3);
                        break;
                    }
                case PlotConditionType.ABLevel:
                    {
                        res = ConditionABLevel(caster, casterCaster, target, targetCaster, skillObject, Param1, Param2,
                            Param3);
                        break;
                    }
            }

            _log.Trace("PlotCondition : {0} | Params : {1}, {2}, {3} | Result : {4}", Kind, Param1, Param2, Param3, NotCondition ? !res : res);

            return NotCondition ? !res : res;
        }

        private static bool ConditionLevel(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int minLevel, int maxLevel, int unk3)
        {
            var unit = (Unit)caster;
            return unit.Level >= minLevel && unit.Level <= maxLevel;
        }

        private static bool ConditionRelation(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int unk1, int unk2, int unk3)
        {
            // Param1 is either 1, 4 or 5
            return true;
        }

        private static bool ConditionDirection(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int unk1, int unk2, int unk3, PlotEventCondition eventCondition)
        {
            return MathUtil.IsFront(caster, target);
        }

        private static bool ConditionBuffTag(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int tagId, int unk2, int unk3, PlotEventCondition eventCondition)
        {
            // if (eventCondition.TargetId == PlotEffectTarget.Source)
            //     return caster.Effects.CheckBuffs(SkillManager.Instance.GetBuffsByTagId((uint)tagId));
            // else if (eventCondition.TargetId == PlotEffectTarget.Target)
            //     return target.Effects.CheckBuffs(SkillManager.Instance.GetBuffsByTagId((uint)tagId));
            return target.Buffs.CheckBuffs(SkillManager.Instance.GetBuffsByTagId((uint)tagId));
        }

        private static bool ConditionWeaponEquipStatus(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int weaponEquipStatus, int unk2, int unk3)
        {
            // Weapon equip status can be :
            // 1 = 1handed
            // 2 = 2handed
            // 3 = duel-wielded
            var wieldKind = (WeaponWieldKind)weaponEquipStatus;
            if (caster is Character character)
            {
                return character.GetWeaponWieldKind() == wieldKind;
            }
            return false;
        }

        private static bool ConditionChance(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int chance, int unk2, int unk3)
        {
            // Param2 is only used once, and its value is "1"
            var unit = (Unit)caster;
            var roll = Rand.Next(0, 100);
            unit.ConditionChance = roll <= chance;
            return roll <= chance;
        }

        private static bool ConditionDead(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int unk1, int unk2, int unk3)
        {
            var unitTarget = (Unit)target;
            return unitTarget.Hp == 0;
        }

        private static bool ConditionCombatDiceResult(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int unk1, int unk2, int unk3, Skill skill)
        {
            var unit = (Unit)caster;
            if (target is Unit trg)
            {
                //Super hacky way to do combat dice....
                var hitType = skill.RollCombatDice(unit, trg);
                if (!skill.HitTypes.ContainsKey(trg.ObjId))
                {
                    skill.HitTypes.Add(trg.ObjId, hitType);
                }
                else
                {
                    skill.HitTypes[trg.ObjId] = hitType;
                }

                return hitType == SkillHitType.MeleeDodge
                       || hitType == SkillHitType.MeleeParry
                       || hitType == SkillHitType.MeleeBlock
                       || hitType == SkillHitType.MeleeMiss
                       || hitType == SkillHitType.RangedDodge
                       || hitType == SkillHitType.RangedParry
                       || hitType == SkillHitType.RangedBlock
                       || hitType == SkillHitType.RangedMiss
                       || hitType == SkillHitType.Immune;
            }
            return true; // Every CombatDiceResult is a NotCondition -> false makes it true.
        }

        private static bool ConditionInstrumentType(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int instrumentTypeId, int unk2, int unk3)
        {
            // Param1 is either 21, 22 or 23
            if (caster is Character character)
            {
                var item = character.Inventory.Equipment.GetItemBySlot((int)EquipmentItemSlot.Musical);
                if (item == null)
                {
                    return false;
                }

                if (item.Template is WeaponTemplate template)
                {
                    if (instrumentTypeId == template.HoldableTemplate.SlotTypeId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool ConditionRange(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int minRange, int maxRange, int unk3)
        {
            // Param1 = Min range
            // Param2 = Max range
            var range = MathUtil.CalculateDistance(caster.Transform.World.Position, target.Transform.World.Position);
            range -= 2;//Temp fix because the calculation is off
            range = Math.Max(0f, range);
            return range >= minRange && range <= maxRange;
        }

        private static bool ConditionVariable(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int unk1, int unk2, int unk3)
        {
            var index = unk1;
            var operation = unk2;
            var value = unk3;
            var unit = (Unit)caster;
            //There is a high chance this is not implemented correctly..
            //If refactoring. See SpecialEffect -> SetVariable as well
            if (operation == 1)
            {
                //TODO obtain variables directly from plot.
                return unit.ActivePlotState.Variables[index] == value;
            }
            _log.Error("Invalid Plot Variable Condition Operation[{0}]", operation);
            return false;
        }

        private static bool ConditionUnitAttrib(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int unk1, int unk2, int unk3)
        {
            // All 3 params used. No idea.
            return true;
        }

        private static bool ConditionActability(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int actabilityId, int op, int level)
        {
            // Check actability level
            // Param1 = Actability ID
            // Param2 = Operator (2, 3, 5) for equal, less than and less than or equal
            // Param3 = Actability Level
            return true;
        }

        private static bool ConditionStealth(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int unk1, int unk2, int unk3)
        {
            // unsure if player or target
            // only used for Flamebolt for some reason.
            // Also always a "NotCondition" so will default to false (result will be True)
            return true;
        }

        private static bool ConditionVisible(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int unk1, int unk2, int unk3)
        {
            // used for LOS ?
            return true;
        }
        private static bool ConditionABLevel(BaseUnit caster, SkillCaster casterCaster, BaseUnit target,
            SkillCastTarget targetCaster, SkillObject skillObject, int abilityType, int min, int max)
        {
            if (caster is Character character)
            {
                var ability = character.Abilities.Abilities[(AbilityType)abilityType];
                int abLevel = ExpirienceManager.Instance.GetLevelFromExp(ability.Exp);
                return abLevel >= min && abLevel <= max;
            }
            //Should this ever not be a character using this condition?
            return false;
        }
    }
}
