using System;

namespace RLP_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {   
            //byte[] bytes = RLP.Encode("dog");
            byte[] bytes = RLP.Encode("Lorem ipsum dolor sit amet, consectetur adipisicing elit");
            
            Console.WriteLine(BitConverter.ToString(bytes));
        }
    }
}
