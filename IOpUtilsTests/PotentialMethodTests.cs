using System.Collections.Generic;
using NGenerics.DataStructures.General;
using NUnit.Framework;
namespace IOpUtils.Tests
{
    [TestFixture]
    public class PotentialMethodTests
    {
        [Test]
        public void PotentialMethodTest0()
        {
            var data = new double[,]
            {
                {1, 2, 1, 1, 1, 4},
                {6, 1, -2, 0, 0, 3},
                {2, 6, 3, 0, 0, 0},
                {6, 3, 3, 9, 1, 5},
                {6, 5, 4, 0, 0, 1}, //-4 -> 4
                {3, 2, 3, 3, 1, 0},
                {5, 3, 4, 0, 0, 0},
                {5, 4, 1, 5, 1, 6},
                {3, 4, 5, 1, 1, 0}
            };
            Dictionary<Edge<int>, double> startX;
            List<Edge<int>> baseU;
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out startX, out baseU, out expectedResult);


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

        [Test]
        public void PotentialMethodTest1()
        {
            var data = new double[,]
            {
                {1, 2, 9, 2, 1, 0},
                {1, 8, 5, 7, 1, 9},
                {2, 3, 1, 4, 1, 4},
                {2, 6, 3, 0, 0, 1},
                {2, 7, 5, 3, 1, 0},
                {3, 9, -2, 0, 0, 0},
                {4, 3, -3, 0, 0, 0},
                {5, 4, 6, 3, 1, 0},
                {6, 5, 8, 4, 1, 5},
                {7, 3, -1, 0, 0, 0},
                {7, 4, 4, 0, 0, 3},
                {7, 5, 7, 5, 1, 1},
                {7, 9, 1, 0, 0, 0},
                {8, 7, 2, 0, 0, 2},
                {8, 9, 2, 0, 0, 0},
                {9, 6, 6, 2, 1, 2}
            };
            Dictionary<Edge<int>, double> startX;
            List<Edge<int>> baseU;
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out startX, out baseU, out expectedResult);

            var pm = new PotentialMethod(graph, startX, baseU);
            var actualResult = pm.Solve();
            var actualCost = pm.ResultCost;

