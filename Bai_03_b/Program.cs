using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai_03_b
{
    class Program
    {
        static void Main(string[] args)
        {
            float a, b;
            Console.WriteLine("Mời nhập a: ");
            a = float.Parse(Console.ReadLine());
            Console.WriteLine("Mời nhập b: ");
            b = float.Parse(Console.ReadLine());
            Console.WriteLine($"a + b = {a+b}");
            Console.WriteLine($"a - b = {a-b}");
            Console.WriteLine($"a x b = {a*b}");
            Console.WriteLine($"a / b = {a/b}");
            Console.ReadLine();
        }
    }
}
