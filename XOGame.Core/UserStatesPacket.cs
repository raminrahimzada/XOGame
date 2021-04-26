
namespace XOGame.Core
{
    public class UserStatesPacket : ServerPacket
    {
        public UserModel[] Users { get; private set; }

        public UserStatesPacket()
        {
        }
        public UserStatesPacket(ref UserModel[] users)
        {
            Users = users;
        }
        public override byte[] Serialize()
        {
            var writer=new ByteWriter();
            writer.Write(PacketType);
            var len = (byte) Users.Length;
            writer.Write(len);
            foreach (var user in Users)
            {
                writer.Write(user.Username);
                writer.Write(user.PositionX);
                writer.Write(user.PositionY);
            }
            return writer.ToArray();
        }

        public override void DeSerializeFrom(ref byte[] buffer)
        {
            var reader = new ByteReader(ref buffer);
            byte packetType = reader.ReadByte();
            int len = reader.ReadByte();
            Users = new UserModel[len];
            for (int i = 0; i < len; i++)
            {
                var user=new UserModel();
                Users[i] = user;
                user.Username = reader.ReadString();
                user.PositionX = reader.ReadInt();
                user.PositionY = reader.ReadInt();
            }
        }

        public override byte PacketType => (byte)PacketTypes.UserStates;

        public override bool IsBroadcast => true;

        public static UserStatesPacket Empty()
        {
            return new UserStatesPacket();
        }
    }
}