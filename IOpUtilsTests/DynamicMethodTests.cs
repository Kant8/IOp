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
    public class DynamicMethodTests
    {
        [Test()]
        public void DynamicMethodTest()
        {
            var f = new int[,]
            {
                {0, 3, 4, 5, 8, 9, 10},
                {0, 2, 3, 7, 9, 12, 13},
                {0, 1, 2, 6, 11, 11, 13},
            };

            int c = f.GetLength(1) - 1;;

            int[] expectedResult = new int[] { 1, 1, 4 };

            var gm = new DynamicMethod(f, c);
            var actualResult = gm.Solve();
            Assert.AreEqual(16, gm.ResultCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void DynamicMethodTest1()
        {
            var f = new int[,]
            {
                {0, 1, 2, 2, 4, 5, 6},
                {0, 2, 3, 5, 7, 7, 8},
                {0, 2, 4, 5, 6, 7, 7},
            };

            int c = f.GetLength(1) - 1; ;

            int[] expectedResult = new int[] { 0, 4, 2 };

            var gm = new DynamicMethod(f, c);
            var actualResult = gm.Solve();
            Assert.AreEqual(11, gm.ResultCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void DynamicMethodTest2()
        {
            var f = new int[,]
            {
                {0, 1, 1, 3, 6, 10, 11},
                {0, 2, 3, 5, 6, 7, 13},
                {0, 1, 4, 4, 7, 8, 9},
            };

            int c = f.GetLength(1) - 1; ;

            int[] expectedResult = new int[] { 0, 6, 0 };

            var gm = new DynamicMethod(f, c);
            var actualResult = gm.Solve();
            Assert.AreEqual(13, gm.ResultCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void DynamicMethodTest3()
        {
            var f = new int[,]
            {
                {0, 1, 2, 4, 8, 9, 9, 23},
                {0, 2, 4, 6, 6, 8, 10, 11},
                {0, 3, 4, 7, 7, 8, 8, 24},
            };

            int c = f.GetLength(1) - 1; ;

            int[] expectedResult = new int[] { 0, 0, 7 };

            var gm = new DynamicMethod(f, c);
            var actualResult = gm.Solve();
            Assert.AreEqual(24, gm.ResultCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void DynamicMethodTest4()
        {
            var f = new int[,]
            {
                {0, 3, 3, 6, 7, 8, 9, 14},
                {0, 2, 4, 4, 5, 6, 8, 13},
                {0, 1, 1, 2, 3, 3, 10, 11},
            };

            int c = f.GetLength(1) - 1; ;

            int[] expectedResult = new int[] { 7, 0, 0 };

            var gm = new DynamicMethod(f, c);
            var actualResult = gm.Solve();
            Assert.AreEqual(14, gm.ResultCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void DynamicMethodTest5()
        {
            var f = new int[,]
            {
                {0, 2, 2, 3, 5, 8, 8, 10, 17},
                {0, 1, 2, 5, 8, 10, 11, 13, 15},
                {0, 4, 4, 5, 6, 7, 13, 14, 14},
                {0, 1, 3, 6, 9, 10, 11, 14, 16},
            };

            int c = f.GetLength(1) - 1; ;

            int[] expectedResult = new int[] { 0, 4, 1, 3 };

            var gm = new DynamicMethod(f, c);
            var actualResult = gm.Solve();
            Assert.AreEqual(18, gm.ResultCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void DynamicMethodTest6()
        {
            var f = new int[,]
            {
                {0, 1, 3, 4, 5, 8, 9, 9, 11, 12, 12, 14},
                {0, 1, 2, 3, 3, 3, 7, 12, 13, 14, 17, 19},
                {0, 4, 4, 7, 7, 8, 12, 14, 14, 16, 18, 22},
                {0, 5, 5, 5, 7, 9, 13, 13, 15, 15, 19, 24},
            };

            int c = f.GetLength(1) - 1; ;

            int[] expectedResult = new int[] { 2, 7, 1, 1 };

            var gm = new DynamicMethod(f, c);
            var actualResult = gm.Solve();
            Assert.AreEqual(24, gm.ResultCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void DynamicMethodTest7()
        {
            var f = new int[,]
            {
                {0, 4, 4, 6, 9, 12, 12, 15, 16, 19, 19, 19},
                {0, 1, 1, 1, 4, 7, 8, 8, 13, 13, 19, 20},
                {0, 2, 5, 6, 7, 8, 9, 11, 11, 13, 13, 18},
                {0, 1, 2, 4, 5, 7, 8, 8, 9, 9, 15, 19},
                {0, 2, 5, 7, 8, 9, 10, 10, 11, 14, 17, 21},
            };

            int c = f.GetLength(1) - 1; ;

            int[] expectedResult = new int[] { 7, 0, 2, 0, 2 };

            var gm = new DynamicMethod(f, c);
            var actualResult = gm.Solve();
            Assert.AreEqual(25, gm.ResultCost);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test()]
        public void DynamicMethodTest8()
        {
            var f = new int[,]
            {
                {0, 1, 2, 2, 2, 3, 5, 8, 9, 13, 14},
                {0, 1, 3, 4, 5, 5, 7, 7, 10, 12, 12},
                {0, 2, 2, 3, 4, 6, 6, 8, 9, 11, 17},
                {0, 1, 1, 1, 2, 3, 9, 9, 12, 12, 15},
                {0, 2, 7, 7, 7, 9, 9, 10, 11, 12, 13},
                {0, 2, 5, 5, 5, 6, 6, 7, 12, 18, 22},
            };

            int c = f.GetLength(1) - 1; ;

            int[] expectedResult = new int[] { 0, 0, 0, 0, 0, 10 };

            var gm = new DynamicMethod(f, c);
            var actualResult = gm.Solve();
            Assert.AreEqual(22, gm.ResultCost);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
