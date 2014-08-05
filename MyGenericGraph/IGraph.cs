using System;
using System.Collections.Generic;
using System.Linq;

namespace MyGenericGraph
{
    public interface IGraph<V, E, W>
        where E : IEdge<V, W> 
        where W : IComparable<W>
    {
        void Add(V vertex);
        void Add(E edge);
        IEnumerator<V> BreadthFirstVisit(V source);
        IEnumerator<V> DepthFirstVisit(V source);
        ICollection<E> Dijkstra(V source);
        int GetOutDegree(V vertex);
        int GetInDegree(V vertex);
        ICollection<V> GetNeighbors(V vertex);
        ICollection<E> GetEdges(V vertex);
        ICollection<E> GetEdges(V src, V dst);
    }
}
