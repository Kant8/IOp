﻿using System.Collections.Generic;
using NGenerics.DataStructures.General;
using NUnit.Framework;

namespace IOpUtils.Tests
{
    [TestFixture]
    public class FordFulkersonMethodTests
    {
		[Test]
        public void FordFulkersonMethodTest00()
        {
            var data = new double[,]
            {
                {1, 2, 4, 4, 0, 4},
                {1, 4, 9, 5, 0, 6},
                {2, 4, 2, 2, 0, 1},
                {2, 5, 4, 2, 0, 3},
                {3, 5, 1, 1, 0, 0}, 
                {3, 6, 10, 0, 0, 1},
                {4, 3, 1, 1, 0, 1},
                {4, 6, 6, 6, 0, 6},
                {5, 6, 1, 1, 0, 1},
                {5, 7, 2, 2, 0, 2},
                {6, 7, 9, 7, 0, 8}
            };
            Dictionary<Edge<int>, double> startX;
            List<Edge<int>> baseU;
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out startX, out baseU, out expectedResult);
            var startVertex = graph.GetVertex(0);
            var endVertex = graph.GetVertex(graph.Vertices.Count - 1);
            const double startCost = 9.0;


            var ffm = new FordFulkersonMethod(graph, startVertex, endVertex, startX, startCost);
            var actualResult = ffm.Solve();
            var actualCost = ffm.ResultCost;

