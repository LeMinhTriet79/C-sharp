using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai_02_Basic_Variables
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string name = "Nguyễn Tuấn Thanh";
            int age = 21;
            double gpa = 9.5f;

            //Kiểu đặc trưng của C#.
            Console.WriteLine("Cách thứ nhứt:");
            Console.WriteLine("Họ và tên: {0}\nTuổi: {1}\nĐiểm trung bình: {2}", name, age, gpa);
            //Giống Java.
            Console.WriteLine("\nCách thứ nhì:");
            Console.WriteLine("Họ và tên: " + name + "\nTuổi: " + age +
                "\nĐiểm trung bình: " +  gpa);

            Console.ReadKey(); // để cho đẹp
        }
    }
}
