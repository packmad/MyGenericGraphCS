using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericGraph
{
    interface IGraph<V, E> where E : IEdge<V, V>
    {

    }
}