            Assert.AreEqual(10, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void FordFulkersonMethodTest01()
        {
            var data = new double[,]
            {
                {1, 2, 12, 2},
                {1, 6, 6, 6},
                {2, 3, 2, 2},
                {3, 4, 1, 1},
                {3, 5, 5, 2}, 
                {5, 4, 15, 7},
                {5, 7, 2, 0},
                {6, 2, 10, 0},
                {6, 5, 5, 5},
                {6, 7, 8, 1},
                {7, 2, 2, 0},
                {7, 3, 6, 1}
            };
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out expectedResult);
            var startVertex = graph.GetVertex(0);
            var endVertex = graph.GetVertex(3);


            var ffm = new FordFulkersonMethod(graph, startVertex, endVertex);
            var actualResult = ffm.Solve();
            var actualCost = ffm.ResultCost;

            Assert.AreEqual(8, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void FordFulkersonMethodTest1()
        {
            var data = new double[,]
            {
                {1, 2, 3, 3},
                {1, 3, 2, 2},
                {1, 4, 1, 1},
                {1, 6, 6, 4},
                {2, 4, 1, 1},
                {2, 5, 2, 2}, 
                {3, 4, 1, 0},
                {3, 5, 2, 2},
                {3, 6, 4, 0},
                {4, 5, 7, 0},
                {4, 6, 5, 0},
                {4, 7, 4, 2},
                {4, 8, 1, 0},
                {5, 7, 3, 2},
                {5, 8, 2, 2},
                {6, 8, 4, 4},
                {7, 6, 3, 0},
                {7, 8, 5, 4}
            };
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out expectedResult);
            var startVertex = graph.GetVertex(0);
            var endVertex = graph.GetVertex(graph.Vertices.Count - 1);


            var ffm = new FordFulkersonMethod(graph, startVertex, endVertex);
            /*var actualResult = */ffm.Solve();
            var actualCost = ffm.ResultCost;

            Assert.AreEqual(10, actualCost);
            //Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void FordFulkersonMethodTest2()
        {
            var data = new double[,]
            {
                {1, 2, 4, 4},
                {1, 3, 1, 1},
                {1, 5, 1, 1},
                {1, 6, 5, 5},
                {1, 7, 2, 2},
                {2, 4, 1, 1}, 
                {2, 6, 6, 3},
                {3, 5, 2, 1},
                {4, 3, 6, 0},
                {4, 7, 3, 1},
                {5, 6, 4, 1},
                {5, 8, 3, 1},
                {6, 7, 1, 0},
                {6, 8, 3, 3},
                {6, 9, 6, 6},
                {7, 8, 4, 0},
                {7, 9, 5, 3},
                {8, 9, 4, 4}
            };
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out expectedResult);
            var startVertex = graph.GetVertex(0);
            var endVertex = graph.GetVertex(graph.Vertices.Count - 1);


            var ffm = new FordFulkersonMethod(graph, startVertex, endVertex);
            /*var actualResult = */ffm.Solve();
            var actualCost = ffm.ResultCost;

            Assert.AreEqual(13, actualCost);
            //Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void FordFulkersonMethodTest3()
        {
            var data = new double[,]
            {
                {1, 2, 2, 2},
                {1, 3, 1, 1},
                {1, 5, 2, 2},
                {2, 4, 2, 2},
                {2, 5, 1, 0},
                {3, 5, 3, 0}, 
                {3, 6, 2, 1},
                {4, 7, 1, 0},
                {4, 8, 2, 2},
                {5, 6, 1, 0},
                {5, 7, 4, 2},
                {5, 8, 3, 0},
                {5, 9, 3, 0},
                {6, 8, 2, 0},
                {6, 9, 2, 1},
                {7, 8, 5, 0},
                {7, 10, 3, 2},
                {8, 9, 1, 0},
                {8, 10, 4, 2},
                {9, 10, 3, 0}
            };
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out expectedResult);
            var startVertex = graph.GetVertex(0);
            var endVertex = graph.GetVertex(graph.Vertices.Count - 1);


            var ffm = new FordFulkersonMethod(graph, startVertex, endVertex);
            /*var actualResult = */ffm.Solve();
            var actualCost = ffm.ResultCost;

            Assert.AreEqual(5, actualCost); 
            //Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void FordFulkersonMethodTest4()
        {
            var data = new double[,]
            {
                {1, 2, 3, 2},
                {1, 3, 6, 3},
                {1, 4, 3, 3},
                {1, 5, 2, 0},
                {2, 3, 4, 2},
                {2, 4, 1, 0}, 
                {2, 5, 4, 0},
                {3, 7, 3, 3},
                {3, 8, 2, 2},
                {4, 3, 1, 0},
                {4, 7, 1, 1},
                {4, 8, 2, 2},
                {5, 6, 1, 0},
                {6, 4, 3, 0},
                {6, 7, 1, 0},
                {7, 8, 4, 4}
            };
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out expectedResult);
            var startVertex = graph.GetVertex(0);
            var endVertex = graph.GetVertex(graph.Vertices.Count - 1);


            var ffm = new FordFulkersonMethod(graph, startVertex, endVertex);
            /*var actualResult = */ffm.Solve();
            var actualCost = ffm.ResultCost;

            Assert.AreEqual(8, actualCost);
            //Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void FordFulkersonMethodTest5()
        {
            var data = new double[,]
            {
                {1, 2, 0, 2},
                {1, 4, 5, 5},
                {1, 6, 1, 3},
                {2, 4, 0, 0},
                {3, 2, 4, 0},
                {3, 5, 1, 1}, 
                {3, 8, 3, 0},
                {3, 9, 1, 1},
                {4, 3, 2, 2},
                {4, 5, 2, 2},
                {4, 6, 4, 0},
                {4, 7, 5, 1},
                {5, 2, 4, 0},
                {5, 6, 0, 0},
                {5, 9, 5, 5},
                {6, 7, 1, 1},
                {7, 5, 1, 1},
                {7, 8, 4, 1},
                {8, 5, 3, 1},
                {9, 8, 2, 0}
            };
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out expectedResult);
            var startVertex = graph.GetVertex(0);
            var endVertex = graph.GetVertex(graph.Vertices.Count - 1);


            var ffm = new FordFulkersonMethod(graph, startVertex, endVertex);
            /*var actualResult = */ffm.Solve();
            var actualCost = ffm.ResultCost;

            Assert.AreEqual(6, actualCost);
            //Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void FordFulkersonMethodTest6()
        {
            var data = new double[,]
            {
                {1, 4, 3, 3},
                {1, 5, 6, 2},
                {2, 1, 1, 0},
                {2, 4, 4, 0},
                {2, 5, 7, 0},
                {2, 7, 1, 0}, 
                {3, 1, 5, 0},
                {3, 2, 5, 0},
                {4, 3, 1, 0},
                {4, 5, 2, 0},
                {4, 6, 2, 0},
                {4, 7, 1, 0},
                {4, 8, 3, 3},
                {5, 6, 4, 0},
                {5, 7, 3, 2},
                {6, 3, 5, 0},
                {6, 8, 2, 2},
                {7, 6, 7, 2}
            };
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out expectedResult);
            var startVertex = graph.GetVertex(0);
            var endVertex = graph.GetVertex(graph.Vertices.Count - 1);


            var ffm = new FordFulkersonMethod(graph, startVertex, endVertex);
            /*var actualResult = */ffm.Solve();
            var actualCost = ffm.ResultCost;

            Assert.AreEqual(5, actualCost);
            //Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void FordFulkersonMethodTest7()
        {
            var data = new double[,]
            {
                {1, 5, 4, 1},
                {1, 6, 6, 6},
                {1, 7, 2, 0},
                {2, 1, 3, 0},
                {2, 3, 2, 0},
                {2, 5, 7, 0}, 
                {4, 2, 3, 0},
                {4, 3, 5, 0},
                {4, 5, 2, 0},
                {4, 7, 4, 0},
                {4, 9, 2, 0},
                {5, 6, 1, 1},
                {6, 9, 7, 7},
                {7, 6, 1, 0},
                {8, 4, 6, 0},
                {8, 7, 4, 0},
                {8, 9, 1, 0}
            };
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out expectedResult);
            var startVertex = graph.GetVertex(0);
            var endVertex = graph.GetVertex(graph.Vertices.Count - 1);


            var ffm = new FordFulkersonMethod(graph, startVertex, endVertex);
            var actualResult = ffm.Solve();
            var actualCost = ffm.ResultCost;

            Assert.AreEqual(7, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void FordFulkersonMethodTest8()
        {
            var data = new double[,]
            {
                {1, 2, 3, 3},
                {1, 3, 3, 3},
                {1, 6, 2, 2},
                {1, 7, 1, 1},
                {2, 3, 6, 3},
                {3, 4, 1, 1}, 
                {3, 5, 2, 0},
                {3, 6, 4, 3},
                {3, 7, 4, 2},
                {4, 5, 7, 1},
                {4, 10, 4, 4},
                {5, 7, 1, 0},
                {5, 9, 1, 0},
                {5, 10, 1, 1},
                {6, 4, 4, 4},
                {6, 5, 7, 1},
                {7, 10, 3, 3},
                {8, 1, 2, 0},
                {8, 2, 4, 0},
                {8, 6, 5, 0},
                {8, 7, 6, 0},
                {8, 9, 2, 0},
                {9, 10, 5, 1}
            };
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out expectedResult);
            var startVertex = graph.GetVertex(0);
            var endVertex = graph.GetVertex(graph.Vertices.Count - 1);


            var ffm = new FordFulkersonMethod(graph, startVertex, endVertex);
            /*var actualResult = */ffm.Solve();
            var actualCost = ffm.ResultCost;

            Assert.AreEqual(9, actualCost);
            //Assert.AreEqual(expectedResult, actualResult);
        }
    }
}