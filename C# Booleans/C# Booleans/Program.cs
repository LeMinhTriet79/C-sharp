using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__Booleans
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isCSharpFun = true;
            bool isFishTasty = false;
            Console.WriteLine(isCSharpFun);   // Outputs True
            Console.WriteLine(isFishTasty);   // Outputs False

            int x = 10;
            int y = 9;
            Console.WriteLine(x > y); // returns True, because 10 is higher than 9

            Console.WriteLine(10 > 9); // returns True, because 10 is higher than 9

            int z = 10;
            Console.WriteLine(z == 10); // returns True, because the value of x is equal to 10
        }
    }
}
