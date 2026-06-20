using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__String_Interpolation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             Một lựa chọn khác của việc nối chuỗi là nội suy chuỗi ,
            phương pháp này thay thế giá trị của các biến vào các vị trí giữ chỗ trong chuỗi. 
            Lưu ý rằng bạn không cần phải lo lắng về khoảng trắng, như với phép nối chuỗi thông thường:

             */
            string firstName = "John";
            string lastName = "Doe";
            string name = $"My full name is: {firstName} {lastName}";
            Console.WriteLine(name);

            string name1 = firstName + lastName;
            string name2 = "My full name is " + firstName + " " +lastName;
            Console.WriteLine(name1);
            Console.WriteLine(name2);

        }
    }
}
