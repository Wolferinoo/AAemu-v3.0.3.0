﻿using AAEmu.Commons.Network;
using AAEmu.Game.Core.Network.Game;

namespace AAEmu.Game.Core.Packets.G2C
{
    public class SCPlotChannelingStoppedPacket : GamePacket
    {
        private readonly ushort _tl;
        private readonly uint _duration;
        private readonly byte _lastEvent;

        public SCPlotChannelingStoppedPacket(ushort tl, uint duration, byte lastEvent) : base(SCOffsets.SCPlotChannelingStoppedPacket, 5)
        {
            _tl = tl;
            _duration = duration;
            _lastEvent = lastEvent;
        }

        public override PacketStream Write(PacketStream stream)
        {
            _log.Warn("SCPlotChannelingStoppedPacket: tl = {0} duration: {1} lastEvent: {2}", _tl, _duration, _lastEvent);
            stream.Write(_tl);
            stream.Write(_duration);
            stream.Write(_lastEvent);

            return stream;
        }
    }
}
