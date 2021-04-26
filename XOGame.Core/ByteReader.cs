using System;
using System.Text;

namespace XOGame.Core
{
    public delegate GamePacket GamePacketHandlerDelegate<T>(ref T item);

    public class ByteReader
    {
        private readonly byte[] _buffer;
        private int _position;
        public ByteReader(ref byte[] buffer)
        {
            _buffer = buffer;
            _position = 0;
        }

        public PacketTypes ReadBytePacketType()
        {
            return (PacketTypes) ReadByte();
        }
        public byte ReadByte()
        {
            return _buffer[_position++];
        }

        public int ReadInt()
        {
            var data = ReadBuffer(4);
            return BitConverter.ToInt32(data);
        }
        public string ReadString()
        {
            int length = ReadByte();
            var data = ReadBuffer(length);
            return Encoding.UTF8.GetString(data);
        }
        private byte[] ReadBuffer(int length)
        {
            var data = new byte[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = _buffer[_position++];
            }

            return data;
        }

        public DateTime ReadDateTime()
        {
            var ticks = ReadLong();
            return new DateTime(ticks);
        }

        private long ReadLong()
        {
            var data = ReadBuffer(8);
            return BitConverter.ToInt64(data);
        }
    }
}