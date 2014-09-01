using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGenericGraph;


namespace MyGenericGraphTests
{
    [TestClass()]
    public class AGraphTests
    {

        void PrintCollectionToConsole(IEnumerable<Place> collection)
        {
            foreach (var place in collection)
            {
                Console.WriteLine(place);
            }
        }

        internal class Place
        {
            public string Name { get; private set;}

            protected Place(string name)
            {
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        internal class City : Place
        {
            internal City(string name) : base(name) { }
        }

        internal class Town : Place
        {
            internal Town(string name) : base(name) { }
        }

        internal class Edge : AEdge<Place, int>
        {
            public Edge(Place source, Place destination, int weight) :
                base(source, destination, weight)
            {
            }
        }

        private class GraphCollectionBuilder : IGraphCollectionBuilder<Place, Edge>
        {
            public IDictionary<Place, ICollection<Edge>> GetDictionary()
            {
                return new Dictionary<Place, ICollection<Edge>>();
            }

            public ICollection<Edge> GetNewEdgeSequence()
            {
                return new List<Edge>();
            }
        }

        private class Graph : AGraph<Place, Edge>
        {
            public Graph(IGraphCollectionBuilder<Place, Edge> collectionBuilder) : base(collectionBuilder)
            {
            }
        }

//-------------------------------------------------------------------------------------------------------------------------------------------------------------

        private static Graph _graph;
        private static readonly City C1 = new City("C1");
        private static readonly City C2 = new City("C2");
        private static readonly City C3 = new City("C3");

        private static readonly Town T1 = new Town("T1");
        private static readonly Town T2 = new Town("T2");
        private static readonly Town T3 = new Town("T3");
        
        private static readonly City start = new City("START");
        private static readonly Town a = new Town("A");
        private static readonly Town b = new Town("B");
        private static readonly Town c = new Town("C");
        private static readonly Town d = new Town("D");
        private static readonly Town e = new Town("E");
        private static readonly Town f = new Town("F");
        private static readonly City end = new City("END");

        [TestInitialize()]
        public void Initialize()
        {
            var edges4Add = new List<Edge>
            {
                new Edge(start, a, 2),
                new Edge(start, d, 8),
                new Edge(a, b, 6),
                new Edge(a, c, 2),
                new Edge(c, d, 2),
                new Edge(d, e, 3),
                new Edge(c, e, 9),
                new Edge(e, end, 1),
                new Edge(b, end, 5),
            };
            _graph = new Graph(new GraphCollectionBuilder());
            foreach (var edge in edges4Add)
                _graph.Add(edge);
        }

        [TestMethod()]
        public void AGraphTest()
        {
            Console.WriteLine(_graph);
            Assert.IsNotNull(_graph);
        }

        [TestMethod()]
        public void AddVertexTest()
        {
            var vTest = new Town("AddVertexTest");
            _graph.Add(vTest);
            Assert.IsTrue(_graph.Vertexes.Contains(vTest));
        }

        [TestMethod()]
        public void AddEdgeTest()
        {
            Edge edge = new Edge(C1, C2, 42);
            _graph.Add(edge);
            Assert.IsTrue(_graph.Edges.Contains(edge));
            Assert.IsTrue(_graph.Vertexes.Contains(C1));
            Assert.IsTrue(_graph.Vertexes.Contains(C2));
        }

        [TestMethod()]
        public void RemoveVertexTest()
        {
            var vTest = new Town("AddVertexTest");
            _graph.Add(vTest);
            Assert.IsTrue(_graph.Vertexes.Contains(vTest));
            _graph.Remove(vTest);
            Assert.IsFalse(_graph.Vertexes.Contains(vTest));
        }

        [TestMethod()]
        public void RemoveEdgeTest()
        {
            var eTest = new Edge(new Town("ContainsEdgeTest"), new City("ContainsEdgeTest"), 42);
            _graph.Add(eTest);
            Assert.IsTrue(_graph.Contains(eTest));
            _graph.Remove(eTest);
            Assert.IsFalse(_graph.Contains(eTest));
        }

        [TestMethod()]
        public void ContainsVertexTest()
        {
            var vTest = new Town("ContainsVertexTest");
            _graph.Add(vTest);
            Assert.IsTrue(_graph.Contains(vTest));
        }

        [TestMethod()]
        public void ContainsEdgeTest()
        {
            var eTest = new Edge(new Town("ContainsEdgeTest"), new City("ContainsEdgeTest"), 42);
            _graph.Add(eTest);
            Assert.IsTrue(_graph.Contains(eTest));
        }

        [TestMethod()]
        public void BreadthFirstVisitTest()
        {
            var deeper = new City("deeper");
            _graph.Add(new Edge(end, deeper, 3));
            List<Place> ans = new List<Place>
            {
                start, a, d, b, c, e, end, deeper
            };

            IList<Place> visit = _graph.BreadthFirstVisit(start).ToList();
            PrintCollectionToConsole(visit);
            Assert.IsTrue(visit.SequenceEqual(ans));
        }

        [TestMethod()]
        public void DepthFirstVisitTest()
        {
            var deeper = new City("deeper");
            _graph.Add(new Edge(end, deeper, 3));
            List<Place> ans = new List<Place>
            {
                start, d, e, end, deeper, a, c, b
            };

            IList<Place> visit = _graph.DepthFirstVisit(start).ToList();
            PrintCollectionToConsole(visit);
            Assert.IsTrue(visit.SequenceEqual(ans));
        }



        [TestMethod()]
        public void GetOutDegreeTest()
        {
            _graph.Add(C1);
            int outd = _graph.GetOutDegree(C1);
            Assert.AreEqual(outd, 0);
            _graph.Add(new Edge(C1, C3, 9));
            Assert.AreEqual(_graph.GetOutDegree(C1), 1);
            _graph.Add(new Edge(C1, T1, 9));
            Assert.AreEqual(_graph.GetOutDegree(C1), 2);
            _graph.Add(new Edge(C1, T2, 9));
            Assert.AreEqual(_graph.GetOutDegree(C1), 3);
        }

        [TestMethod()]
        public void GetInDegreeTest()
        {
            _graph.Add(C1);
            Assert.AreEqual(_graph.GetInDegree(C1), 0);
            _graph.Add(new Edge(C3, C1, 9));
            Assert.AreEqual(_graph.GetInDegree(C1), 1);
            _graph.Add(new Edge(T1, C1, 9));
            Assert.AreEqual(_graph.GetInDegree(C1), 2);
            _graph.Add(new Edge(T2, C1, 9));
            Assert.AreEqual(_graph.GetInDegree(C1), 3);
        }

        [TestMethod()]
        public void GetNeighborsTest()
        {
            _graph.Add(C1);
            Assert.AreEqual(_graph.GetNeighbors(C1).Count, 0);
            _graph.Add(new Edge(C1, C3, 9));
            Assert.IsTrue(_graph.GetNeighbors(C1).Contains(C3));
            _graph.Add(new Edge(C1, T1, 9));
            Assert.IsTrue(_graph.GetNeighbors(C1).Contains(T1));
            _graph.Add(new Edge(C1, T2, 9));
            Assert.IsTrue(_graph.GetNeighbors(C1).Contains(T2));
            Console.WriteLine(_graph.GetNeighbors(C1));
        }

        [TestMethod()]
        public void GetEdgesFromVertexTest()
        {
            var e1 = new Edge(C1, C3, 9);
            var e2 = new Edge(C1, T1, 9);
            var e3 = new Edge(C1, T2, 9);
            var assertEdges = new List<Edge> {e1, e2, e3};
            _graph.Add(e1);
            _graph.Add(e2);
            _graph.Add(e3);
            var gettedEdge = _graph.GetEdges(C1);
            Assert.IsTrue(gettedEdge.Count == assertEdges.Count);
            foreach (var edge in gettedEdge)
            {
                Assert.IsTrue(assertEdges.Contains(edge));
            }
        }

        [TestMethod()]
        public void GetEdgesFromSrcDstTest()
        {
            var e1 = new Edge(C1, C3, 9);
            var e2 = new Edge(C1, C3, 7);
            var assertEdges = new List<Edge> { e1, e2};
            _graph.Add(e1);
            _graph.Add(e2);
            var gettedEdge = _graph.GetEdges(C1, C3);
            Assert.IsTrue(gettedEdge.Count == assertEdges.Count);
            foreach (var edge in gettedEdge)
            {
                Assert.IsTrue(assertEdges.Contains(edge));
            }
        }

        [TestMethod()]
        public void DijkstraAlgorithmTest()
        {
            DijkstraAlgorithm<Place, Edge> dijkstraAlgorithm = new DijkstraAlgorithm<Place, Edge>(_graph, start);
            Assert.IsNotNull(dijkstraAlgorithm);
            foreach (var edge in dijkstraAlgorithm.Paths)
            {
                Console.WriteLine(edge);
            }
            Console.WriteLine("\n\n");
            foreach (var edge in dijkstraAlgorithm.GetPathToDestination(end))
            {
                Console.WriteLine(edge);
            }
            Assert.Fail();
        }
    }
}
