using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__Access_Strings
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string myString = "Hello";
            Console.WriteLine(myString[0]);  // Outputs "H"

            
            Console.WriteLine(myString[1]);  // Outputs "e"

            
            Console.WriteLine(myString.IndexOf("e"));  // Outputs "1"

            string name = "John Doe";

            // Location of the letter D
            int charPos = name.IndexOf("D");
            Console.WriteLine("charPos = " + charPos);
            // Get last name
            string lastName = name.Substring(charPos);

            // Print the result
            Console.WriteLine(lastName);
        }
    }
}
