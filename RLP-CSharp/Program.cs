using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RLP_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {   
            // byte[] emptyStr = RLP.Encode("");
            // Console.WriteLine(BitConverter.ToString(emptyStr));
            // System.Console.WriteLine(RLP.Decode(emptyStr));

            // [ 0x83, 'd', 'o', 'g' ]
            // byte[] shortStr = RLP.Encode("dog");
            // Console.WriteLine(BitConverter.ToString(shortStr));

            // [ 0xb8, 0x38, 'L', 'o', 'r', 'e', 'm', ' ', ... , 'e', 'l', 'i', 't' ]
            // byte[] longStr = RLP.Encode("Lorem ipsum dolor sit amet, consectetur adipisicing elit");
            // Console.WriteLine(BitConverter.ToString(longStr));

            // [ 0xc8, 0x83, 'c', 'a', 't', 0x83, 'd', 'o', 'g' ]
            // byte[] bytes = RLP.Encode(new string[]{"cat", "dog"});

            // byte[] bytes = RLP.Encode(new string[]{"this is a very long list", "you never guess how long it is", "indeed, how did you know it was this long", "good job, that I can tell you in honestlyyyyy"});
            // Console.Write(String.Join(',', bytes));
            object obj = new List<object>(){
                new List<string>(),
                "dog",
                new List<string>(){"cat"},
                ""
            };
            byte[] bytes = RLP.Encode(obj);
            Console.WriteLine(BitConverter.ToString(bytes));
            System.Console.WriteLine(BitConverter.ToString(Encoding.UTF8.GetBytes("dog")));
            System.Console.WriteLine(BitConverter.ToString(Encoding.UTF8.GetBytes("cat")));
        }
    }
}
