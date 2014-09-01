
namespace MyGenericGraph
{
    public interface IEdge<V>
    {
        V Source { get; }
        V Destination { get; }
        int Weight { get; }
    }
}
