namespace XOGame.Core
{
    public abstract class PacketDetector
    {
        public static GamePacket Detect(ref byte[] buffer, out PacketTypes packetType)
        {
            GamePacket packet;
            packetType = GamePacket.DetectPacket(ref buffer);
            if (packetType == 0) return null;
            switch (packetType)
            {
                case PacketTypes.Login:
                    packet = LoginPacket.Empty();
                    break;
                case PacketTypes.State:
                    packet = StatePacket.Empty();
                    break;
                case PacketTypes.LoginResponse:
                    packet = LoginResponsePacket.Empty();
                    break;
                case PacketTypes.UserStates:
                    packet = UserStatesPacket.Empty();
                    break;
                default:
                    return null;
            }
            packet.DeSerializeFrom(ref buffer);
            return packet;
        }
    }
}