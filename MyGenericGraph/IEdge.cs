using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericGraph
{
    public interface IEdge<V>
    {
        V GetSource();
        V GetDestination();
    }
}
