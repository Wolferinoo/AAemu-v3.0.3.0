﻿using AAEmu.Game.Models.Game.DoodadObj.Templates;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.DoodadObj.Funcs
{
    public class DoodadFuncOpenPaper : DoodadFuncTemplate
    {
        // doodad_funcs
        public uint BookPageId { get; set; }
        public uint BookId { get; set; }

        public override void Use(BaseUnit caster, Doodad owner, uint skillId, int nextPhase = 0)
        {
            _log.Trace("DoodadFuncOpenPaper");

        }
    }
}
