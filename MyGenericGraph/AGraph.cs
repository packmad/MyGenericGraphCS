using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MyGenericGraph
{
    public interface IGraphCollectionBuilder<V, E> where E : IEdge<V>
    {
        IDictionary<V, ICollection<E>> GetDictionary();
        ICollection<E> GetNewEdgeSequence();
    }


    public abstract class AGraph<V, E> : IGraph<V, E> 
        where E : IEdge<V>
    {
        private readonly IGraphCollectionBuilder<V, E> _collectionBuilder;

        public ReadOnlyCollection<V> Vertexes
        {
            get { return new ReadOnlyCollection<V>(_vertexToNeighbors.Keys.ToList()); }
        }

        public ReadOnlyCollection<E> Edges
        {
            get
            {
                var llEdges = _vertexToNeighbors.Values;
                var retList = new List<E>();
                foreach (var llEdge in llEdges)
                {
                    retList.AddRange(llEdge);
                }
                return new ReadOnlyCollection<E>(retList);
            }
        }
        private readonly IDictionary<V, ICollection<E>> _vertexToNeighbors;
        //private readonly ICollection<E> _edges;

        protected AGraph(IGraphCollectionBuilder<V, E> collectionBuilder)
        {
            if (collectionBuilder == null)
                throw new ArgumentNullException();
            _collectionBuilder = collectionBuilder;
            _vertexToNeighbors = _collectionBuilder.GetDictionary();
            //_edges = _collectionBuilder.GetNewEdgeSequence();
        }

        private void AddSrcDst(E edge)
        {
            if (Equals(edge, default(E)))
                throw new ArgumentNullException();
            if (!_vertexToNeighbors.Keys.Contains(edge.Source))
                Add(edge.Source);
            if (!_vertexToNeighbors.Keys.Contains(edge.Destination))
                Add(edge.Destination);
        }


        public void Add(V vertex)
        {
            if (Equals(vertex, default(V)))
                throw new ArgumentNullException();
            if (!_vertexToNeighbors.Keys.Contains(vertex))
                _vertexToNeighbors.Add(vertex, _collectionBuilder.GetNewEdgeSequence());
        }

        public void Add(E edge)
        {
            if (Equals(edge, default(E)))
                throw new ArgumentNullException();
            if (!Contains(edge)) // no duplicates
            {
                AddSrcDst(edge);
                _vertexToNeighbors[edge.Source].Add(edge); //src -> neighbors
            }
        }

        public bool Remove(V vertex)
        {

            return _vertexToNeighbors.Remove(vertex);
        }

        public bool Remove(E edge)
        {
            return _vertexToNeighbors[edge.Source].Remove(edge);
        }

        public bool Contains(V vertex)
        {
            return _vertexToNeighbors.ContainsKey(vertex);
        }

        public bool Contains(E edge)
        {
            return Edges.Contains(edge);
        }


        public IEnumerable<V> BreadthFirstVisit(V source)
        {
            if (Equals(source, default(V)))
                throw new ArgumentNullException();

            Dictionary<V, Color> color = new Dictionary<V, Color>();
            Queue<V> queue = new Queue<V>();

            foreach (var v in Vertexes)
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

        public IEnumerable<V> DepthFirstVisit(V source)
        {
            if (Equals(source, default(V)))
                throw new ArgumentNullException();

            Dictionary<V, Color> color = new Dictionary<V, Color>();
            Stack<V> stack = new Stack<V>();

            foreach (var v in Vertexes)
            {
                color[v] = Color.White;
            }
            //color[source] = Color.Black;
            stack.Push(source);
            while (stack.Count != 0)
            {
                V tmp = stack.Pop();
                if (color[tmp] == Color.White)
                {
                    color[tmp] = Color.Black;
                    yield return tmp; // modifica???
                    foreach (var n in GetNeighbors(tmp))
                    {
                        stack.Push(n);
                    }
                }
            }
        }


        private void CheckNullBelong(V vertex)
        {
            if (Equals(vertex, default(V)))
                throw new ArgumentNullException();
            if (!Contains(vertex))
                throw new Exception("The vertex does not belong to the graph"); //TODO
        }

        public int GetOutDegree(V vertex)
        {
            CheckNullBelong(vertex);
            return _vertexToNeighbors[vertex].Count;
        }

        public int GetInDegree(V vertex)
        {
            CheckNullBelong(vertex);
            return Edges.Count(e => e.Destination.Equals(vertex));
        }

        public ReadOnlyCollection<V> GetNeighbors(V vertex)
        {
            CheckNullBelong(vertex);
            HashSet<V> neighbors = new HashSet<V>();
            foreach (var e in _vertexToNeighbors[vertex])
            {
                neighbors.Add(e.Destination);
            }
            return new ReadOnlyCollection<V>(neighbors.ToList());
        }

        public ReadOnlyCollection<E> GetEdges(V vertex)
        {
            CheckNullBelong(vertex);
            return new ReadOnlyCollection<E>(_vertexToNeighbors[vertex].ToList());
        }

        public ReadOnlyCollection<E> GetEdges(V src, V dst)
        {
            CheckNullBelong(src);
            CheckNullBelong(dst);
            HashSet<E> retEdges = new HashSet<E>();
            foreach (var e in _vertexToNeighbors[src].Where(e => e.Destination.Equals(dst)))
            {
                retEdges.Add(e);
            }
            return new ReadOnlyCollection<E>(retEdges.ToList());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var v in Vertexes)
            {
                sb.Append(v + "\n");
            }
            foreach (var e in Edges)
            {
                sb.Append(e + "\n");
            }
            return sb.ToString();
        }
    }
}
