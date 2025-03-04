﻿using AAEmu.Game.Models.Game.DoodadObj.Templates;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.DoodadObj.Funcs
{
    public class DoodadFuncNaviMarkPosToMap : DoodadFuncTemplate
    {
        // doodad_funcs
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override void Use(BaseUnit caster, Doodad owner, uint skillId, int nextPhase = 0)
        {
            _log.Trace("DoodadFuncNaviMarkPosToMap");

        }
    }
}
