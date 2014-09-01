using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericGraph
{

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int a, int b)
        {
            X = a;
            Y = b;

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Point point1 = new Point(20, 30);
            Point point2 = point1;
            point2.X = 50;
            Console.WriteLine(point1.X);       // 20 (does this surprise you?)
            Console.WriteLine(point2.X);       // 50
            Console.ReadLine();
            /*
            Pen pen1 = new Pen(Color.Black);
            Pen pen2 = pen1;
            pen2.Color = Color.Blue;
            Console.WriteLine(pen1.Color);
            Console.WriteLine(pen2.Color); 
             */
        }
    }
}
