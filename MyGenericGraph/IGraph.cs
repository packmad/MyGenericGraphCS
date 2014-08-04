using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGenericGraph
{
    public enum Color
    {
        White,
        Gray,
        Black
    }


    public interface IGraph<V, E, W>
        where E : IEdge<V, W> 
        where W : IComparable<W>
    {
        void Add(V vertex);
        void Add(E edge);
        IEnumerator<V> BreadthFirstVisit(V source);
        IEnumerator<V> DepthFirstVisit(V source);
        ICollection<V> Dijkstra(V source);
        int GetOutDegree(V vertex);
        int GetInDegree(V vertex);
        ICollection<V> GetNeighbors(V vertex);
        ICollection<E> GetEdges(V vertex);
        ICollection<E> GetEdges(V src, V dst);
    }


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
			    foreach (var n in GetNeighbors(tmp)) {
				    if (color[n] == Color.White) {
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


        public ICollection<V> Dijkstra(V source)
        {
            Dictionary<V, uint> distance = new Dictionary<V, uint>();
            distance[source] = 0;
            foreach (var v in _vertexes)
            {
                if (!v.Equals(source))
                    distance[v] = uint.MaxValue;

            }
            

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
