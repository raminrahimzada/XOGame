using System.Linq;

namespace XOGame.Core
{
    public class LoginResponsePacket : ServerPacket
    {
        public int SecretToken { get; private set; }

        public LoginResponsePacket()
        {
            
        }
        public LoginResponsePacket(int secretToken)
        {
            SecretToken = secretToken;
        }

        public override bool IsBroadcast => false;

        public override byte[] Serialize()
        {
           var writer=new ByteWriter();
           writer.Write(PacketType);
           writer.Write(SecretToken);
           return writer.ToArray();
        }

        public override void DeSerializeFrom(ref byte[] buffer)
        {
            var reader = new ByteReader(ref buffer);
            var packetType = reader.ReadByte();
            SecretToken = reader.ReadInt();
        }

        public override byte PacketType => (byte) PacketTypes.LoginResponse;

        public static LoginResponsePacket Empty()
        {
            return new LoginResponsePacket(0);
        }
    }
}