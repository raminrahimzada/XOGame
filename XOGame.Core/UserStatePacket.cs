
using System.Diagnostics;

namespace XOGame.Core
{
    public class UserStatePacket : ServerPacket
    {
        public UserModel User { get; private set; }

        private UserStatePacket()
        {

        }
        public UserStatePacket(ref UserModel user)
        {
            Debug.Assert(user != null);
            User = user;
        }

        protected override void Serialize(ref ByteWriter writer)
        {
            User.Serialize(writer, full: false);
        }

        protected override void DeSerialize(ref ByteReader reader)
        {
            User = new UserModel();
            User.Deserialize(reader, full: false);
        }

        public override byte PacketType => (byte) PacketTypes.UserStateResponse;

        public override bool IsBroadcast => true;

        public static UserStatePacket Empty()
        {
            return new UserStatePacket()
            {
                User = new UserModel()
            };
        }
    }
}