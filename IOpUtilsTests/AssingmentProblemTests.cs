﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOpUtils;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
namespace IOpUtils.Tests
{
    [TestFixture()]
    public class AssingmentProblemTests
    {
        [Test()]
        public void AssingmentProblemTest00()
        {
            DenseMatrix costs = DenseMatrix.OfArray(new double[,]
            {
                {2, -1, 9, 4},
                {3, 2, 5, 1},
                {13, 0, -3, 4},
                {5, 6, 1, 2},
            });

            var expectedResult = BuildExpectedResult(new int[,]
            {
                {1, 2},
                {2, 1},
                {3, 3},
                {4, 4},
            });

            var am = new AssingmentProblem(costs);
            var actualResult = am.Solve();

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void AssingmentProblemTest1()
        {
            DenseMatrix costs = DenseMatrix.OfArray(new double[,]
            {
                {6, 4, 13, 4, 19, 15, 11, 8},
                {17, 15, 18, 14, 0, 7, 18, 7},
                {3, 5, 11, 9, 7, 7, 18, 16},
                {17, 10, 16, 19, 9, 6, 1, 5},
                {14, 2, 10, 14, 11, 6, 4, 10},
                {17, 11, 17, 12, 1, 10, 6, 19},
                {13, 1, 4, 2, 2, 7, 2, 14},
                {12, 15, 19, 11, 13, 1, 7, 8},
            });

            var expectedResult = BuildExpectedResult(new int[,]
            {
                {1, 4},
                {2, 8},
                {3, 1},
                {4, 7},
                {5, 2},
                {6, 5},
                {7, 3},
                {8, 6},
            });

            var am = new AssingmentProblem(costs);
            var actualResult = am.Solve();
            var actualCost = am.TotalCost;

            Assert.AreEqual(23, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void AssingmentProblemTest2()
        {
            DenseMatrix costs = DenseMatrix.OfArray(new double[,]
            {
                {9, 6, 4, 9, 3, 8, 0},
                {5, 8, 6, 8, 8, 3, 5},
                {5, 2, 1, 1, 8, 6, 8},
                {1, 0, 9, 2, 5, 9, 2},
                {9, 2, 3, 3, 0, 3, 0},
                {7, 3, 0, 9, 4, 5, 6},
                {0, 9, 6, 0, 8, 8, 9},
            });

            var expectedResult = BuildExpectedResult(new int[,]
            {
                {1, 7},
                {2, 6},
                {3, 4},
                {4, 2},
                {5, 5},
                {6, 3},
                {7, 1},
            });

            var am = new AssingmentProblem(costs);
            var actualResult = am.Solve();
            var actualCost = am.TotalCost;

            Assert.AreEqual(4, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void AssingmentProblemTest3()
        {
            DenseMatrix costs = DenseMatrix.OfArray(new double[,]
            {
                {6, 6, 2, 4, 7, 1, 9, 4, 6},
                {5, 0, 2, 4, 9, 2, 9, 2, 0},
                {7, 6, 0, 5, 2, 3, 0, 5, 5},
                {9, 5, 8, 9, 2, 3, 1, 5, 7},
                {3, 1, 7, 3, 0, 2, 2, 8, 1},
                {3, 0, 0, 6, 1, 7, 2, 4, 7},
                {5, 6, 1, 9, 9, 8, 4, 1, 8},
                {5, 4, 5, 2, 2, 6, 6, 5, 6},
                {3, 6, 1, 6, 3, 0, 5, 2, 2},
            });

            var expectedResult = BuildExpectedResult(new int[,]
            {
                {1, 6},
                {2, 9},
                {3, 3},
                {4, 7},
                {5, 5},
                {6, 2},
                {7, 8},
                {8, 4},
                {9, 1},
            });

            var am = new AssingmentProblem(costs);
            var actualResult = am.Solve();
            var actualCost = am.TotalCost;

            Assert.AreEqual(8, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void AssingmentProblemTest4()
        {
            DenseMatrix costs = DenseMatrix.OfArray(new double[,]
            {
                {6, 5, 6, 8, 4, 0, 4, 6},
                {5, 7, 8, 7, 4, 4, 0, 9},
                {0, 7, 9, 2, 8, 7, 0, 3},
                {6, 6, 6, 3, 0, 3, 0, 8},
                {7, 4, 7, 1, 1, 1, 8, 9},
                {8, 0, 7, 5, 0, 9, 1, 3},
                {3, 2, 4, 7, 1, 7, 3, 4},
                {9, 2, 4, 3, 2, 4, 3, 9},
            });

            var expectedResult = BuildExpectedResult(new int[,]
            {
                {1, 6},
                {2, 7},
                {3, 1},
                {4, 5},
                {5, 4},
                {6, 2},
                {7, 8},
                {8, 3},
            });

            var am = new AssingmentProblem(costs);
            var actualResult = am.Solve();
            var actualCost = am.TotalCost;

            Assert.AreEqual(9, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void AssingmentProblemTest5()
        {
            DenseMatrix costs = DenseMatrix.OfArray(new double[,]
            {
                {7, 4, 5, 3, 8, 9, 6, 5, 5, 3, 2},
                {5, 6, 9, 4, 9, 0, 0, 4, 4, 7, 2},
                {8, 8, 3, 2, 7, 3, 7, 6, 7, 4, 6},
                {7, 4, 9, 9, 3, 7, 3, 8, 1, 5, 8},
                {5, 2, 4, 3, 3, 9, 6, 2, 5, 1, 3},
                {9, 4, 5, 8, 6, 3, 3, 1, 7, 6, 5},
                {9, 1, 0, 3, 1, 2, 7, 6, 9, 4, 6},
                {5, 6, 8, 0, 9, 9, 1, 9, 3, 0, 8},
                {4, 6, 5, 6, 4, 7, 5, 3, 8, 0, 1},
                {2, 3, 7, 8, 4, 9, 5, 0, 2, 8, 0},
                {7, 6, 7, 1, 9, 5, 7, 4, 2, 3, 0},
            });

            var expectedResult = BuildExpectedResult(new int[,]
            {
                {1, 2},
                {2, 6},
                {3, 4},
                {4, 9},
                {5, 5},
                {6, 8},
                {7, 3},
                {8, 7},
                {9, 10},
                {10, 1},
                {11, 11},
            });

            var am = new AssingmentProblem(costs);
            var actualResult = am.Solve();
            var actualCost = am.TotalCost;

            Assert.AreEqual(14, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void AssingmentProblemTest6()
        {
            DenseMatrix costs = DenseMatrix.OfArray(new double[,]
            {
                {7, -4, 5, 3, 8, 9, 6, 5},
                {5, 6, 9, 4, 9, 0, 0, 4},
                {8, 8, 3, -2, 7, -3, 7, 6},
                {7, 4, 9, 9, 3, 7, 3, 8},
                {5, 2, 4, 3, 3, 9, 6, 2},
                {9, 4, 5, 8, 6, 3, 3, 1},
                {9, 1, 0, -3, 1, 2, 7, 6},
                {5, 6, 8, 0, 9, 9, 1, 9},
            });

            var expectedResult = BuildExpectedResult(new int[,]
            {
                {1, 2},
                {2, 7},
                {3, 6},
                {4, 5},
                {5, 1},
                {6, 6},
                {7, 3},
                {8, 4},
            });

            var am = new AssingmentProblem(costs);
            var actualResult = am.Solve();
            var actualCost = am.TotalCost;

            Assert.AreEqual(2, actualCost);
            //Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void AssingmentProblemTest7()
        {
            DenseMatrix costs = DenseMatrix.OfArray(new double[,]
            {
                {2, 6, 5, -1, 6, 1, 8, 4, 6},
                {2, 1, 2, 7, 9, -2, 8, 2, 0},
                {0, 6, 0, 5, 1, 3, 4, 3, 5},
                {7, 0, 8, 9, 2, 4, 1, 6, 7},
                {-1, 1, 0, -3, 0, 2, 2, 2, 1},
                {3, 0, 6, 6, 1, -2, 2, 4, 0},
                {1, 7, 1, 9, 4, 8, 2, 6, 8},
                {5, 1, 5, 2, 2, 6, -1, 5, 4},
                {3, 6, 0, 6, 3, 0, 9, 1, 2},
            });

            var expectedResult = BuildExpectedResult(new int[,]
            {
                {1, 4},
                {2, 9},
                {3, 3},
                {4, 2},
                {5, 5},
                {6, 6},
                {7, 1},
                {8, 7},
                {9, 8},
            });

            var am = new AssingmentProblem(costs);
            var actualResult = am.Solve();
            var actualCost = am.TotalCost;

            Assert.AreEqual(-2, actualCost);
            //Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void AssingmentProblemTest8()
        {
            DenseMatrix costs = DenseMatrix.OfArray(new double[,]
            {
                {2, 4, 0, 3, 8, -1, 6, 5},
                {8, 6, 3, 4, 2, 0, 0, 4},
                {8, -4, 3, 2, 7, 3, 1, 0},
                {2, 4, 9, 5, 3, 0, 3, 8},
                {5, 2, 7, 3, -1, 0, 3, 2},
                {3, 2, 5, 1, 4, 3, 0, 1},
                {2, 1, 0, -3, 1, 2, 7, 0},
                {1, 6, 4, 0, 0, 9, 1, 7},
            });

            var expectedResult = BuildExpectedResult(new int[,]
            {
                {1, 3},
                {2, 7},
                {3, 2},
                {4, 6},
                {5, 5},
                {6, 8},
                {7, 4},
                {8, 1},
            });

            var am = new AssingmentProblem(costs);
            var actualResult = am.Solve();
            var actualCost = am.TotalCost;

            Assert.AreEqual(-6, actualCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        private SparseMatrix BuildExpectedResult(int[,] data)
        {
            int n = data.GetLength(0);
            var res = new SparseMatrix(n);
            for (int i = 0; i < n; i++)
            {
                res[data[i, 0] - 1, data[i, 1] - 1] = 1;
            }
            return res;
        }
    }
}