            Assert.AreEqual(127, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void PotentialMethodTest2()
        {
            var data = new double[,]
            {
                {1, 2, 8, 5},
                {1, 8, 3, 0},
                {2, 3, 2, 0},
                {2, 7, 9, 0},
                {3, 6, 4, 0},
                {4, 3, -2, 0},
                {4, 6, 1, 0},
                {5, 4, 8, 6},
                {6, 5, 4, 5},
                {7, 3, 11, 1},
                {7, 5, 6, 2},
                {7, 6, 2, 0},
                {8, 6, 5, 11},
                {8, 7, 5, 0}
            };
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out expectedResult);

            List<int> production = new List<int> {5, -5, -1, -6, -1, -6, 3, 11};

            var pm = new PotentialMethod(graph, production);
            var actualResult = pm.Solve();
            var actualCost = pm.ResultCost;

            Assert.AreEqual(186, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void PotentialMethodTest3()
        {
            var data = new double[,]
            {
                {1, 2, 8, 5, 1, 2},
                {1, 8, 3, 4, 1, 7},
                {2, 3, 2, 0, 0, 0},
                {2, 7, 9, 3, 1, 0},
                {3, 6, 4, 0, 0, 0},
                {4, 3, -2, 0, 0, 4},
                {5, 4, -3, 6, 1, 10},
                {6, 5, 8, 7, 1, 4},
                {7, 3, 13, 4, 1, 0},
                {7, 4, 1, 0, 0, 0},
                {7, 5, 1, 0, 0, 7},
                {7, 6, 7, 3, 1, 0},
                {8, 6, -1, 0, 0, 0},
                {8, 7, 1, 0, 0, 3}
            };
            Dictionary<Edge<int>, double> startX;
            List<Edge<int>> baseU;
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out startX, out baseU, out expectedResult);

            var pm = new PotentialMethod(graph, startX, baseU);
            var actualResult = pm.Solve();
            var actualCost = pm.ResultCost;

            Assert.AreEqual(41, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void PotentialMethodTest4()
        {
            var data = new double[,]
            {
                {1, 2, 1, 0, 0, 3},
                {1, 7, 4, 3, 1, 0},
                {2, 3, 5, 2, 1, 5},
                {2, 5, 3, 0, 0, 0},
                {3, 4, 3, 0, 0, 0},
                {3, 6, 2, 0, 0, 0},
                {5, 3, 10, 4, 1, 1},
                {5, 4, 5, 7, 1, 0},
                {5, 6, 2, 0, 0, 8},
                {6, 4, 1, 0, 0, 7},
                {7, 5, 8, 2, 1, 0},
                {7, 6, 6, 5, 1, 4}
            };
            Dictionary<Edge<int>, double> startX;
            List<Edge<int>> baseU;
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out startX, out baseU, out expectedResult);

            var pm = new PotentialMethod(graph, startX, baseU);
            var actualResult = pm.Solve();
            var actualCost = pm.ResultCost;

            Assert.AreEqual(85, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void PotentialMethodTest5()
        {
            var data = new double[,]
            {
                {1, 2, 3, 0, 0, 1},
                {1, 5, 5, 2, 1, 0},
                {1, 6, 4, 4, 1, 0},
                {1, 7, 2, 0, 0, 5},
                {2, 3, 5, 3, 1, 0},
                {2, 5, -1, 0, 0, 5},
                {2, 7, 7, 1, 1, 0},
                {3, 4, 6, 2, 1, 0},
                {3, 7, 1, 0, 0, 0},
                {4, 6, 1, 0, 0, 0},
                {5, 3, 2, 0, 0, 1},
                {5, 4, -2, 0, 0, 2},
                {5, 6, 1, 0, 0, 0},
                {6, 7, 7, 5, 1, 1}
            };
            Dictionary<Edge<int>, double> startX;
            List<Edge<int>> baseU;
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out startX, out baseU, out expectedResult);

            var pm = new PotentialMethod(graph, startX, baseU);
            var actualResult = pm.Solve();
            var actualCost = pm.ResultCost;

            Assert.AreEqual(13, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void PotentialMethodTest6()
        {
            var data = new double[,]
            {
                {1, 2, 6, 2, 1, 0},
                {1, 6, 2, 0, 0, 0},
                {1, 7, -2, 0, 0, 2},
                {2, 3, 3, 0, 0, 0},
                {2, 4, 6, 2, 1, 2},
                {2, 6, 1, 0, 0, 0},
                {3, 5, 3, 0, 0, 0},
                {3, 6, 4, 6, 1, 6},
                {4, 3, -1, 0, 0, 0},
                {4, 8, 1, 0, 0, 0},
                {5, 8, 7, 5, 1, 2},
                {6, 5, 5, 3, 1, 0},
                {6, 7, 5, 3, 1, 3},
                {6, 8, 3, 0, 0, 3},
                {7, 2, 4, 4, 1, 6},
                {7, 5, 2, 0, 0, 0},
                {7, 8, 2, 0, 0, 0}
            };
            Dictionary<Edge<int>, double> startX;
            List<Edge<int>> baseU;
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out startX, out baseU, out expectedResult);

            var pm = new PotentialMethod(graph, startX, baseU);
            var actualResult = pm.Solve();
            var actualCost = pm.ResultCost;

            Assert.AreEqual(94, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void PotentialMethodTest7()
        {
            var data = new double[,]
            {
                {1, 2, 7, 2, 1, 2},
                {1, 3, 6, 3, 1, 0},
                {1, 5, 3, 0, 0, 8},
                {2, 3, 4, 0, 0, 0},
                {2, 6, 3, 0, 0, 0},
                {3, 4, 6, 4, 1, 4},
                {3, 5, 5, 4, 1, 1},
                {4, 6, 1, 0, 0, 0},
                {5, 4, 4, 0, 0, 0},
                {5, 6, -1, 0, 0, 0},
                {6, 7, 4, 2, 1, 2},
                {7, 1, 2, 0, 0, 5},
                {7, 5, 7, 5, 1, 0}
            };
            Dictionary<Edge<int>, double> startX;
            List<Edge<int>> baseU;
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out startX, out baseU, out expectedResult);

            var pm = new PotentialMethod(graph, startX, baseU);
            var actualResult = pm.Solve();
            var actualCost = pm.ResultCost;

            Assert.AreEqual(85, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void PotentialMethodTest8()
        {
            var data = new double[,]
            {
                {1, 2, 5, 4, 1, 0},
                {1, 5, 1, 0, 0, 3},
                {1, 6, 5, 2, 1, 3},
                {2, 4, 10, 5, 1, 0},
                {2, 6, 3, 0, 0, 1},
                {3, 1, 1, 0, 0, 0},
                {3, 2, 3, 0, 0, 0},
                {3, 4, 6, 1, 1, 1},
                {3, 5, 2, 0, 0, 0},
                {4, 5, 3, 0, 0, 0},
                {6, 4, 2, 0, 0, 5},
                {6, 5, 4, 3, 1, 0}
            };
            Dictionary<Edge<int>, double> startX;
            List<Edge<int>> baseU;
            Dictionary<Edge<int>, double> expectedResult;
            var graph = GraphHelper.CreateGraph(data, out startX, out baseU, out expectedResult);

            var pm = new PotentialMethod(graph, startX, baseU);
            var actualResult = pm.Solve();
            var actualCost = pm.ResultCost;

            Assert.AreEqual(37, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
