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

        public byte ReadByte()
        {
            return _buffer[_position++];
        }

        public int ReadInt()
        {
            var data = new byte[4];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = _buffer[_position++];
            }

            return BitConverter.ToInt32(data);
        }
        public string ReadString()
        {
            int length = ReadByte();
            var data = new byte[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = _buffer[_position++];
            }
            return Encoding.UTF8.GetString(data);
        }
    }
}