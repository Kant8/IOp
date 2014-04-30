using System;
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
