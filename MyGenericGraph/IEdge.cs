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
    }


    public abstract class AEdge<V, W> : IEdge<V, W> where W : IComparable<W>
    {
        private readonly V _source;
        private readonly V _destination;
        private readonly W _weight;

        public AEdge(V source, V destination, W weight)
        {
            _source = source;
            _destination = destination;
            _weight = weight;
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
    }
}
