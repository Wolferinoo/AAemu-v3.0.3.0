﻿using AAEmu.Commons.Network;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Models.Game.DoodadObj;

namespace AAEmu.Game.Core.Packets.G2C
{
    public class SCDoodadPhaseChangedPacket : GamePacket
    {
        private Doodad _doodad;

        public SCDoodadPhaseChangedPacket(Doodad doodad) : base(SCOffsets.SCDoodadPhaseChangedPacket, 5)
        {
            _doodad = doodad;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.WriteBc(_doodad.ObjId);     // objId
            stream.Write(_doodad.FuncGroupId); // funcGroupId
            stream.Write(_doodad.TimeLeft);    // growing
            stream.Write(-1);                  // puzzleGroup
            stream.Write(0u);                  // type(id)

            return stream;
        }
    }
}
