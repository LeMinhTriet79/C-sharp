using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bai_03_c_Two_Sum
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] a = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int target;
            bool b = false;
            Console.WriteLine("Mời Nhập Con Số Để Thầy Tìm Đi Con!");
            target = int.Parse(Console.ReadLine());
            Console.WriteLine($"Số mày nhập là {target}");

            for(int i = 0; i <= a.Length; i++)
            {
                for(int j = i + 1; j < a.Length; j++)
                {
                    if (a[i] + a[j] == target)
                    {
                        b = true;
                        Console.WriteLine("Kết quả tìm được: ");
                        Console.WriteLine($"{a[i]} + {a[j]} tại vị trí a[i]= {i} và a[j]= {j}");
                    }
                }
            }

            if (!b)
            {
                Console.WriteLine("Không tìm thấy con ơi!");
            }


            Console.ReadLine();
        }
    }
}
