using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericGraph
{
    class Edge : IEdge<ValueType>
    {
        
    }

    class Graph : IGraph<ValueType, Edge>
    {
        
    }

    class Program
    {
        static void Main(string[] args)
        {
            int x = 4;
            ValueType v = x;
            Console.WriteLine(typeof(int));
            Console.WriteLine(typeof(ValueType));
            Console.ReadLine();
        }
    }
}
