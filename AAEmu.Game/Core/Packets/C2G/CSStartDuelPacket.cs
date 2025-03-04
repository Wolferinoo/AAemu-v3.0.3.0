﻿using AAEmu.Commons.Network;
using AAEmu.Game.Core.Managers.World;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.DoodadObj;

namespace AAEmu.Game.Core.Packets.C2G
{
    public class CSStartDuelPacket : GamePacket
    {
        public CSStartDuelPacket() : base(CSOffsets.CSStartDuelPacket, 5)
        {
        }

        public override void Read(PacketStream stream)
        {
            var challengerId = stream.ReadUInt32();  // ID of the one who challenged us to a duel
            var errorMessage = stream.ReadInt16();  // 0 - accepted the duel, 507 - refused

            _log.Warn("StartDuel, Id: {0}, ErrorMessage: {1}", challengerId, errorMessage);

            if (errorMessage != 0)
            {
                return;
            }

            var challengedObjId = Connection.ActiveChar.ObjId;
            var challenger = WorldManager.Instance.GetCharacterById(challengerId);
            var challengerObjId = challenger.ObjId;

            Connection.ActiveChar.BroadcastPacket(new SCDuelStartedPacket(challengerObjId, challengedObjId), true);
            Connection.ActiveChar.BroadcastPacket(new SCAreaChatBubblePacket(true, Connection.ActiveChar.ObjId, 543), true);
            Connection.ActiveChar.BroadcastPacket(new SCDuelStartCountdownPacket(), true);

            var doodadFlag = new DoodadSpawner();
            const uint unitId = 5014u; // Combat Flag
            doodadFlag.Id = 0;
            doodadFlag.UnitId = unitId;
            doodadFlag.Position = Connection.ActiveChar.Transform.CloneAsSpawnPosition();
            doodadFlag.Position.X = Connection.ActiveChar.Transform.World.Position.X - (Connection.ActiveChar.Transform.World.Position.X - challenger.Transform.World.Position.X) / 2;
            doodadFlag.Position.Y = Connection.ActiveChar.Transform.World.Position.Y - (Connection.ActiveChar.Transform.World.Position.Y - challenger.Transform.World.Position.Y) / 2;
            doodadFlag.Position.Z = Connection.ActiveChar.Transform.World.Position.Z - (Connection.ActiveChar.Transform.World.Position.Z - challenger.Transform.World.Position.Z) / 2;
            doodadFlag.Spawn(0);

            Connection.ActiveChar.BroadcastPacket(new SCDuelStatePacket(challengerObjId, doodadFlag.Last.ObjId), true);
            Connection.ActiveChar.BroadcastPacket(new SCDuelStatePacket(challengedObjId, doodadFlag.Last.ObjId), true);
            Connection.SendPacket(new SCDoodadPhaseChangedPacket(doodadFlag.Last));
            Connection.SendPacket(new SCCombatEngagedPacket(challengerObjId));
            Connection.ActiveChar.BroadcastPacket(new SCCombatEngagedPacket(challengedObjId), false);
        }
    }
}
