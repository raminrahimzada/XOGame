namespace XOGame.Core
{
    public class StatePacket : ClientPacket
    {        
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public StatePacket(int secretToken, int positionX, int positionY):base(secretToken)
        {
            PositionX = positionX;
            PositionY = positionY;
        }
        public override byte[] Serialize()
        {
            var writer=new ByteWriter();
            writer.Write(PacketType);
            writer.Write(SecretToken);
            writer.Write(PositionX);
            writer.Write(PositionY);
            return writer.ToArray();
        }

        public override void DeSerializeFrom(ref byte[] buffer)
        {
            var reader=new ByteReader(ref buffer);
            var packetType = reader.ReadByte();
            SecretToken = reader.ReadInt();
            PositionX = reader.ReadInt();
            PositionY = reader.ReadInt();
        }

        public override byte PacketType => (byte) PacketTypes.State;

        public static StatePacket Empty()
        {
            return new StatePacket(0, 0, 0);
        }
    }
}