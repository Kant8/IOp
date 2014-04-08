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
        public void PotentialMethodTest0()
        {
            var graph = new Graph<int>(true);

            var v = new List<Vertex<int>>(Enumerable.Repeat(new Vertex<int>(0), 7));
            for (int i = 1; i < v.Count; i++)
            {
                v[i] = new Vertex<int>(i - 1);
                graph.AddVertex(v[i]);
            }

            var e1 = new Edge<int>(v[1], v[2], 1, true);
            var e2 = new Edge<int>(v[6], v[1], -2, true);
            var e3 = new Edge<int>(v[2], v[6], 3, true);
            var e4 = new Edge<int>(v[6], v[3], 3, true);
            var e5 = new Edge<int>(v[6], v[5], 4, true); //-4 -> 4
            var e6 = new Edge<int>(v[3], v[2], 3, true);
            var e7 = new Edge<int>(v[5], v[3], 4, true);
            var e8 = new Edge<int>(v[5], v[4], 1, true);
            var e9 = new Edge<int>(v[3], v[4], 5, true);

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
            var actualCost = pm.ResultCost;

            Assert.AreEqual(23, actualCost);
            Assert.AreEqual(expectedResult, actualResult);

            //foreach (var x in expectedResult)
            //{
            //    Assert.True(actualResult.Contains(x));
            //}
        }

        [Test()]
        public void PotentialMethodTest1()
        {
            var graph = new Graph<int>(true);

            var v = new List<Vertex<int>>(Enumerable.Repeat(new Vertex<int>(0), 10));
            for (int i = 1; i < v.Count; i++)
            {
                v[i] = new Vertex<int>(i - 1);
                graph.AddVertex(v[i]);
            }

            var e1 = new Edge<int>(v[1], v[2], 9, true);
            var e2 = new Edge<int>(v[1], v[8], 5, true);
            var e3 = new Edge<int>(v[2], v[3], 1, true);
            var e4 = new Edge<int>(v[2], v[6], 3, true);
            var e5 = new Edge<int>(v[2], v[7], 5, true);
            var e6 = new Edge<int>(v[3], v[9], -2, true);
            var e7 = new Edge<int>(v[4], v[3], -3, true);
            var e8 = new Edge<int>(v[5], v[4], 6, true);
            var e9 = new Edge<int>(v[6], v[5], 8, true);
            var e10 = new Edge<int>(v[7], v[3], -1, true);
            var e11 = new Edge<int>(v[7], v[4], 4, true);
            var e12 = new Edge<int>(v[7], v[5], 7, true);
            var e13 = new Edge<int>(v[7], v[9], 1, true);
            var e14 = new Edge<int>(v[8], v[7], 2, true);
            var e15 = new Edge<int>(v[8], v[9], 2, true);
            var e16 = new Edge<int>(v[9], v[6], 6, true);

            graph.AddEdge(e1); graph.AddEdge(e2); graph.AddEdge(e3);
            graph.AddEdge(e4); graph.AddEdge(e5); graph.AddEdge(e6);
            graph.AddEdge(e7); graph.AddEdge(e8); graph.AddEdge(e9);

            var startX = new Dictionary<Edge<int>, double>
            {
                {e1, 2}, {e2, 7}, {e3, 4},
                {e4, 0}, {e5, 3}, {e6, 0}, 
                {e7, 0}, {e8, 3}, {e9, 4},
                {e10, 0}, {e11, 0}, {e12, 5},
                {e13, 0}, {e14, 0}, {e15, 0},
                {e16, 2}
            };

            var baseU = new List<Edge<int>>()
            {
                e1, e2, e3, e5, e8, e9, e12, e16
            };


            var expectedResult = new Dictionary<Edge<int>, double>
            {
                {e1, 4}, {e2, 3}, {e3, 0},
                {e4, 5}, {e5, 1}, {e6, 0}, 
                {e7, 0}, {e8, 6}, {e9, 0}
            };

            var pm = new PotentialMethod(graph, startX, baseU);
            var actualResult = pm.Solve();
            var actualCost = pm.ResultCost;

            Assert.AreEqual(127, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
