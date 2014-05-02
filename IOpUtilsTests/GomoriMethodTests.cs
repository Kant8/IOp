using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
namespace IOpUtils.Tests
{
    [TestFixture]
    public class GomoriMethodTests
    {
        [Test]
        public void GomoriMethodTest()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 5, -1, 1, 0, 0 },
                                                    { -1, 2, 0, 1, 0 },
                                                    { -7, 2, 0, 0, 1 }
                                                });
            DenseVector b = new DenseVector(new double[] { 15, 6, 0 });
            DenseVector c = new DenseVector(new double[] { 3.5, -1, 0, 0, 0 });

            DenseVector expectedResult = new DenseVector(new double[] { 0, 0, 15, 6, 0 });

            var gm = new GomoriMethod(a, b, c, Enumerable.Range(0, a.ColumnCount));
            var actualResult = gm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(0, gm.ResultCost);
        }

        [Test, Ignore]
        public void GomoriMethodTest4()
        {
            DenseMatrix a = DenseMatrix.OfArray(new double[,]
                                                {
                                                    { 1, 2, 3, 12, 1, -3, 4, -1, 2, 3 },
                                                    { 0, 2, 0, 11, 12, 3, 5, 3, 4, 5 },
                                                    { 0, 0, 2, 1, 0, 22, -2, 1, 6, 7 }
                                                });
            DenseVector b = new DenseVector(new double[] { 153, 123, 112 });
            DenseVector c = new DenseVector(new double[] { 2, 1, -2, -1, 4, -5, 5, 5, 1, 2 });

            DenseVector expectedResult = new DenseVector(new double[] { 188, 0, 4, 0, 0, 3, 0, 38, 0, 0 });

            var gm = new GomoriMethod(a, b, c, Enumerable.Range(0, a.ColumnCount));
            var actualResult = gm.Solve();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(543, gm.ResultCost);
        }
    }
}
