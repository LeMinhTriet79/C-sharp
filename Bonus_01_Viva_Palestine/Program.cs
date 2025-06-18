using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonus_01_Viva_Palestine
{
    class Program
    {
        static void Main()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Clear();

            int width = 60;
            int height = 18;
            int triangleWidth = width / 3; // chiều dài của tam giác đỏ
            double centerY = (height - 1) / 2.0; // điểm giữa chiều cao

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Cạnh trên tam giác: từ (0,0) đến (triangleWidth, centerY)
                    double upperY = centerY * x / triangleWidth;

                    // Cạnh dưới tam giác: từ (0,height-1) đến (triangleWidth, centerY)
                    double lowerY = (height - 1) - (centerY * x / triangleWidth);

                    // Trong tam giác đỏ nếu:
                    bool inTriangle = (x <= triangleWidth) && (y >= upperY) && (y <= lowerY);

                    if (inTriangle)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("*");
                    }
                    else if (y < height / 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("*");
                    }
                    else if (y < 2 * height / 3)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("*");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("*");
                    }
                   
                }
            
                Console.WriteLine();
            }

            Console.ResetColor();
        }
    }
}
