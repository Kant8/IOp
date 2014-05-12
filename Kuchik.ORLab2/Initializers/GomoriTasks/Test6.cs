using System;
using System.Collections.Generic;
using Kuchik.ORLab2.Interfaces;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Kuchik.ORLab2.Initializers.GomoriTasks
{
    // xo = (5 6 0 8 6 1 0) 
    public class Test6 : IGomoriInitializer
    {
        private Matrix<double> _a = 
            DenseMatrix.OfArray(new double[,]
                {
                    {0, 7, 1, -1, -4, 2, 4},
                    {5, 1, 4, 3, -5, 2, 1},
                    {2, 0, 3, 1, 0, 1, 5}
                });
        public Matrix<double> A
        {
            get { return _a; }
            set { _a = value; }
        }

        private Vector<double> _b = new DenseVector(new double[] { 12, 27, 19});
        public Vector<Double> b
        {
            get { return _b; }
            set { _b = value; }
        }

        private Vector<double> _c = new DenseVector(new double[] { 10, 2, 1, 7, 6, 3, 1 });
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
            return new Test6();
        }
    }
}
