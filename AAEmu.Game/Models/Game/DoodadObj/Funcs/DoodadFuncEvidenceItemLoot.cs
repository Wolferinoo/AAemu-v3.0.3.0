﻿using AAEmu.Game.Models.Game.DoodadObj.Templates;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.DoodadObj.Funcs
{
    public class DoodadFuncEvidenceItemLoot : DoodadFuncTemplate
    {
        // doodad_funcs
        public uint SkillId { get; set; }
        public int CrimeValue { get; set; }
        public uint CrimeKindId { get; set; }

        public override void Use(BaseUnit caster, Doodad owner, uint skillId, int nextPhase = 0)
        {
            _log.Trace("DoodadFuncEvidenceItemLoot");

        }
    }
}
