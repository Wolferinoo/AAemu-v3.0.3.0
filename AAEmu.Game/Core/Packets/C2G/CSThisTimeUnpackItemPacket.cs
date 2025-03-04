﻿using AAEmu.Commons.Network;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Models.Game;
using AAEmu.Game.Models.Game.Items;

namespace AAEmu.Game.Core.Packets.C2G
{
    public class CSThisTimeUnpackItemPacket : GamePacket
    {
        public CSThisTimeUnpackItemPacket() : base(CSOffsets.CSThisTimeUnpackItemPacket, 5)
        {

        }

        public override void Read(PacketStream stream)
        {
            var slotType = (SlotType)stream.ReadByte();
            var slot = stream.ReadByte();
            var itemId = stream.ReadUInt64();

            _log.Debug("CSThisTimeUnpackItemPacket, slotType: {0}, slot: {1}, itemId: {2}", slotType, slot, itemId);
            if (!ItemManager.Instance.UnwrapItem(Connection.ActiveChar, slotType, slot, itemId))
            {
                Connection.ActiveChar.SendErrorMessage(ErrorMessageType.ItemUpdateFail);
            }
        }
    }
}
