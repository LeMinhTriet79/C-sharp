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

            // Thiết lập nền đen và chữ sáng
            Console.WriteLine("\u001B[40m\u001B[37m");  // Nền đen, chữ trắng

            // Chữ màu xanh dương đậm
            Console.WriteLine("\u001B[34mThis is like Windows 98/XP!");

            // Thay đổi màu sắc thêm
            Console.WriteLine("\u001B[36mThis is cyan, simulating classic XP style.");

            // Thêm chữ sáng, màu trắng với nền đen
            Console.WriteLine("\u001B[37mText is now white with black background!");

            // Reset màu sắc
            Console.WriteLine("\u001B[0mThis is the default text after reset.");
            Console.ReadLine();
        }
    }
}
