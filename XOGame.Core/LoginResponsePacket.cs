namespace XOGame.Core
{
    public class LoginResponsePacket : ServerPacket
    {
        public int SecretToken { get; private set; }

         
        public LoginResponsePacket(int secretToken)
        {
            SecretToken = secretToken;
        }

        public override bool IsBroadcast => false;

        protected override void Serialize(ref ByteWriter writer)
        {
            writer.Write(SecretToken);
        }

        protected override void DeSerialize(ref ByteReader reader)
        {
            SecretToken = reader.ReadInt();
        }

        public override byte PacketType => (byte) PacketTypes.LoginResponse;

        public static LoginResponsePacket Empty()
        {
            return new LoginResponsePacket(0);
        }
    }
}