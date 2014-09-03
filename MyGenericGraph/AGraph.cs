using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MyGenericGraph
{
    public interface IGraphDataStructureFactory<V, E> where E : IEdge<V>
    {
        IDictionary<V, ICollection<E>> GetDictionary();
        ICollection<E> GetEdgeCollection();
    }


    public abstract class AGraph<V, E> : IGraph<V, E> where E : IEdge<V>
    {
        private readonly IGraphDataStructureFactory<V, E> _dataStructureFactory;
        private readonly IDictionary<V, ICollection<E>> _vertexToNeighbors;

        private object _footprint;
        private void CheckAccess(object localFootprint)
        {
            if (_footprint != localFootprint)
                throw new InvalidOperationException();
        }

        public ReadOnlyCollection<V> Vertexes
        {
            get { return new ReadOnlyCollection<V>(_vertexToNeighbors.Keys.ToList()); }
        }

        public ReadOnlyCollection<E> Edges
        {
            get
            {
                var allEdges = _vertexToNeighbors.Values;
                var returnList = new List<E>();
                foreach (var llEdge in allEdges)
                {
                    returnList.AddRange(llEdge);
                }
                return new ReadOnlyCollection<E>(returnList);
            }
        }

        protected AGraph(IGraphDataStructureFactory<V, E> dataStructureFactory)
        {
            if (dataStructureFactory == null)
                throw new ArgumentNullException();
            _dataStructureFactory = dataStructureFactory; // for getting new edges collection
            _vertexToNeighbors = _dataStructureFactory.GetDictionary();
        }


        private bool VertexIsNull(V vertex)
        {
            return EqualityComparer<V>.Default.Equals(vertex, default(V));
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
            if (VertexIsNull(vertex))
                throw new ArgumentNullException();
            if (!_vertexToNeighbors.Keys.Contains(vertex))
            {
                _footprint = new Object();
                _vertexToNeighbors.Add(vertex, _dataStructureFactory.GetEdgeCollection());
            }
        }

        public void Add(E edge)
        {
            if (Equals(edge, default(E)))
                throw new ArgumentNullException();
            if (!Contains(edge)) // no duplicates
            {
                _footprint = new Object();
                AddSrcDst(edge);
                _vertexToNeighbors[edge.Source].Add(edge); //src -> neighbors
            }
        }

        public bool Remove(V vertex)
        {
            if (VertexIsNull(vertex))
                throw new ArgumentNullException();
            _footprint = new Object();
            return _vertexToNeighbors.Remove(vertex);
        }

        public bool Remove(E edge)
        {
            if (Equals(edge, default(E)))
                throw new ArgumentNullException();
            _footprint = new Object();
            return _vertexToNeighbors[edge.Source].Remove(edge);
        }

        public bool Contains(V vertex)
        {
            if (VertexIsNull(vertex))
                throw new ArgumentNullException();
            return _vertexToNeighbors.ContainsKey(vertex);
        }

        public bool Contains(E edge)
        {
            if (Equals(edge, default(E)))
                throw new ArgumentNullException();
            return Edges.Contains(edge);
        }


        public IEnumerable<V> BreadthFirstVisit(V source)
        {
            if (VertexIsNull(source))
                throw new ArgumentNullException();
            object localFootprint = _footprint;

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
                CheckAccess(localFootprint);
                yield return tmp;
            }
        }

        public IEnumerable<V> DepthFirstVisit(V source)
        {
            if (VertexIsNull(source))
                throw new ArgumentNullException();
            object localFootprint = _footprint;

            Dictionary<V, Color> color = new Dictionary<V, Color>();
            Stack<V> stack = new Stack<V>();

            foreach (var v in Vertexes)
            {
                color[v] = Color.White;
            }
            stack.Push(source);
            while (stack.Count != 0)
            {
                V tmp = stack.Pop();
                if (color[tmp] == Color.White)
                {
                    color[tmp] = Color.Black;
                    CheckAccess(localFootprint);
                    yield return tmp;
                    foreach (var n in GetNeighbors(tmp))
                    {
                        stack.Push(n);
                    }
                }
            }
        }


        private void CheckNullBelong(V vertex)
        {
            if (VertexIsNull(vertex))
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
            sb.Append("Vertexes:\n");
            foreach (var v in Vertexes)
            {
                sb.Append("\t" + v + "\n");
            }
            sb.Append("Edges:\n");
            foreach (var e in Edges)
            {
                sb.Append("\t" + e + "\n");
            }
            return sb.ToString();
        }
    }
}
