using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericGraph
{
    public abstract class AGraph<V, E, W>
        where E : IEdge<V, W>
        where W : IComparable<W>
    {
        private readonly ICollection<V> _vertexes;
        private readonly ICollection<E> _edges;

        public AGraph(ICollection<V> vertexes, ICollection<E> edges)
        {
            _vertexes = vertexes;
            _edges = edges;
        }

        public void Add(V vertex)
        {
            _vertexes.Add(vertex);
        }

        public void Add(E edge)
        {
            _edges.Add(edge);
        }

        public IEnumerator<V> BreadthFirstVisit(V source)
        {
            Dictionary<V, Color> color = new Dictionary<V, Color>();
            Queue<V> queue = new Queue<V>();

            foreach (var v in _vertexes)
            {
                color[v] = Color.White;
            }
            color[source] = Color.Gray;
            queue.Enqueue(source);

            while (queue.Count != 0)
            {
                V tmp = queue.Dequeue();
                foreach (var n in GetNeighbors(tmp))
                {
                    if (color[n] == Color.White)
                    {
                        color[n] = Color.Gray;
                        queue.Enqueue(n);
                    }
                }
                color[tmp] = Color.Black;
                yield return tmp;
            }
        }

        public IEnumerator<V> DepthFirstVisit(V source)
        {
            Dictionary<V, Color> color = new Dictionary<V, Color>();
            Stack<V> stack = new Stack<V>();

            foreach (var v in _vertexes)
            {
                color[v] = Color.White;
            }
            color[source] = Color.Black;
            stack.Push(source);
            while (stack.Count != 0)
            {
                V tmp = stack.Pop();
                if (color[tmp] == Color.White)
                {
                    color[tmp] = Color.Black;
                    yield return tmp;
                    foreach (var n in GetNeighbors(tmp))
                    {
                        stack.Push(n);
                    }
                }
            }
        }


        public ICollection<E> Dijkstra(V source)
        {
            Dictionary<V, uint> distance = new Dictionary<V, uint>();
            Dictionary<V, E> nextEdge = new Dictionary<V, E>();
            Func<V, V, int> compare = (x, y) => distance[x] > distance[y] ? -1 : 1;
            PriorityQueue<uint, V> pq = new PriorityQueue<uint, V>(); // <W,V>
            List<E> path = new List<E>();

            distance[source] = 0;
            foreach (var v in _vertexes)
            {
                if (!v.Equals(source))
                    distance[v] = uint.MaxValue;
                pq.Enqueue(distance[v], v);
            }

            while (!pq.IsEmpty)
            {
                V tmp = pq.Dequeue();
                if (distance[tmp] == uint.MaxValue)
                    break;
                foreach (var v in GetNeighbors(tmp))
                {
                    foreach (var e in GetEdges(tmp, v))
                    {
                        uint alt = distance[tmp] + e.GetDijWeight();
                        if (alt < distance[v])
                        {
                            pq.SafeChangePriority(distance[v], alt, v);
                            distance[v] = alt;
                            nextEdge[tmp] = e;
                        }
                    }
                }
            }
            return nextEdge.Values;
        }


        public int GetOutDegree(V vertex)
        {
            return _edges.Count(e => e.GetSource().Equals(vertex));
        }

        public int GetInDegree(V vertex)
        {
            return _edges.Count(e => e.GetDestination().Equals(vertex));
        }

        public ICollection<V> GetNeighbors(V vertex)
        {
            HashSet<V> neighbors = new HashSet<V>();
            foreach (var e in _edges.Where(e => e.GetSource().Equals(vertex)))
            {
                neighbors.Add(e.GetDestination());
            }
            return neighbors;
        }

        public ICollection<E> GetEdges(V vertex)
        {
            HashSet<E> edges = new HashSet<E>();

            foreach (var e in _edges.Where(e => e.GetSource().Equals(vertex)))
            {
                edges.Add(e);
            }
            return edges;
        }

        public ICollection<E> GetEdges(V src, V dst)
        {
            HashSet<E> edges = new HashSet<E>();

            foreach (var e in _edges.Where(e => e.GetSource().Equals(src) && e.GetDestination().Equals(dst)))
            {
                edges.Add(e);
            }
            return edges;
        }

    }
}
