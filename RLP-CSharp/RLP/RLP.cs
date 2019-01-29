using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace RLP_CSharp
{
    public class RLP
    {
        private const int DELTA_SIZE = 55;
        private static readonly double MAX_BYTE_LENGTH = Math.Pow(256, 8);
        private const int SHORT_ITEM_OFFSET = 0x80; // ~ 0xb7 = 0x80 + 55
        private const int SHORT_LIST_OFFSET = 0xc0; // ~ 0xf7 = 0xc0 + 55

        public static byte[] Encode(string str)
        {
            if(String.IsNullOrEmpty(str))
            {
                return new byte[]{ SHORT_ITEM_OFFSET };
            }

            byte[] bytes = Encoding.UTF8.GetBytes(str);
            if (bytes.Length == 1 && bytes[0] < 0x80)
            {
                return bytes;
            }
            return Encode(bytes, SHORT_ITEM_OFFSET);
        }

        public static byte[] Encode(List<string> array)
        {
            if(array == null || array.Count == 0)
            {
                return new byte[]{ SHORT_LIST_OFFSET };
            }

            var data = array.Select(s => Encode(s)).SelectMany(b => b).ToArray();
            return Encode(data, SHORT_LIST_OFFSET);
        }

        public static byte[] Encode(object obj)
        {
            if (obj is string str)
                return Encode(str);
            if (obj is IEnumerable list)
            {
                List<byte> output = new List<byte>();
                foreach (object item in list)
                {
                    output.AddRange(Encode(item));
                }
                return Encode(output.ToArray(), SHORT_LIST_OFFSET);
            }
            throw new ArgumentOutOfRangeException($"{nameof(obj)} must be string or list");
        }
        
        public static byte[] Encode(byte[] bytes, int offset)
        {
            if(bytes.Length <= DELTA_SIZE)
            {
                var prefix = (byte)(offset + bytes.Length);
                return new List<byte>().Append(prefix).Concat(bytes).ToArray();
            }
            else if(bytes.Length < MAX_BYTE_LENGTH)
            {
                byte[] lenBytes = GetExactBytes(bytes);
                var prefix = (byte)(offset + DELTA_SIZE + lenBytes.Length);
                return new List<byte>().Append(prefix).Concat(lenBytes).Concat(bytes).ToArray();
            }
            else
            {
                throw new ArgumentException("Input too long");
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

        public static string Decode(byte[] array)
        {
            var prefix = array[0];
            if(prefix < SHORT_ITEM_OFFSET)
            {
                return Convert.ToString(prefix);
            }
            else if(prefix <= SHORT_ITEM_OFFSET + DELTA_SIZE)
            {
                int len = prefix - SHORT_ITEM_OFFSET;
                return Encoding.UTF8.GetString(array, 1, len);
            }
            else if(prefix < SHORT_LIST_OFFSET)
            {
                var lenOfLen = prefix - (SHORT_ITEM_OFFSET + DELTA_SIZE);
                var data = array.Skip(1).Take(lenOfLen).ToArray();
                int len = 0;
                if(data.Length == 1)
                {
                    len = Convert.ToInt32(data[0]);
                }
                else
                {
                    len = BitConverter.ToInt32(data);
                }
                return Encoding.UTF8.GetString(array, 1 + lenOfLen, len);
            }
            else
            {
                throw new ArgumentException("Input don't conform RLP encoding form");
            }
        }
    }
}
