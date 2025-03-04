﻿using AAEmu.Game.Models.Game.DoodadObj.Templates;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.DoodadObj.Funcs
{
    public class DoodadFuncClimb : DoodadFuncTemplate
    {
        // doodad_funcs
        public uint ClimbTypeId { get; set; }
        public bool AllowHorizontalMultiHanger { get; set; }

        public override void Use(BaseUnit caster, Doodad owner, uint skillId, int nextPhase = 0)
        {
            _log.Trace("DoodadFuncClimb");

        }
    }
}
