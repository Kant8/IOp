using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOpUtils;
using NGenerics.DataStructures.General;
using NUnit.Framework;
namespace IOpUtils.Tests
{
    [TestFixture()]
    public class PotentialMethodTests
    {
        [Test()]
        public void PotentialMethodTest()
        {
            var graph = new Graph<int>(true);

            var v = new List<Vertex<int>>(Enumerable.Repeat(new Vertex<int>(0), 7));
            for (int i = 1; i < v.Count; i++)
            {
                v[i] = new Vertex<int>(i - 1);
            }
            //var v1 = new Vertex<int>(1);
            //var v2 = new Vertex<int>(2);
            //var v3 = new Vertex<int>(3);
            //var v4 = new Vertex<int>(4);
            //var v5 = new Vertex<int>(5);
            //var v6 = new Vertex<int>(6);


            var e1 = new Edge<int>(v[1], v[2], 1, true);
            var e2 = new Edge<int>(v[6], v[1], -2, true);
            var e3 = new Edge<int>(v[2], v[6], 3, true);
            var e4 = new Edge<int>(v[6], v[3], 3, true);
            var e5 = new Edge<int>(v[6], v[5], 4, true);
            var e6 = new Edge<int>(v[3], v[2], 3, true);
            var e7 = new Edge<int>(v[5], v[3], -4, true);
            var e8 = new Edge<int>(v[5], v[4], 1, true);
            var e9 = new Edge<int>(v[3], v[4], 5, true);

            graph.AddVertex(v[1]); graph.AddVertex(v[2]); graph.AddVertex(v[3]);
            graph.AddVertex(v[4]); graph.AddVertex(v[5]); graph.AddVertex(v[6]);

            graph.AddEdge(e1); graph.AddEdge(e2); graph.AddEdge(e3);
            graph.AddEdge(e4); graph.AddEdge(e5); graph.AddEdge(e6);
            graph.AddEdge(e7); graph.AddEdge(e8); graph.AddEdge(e9);

            var startX = new Dictionary<Edge<int>, double>
            {
                {e1, 1}, {e2, 0}, {e3, 0},
                {e4, 9}, {e5, 0}, {e6, 3}, 
                {e7, 0}, {e8, 5}, {e9, 1}
            };

            var baseU = new List<Edge<int>>()
            {
                e1, e4, e6, e8, e9
            };


            var expectedResult = new Dictionary<Edge<int>, double>
            {
                {e1, 4}, {e2, 3}, {e3, 0},
                {e4, 5}, {e5, 1}, {e6, 0}, 
                {e7, 0}, {e8, 6}, {e9, 0}
            };

            var pm = new PotentialMethod(graph, startX, baseU);

            var actualResult = pm.Solve();


            Assert.AreEqual(startX, actualResult);
        }
    }
}
