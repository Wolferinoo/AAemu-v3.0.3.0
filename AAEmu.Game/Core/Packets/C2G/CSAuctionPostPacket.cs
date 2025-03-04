﻿using AAEmu.Commons.Network;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Network.Game;

namespace AAEmu.Game.Core.Packets.C2G
{
    public class CSAuctionPostPacket : GamePacket
    {
        public CSAuctionPostPacket() : base(CSOffsets.CSAuctionPostPacket, 5)
        {
        }

        public override void Read(PacketStream stream)
        {
            var npcObjId = stream.ReadBc();
            var npcObjId2 = stream.ReadBc();
            var itemId = stream.ReadUInt64();
            var startPrice = stream.ReadUInt32();
            var buyoutPrice = stream.ReadUInt32();
            var duration = stream.ReadByte();

            AuctionManager.Instance.ListAuctionItem(Connection.ActiveChar, itemId, startPrice, buyoutPrice, duration);
        }
    }
}
