using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
namespace IOpUtils.Tests
{
    [TestFixture]
    public class BranchAndBoundMethodTests
    {
        [Test]
        public void BranchAndBoundMethodTest00()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, -5, 3, 1, 0, 0 },
                                                    { 4, -1, 1, 0, 1, 0 },
                                                    { 2, 4, 2, 0, 0, 1 }
                                                });
            DenseVector b = new DenseVector(new double[] { -8, 22, 30 });
            DenseVector c = new DenseVector(new double[] { 7, -2, 6, 0, 5, 2 });
            DenseVector dLower = new DenseVector(new double[] { 2, 1, 0, 0, 1, 1 });
            DenseVector dUpper = new DenseVector(new double[] { 6, 6, 5, 2, 4, 6 });
            const double startRecord = Double.NegativeInfinity;


            DenseVector expectedResult = new DenseVector(new double[] { 6, 3, 0, 1, 1, 6 });

            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            var actualResult = babm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(53, babm.ResultCost);
        }

        [Test]
        public void BranchAndBoundMethodTest01()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, 0, 3, 1, 0, 0 },
                                                    { 0, -1, 1, 1, 1, 2 },
                                                    { -2, 4, 2, 0, 0, 1 }
                                                });
            DenseVector b = new DenseVector(new double[] { 10, 8, 10 });
            DenseVector c = new DenseVector(new double[] { 7, -2, 6, 0, 5, 2 });
            DenseVector dLower = new DenseVector(new double[] { 0, 1, -1, 0, -2, 1 });
            DenseVector dUpper = new DenseVector(new double[] { 3, 3, 6, 2, 4, 6 });
            double startRecord = Double.NegativeInfinity;


            DenseVector expectedResult = new DenseVector(new double[] { 1, 1, 3, 0, 2, 2 });

            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            var actualResult = babm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(37, babm.ResultCost);
        }

        [Test]
        public void BranchAndBoundMethodTest02()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, 0, 1, 0, 0, 1 },
                                                    { 1, 2, -1, 1, 1, 2 },
                                                    { -2, 4, 1, 0, 1, 0 }
                                                });
            DenseVector b = new DenseVector(new double[] { -3, 3, 13 });
            DenseVector c = new DenseVector(new double[] { -3, 2, 0, -2, -5, 2 });
            DenseVector dLower = new DenseVector(new double[] { -2, -1, -2, 0, 1, -4 });
            DenseVector dUpper = new DenseVector(new double[] { 2, 3, 1, 5, 4, -1 });
            double startRecord = Double.NegativeInfinity;


            DenseVector expectedResult = new DenseVector(new double[] { -2, 2, 0, 2, 1, -1 });

            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            var actualResult = babm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(-1, babm.ResultCost);
        }

        [Test]
        public void BranchAndBoundMethodTest1()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, 0, 0, 12, 1, -3, 4, -1 },
                                                    { 0, 1, 0, 11, 12, 3, 5, 3 },
                                                    { 0, 0, 1, 1, 0, 22, -2, 1 }
                                                });
            DenseVector b = new DenseVector(new double[] { 40, 107, 61 });
            DenseVector c = new DenseVector(new double[] { 2, 1, -2, -1, 4, -5, 5, 5 });
            DenseVector dLower = new DenseVector(new double[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 3, 5, 5, 3, 4, 5, 6, 3 });
            double startRecord = Double.NegativeInfinity;


            DenseVector expectedResult = new DenseVector(new double[] { 1, 1, 2, 2, 3, 3, 6, 3 });

            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            var actualResult = babm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(39, babm.ResultCost);
        }

        [Test]
        public void BranchAndBoundMethodTest2()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, -3, 2, 0, 1, -1, 4, -1, 0 },
                                                    { 1, -1, 6, 1, 0, -2, 2, 2, 0 },
                                                    { 2, 2, -1, 1, 0, -3, 8, -1, 1 },
                                                    { 4, 1, 0, 0, 1, -1, 0, -1, 1 },
                                                    { 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                                                });
            DenseVector b = new DenseVector(new double[] { 3, 9, 9, 5, 9 });
            DenseVector c = new DenseVector(new double[] { -1, 5, -2, 4, 3, 1, 2, 8, 3 });
            DenseVector dLower = new DenseVector(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 5, 5, 5, 5, 5, 5, 5, 5, 5 });
            double startRecord = Double.NegativeInfinity;


            DenseVector expectedResult = new DenseVector(new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });

            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            var actualResult = babm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(23, babm.ResultCost);
        }

        [Test]
        public void BranchAndBoundMethodTest3()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, 0, 0, 12, 1, -3, 4, -1, 2.5, 3 },
                                                    { 0, 1, 0, 11, 12, 3, 5, 3, 4, 5.1 },
                                                    { 0, 0, 1, 1, 0, 22, -2, 1, 6.1, 7 }
                                                });
            DenseVector b = new DenseVector(new double[] { 43.5, 107.3, 106.3 });
            DenseVector c = new DenseVector(new double[] { 2, 1, -2, -1, 4, -5, 5, 5, 1, 2 });
            DenseVector dLower = new DenseVector(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 2, 4, 5, 3, 4, 5, 4, 4, 5, 6 });
            double startRecord = Double.NegativeInfinity;


            DenseVector expectedResult = new DenseVector(new double[] { 1, 1, 2, 2, 2, 3, 3, 3, 3, 3 });

            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            var actualResult = babm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(29, babm.ResultCost);
        }

        [Test]
        public void BranchAndBoundMethodTest4()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 4, 0, 0, 0, 0, -3, 4, -1, 2, 3 },
                                                    { 0, 1, 0, 0, 0, 3, 5, 3, 4, 5 },
                                                    { 0, 0, 1, 0, 0, 22, -2, 1, 6, 7 },
                                                    { 0, 0, 0, 1, 0, 6, -2, 7, 5, 6 },
                                                    { 0, 0, 0, 0, 1, 5, 5, 1, 6, 7 }
                                                });
            DenseVector b = new DenseVector(new double[] { 8, 5, 4, 7, 8 });
            DenseVector c = new DenseVector(new double[] { 2, 1, -2, -1, 4, -5, 5, 5, 1, 2 });
            DenseVector dLower = new DenseVector(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 });
            double startRecord = Double.NegativeInfinity;


            DenseVector expectedResult = new DenseVector(new double[] { 2, 5, 4, 7, 8, 0, 0, 0, 0, 0 });

            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            var actualResult = babm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(26, babm.ResultCost);
        }

        [Test]
        public void BranchAndBoundMethodTest5()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, -5, 3, 1, 0, 0 },
                                                    { 4, -1, 1, 0, 1, 0 },
                                                    { 2, 4, 2, 0, 0, 1 }
                                                });
            DenseVector b = new DenseVector(new double[] { -8, 22, 30 });
            DenseVector c = new DenseVector(new double[] { 7, -2, 6, 0, 5, 2 });
            DenseVector dLower = new DenseVector(new double[] { 2, 1, 0, 0, 1, 1 });
            DenseVector dUpper = new DenseVector(new double[] { 6, 6, 5, 2, 4, 6 });
            double startRecord = Double.NegativeInfinity;


            DenseVector expectedResult = new DenseVector(new double[] { 6, 3, 0, 1, 1, 6 });

            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            var actualResult = babm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(53, babm.ResultCost);
        }

        [Test]
        public void BranchAndBoundMethodTest6()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, 0, 0, 3, 1, -3, 4, -1 },
                                                    { 0, 1, 0, 4, -3, 3, 5, 3 },
                                                    { 0, 0, 1, 1, 0, 2, -2, 1 }
                                                });
            DenseVector b = new DenseVector(new double[] { 30, 78, 18 });
            DenseVector c = new DenseVector(new double[] { 2, 1, -2, -1, 4, -5, 5, 5 });
            DenseVector dLower = new DenseVector(new double[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 5, 5, 3, 4, 5, 6, 6, 8 });
            double startRecord = Double.NegativeInfinity;


            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            Assert.Catch<ArithmeticException>(() => babm.Solve());
        }

        [Test]
        public void BranchAndBoundMethodTest7()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, -3, 2, 0, 1, -1, 4, -1, 0 },
                                                    { 1, -1, 6, 1, 0, -2, 2, 2, 0 },
                                                    { 2, 2, -1, 1, 0, -3, 2, -1, 1 },
                                                    { 4, 1, 0, 0, 1, -1, 0, -1, 1 },
                                                    { 1, 1, 1, 1, 1, 3, 1, 1, 1 }
                                                });
            DenseVector b = new DenseVector(new double[] { 18, 18, 30, 15, 18 });
            DenseVector c = new DenseVector(new double[] { 7, 5, -2, 4, 3, 1, 2, 8, 3 });
            DenseVector dLower = new DenseVector(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 8, 8, 8, 8, 8, 8, 8, 8, 8 });
            double startRecord = Double.NegativeInfinity;


            DenseVector expectedResult = new DenseVector(new double[] { 3, 5, 0, 0, 0, 0, 8, 2, 0 });

            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            var actualResult = babm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(78, babm.ResultCost);
        }

        [Test]
        public void BranchAndBoundMethodTest8()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, 0, 1, 0, 4, 3, 4 },
                                                    { 0, 1, 2, 0, 55, 3.5, 5 },
                                                    { 0, 0, 3, 1, 6, 2, -2.5 }
                                                });
            DenseVector b = new DenseVector(new double[] { 26, 185, 32.5 });
            DenseVector c = new DenseVector(new double[] { 1, 2, 3, -1, 4, -5, 6 });
            DenseVector dLower = new DenseVector(new double[] { 0, 1, 0, 0, 0, 0, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 1, 2, 5, 7, 8, 4, 2 });
            double startRecord = Double.NegativeInfinity;


            DenseVector expectedResult = new DenseVector(new double[] { 1, 2, 3, 4, 3, 2, 1 });

            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            var actualResult = babm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(18, babm.ResultCost);
        }

        [Test]
        public void BranchAndBoundMethodTest9()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 2, 0, 1, 0, 0, 3, 5 },
                                                    { 0, 2, 2.1, 0, 0, 3.5, 5 },
                                                    { 0, 0, 3, 2, 0, 2, 1.1 },
                                                    { 0, 0, 3, 0, 2, 2, -2.5 }
                                                });
            DenseVector b = new DenseVector(new double[] { 58, 66.3, 36.7, 13.5 });
            DenseVector c = new DenseVector(new double[] { 1, 2, 3, 1, 2, 3, 4 });
            DenseVector dLower = new DenseVector(new double[] { 1, 1, 1, 1, 1, 1, 1 });
            DenseVector dUpper = new DenseVector(new double[] { 2, 3, 4, 5, 8, 7, 7 });
            double startRecord = Double.NegativeInfinity;


            DenseVector expectedResult = new DenseVector(new double[] { 1, 2, 3, 4, 5, 6, 7 });

            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            var actualResult = babm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(74, babm.ResultCost);
        }

        [Test]
        public void BranchAndBoundMethodTest10()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, 0, 0, 1, 1, -3, 4, -1, 3, 3 },
                                                    { 0, 1, 0, -2, 1, 1, 7, 3, 4, 5 },
                                                    { 0, 0, 1, 1, 0, 2, -2, 1, -4, 7 }
                                                });
            DenseVector b = new DenseVector(new double[] { 27, 6, 18 });
            DenseVector c = new DenseVector(new double[] { -2, 1, -2, -1, 8, -5, 3, 5, 1, 2 });
            DenseVector dLower = new DenseVector(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            DenseVector dUpper = new DenseVector(new double[] { 8, 7, 6, 7, 8, 5, 6, 7, 8, 5 });
            double startRecord = Double.NegativeInfinity;


            DenseVector expectedResult = new DenseVector(new double[] { 5, 0, 6, 7, 8, 0, 1, 0, 0, 1 });

            var babm = new BranchAndBoundMethod(a, b, c, dLower, dUpper,
                Enumerable.Range(0, a.ColumnCount), startRecord);
            var actualResult = babm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(40, babm.ResultCost);
        }


    }
}
