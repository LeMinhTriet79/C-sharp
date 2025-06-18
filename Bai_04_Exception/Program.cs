using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai_04_Exception
{
    class Program
    {
        static void Main(string[] args)
        {
            double a, b;
            Console.WriteLine("Chương Trình Kết Quả Phép Chia");
            Console.WriteLine("Nhập số bị chia a!");
            a = double.Parse(Console.ReadLine());
            Console.WriteLine("a = " + a);
            Console.WriteLine("Nhập số chia b!");
            b = double.Parse(Console.ReadLine());
            Console.WriteLine("b = " + b);
            Console.WriteLine("Kết quả phép chia a và b là: " + a/b);

            Console.WriteLine("Thử Phép Tính Nhảm:");
            try{
                int c = 10;
                Console.WriteLine("c = " + c);
                int d = 0;
                Console.WriteLine("d = " + d);
                
                Console.WriteLine("e = " + c + "/" + d + " = " + "?");
                int e = c / d;
                Console.WriteLine(e);
            }
            catch(Exception ex){
                Console.WriteLine("Nhập tào lao rồi người đẹp!");
                Console.WriteLine(ex);
            }
            
            

            Console.ReadLine();

        }
    }
}
