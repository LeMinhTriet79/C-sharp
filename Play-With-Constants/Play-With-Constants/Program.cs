using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_With_Constants
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int a = 9;
            const int b = 10;
            int c = 11;
            Console.WriteLine(a + b);
            c = 39;
            Console.WriteLine($"c = {c}");
            //Hằng số không cho phép thay đổi là lỗi ngay, vì có chữ const
            //a = 0;
            //Console.WriteLine(a);

        }
    }
}
