﻿using System;

using AAEmu.Commons.Network;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Models.Game.Faction;

namespace AAEmu.Game.Core.Packets.G2C
{
    public class SCFactionRelationListPacket : GamePacket
    {
        private readonly FactionRelation[] _relations;

        public SCFactionRelationListPacket() : base(SCOffsets.SCFactionRelationListPacket, 5)
        {
            _relations = new FactionRelation[] { };
        }

        public SCFactionRelationListPacket(FactionRelation[] relations) : base(SCOffsets.SCFactionRelationListPacket, 5)
        {
            _relations = relations;
        }

        public override PacketStream Write(PacketStream stream)
        {
            var updaterName = "";
            var memo = "";

            stream.Write(false);                    // uiRequest
            stream.Write(false);                    // relationRequest
            stream.Write(false);                    // relationVotePeriod
            stream.Write((byte)_relations.Length);  // TODO max length 200
            foreach (var relation in _relations)
            {
                stream.Write(relation.Id);          //type
                stream.Write(relation.Id2);         //type
                stream.Write((byte)relation.State); //state
                stream.Write((byte)0);              // nState
                stream.Write(0L);                   //type
                stream.Write(DateTime.MinValue);    //updateTime
                stream.Write(DateTime.MinValue);    //changeTime relation.ExpTime
                stream.Write(relation.Id);          //type
                stream.Write(updaterName);          //updaterName
                stream.Write(0);                    // updaterItemCount
                stream.Write(memo);                 //memo
                stream.Write(false);                // votePossible
            }

            return stream;
        }
    }
}
