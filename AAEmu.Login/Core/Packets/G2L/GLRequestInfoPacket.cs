﻿using AAEmu.Commons.Models;
using AAEmu.Commons.Network;
using AAEmu.Login.Core.Controllers;
using AAEmu.Login.Core.Network.Connections;
using AAEmu.Login.Core.Network.Internal;

namespace AAEmu.Login.Core.Packets.G2L
{
    public class GLRequestInfoPacket : InternalPacket
    {
        public GLRequestInfoPacket() : base(GLOffsets.GLRequestInfoPacket)
        {
        }

        public override void Read(PacketStream stream)
        {
            var connection = LoginConnectionTable.Instance.GetConnection(stream.ReadUInt32());
            var requestId = stream.ReadUInt32();
            var characters = stream.ReadCollection<LoginCharacterInfo>();

            if (characters.Count > 0)
            {
                connection.AddCharacters(Connection.GameServer.Id, characters);
            }

            RequestController.Instance.ReleaseId(requestId);
        }
    }
}
