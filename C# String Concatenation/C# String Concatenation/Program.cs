using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__String_Concatenation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string firstName = "John ";
            //string lastName = "Doe";
            //string name = firstName + lastName;
            //Console.WriteLine(name);

            string firstName = "John ";
            string lastName = "Doe";
            string name = string.Concat(firstName, lastName);
            Console.WriteLine(name);
            //=========================================================//

            int x = 10;
            int y = 20;
            int z = x + y;  // z will be 30 (an integer/number)
            Console.WriteLine(z);

            string a = "10";
            string b = "20";
            string c = a + b;  // z will be 1020 (a string)
            Console.WriteLine(c);
        }
    }
}
