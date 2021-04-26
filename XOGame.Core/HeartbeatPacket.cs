namespace XOGame.Core
{
    public class HeartbeatPacket : ClientPacket
    {
        public HeartbeatPacket(int secretToken) : base(secretToken)
        {
        }

        protected override void Serialize(ref ByteWriter writer)
        {
        }

        protected override void DeSerialize(ref ByteReader reader)
        {
        }
        public override byte PacketType => (byte)PacketTypes.HeartBeat;

        public static HeartbeatPacket Empty()
        {
            return new HeartbeatPacket(0);
        }
    }
}