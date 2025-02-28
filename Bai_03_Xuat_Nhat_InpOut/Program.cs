using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai_03_Xuat_Nhat_InpOut
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string name;
            int age;
            double gpa;

            Console.WriteLine("Mời nhập tên:");
            name = Console.ReadLine();
            Console.WriteLine("Mời nhập tuổi:");
            age = int.Parse(Console.ReadLine());
            Console.WriteLine("Mời nhập điểm trung bình:");
            gpa = double.Parse(Console.ReadLine());
            Console.WriteLine();
            //Kiểu đặc trưng của C#.
            Console.WriteLine("Cách thứ nhứt:");
            Console.WriteLine("Họ và tên: {0}\nTuổi: {1}\nĐiểm trung bình: {2}", name, age, gpa);
            //Giống Java.
            Console.WriteLine("\nCách thứ nhì:");
            Console.WriteLine("Họ và tên: " + name + "\nTuổi: " + age +
                "\nĐiểm trung bình: " + gpa);

            Console.ReadKey(); // để cho đẹp
        }
    }
}
