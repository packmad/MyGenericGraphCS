using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericGraph
{
    public class DijkstraAlgorithm<V, E> where E : IEdge<V>
    {
        private readonly Dictionary<V, uint> _distance = new Dictionary<V, uint>();
        private readonly Dictionary<V, E> _nextEdge = new Dictionary<V, E>();
        private readonly PriorityQueue<uint, V> _priorityQueue = new PriorityQueue<uint, V>();
        private readonly AGraph<V, E> _graph;
        private readonly V _source;
        public IReadOnlyCollection<E> Paths { get; private set; }

        public DijkstraAlgorithm(AGraph<V, E> graph, V source)
        {
            _graph = graph;
            _source = source;
            RunAlgorithm();
        }

        public IReadOnlyCollection<E> GetPathToDestination(V destination)
        {
            var p = new List<E>();
            V tmp = destination;
            while (!tmp.Equals(_source))
            {
                foreach (var e in Paths)
                {
                    if (e.Destination.Equals(tmp))
                    {
                        p.Add(e);
                        tmp = e.Source;
                        break;
                    }
                }
            }
            p.Reverse();
            return new ReadOnlyCollection<E>(p);
        }


        private void RunAlgorithm()
        {
            if (Equals(_source, default(V)))
                throw new ArgumentNullException();
            //Func<V, V, int> compare = (x, y) => _distance[x] > _distance[y] ? -1 : 1;
            _distance[_source] = 0;

            foreach (var v in _graph.Vertexes)
            {
                if (!v.Equals(_source))
                    _distance[v] = uint.MaxValue;
                _nextEdge[v] = default(E);
                _priorityQueue.Enqueue(_distance[v], v);
            }
            //IList<E> path = new List<E>();

            while (!_priorityQueue.IsEmpty)
            {
                V u = _priorityQueue.Dequeue();
                if (_distance[u] == uint.MaxValue)
                    break;
                foreach (var v in _graph.GetNeighbors(u))
                {
                    foreach (var e in _graph.GetEdges(u, v))
                    {
                        uint alt = _distance[u] + (uint) e.Weight;
                        if (alt < _distance[v])
                        {
                            _priorityQueue.SafeChangePriority(_distance[v], alt, v);
                            _distance[v] = alt;
                            if (Equals(_nextEdge[v], default(E)) || e.Weight < _nextEdge[v].Weight)
                                _nextEdge[v] = e;
                        }
                    }
                }
            }
            List<E> p = _nextEdge.Values.ToList();
            int r = p.RemoveAll(e => Equals(e, default(E)) );
            Console.WriteLine(r);
            Paths = new ReadOnlyCollection<E>(p);
        }

    }

}
