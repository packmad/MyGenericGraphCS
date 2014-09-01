
namespace MyGenericGraph
{
    public abstract class AEdge<V, W> : IEdge<V>
    {
        public V Source { get; private set; }
        public V Destination { get; private set; }
        public int Weight { get; private set; }

        protected AEdge(V source, V destination, int weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }

        public override string ToString()
        {
            return "[" + Source + "---" + Weight + "--->" + Destination + "]";
        }
    }
}
