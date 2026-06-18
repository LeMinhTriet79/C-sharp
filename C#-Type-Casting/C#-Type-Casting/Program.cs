using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__Type_Casting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int myInt = 9;
            double myDouble = myInt;
            Console.WriteLine(myInt);
            Console.WriteLine(myDouble);

            double a = 9.79;
            int b = (int)a;
            Console.WriteLine(a);
            Console.WriteLine(b);

            int c = 10;
            double d = 5.25;
            bool e = true;

            Console.WriteLine(Convert.ToString(c));    // convert int to string
            Console.WriteLine(Convert.ToDouble(c));    // convert int to double
            Console.WriteLine(Convert.ToInt32(d));  // convert double to int
            Console.WriteLine(Convert.ToString(e));   // convert bool to string
        }
    }
    
}
