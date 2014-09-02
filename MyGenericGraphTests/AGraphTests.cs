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
        
        private static readonly City Start = new City("START");
        private static readonly Town A = new Town("A");
        private static readonly Town B = new Town("B");
        private static readonly Town C = new Town("C");
        private static readonly Town D = new Town("D");
        private static readonly Town E = new Town("E");
        private static readonly City End = new City("END");

        private static readonly Edge Edge1 = new Edge(Start, A, 2);
        private static readonly Edge Edge2 = new Edge(Start, D, 8);
        private static readonly Edge Edge3 = new Edge(A, B, 6);
        private static readonly Edge Edge4 = new Edge(A, C, 2);
        private static readonly Edge Edge5 = new Edge(C, D, 2);
        private static readonly Edge Edge6 = new Edge(D, E, 3);
        private static readonly Edge Edge7 = new Edge(C, E, 9);
        private static readonly Edge Edge8 = new Edge(E, End, 1);
        private static readonly Edge Edge9 = new Edge(B, End, 5);
        

        [TestInitialize()]
        public void Initialize()
        {
            var edges4Add = new List<Edge>
            {
                Edge1,
                Edge2,
                Edge3,
                Edge4,
                Edge5,
                Edge6,
                Edge7,
                Edge8,
                Edge9,
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
            _graph.Add(new Edge(End, deeper, 3));
            List<Place> ans = new List<Place>
            {
                Start, A, D, B, C, E, End, deeper
            };

            IList<Place> visit = _graph.BreadthFirstVisit(Start).ToList();
            PrintCollectionToConsole(visit);
            Assert.IsTrue(visit.SequenceEqual(ans));
        }

        [TestMethod()]
        public void DepthFirstVisitTest()
        {
            var deeper = new City("deeper");
            _graph.Add(new Edge(End, deeper, 3));
            List<Place> ans = new List<Place>
            {
                Start, D, E, End, deeper, A, C, B
            };

            IList<Place> visit = _graph.DepthFirstVisit(Start).ToList();
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
            List<Edge> ansPaths;
            IReadOnlyCollection<Edge> paths;
            DijkstraAlgorithm<Place, Edge> dijkstraAlgorithm = new DijkstraAlgorithm<Place, Edge>(_graph, Start);
            Assert.IsNotNull(dijkstraAlgorithm);

            paths = dijkstraAlgorithm.Paths;
            ansPaths = new List<Edge> { Edge1, Edge5, Edge3, Edge4, Edge6, Edge8 };
            foreach (var edge in paths)
            {
                Console.WriteLine(edge);
            }
            Assert.IsTrue(paths.SequenceEqual(ansPaths));

            Console.WriteLine("\n\n");

            paths = dijkstraAlgorithm.GetPathToDestination(End);
            ansPaths = new List<Edge> { Edge1, Edge4, Edge5, Edge6, Edge8 };
            foreach (var edge in paths)
            {
                Console.WriteLine(edge);
            }
            Assert.IsTrue(paths.SequenceEqual(ansPaths));
        }
    }
}
