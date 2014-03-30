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
    public class DuplexMethodSolverTests
    {
        [Test()]
        public void DuplexMethodSolverTest0()
        {
            DenseMatrix A = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 2, 1, -1, 0, 0, 1 },
                                                    { 1, 0, 1, 1, 0, 0 },
                                                    { 0, 1, 0, 0, 1, 0 },
                                                });
            DenseVector b = new DenseVector(new double[] { 2, 5, 0 });
            DenseVector c = new DenseVector(new double[] { 3, 2, 0, 3, -2, -4 });
            DenseVector dLower = new DenseVector(new double[] { 0, -1, 2, 1, -1, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 2, 4, 4, 3, 3, 5 });

           
            DenseVector expectedResult = new DenseVector(new double[] { 3.0 / 2, 1 , 2, 3.0 / 2, -1, 0 });

            var qps = new DuplexMethodBoundsSolver(A, b, c, dLower, dUpper, 
                DenseVector.Create(1, (i) => 0), new[] {3, 4, 5});
            var actualResult = qps.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(13, qps.ResultCost);
        }

        [Test()]
        public void DuplexMethodSolverTest1()
        {
            DenseMatrix A = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 2, 1, -1, 0, 0, 1 },
                                                    { 1, 0, 1, 1, 0, 0 },
                                                    { 0, 1, 0, 0, 1, 0 },
                                                });
            DenseVector b = new DenseVector(new double[] { 2, 5, 0 });
            DenseVector c = new DenseVector(new double[] { 3, 2, 0, 3, -2, -4 });
            DenseVector dLower = new DenseVector(new double[] { 0, -1, 2, 1, -1, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 2, 4, 4, 3, 3, 5 });


            DenseVector expectedResult = new DenseVector(new double[] { 3.0 / 2, 1, 2, 3.0 / 2, -1, 0 });

            var qps = new DuplexMethodBoundsSolver(A, b, c, dLower, dUpper);
            var actualResult = qps.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(13, qps.ResultCost);
        }

        [Test()]
        public void DuplexMethodSolverTest2()
        {
            DenseMatrix A = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, 0, 2, 2, -3, 3 },
                                                    { 0, 1, 0, -1, 0, 1 },
                                                    { 1, 0, 1, 3, 2, 1 },
                                                });
            DenseVector b = new DenseVector(new double[] { 15, 0, 13 });
            DenseVector c = new DenseVector(new double[] { 3, 0.5, 4, 4, 1, 5 });
            DenseVector dLower = new DenseVector(new double[] { 0, 0, 0, 0, 0, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 3, 5, 4, 3, 3, 4 });


            DenseVector expectedResult = new DenseVector(new double[] { 3,  0, 4, 1.1818, 0.6364, 1.1818 });

            var qps = new DuplexMethodBoundsSolver(A, b, c, dLower, dUpper);
            var actualResult = qps.Solve();
            Assert.AreEqual(expectedResult, RoundVector(actualResult, 4));
            Assert.AreEqual(36.2727, Math.Round(qps.ResultCost, 4));
        }

        [Test()]
        public void DuplexMethodSolverTest3()
        {
            DenseMatrix A = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, 0, 0, 12, 1, -3, 4, -1 },
                                                    { 0, 1, 0, 11, 12, 3, 5, 3 },
                                                    { 0, 0, 1, 1, 0, 22, -2, 1 },
                                                });
            DenseVector b = new DenseVector(new double[] { 40, 107, 61 });
            DenseVector c = new DenseVector(new double[] { 2, 1, -2, -1, 4, -5, 5, 5 });
            DenseVector dLower = new DenseVector(new double[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 3, 5, 5, 3, 4, 5, 6, 3 });


            DenseVector expectedResult = new DenseVector(new double[] { 3, 5, 0, 1.8779, 2.7545, 3.0965, 6, 3 });

            var qps = new DuplexMethodBoundsSolver(A, b, c, dLower, dUpper);
            var actualResult = qps.Solve();
            Assert.AreEqual(expectedResult, RoundVector(actualResult, 4));
            Assert.AreEqual(49.6577, Math.Round(qps.ResultCost, 4));
        }

        [Test()]
        public void DuplexMethodSolverTest4()
        {
            DenseMatrix A = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, -3, 2, 0, 1, -1, 4, -1, 0 },
                                                    { 1, -1, 6, 1, 0, -2, 2, 2, 0 },
                                                    { 2, 2, -1, 1, 0, -3, 8, -1, 1 },
                                                    { 4, 1, 0, 0, 1, -1, 0, -1, 1 },
                                                    { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                                                });
            DenseVector b = new DenseVector(new double[] { 3, 9, 9, 5, 9 });
            DenseVector c = new DenseVector(new double[] { -1, 5, -2, 4, 3, 1, 2, 8, 3 });
            DenseVector dLower = new DenseVector(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 5, 5, 5, 5, 5, 5, 5, 5, 5 });


            DenseVector expectedResult = new DenseVector(new double[] { 1.1579, 0.6942, 0, 0, 2.8797, 0, 1.0627, 3.2055, 0 });

            var qps = new DuplexMethodBoundsSolver(A, b, c, dLower, dUpper);
            var actualResult = qps.Solve();
            Assert.AreEqual(expectedResult, RoundVector(actualResult, 4));
            Assert.AreEqual(38.7218, Math.Round(qps.ResultCost, 4));
        }

        [Test()]
        public void DuplexMethodSolverTest5()
        {
            DenseMatrix A = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, 7, 2, 0, 1, -1, 4 },
                                                    { 0, 5, 6, 1, 0, -3, 2 },
                                                    { 3, 2, 2, 1, 1, 1, 5 },
                                                });
            DenseVector b = new DenseVector(new double[] { 1, 4, 7 });
            DenseVector c = new DenseVector(new double[] { 1, 2, 1, -3, 3, 1, 0 });
            DenseVector dLower = new DenseVector(new double[] { -1, 1, -2, 0, 1, 2, 4 });
            DenseVector dUpper = new DenseVector(new double[] { 3, 2, 2, 5, 3, 4, 5 });

            var qps = new DuplexMethodBoundsSolver(A, b, c, dLower, dUpper);
            Assert.Catch<ArithmeticException>(() => qps.Solve());
        }

        [Test()]
        public void DuplexMethodSolverTest6()
        {
            DenseMatrix A = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 2, -1, 1, 0, 0, -1, 3 },
                                                    { 0, 4, -1, 2, 3, -2, 2 },
                                                    { 3, 1, 0, 1, 0, 1, 4 },
                                                });
            DenseVector b = new DenseVector(new double[] { 4, 9, 2 });
            DenseVector c = new DenseVector(new double[] { 0, 1, 2, 1, -3, 4, 7 });
            DenseVector dLower = new DenseVector(new double[] { 0, 0, -3, 0, -1, 1, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 3, 3, 4, 7, 5, 3, 2 });

            var qps = new DuplexMethodBoundsSolver(A, b, c, dLower, dUpper);
            Assert.Catch<ArithmeticException>(() => qps.Solve());
        }

        [Test()]
        public void DuplexMethodSolverTest7()
        {
            DenseMatrix A = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 2, 1, 0, 3, -1, -1 },
                                                    { 0, 1, 2, 1, 0, 3 },
                                                    { 3, 0, 1, 1, 1, 1 },
                                                });
            DenseVector b = new DenseVector(new double[] { 2, 2, 5 });
            DenseVector c = new DenseVector(new double[] { 0, -1, 1, 0, 4, 3 });
            DenseVector dLower = new DenseVector(new double[] { 2, 0, -1, -3, 2, 1 });
            DenseVector dUpper = new DenseVector(new double[] { 7, 3, 2, 3, 4, 5 });

            var qps = new DuplexMethodBoundsSolver(A, b, c, dLower, dUpper);
            Assert.Catch<ArithmeticException>(() => qps.Solve());
        }

        [Test()]
        public void DuplexMethodSolverTest8()
        {
            DenseMatrix A = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, 3, 1, -1, 0, -3, 2, 1 },
                                                    { 2, 1, 3, -1, 1, 4, 1, 1 },
                                                    { -1, 0, 2, -2, 2, 1, 1, 1 },
                                                });
            DenseVector b = new DenseVector(new double[] { 4, 12, 4 });
            DenseVector c = new DenseVector(new double[] { 2, -1, 2, 3, -2, 3, 4, 1 });
            DenseVector dLower = new DenseVector(new double[] { -1, -1, -1, -1, -1, -1, -1, -1 });
            DenseVector dUpper = new DenseVector(new double[] { 2, 3, 1, 4, 3, 2, 4, 4 });


            DenseVector expectedResult = new DenseVector(new double[] { -1, 0.4074, 1, 4, -0.3704, 1.7407, 4, 4 });

            var qps = new DuplexMethodBoundsSolver(A, b, c, dLower, dUpper);
            var actualResult = qps.Solve();
            Assert.AreEqual(expectedResult, RoundVector(actualResult, 4));
            Assert.AreEqual(37.5556, Math.Round(qps.ResultCost, 4));
        }

        private DenseVector RoundVector(DenseVector vector, int decimals)
        {
            var res = new DenseVector(vector.Count);
            for (int i = 0; i < vector.Count; i++)
            {
                res[i] = Math.Round(vector[i], decimals);
            }
            return res;
        }
    }
}
