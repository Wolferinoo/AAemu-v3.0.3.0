﻿using AAEmu.Commons.Network;
using AAEmu.Commons.Utils;
using AAEmu.Game.Core.Managers.UnitManagers;
using AAEmu.Game.Core.Network.Game;

namespace AAEmu.Game.Core.Packets.C2G
{
    public class CSCreateDoodadPacket : GamePacket
    {
        public CSCreateDoodadPacket() : base(CSOffsets.CSCreateDoodadPacket, 5)
        {
        }

        public override void Read(PacketStream stream)
        {
            var id = stream.ReadUInt32();
            var x = Helpers.ConvertLongX(stream.ReadInt64());
            var y = Helpers.ConvertLongY(stream.ReadInt64());
            var z = stream.ReadSingle();
            var zRot = stream.ReadSingle();
            var scale = stream.ReadSingle();
            var itemId = stream.ReadUInt64();

            _log.Warn("CreateDoodad, Id: {0}, X: {1}, Y: {2}, Z: {3}, zRot: {4}  ItemId: {5}", id, x, y, z, zRot, itemId);
            DoodadManager.Instance.CreatePlayerDoodad(Connection.ActiveChar, id, x, y, z, zRot, scale, itemId);
        }
    }
}
