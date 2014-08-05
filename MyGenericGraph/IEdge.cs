using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericGraph
{
    public interface IEdge<V, W> where W : IComparable<W>
    {
        V GetSource();
        V GetDestination();
        W GetWeight();
        uint GetDijWeight();
    }
}
