namespace XOGame.Core
{
    public abstract class PacketDetector
    {
        public static GamePacket Detect(ref byte[] buffer, out PacketTypes packetType)
        {
            GamePacket packet;
            if (buffer == null || buffer.Length == 0)
            {
                packetType = 0;
                return null;
            }
            packetType = (PacketTypes) buffer[0];
            if (packetType == 0) return null;

            switch (packetType)
            {
                case PacketTypes.HeartBeat:
                    packet = HeartbeatPacket.Empty();
                    break;
                case PacketTypes.LoginRequest:
                    packet = LoginPacket.Empty();
                    break;
                case PacketTypes.SetStateRequest:
                    packet = SetStatePacket.Empty();
                    break;
                case PacketTypes.LoginResponse:
                    packet = LoginResponsePacket.Empty();
                    break;
                case PacketTypes.UserStateResponse:
                    packet = UserStatePacket.Empty();
                    break;
                case PacketTypes.HeartBeatResponse:
                    packet=HeartbeatResponsePacket.Empty();
                    break;
                default:
                    return null;
            }
            packet.DeSerializeRaw(ref buffer);
            return packet;
        }
    }
}