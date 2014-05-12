using System;
using System.Collections.Generic;
using Kuchik.ORLab2.Interfaces;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Kuchik.ORLab2.Initializers
{
    // xo = (0 2 0 2 24 22) True
    public class Test1 : IGomoriInitializer
    {
        private Matrix<double> _a = 
            DenseMatrix.OfArray(new double[,]
                {
                    {1, -5, 3, 1, 0, 0},
                    {4, -1, 1, 0, 1, 0},
                    {2, 4, 2, 0, 0, 1}
                });
        public Matrix<double> A
        {
            get { return _a; }
            set { _a = value; }
        }

        private Vector<double> _b = new DenseVector(new double[] { -8, 22, 30});
        public Vector<Double> b
        {
            get { return _b; }
            set { _b = value; }
        }

        private Vector<double> _c = new DenseVector(new double[] { 7, -2, 6, 0, 5, 2 });
        public Vector<Double> c
        {
            get { return _c; }
            set { _c = value; }
        }

        public Vector<Double> dLower
        {
            get
            {
                return new DenseVector(new double[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            }
        }

        public Vector<Double> dUpper
        {
            get
            {
                return DenseVector.Create(8, i => Double.PositiveInfinity);
            }
        }

        private List<int> _j = new List<int>(new []{ 0 });
        public List<int> Jb
        {
            get { return _j; }
            set { _j = value; }
        }

        public Vector<double> _xo = new DenseVector(7, 0);
        public Vector<double> xo
        {
            get { return _xo; }
            set { _xo = value; }
        }

        public IGomoriInitializer Clone()
        {
            return new Test1();
        }
    }
}
