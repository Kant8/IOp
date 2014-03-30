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
    public class SimplexMethodSolverTests
    {
        [Test()]
        public void SimplexMethodSolverTest()
        {
            DenseMatrix A = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 5, 3, 1, 0, 0 },
                                                    { -1, 2, 0, 1, 0 },
                                                    { 1, -2, 0, 0, 1 },
                                                });
            DenseVector b = new DenseVector(new double[] { 4, 3, 7 });
            DenseVector c = new DenseVector(new double[] { 1, -1, 0, 0, 0 });

            DenseVector expectedResult = new DenseVector(new double[] { 0.8, 0, 0, 3.8, 6.2 });

            var sms = new SimplexMethodSolver(A, b, c);
            var actualResult = sms.Solve();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void SimplexMethodSolverTest1()
        {
            Assert.Pass();
            DenseMatrix A = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 5, 3, 1, 0, 0, 0 },
                                                    { -1, 2, 0, 1, 0, 0 },
                                                    { 1, -2, 0, 0, 1, 0 },
                                                    { 0, -0.6, -0.2, 0, 0, 1 },
                                                });
            DenseVector b = new DenseVector(new double[] { 4, 3, 7, -0.8 });
            DenseVector c = new DenseVector(new double[] { 1, -1, 0, 0, 0, 0 });

            DenseVector expectedResult = new DenseVector(new double[] { 0, 0, 4, 3, 7, 0 });

            var sms = new SimplexMethodSolver(A, b, c);
            var actualResult = sms.Solve();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void FirstPhaseOnlyTest()
        {

        }

        [Test()]
        public void SolveTest()
        {

        }

        [Test()]
        public void RoundResultTest()
        {

        }
    }
}
