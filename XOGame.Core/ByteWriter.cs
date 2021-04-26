using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XOGame.Core
{
    public class ByteWriter
    {
        private readonly LinkedList<byte> _list;

        public ByteWriter()
        {
            _list = new LinkedList<byte>();
        }

        public void Write(byte b)
        {
            _list.AddLast(b);
        }

        public void Write(int i)
        {
            var data = BitConverter.GetBytes(i);
            foreach (var b in data)
            {
                _list.AddLast(b);
            }
        }

        public void Write(string s)
        {
            var data = Encoding.UTF8.GetBytes(s);
            var len = (byte)data.Length;
            Write(len);
            foreach (var b in data)
            {
                _list.AddLast(b);
            }
        }

        public byte[] ToArray()
        {
            return _list.ToArray();
        }
    }
}