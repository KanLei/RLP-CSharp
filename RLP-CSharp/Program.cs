using System;

namespace RLP_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {   
            // byte[] bytes = RLP.Encode("dog");
            // byte[] bytes = RLP.Encode("Lorem ipsum dolor sit amet, consectetur adipisicing elit");
            // byte[] bytes = RLP.Encode(new string[]{"cat", "dog"});
            byte[] bytes = RLP.Encode(new string[]{"this is a very long list", "you never guess how long it is", "indeed, how did you know it was this long", "good job, that I can tell you in honestlyyyyy"});
            Console.Write(String.Join(',', bytes));
        }
    }
}
