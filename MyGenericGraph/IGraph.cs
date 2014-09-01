using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyGenericGraph
{
    public interface IGraph<V, E> where E : IEdge<V> 
    {
        ReadOnlyCollection<V> Vertexes { get; }
        ReadOnlyCollection<E> Edges { get; }
        void Add(V vertex);
        void Add(E edge);
        bool Remove(V vertex);
        bool Remove(E edge);
        bool Contains(V vertex);
        bool Contains(E edge);
        int GetOutDegree(V vertex);
        int GetInDegree(V vertex);
        ReadOnlyCollection<V> GetNeighbors(V vertex);
        ReadOnlyCollection<E> GetEdges(V vertex);
        ReadOnlyCollection<E> GetEdges(V src, V dst);
        IEnumerable<V> BreadthFirstVisit(V source);
        IEnumerable<V> DepthFirstVisit(V source);
    }
}
