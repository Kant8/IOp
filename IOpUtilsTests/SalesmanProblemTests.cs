using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
namespace IOpUtils.Tests
{
    [TestFixture]
    public class SalesmanProblemTests
    {
        private const double Inf = Double.PositiveInfinity;
        [Test]
        public void SalesmanProblemTest0()
        {
            var costs = DenseMatrix.OfArray(new[,]
            {
                {Inf, 2, 1, 10, 6},
                {4, Inf, 3, 1, 3},
                {2, 5, Inf, 8, 4},
                {6, 7, 13, Inf, 3},
                {10, 2, 4, 6, Inf}
            });

            var expectedResult = BuildExpectedResult((new[] {3, 1, 2, 4, 5, 3}).ToList());

            var sp = new SalesmanProblem(costs, 26, new List<int> {0, 1, 2, 3, 4, 0});
            var actualResult = sp.Solve();
            var actualCost = sp.OptimalCost;

            Assert.AreEqual(12, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void SalesmanProblemTest1()
        {
            var costs = DenseMatrix.OfArray(new[,]
            {
                {Inf, 10, 25, 25, 10},
                {1, Inf, 10, 15, 2},
                {8, 9, Inf, 20, 10},
                {14, 10, 24, Inf, 15},
                {10, 8, 25, 27, Inf}
            });

            var expectedResult = BuildExpectedResult((new[] { 1, 5, 2, 3, 4, 1 }).ToList());

            var sp = new SalesmanProblem(costs);
            var actualResult = sp.Solve();
            var actualCost = sp.OptimalCost;

            Assert.AreEqual(62, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void SalesmanProblemTest2()
        {
            var costs = DenseMatrix.OfArray(new[,]
            {
                {Inf, 10, 10, 8, 13, 1},
                {3, Inf, 1, 17, 17, 7},
                {1, 10, Inf, 6, 1, 17},
                {6, 3, 2, Inf, 5, 12},
                {8, 17, 8, 13, Inf, 11},
                {11, 14, 12, 6, 11, Inf}
            });

            var expectedResult = BuildExpectedResult((new[] { 1, 6, 4, 2, 3, 5, 1 }).ToList());

            var sp = new SalesmanProblem(costs);
            var actualResult = sp.Solve();
            var actualCost = sp.OptimalCost;

            Assert.AreEqual(20, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void SalesmanProblemTest3()
        {
            var costs = DenseMatrix.OfArray(new[,]
            {
                {Inf, 8, 0, 1, 18, 16, 5},
                {19, Inf, 12, 5, 11, 8, 17},
                {10, 19, Inf, 17, 11, 15, 5},
                {1, 8, 9, Inf, 11, 2, 2},
                {11, 12, 14, 8, Inf, 4, 1},
                {9, 3, 5, 17, 15, Inf, 19},
                {13, 6, 15, 13, 18, 10, Inf}
            });

            var expectedResult = BuildExpectedResult((new[] { 1, 3, 5, 7, 6, 2, 4, 1 }).ToList());

            var sp = new SalesmanProblem(costs);
            var actualResult = sp.Solve();
            var actualCost = sp.OptimalCost;

            Assert.AreEqual(31, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void SalesmanProblemTest4()
        {
            var costs = DenseMatrix.OfArray(new[,]
            {
                {Inf, 18, 13, 18, 8, 16, 11, 0},
                {0, Inf, 1, 8, 2, 15, 19, 11},
                {1, 10, Inf, 18, 5, 15, 12, 12},
                {15, 16, 10, Inf, 16, 10, 6, 9},
                {2, 18, 14, 16, Inf, 18, 13, 1},
                {5, 19, 1, 19, 1, Inf, 7, 4},
                {5, 7, 16, 0, 0, 8, Inf, 6},
                {10, 8, 13, 10, 12, 3, 13, Inf}
            });

            var expectedResult = BuildExpectedResult((new[] { 1, 8, 6, 3, 2, 4, 7, 5, 1 }).ToList());

            var sp = new SalesmanProblem(costs);
            var actualResult = sp.Solve();
            var actualCost = sp.OptimalCost;

            Assert.AreEqual(30, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void SalesmanProblemTest5()
        {
            var costs = DenseMatrix.OfArray(new[,]
            {
                {Inf, 13, 2, 17, 14},
                {11, Inf, 11, 8, 2},
                {4, 10, Inf, 3, 6},
                {9, 4, 6, Inf, 19},
                {3, 7, 12, 18, Inf}
            });

            var expectedResult = BuildExpectedResult((new[] { 1, 3, 4, 2, 5, 1 }).ToList());

            var sp = new SalesmanProblem(costs);
            var actualResult = sp.Solve();
            var actualCost = sp.OptimalCost;

            Assert.AreEqual(14, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void SalesmanProblemTest6()
        {
            var costs = DenseMatrix.OfArray(new[,]
            {
                {Inf, 6, 16, 16, 4, 12, 11, 1, 4, 10},
                {1, Inf, 16, 9, 17, 5, 3, 2, 6, 19},
                {19, 4, Inf, 11, 17, 8, 10, 4, 15, 11},
                {7, 1, 17, Inf, 17, 2, 5, 6, 10, 17},
                {8, 18, 18, 13, Inf, 0, 19, 6, 12, 14},
                {3, 5, 13, 19, 16, Inf, 12, 17, 2, 19},
                {1, 4, 1, 18, 2, 17, Inf, 8, 12, 10},
                {6, 14, 19, 7, 19, 19, 10, Inf, 2, 9},
                {2, 14, 18, 0, 16, 17, 13, 15, Inf, 1},
                {1, 12, 2, 6, 19, 4, 13, 7, 0, Inf}
            });

            //var expectedResult = BuildExpectedResult((new[] { 1, 8, 4, 7, 5, 6, 9, 10, 3, 2, 1 }).ToList());

            var sp = new SalesmanProblem(costs);
            /*var actualResult = */ sp.Solve();
            var actualCost = sp.OptimalCost;

            Assert.AreEqual(25, actualCost);
            //Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void SalesmanProblemTest7()
        {
            var costs = DenseMatrix.OfArray(new[,]
            {
                {Inf, 12, 11, 1, 18, 4, 14, 3, 18},
                {9, Inf, 14, 12, 7, 10, 4, 18, 9},
                {7, 8, Inf, 18, 1, 6, 1, 9, 19},
                {10, 18, 0, Inf, 3, 14, 3, 11, 4},
                {7, 3, 17, 10, Inf, 14, 14, 9, 8},
                {17, 16, 17, 16, 8, Inf, 9, 3, 19},
                {13, 19, 8, 19, 12, 0, Inf, 13, 4},
                {3, 3, 7, 6, 9, 15, 16, Inf, 15},
                {5, 13, 15, 19, 6, 5, 5, 2, Inf}
            });

            var expectedResult = BuildExpectedResult((new[] { 1, 4, 3, 5, 2, 7, 9, 6, 8, 1 }).ToList());

            var sp = new SalesmanProblem(costs);
            var actualResult = sp.Solve();
            var actualCost = sp.OptimalCost;

            Assert.AreEqual(24, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void SalesmanProblemTest8()
        {
            var costs = DenseMatrix.OfArray(new[,]
            {
                {Inf, 1, 14, 18, 11, 5, 13, 18, 17, 5, 11},
                {4, Inf, 19, 14, 5, 3, 6, 15, 14, 15, 14},
                {12, 6, Inf, 16, 19, 15, 6, 2, 12, 15, 8},
                {14, 4, 18, Inf, 15, 0, 18, 13, 6, 2, 8},
                {19, 15, 19, 14, Inf, 12, 9, 15, 3, 11, 16},
                {10, 6, 11, 4, 15, Inf, 10, 9, 0, 9, 6},
                {16, 0, 10, 17, 18, 6, Inf, 4, 4, 1, 0},
                {7, 17, 17, 6, 7, 12, 10, Inf, 14, 9, 17},
                {19, 5, 7, 6, 16, 4, 6, 17, Inf, 13, 14},
                {2, 11, 11, 16, 12, 7, 14, 12, 15, Inf, 0},
                {1, 14, 10, 0, 10, 3, 1, 0, 5, 6, Inf}
            });

            var expectedResult = BuildExpectedResult((new[] { 1, 2, 5, 7, 10, 11, 4, 6, 9, 3, 8, 1 }).ToList());

            var sp = new SalesmanProblem(costs);
            var actualResult = sp.Solve();
            var actualCost = sp.OptimalCost;

            Assert.AreEqual(32, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void SalesmanProblemTest9()
        {
            var costs = DenseMatrix.OfArray(new[,]
            {
                {Inf, 8, 12, 7, 5, 0, 11, 5, 13, 9, 18, 1},
                {10, Inf, 14, 4, 7, 4, 10, 10, 6, 6, 4, 3},
                {4, 16, Inf, 13, 3, 2, 5, 5, 15, 7, 11, 19},
                {3, 7, 11, Inf, 7, 6, 14, 3, 3, 8, 8, 18},
                {11, 15, 18, 12, Inf, 19, 12, 13, 11, 16, 1, 12},
                {8, 7, 16, 19, 1, Inf, 3, 16, 12, 11, 0, 5},
                {5, 10, 8, 0, 17, 10, Inf, 6, 13, 1, 0, 6},
                {6, 6, 6, 5, 1, 5, 17, Inf, 7, 14, 11, 5},
                {19, 8, 4, 19, 13, 2, 5, 14, Inf, 12, 15, 16},
                {11, 8, 8, 3, 4, 3, 4, 11, 2, Inf, 4, 15},
                {9, 6, 12, 0, 18, 13, 14, 3, 12, 16, Inf, 4},
                {18, 10, 8, 3, 18, 17, 16, 19, 7, 0, 12, Inf}
            });

            var expectedResult = BuildExpectedResult((new[] { 1, 6, 7, 4, 8, 5, 11, 2, 12, 10, 9, 3, 1 }).ToList());

            var sp = new SalesmanProblem(costs);
            var actualResult = sp.Solve();
            var actualCost = sp.OptimalCost;

            Assert.AreEqual(27, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void SalesmanProblemTest10()
        {
            var costs = DenseMatrix.OfArray(new[,]
            {
                {Inf, 10, 17, 15, 0, 15, 2, 16, 10, 2, 6, 19, 10},
                {1, Inf, 9, 5, 13, 4, 13, 9, 18, 10, 14, 2, 9},
                {7, 9, Inf, 12, 13, 12, 7, 7, 9, 15, 0, 3, 12},
                {6, 1, 19, Inf, 9, 17, 4, 1, 0, 10, 10, 15, 18},
                {13, 9, 9, 8, Inf, 2, 6, 4, 14, 2, 0, 17, 9},
                {17, 10, 10, 13, 1, Inf, 14, 8, 14, 17, 14, 14, 2},
                {17, 18, 3, 2, 6, 0, Inf, 19, 14, 3, 13, 3, 13},
                {0, 4, 1, 9, 6, 6, 16, Inf, 3, 19, 8, 15, 4},
                {15, 7, 5, 14, 6, 10, 1, 4, Inf, 4, 16, 17, 19},
                {1, 9, 18, 7, 16, 16, 1, 19, 16, Inf, 1, 6, 12},
                {7, 6, 7, 13, 8, 18, 10, 5, 19, 9, Inf, 5, 10},
                {10, 16, 10, 5, 2, 5, 9, 13, 6, 7, 9, Inf, 7},
                {18, 19, 4, 14, 13, 12, 7, 11, 8, 11, 12, 13, Inf}
            });

            var expectedResult = BuildExpectedResult((new[] { 1, 5, 10, 7, 6, 13, 3, 11, 2, 12, 4, 9, 8, 1 }).ToList());

            var sp = new SalesmanProblem(costs);
            var actualResult = sp.Solve();
            var actualCost = sp.OptimalCost;

            Assert.AreEqual(26, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        private SparseMatrix BuildExpectedResult(List<int> points)
        {
            var res = new SparseMatrix(points.Count - 1);
            for (int i = 0; i < points.Count - 1; i++)
            {
                res[points[i] - 1, points[i + 1] - 1] = 1;
            }
            return res;
        }
    }
}
