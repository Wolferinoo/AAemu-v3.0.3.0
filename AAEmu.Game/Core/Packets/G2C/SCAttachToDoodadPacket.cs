﻿using AAEmu.Commons.Network;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Models.Game.DoodadObj;

namespace AAEmu.Game.Core.Packets.G2C
{
    public class SCAttachToDoodadPacket : GamePacket
    {
        private readonly uint _unitObjId;
        private readonly BondDoodad _bond;

        public SCAttachToDoodadPacket(uint unitObjId, BondDoodad bond) : base(SCOffsets.SCAttachToDoodadPacket, 5)
        {
            _unitObjId = unitObjId;
            _bond = bond;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.WriteBc(_unitObjId);
            stream.Write(_bond);
            return stream;
        }
    }
}
