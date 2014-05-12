using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Kuchik.ORLab2
{
    public class DualSimplexMethod
    {
        private const double Eps = 1.0e-07;

        private static DenseMatrix _A = DenseMatrix.OfArray(
            new double[,]
                {
                    {7, 4, 1}
                });

        private static Vector _b = new DenseVector(
            new double[] { 13 });

        private static Vector _C = new DenseVector(
            new double[] { 21, 11, 0 });

        private static Vector<Double> _Yb = new DenseVector(
            new double[] { 0, 0 });

        private static int[] _Jb = { 2 };

        public static void DualSiplexMethod(TextWriter writer)
        {
            while (DualIteration())
            { }
            writer.WriteLine("Optimal basis:");
            writer.WriteLine(_Yb.ToString());
        }

        private static bool DualIteration()
        {
            // Step1
            var vectorCollection = new List<Vector<double>>();
            foreach (var item in _Jb)
            {
                vectorCollection.Add(_A.Column(item));
            }

            var aBMatrix = DenseMatrix.OfColumnVectors(vectorCollection.ToArray());

            var bMatrix = aBMatrix.Inverse();

            var kappa = bMatrix * _b;

            // Step2
            if (kappa.ToList().TrueForAll(x => x >= Eps))
            {
                //Logger.Log("Stopped at second step");
                return false;
            }

            // Step3
            var minJ = kappa.MinimumIndex();
            var deltaYT = DenseVector.Create(_Jb.Length, i => i == minJ ? 1 : 0) * DenseMatrix.OfColumnVectors(vectorCollection.ToArray()).Inverse();
            var nVector = new DenseVector(_A.ColumnCount);

            for (int i = 0; i < _A.ColumnCount; i++)
            {
                if (!_Jb.Contains(i))
                {
                    nVector[i] = (deltaYT * _A.Column(i));
                }
            }

            if (nVector.ToList().TrueForAll(n => n >= Eps))
            {
                //Logger.Log("Stopped at third step");
                return false;
            }

            // Step4
            double sigma0 = Double.PositiveInfinity;
            int sigmaIndex = 0;
            var sigmaList = new DenseVector(_A.ColumnCount);
            for (int i = 0, j = 0; i < _A.ColumnCount; i++)
            {
                if (!_Jb.Contains(i) && (nVector[i] < 0))
                {
                    sigmaList[i] = ((_C[i] - _A.Column(i) * _Yb) / nVector[i]);
                    if (sigmaList[i] < sigma0)
                    {
                        sigma0 = sigmaList[i];
                        sigmaIndex = i;
                    }
                }
            }

            // Step5
            var newYb = _Yb.Add(sigma0 * deltaYT);
            _Yb = newYb;
            _Jb[minJ] = sigmaIndex;

            return true;
        }
    }
}
