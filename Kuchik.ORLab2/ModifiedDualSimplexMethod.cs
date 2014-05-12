using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kuchik.ORLab2.Interfaces;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Kuchik.ORLab2
{
    public class ModifiedDualSimplexMethod
    {
        private const double Eps = 1.0e-07;

        private static List<int> _JNbUpper;
        private static List<int> _JNbLower;
        private static Vector<double> yVector;
        private static List<double> deltas;
        private static int _stopStep = 0;
        private static IGomoriInitializer _task;

        public static bool Solve(IGomoriInitializer initializer, TextWriter writer)
        {
            _task = initializer;
            // Step1
            var vectorCollection = new List<Vector<double>>();
            foreach (var item in _task.Jb)
            {
                vectorCollection.Add(_task.A.Column(item));
            }

            var aBMatrix = DenseMatrix.OfColumnVectors(vectorCollection.ToArray());

            var bMatrix = aBMatrix.Inverse();

            var cVector = new DenseVector(_task.Jb.Count);
            for (int i = 0; i < cVector.Count; i++)
            {
                cVector[i] = _task.c[_task.Jb[i]];
            }

            yVector = cVector * bMatrix;
            deltas = new List<double>();

            _JNbUpper = new List<int>();
            _JNbLower = new List<int>();

            for (int i = 0; i < _task.A.ColumnCount; i++)
            {
                var delta = yVector * _task.A.Column(i) - _task.c[i];
                deltas.Add(delta);
                if (!_task.Jb.Contains(i))
                {
                    if (delta >= Eps)
                    {
                        _JNbUpper.Add(i);
                    }
                    else
                    {
                        _JNbLower.Add(i);
                    }
                }
            }

            // Iterations
            while (ModificatedDualIteration())
            {

            }
            writer.WriteLine("Optimal basis:");
            writer.WriteLine(_task.xo.ToString());
            return _stopStep == 3;
        }

        private static bool ModificatedDualIteration()
        {
            var vectorCollection = new List<Vector<double>>();
            foreach (var item in _task.Jb)
            {
                vectorCollection.Add(_task.A.Column(item));
            }

            var aBMatrix = DenseMatrix.OfColumnVectors(vectorCollection.ToArray());

            var bMatrix = aBMatrix.Inverse();

            // Step3
            var kappaList = bMatrix * _task.b;
            Vector<double> allKappaList = new DenseVector(_task.A.ColumnCount, 0);
            for (int i = 0; i < allKappaList.Count; i++)
            {
                if (_JNbUpper.Contains(i))
                {
                    allKappaList[i] = _task.dLower[i];
                }
                else if (_JNbLower.Contains(i))
                {
                    allKappaList[i] = _task.dUpper[i];
                }
                else
                {
                    allKappaList[i] = 0;
                }
            }

            kappaList = bMatrix * (_task.b - _task.A * allKappaList);

            for (int i = 0; i < _task.Jb.Count; i++)
            {
                allKappaList[_task.Jb[i]] = kappaList[i];
            }

            // Step3
            double kappaValue = 0;
            int kappaIndex = 0;
            var check = false;
            for (int i = 0; i < allKappaList.Count; i++)
            {
                if (allKappaList[i] > _task.dUpper[i] || allKappaList[i] < _task.dLower[i])
                {
                    check = true;
                    kappaValue = allKappaList[i];
                    kappaIndex = i;
                    break;
                }
            }

            if (!check)
            {
                _task.xo = allKappaList;
                //Logger.Log("Stopped at third step");
                _stopStep = 3;
                return false;
            }

            // Step4 is unnecessary

            // Step5
            var nk = kappaValue < _task.dLower[kappaIndex] ? 1 : -1;
            var deltaYT =
                nk * DenseVector.Create(_task.Jb.Count, i => i == _task.Jb.ToList().IndexOf(kappaIndex) ? 1 : 0)
                * DenseMatrix.OfColumnVectors(vectorCollection.ToArray()).Inverse();

            var nVector = new DenseVector(_task.A.ColumnCount);
            for (int i = 0; i < _task.A.ColumnCount; i++)
            {
                if (!_task.Jb.Contains(i))
                {
                    nVector[i] = (deltaYT * _task.A.Column(i));
                }
            }

            // Step6
            Vector<double> sigmaVector = new DenseVector(_task.A.ColumnCount);
            for (int i = 0; i < sigmaVector.Count; i++)
            {
                if (_task.dLower[i] == _task.dUpper[i])
                {
                    sigmaVector[i] = double.PositiveInfinity;
                }
                else if (_JNbUpper.Contains(i) && nVector[i] < Eps)
                {
                    sigmaVector[i] = -deltas[i] / nVector[i];
                }
                else if (_JNbLower.Contains(i) && nVector[i] > Eps)
                {
                    sigmaVector[i] = -deltas[i] / nVector[i];
                }
                else
                {
                    sigmaVector[i] = double.PositiveInfinity;
                }
            }

            var sigma0 = sigmaVector.Min();
            var sigma0Index = sigmaVector.MinimumIndex();

            // Step7
            if (sigma0 == double.PositiveInfinity)
            {
                //Logger.Log("Stopped at seventh step");
                _stopStep = 7;
                return false;
            }

            // Step8
            Vector<double> newDeltas = new DenseVector(_task.A.ColumnCount);
            for (int i = 0; i < newDeltas.Count; i++)
            {
                if (_task.Jb.Contains(i) && i != kappaIndex)
                {
                    newDeltas[i] = 0;
                }
                else if (i == kappaIndex)
                {
                    newDeltas[i] = sigma0 * nk;
                }
                else
                {
                    newDeltas[i] = deltas[i] + sigma0 * nVector[i];
                }
            }

            deltas = newDeltas.ToList();
            // Step9
            _task.Jb[_task.Jb.ToList().IndexOf(kappaIndex)] = sigma0Index;

            // Step10
            if (nk == 1.0)
            {
                if (_JNbUpper.Contains(sigma0Index))
                {
                    _JNbUpper[_JNbUpper.IndexOf(sigma0Index)] = kappaIndex;
                }
                else
                {
                    _JNbUpper.Add(kappaIndex);
                }
            }
            else if (nk == -1.0)
            {
                if (_JNbUpper.Contains(sigma0Index))
                {
                    _JNbUpper.Remove(sigma0Index);
                }
            }

            _JNbLower.Clear();
            for (int i = 0; i < _task.A.ColumnCount; i++)
            {
                if (!_JNbUpper.Contains(i) && !_task.Jb.Contains(i))
                {
                    _JNbLower.Add(i);
                }
            }

            return true;
        }
    }
}
