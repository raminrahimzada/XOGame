using System;

namespace XOGame.Core
{
    public class HeartbeatResponsePacket : ServerPacket
    {
        public DateTime ServerTime { get; set; }

        public HeartbeatResponsePacket(DateTime serverTime)
        {
            ServerTime = serverTime;
        }

        protected override void Serialize(ref ByteWriter writer)
        {
            writer.Write(ServerTime);
        }

        protected override void DeSerialize(ref ByteReader reader)
        {
            ServerTime = reader.ReadDateTime();
        }

        public override byte PacketType => (byte) PacketTypes.HeartBeatResponse;
        public override bool IsBroadcast => false;

        public static HeartbeatResponsePacket Empty()
        {
            return new HeartbeatResponsePacket(DateTime.MinValue);
        }
    }
}