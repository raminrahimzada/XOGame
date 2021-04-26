namespace XOGame.Core
{
    public  class LoginPacket : ClientPacket
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public LoginPacket(string username, string password):base(0)
        {
            Username = username;
            Password = password;
        }

        protected override void Serialize(ref ByteWriter writer)
        {
            writer.Write(Username);
            writer.Write(Password);
        }

        protected override void DeSerialize(ref ByteReader reader)
        {
            Username = reader.ReadString();
            Password = reader.ReadString();
        }

        public override byte PacketType => (byte) PacketTypes.LoginRequest;

        public static LoginPacket Empty()
        {
            return new LoginPacket("", "");
        }
    }
}