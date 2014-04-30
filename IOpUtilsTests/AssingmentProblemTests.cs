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

            var expectedResult = SparseMatrix.OfArray(new double[,]
            {
                {0, 1, 0, 0},
                {1, 0, 0, 0},
                {0, 0, 1, 0},
                {0, 0, 0, 1},
            });

            var am = new AssingmentProblem(costs);
            var actualResult = am.Solve();

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
