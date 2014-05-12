using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kuchik.ORLab2.Interfaces;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Kuchik.ORLab2
{
    public class SimplexMethod
    {
        private IGomoriInitializer _task;
        private const double Eps = 1.0e-07;

        //private Matrix<double> _A;
        //private Vector<double> _b;
        //private Vector<double> _c;
        //private List<int> _jb;
        //private Vector<double> _xo;

        private TextWriter _writer;

        public SimplexMethod(TextWriter writer)
        {
            //_A = DenseMatrix.OfArray(new double[,]
            //    {
            //        {3, 1, 1, 0},
            //        {1, -2, 0, 1}
            //    });

            //_b = new DenseVector(new double[] { 1, 1 });

            //_c = new DenseVector(new double[] { 1, 4, 1, -1 });

            //_xo = new DenseVector(new double[] { 0, 0, 1, 1 });

            //_jb = new List<int>(new[] { 2, 3 });

            //_writer = writer;

            // Example 1
            //_A = DenseMatrix.OfArray(new double[,]
            //    {
            //        {1, 4, 4, 1},
            //        {1, 7, 8, 2}
            //    });

            //_b = new DenseVector(new double[] { 5, 9 });

            //_c = new DenseVector(new double[] { 1, -3, -5, -1 });

            //_xo = new DenseVector(new double[] { 1, 0, 1, 0 });

            //_jb = new List<int>(new[] { 0, 2 });

            //_writer = writer;

            //_A = DenseMatrix.OfArray(new double[,]
            //    {
            //        {1, 3, 1, 2},
            //        {2, 0, -1, 1}
            //    });

            //_b = new DenseVector(new double[] { 5, 1 });

            //_c = new DenseVector(new double[] { 1, 1, 1, 1 });

            //_xo = new DenseVector(new double[] { 0, 1, 0, 1 });

            //_jb = new List<int>(new[] { 1, 3 });

            //_writer = writer;

            //_A = DenseMatrix.OfArray(new double[,]
            //    {
            //        {3, 1, 2, 6, 9, 3},
            //        {1, 2, -1, 2, 3, 1}
            //    });

            //_b = new DenseVector(new double[] { 15, 5 });

            //_c = new DenseVector(new double[] { -2, 1, 1, -1, 4, 1 });

            //_xo = new DenseVector(new double[] { 1, 0, 0, 0, 0, 4 });

            //_jb = new List<int>(new[] { 2, 5 });

            _writer = writer;
        }



        public void Solve(IGomoriInitializer task)
        {
            _task = task;
            //_task.c = -_task.c;
            FirstPhase();
            //_writer.WriteLine(task.A*_task.xo);
        }

        private void FirstPhase()
        {
            var oldA = _task.A.Clone();

            var oldB = _task.b.Clone();
            for (int i = 0; i < _task.b.Count; i++)
            {
                if (_task.b[i] < 0)
                {
                    _task.b[i] *= -1;
                    _task.A.SetRow(i, -_task.A.Row(i));
                }
            }

            _task.A = DenseMatrix.Create(_task.A.RowCount, _task.A.ColumnCount + _task.A.RowCount,
                (i, j) => i < _task.A.RowCount && j < _task.A.ColumnCount ? _task.A[i, j] : (j - _task.A.ColumnCount == i ? 1 : 0));
            
            var artJ = new List<int>();
            for (int i = _task.c.Count; i < _task.A.ColumnCount; i++)
            {
                artJ.Add(i);
            }

            _task.Jb = new List<int>(artJ);

            Vector<double> oldC = _task.c.Clone();
            _task.c = DenseVector.Create(_task.c.Count + artJ.Count, i => i < oldC.Count ? 0 : -1);

            _task.xo = DenseVector.Create(_task.c.Count, i => i < oldC.Count ? 0 : _task.b[i - oldC.Count]);

            SecondPhase();

            bool check1 = artJ.All(j => Math.Abs(_task.xo[j]) < Eps);

            if (!check1)
            {
                _writer.WriteLine("Task has no plans.");
                return;
            }

            while (true)
            {
                int k = 0;
                var check2 = true;
                foreach (var j in artJ)
                {
                    if (_task.Jb.Contains(j))
                    {
                        k = j;
                        check2 = false;
                        break;
                    }
                }

                if (check2)
                {
                    _task.A = oldA.Clone();
                    _task.b = oldB.Clone();
                    _task.c = oldC.Clone();
                    _task.xo = DenseVector.Create(_task.c.Count, i => _task.xo[i]);
                    _task.Jb = _task.Jb.OrderBy(x => x).ToList();
                    SecondPhase();
                    _task.Jb = _task.Jb.OrderBy(x => x).ToList();
                    break;
                }
                else
                {
                    var _AbStar = DenseMatrix.Create(_task.A.RowCount, _task.Jb.Count(), (i, j) => _task.A[i, _task.Jb[j]]);
                    bool bCase = true;

                    for (int i = 0; i < _task.A.ColumnCount; i++ )
                    {
                        if (!_task.Jb.Contains(i))
                        {
                            var ek = DenseMatrix.Identity(_AbStar.ColumnCount).Column(_task.Jb.IndexOf(k));
                            var alpha = ek * _AbStar.Inverse() * _task.A.Column(i);
                            if (Math.Abs(alpha) > Eps)
                            {
                                // NOTE probably needs other including
                                _task.Jb[_task.Jb.IndexOf(k)] = i;
                                bCase = false;
                                break;
                            }
                        }
                    }

                    if (bCase)
                    {
                        var newA = DenseMatrix.Create(_task.A.RowCount - 1, _task.A.ColumnCount - 1, (i, j) => 0);
                        for (int i = 0; i < _task.A.RowCount; i++)
                        {
                            for (int j = 0; j < _task.A.ColumnCount; j++)
                            {
                                if (i < k - _task.A.RowCount)
                                {
                                    newA[i, j] = _task.A[i, j];
                                }
                                else if (i > k - _task.A.RowCount)
                                {
                                    newA[i - 1, j] = _task.A[i, j];
                                }
                            }
                        }
                        _task.A = newA.Clone();

                        _task.Jb.Remove(k);
                        artJ.Remove(k);
                    }
                }
            }
        }

        private void SecondPhase()
        {
            while (Iterate())
            {

            }
        }

        private bool Iterate()
        {
            // Step 1
            var cb = new DenseVector(_task.Jb.Count());
            for (int i = 0; i < _task.Jb.Count(); i++)
            {
                cb[i] = _task.c[_task.Jb[i]];
            }

            Matrix<double> bMatrix = new DenseMatrix(_task.Jb.Count());
            for (int i = 0; i < bMatrix.ColumnCount; i++)
            {
                bMatrix.SetColumn(i, _task.A.Column(_task.Jb[i]));
            }
            bMatrix = bMatrix.Inverse();

            var u = cb * bMatrix;
            var deltas = DenseVector.Create(
                _task.c.Count, i => _task.Jb.Contains(i) ? double.PositiveInfinity : u * _task.A.Column(i) - _task.c[i]);

            //Step 2
            if (deltas.ToList().TrueForAll(x => x > 0 || Math.Abs(x) < Eps))
            {
                // STOP vector is optimal
                //_writer.WriteLine("Optimal plan is found: {0}", _task.xo);
                //_writer.WriteLine("Target function value = {0}", _task.c * _task.xo);
                return false;
            }

            // Step 3
            int j0 = 0;
            for (int j = 0; j < deltas.Count; j++)
            {
                if (!_task.Jb.Contains(j) && deltas[j] < 0 && Math.Abs(deltas[j]) >= Eps)
                {
                    j0 = j;
                    break;
                }
            }

            var z = bMatrix * _task.A.Column(j0);

            if (z.ToList().TrueForAll(x => x < -Eps || Math.Abs(x) < Eps ))
            {
                // STOP target function is unlimited 
                _writer.WriteLine("Target function is unlimited");
                return false;
            }

            // Step 4
            var tetta0 = double.PositiveInfinity;
            var s = -1;
            for (int i = 0; i < z.Count; i++)
            {
                if (z[i] > 0 && Math.Abs(z[i]) > Eps)
                {
                    var tetta = _task.xo[_task.Jb[i]]/z[i];
                    if(double.IsPositiveInfinity(tetta0) || (Math.Abs(tetta0 - tetta) > Eps && tetta < tetta0))
                    {
                        tetta0 = tetta;
                        s = i;
                    }
                }
            }

            // Step 5
            var newXo = DenseVector.Create(_task.xo.Count, i => 0);
            for (int i = 0; i < _task.Jb.Count(); i++)
            {
                newXo[_task.Jb[i]] = _task.xo[_task.Jb[i]] - tetta0 * z[i];
            }
            newXo[j0] = tetta0;

            _task.xo = newXo;
            _task.Jb[s] = j0;

            // Step 6 is unneccesary
            return true;
        }
    }
}
