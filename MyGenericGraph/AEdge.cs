using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericGraph
{
    public abstract class AEdge<V, W> : IEdge<V, W> where W : IComparable<W>
    {
        private readonly V _source;
        private readonly V _destination;
        private readonly W _weight;
        private readonly uint _dijWeight;

        public AEdge(V source, V destination, W weight, uint dijWeight)
        {
            _source = source;
            _destination = destination;
            _weight = weight;
            _dijWeight = dijWeight;
        }

        public V GetSource()
        {
            return _source;
        }

        public V GetDestination()
        {
            return _destination;
        }

        public W GetWeight()
        {
            return _weight;
        }

        public uint GetDijWeight()
        {
            return _dijWeight;
        }
    }
}
