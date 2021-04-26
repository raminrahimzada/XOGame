using System;

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
        public override byte[] Serialize()
        {
            var writer=new ByteWriter();
            writer.Write(PacketType);
            writer.Write(Username);
            writer.Write(Password);
            return writer.ToArray();
        }
        
        public override void DeSerializeFrom(ref byte[] buffer)
        {
            if (buffer == null || buffer.Length==0)
            {
                throw new Exception("Invalid Buffer for Login Packet");
            }

            var reader = new ByteReader(ref buffer);
            var packetType = reader.ReadByte();
            if (packetType != PacketType)
            {
                throw new Exception("Invalid Buffer for Login Packet");
            }

            Username = reader.ReadString();
            Password = reader.ReadString();
        }

        public override byte PacketType => (byte) PacketTypes.Login;

        public static LoginPacket Empty()
        {
            return new LoginPacket("", "");
        }
    }
}