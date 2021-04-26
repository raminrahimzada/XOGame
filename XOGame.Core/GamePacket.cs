namespace XOGame.Core
{
    public abstract class ClientPacket : GamePacket
    {
        public int SecretToken { get; protected set; }

        protected ClientPacket(int secretToken)
        {
            SecretToken = secretToken;
        }
    }
    public abstract class ServerPacket : GamePacket
    {
        public abstract bool IsBroadcast { get; }
    }
    public abstract class GamePacket
    {
        public abstract byte[] Serialize();
        public abstract void DeSerializeFrom(ref byte[] buffer);
        public abstract byte PacketType { get; }

        public static PacketTypes DetectPacket(ref byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0) return 0;
            return (PacketTypes) buffer[0];
        }
    }
}
