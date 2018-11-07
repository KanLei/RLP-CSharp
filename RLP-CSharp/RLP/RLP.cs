using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace RLP_CSharp
{
    public class RLP
    {
        private const int SIZE_THRESHOLD = 56;
        private const int SHORT_ITEM_OFFSET = 0x80;
        private const int LONG_ITEM_OFFSET = 0xb7;
        private const int SHORT_LIST_OFFSET = 0xc0;
        private const int LONG_LIST_OFFSET = 0xf7;
    
        public static byte[] Encode(string str)
        {
            if(String.IsNullOrEmpty(str))
            {
                return new byte[]{ SHORT_ITEM_OFFSET };
            }
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return Encode(bytes);
        }

        public static byte[] Encode(string[] array)
        {
            if(array == null || array.Length == 0)
            {
                return new byte[]{ SHORT_LIST_OFFSET };
            }

            var data = array.Select(s => Encode(s)).SelectMany(b => b).ToArray();
            if(data.Length < 56)
            {
                var prefix = (byte)(SHORT_LIST_OFFSET + data.Length);
                return new List<byte>().Append(prefix).Concat(data).ToArray();
            }
            else
            {
                var lenBytes = GetExactBytes(data);
                var prefix = (byte)(LONG_LIST_OFFSET + lenBytes.Length);
                return new List<byte>().Append(prefix).Concat(lenBytes).Concat(data).ToArray();
            }
        }
        
        public static byte[] Encode(byte[] bytes)
        {
            if (bytes.Length == 1 && bytes[0] < 0x80)
            {
                return bytes;
            }
            else if(bytes.Length < 56)
            {
                var prefix = (byte)(SHORT_ITEM_OFFSET + bytes.Length);
                return new List<byte>().Append(prefix).Concat(bytes).ToArray();
            }
            else
            {
                byte[] lenBytes = GetExactBytes(bytes);
                var prefix = (byte)(LONG_ITEM_OFFSET + lenBytes.Length);
                return new List<byte>().Append(prefix).Concat(lenBytes).Concat(bytes).ToArray();
            }
        }
        
        private static byte[] GetExactBytes(byte[] bytes)
        {
            byte[] dst = BitConverter.GetBytes(bytes.Length);
            if(BitConverter.IsLittleEndian)
                return dst.Reverse().SkipWhile(b => b == 0).ToArray();
            else
                return dst.SkipWhile(b => b == 0).ToArray();
        }
    }
}
