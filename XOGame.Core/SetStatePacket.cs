namespace XOGame.Core
{
    public class SetStatePacket : ClientPacket
    {        
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public SetStatePacket(int secretToken, int positionX, int positionY):base(secretToken)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        protected override void Serialize(ref ByteWriter writer)
        {
            writer.Write(SecretToken);
            writer.Write(PositionX);
            writer.Write(PositionY);
        }

        protected override void DeSerialize(ref ByteReader reader)
        {
            SecretToken = reader.ReadInt();
            PositionX = reader.ReadInt();
            PositionY = reader.ReadInt();
        }

        public override byte PacketType => (byte) PacketTypes.SetStateRequest;

        public static SetStatePacket Empty()
        {
            return new SetStatePacket(0, 0, 0);
        }
    }
}