using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__Strings
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string greeting = "Hello";

            string greeting2 = "Nice to meet you!";

            string txt = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Console.WriteLine("The length of the txt string is: " + txt.Length);

            string txt1 = "Hello World";
            Console.WriteLine(txt1.ToUpper());   // Outputs "HELLO WORLD"
            Console.WriteLine(txt1.ToLower());   // Outputs "hello world"
        }
    }
}
