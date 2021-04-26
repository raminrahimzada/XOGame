using System.Diagnostics;

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
        public  void SerializeRaw(ref ByteWriter writer)
        {
            writer.Write(PacketType);
            Serialize(ref writer);
        }

        protected abstract void Serialize(ref ByteWriter writer);

        public void DeSerializeRaw(ref byte[] buffer)
        {
            var reader = new ByteReader(ref buffer);
            var packetType = reader.ReadByte();
            Debug.Assert(packetType == PacketType);
            DeSerialize(ref reader);            
        }

        protected abstract void DeSerialize(ref ByteReader reader);
        public abstract byte PacketType { get; }
    }
}
