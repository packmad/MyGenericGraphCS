using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace MyGenericGraph
{
    public interface IGraph<V, E> where E : IEdge<V>
    {
        void Add(V vertex);
        void Add(E edge);
        ICollection<V> BreadthFirstVisit(V source);
        ICollection<V> DepthFirstVisit(V source);
        ICollection<V> Dijkstra(V source);
        
        int GetOutDegree(V vertex);
        int GetInDegree(V vertex);
        ICollection<V> GetNeighbors(V vertex);
        ICollection<E> GetEdges(V vertex);
        E GetEdge(V src, V dst);
    }

    public abstract class AGraph<V, E> : IGraph<V, E> where E : IEdge<V>
    {
        private readonly ICollection<V> _vertexes;
        private readonly ICollection<E> _edges;

        public AGraph(ICollection<V> vertexes, ICollection<E> edges) //protected abstract?
        {
            this._vertexes = vertexes;
            this._edges = edges;
        }

        public void Add(V vertex)
        {
            _vertexes.Add(vertex);
        }

        public void Add(E edge)
        {
            _edges.Add(edge);
        }

        public ICollection<V> BreadthFirstVisit(V source)
        {
            throw new NotImplementedException();
        }

        public ICollection<V> DepthFirstVisit(V source)
        {
            throw new NotImplementedException();
        }

        public ICollection<V> Dijkstra(V source)
        {
            throw new NotImplementedException();
        }

        public E GetEdge(V src, V dst)
        {
            foreach (var e in _edges)
            {
                if (e.GetSource().Equals(src) && e.GetDestination().Equals(dst))
                    return e;
            }
            throw new NotImplementedException(); // null?
        }

        public int GetOutDegree(V vertex)
        {
            return GetNeighbors(vertex).Count; //problema dupicati
        }

        public int GetInDegree(V vertex)
        {
            throw new NotImplementedException();
        }

        public ICollection<V> GetNeighbors(V vertex)
        {
            ICollection<V> neighbors = new Collection<V>(); //senza dupicati?
            foreach (var e in _edges)
            {
                if (e.GetSource().Equals(vertex))
                    neighbors.Add(e.GetDestination());
            }
            return neighbors;
        }

        public ICollection<E> GetEdges(V vertex)
        {
            throw new NotImplementedException();
        }


    }
}
