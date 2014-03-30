using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace IOpUtils
{
    public class GomoriMethod
    {
        #region Public Properties

        public DenseMatrix A { get; set; }
        public DenseVector b { get; set; }
        public DenseVector c { get; set; }

        public List<int> IntegerJ { get; set; }

        public List<int> StartBaseJ { get; set; }
        public DenseVector StartX { get; set; }
        public double StartRecord { get; set; }

        public DenseVector ResultX { get; set; }
        public double ResultCost { get; set; }

        public int MeaningDecimals { get; set; }

        #endregion

        #region Private Properties

        private DenseMatrix CurrA { get; set; }
        private DenseVector CurrB { get; set; }
        private DenseVector CurrC { get; set; }


        private bool IsFirstPhaseNeeded { get; set; }

        private int NonIntegerJ { get; set; }

        private List<int> J { get; set; }
        private List<int> FakeJ { get; set; }

        private double RecordCost { get; set; }
        private bool HasResult { get; set; }
        private DenseVector RecordX { get; set; }

        private double Eps { get; set; }

        #endregion

        #region Constructors

        public GomoriMethod(DenseMatrix a, DenseVector b, DenseVector c,
            IEnumerable<int> integerJ)
        {
            A = DenseMatrix.OfMatrix(a);
            this.b = DenseVector.OfVector(b);
            this.c = DenseVector.OfVector(c);
            IntegerJ = new List<int>(integerJ);
            MeaningDecimals = 8;
            HasResult = false;
            IsFirstPhaseNeeded = true;
        }

        public GomoriMethod(DenseMatrix a, DenseVector b, DenseVector c,
            IEnumerable<int> integerJ, DenseVector startX, IEnumerable<int> startBaseJ)
            : this(a, b, c, integerJ)
        {
            StartBaseJ = new List<int>(startBaseJ);
            StartX = DenseVector.OfVector(startX);
            IsFirstPhaseNeeded = false;
        }

        #endregion

        private bool AreAllInteger(DenseVector x)
        {
            return IntegerJ.All(j => IsInteger(x[j]));
        }

        public DenseVector Solve()
        {
            Eps = Math.Round(Math.Pow(0.1, MeaningDecimals), MeaningDecimals);
            J = Enumerable.Range(0, A.ColumnCount).ToList();
            FakeJ = new List<int>();
            CurrA = DenseMatrix.OfMatrix(A);
            CurrB = DenseVector.OfVector(b);
            CurrC = DenseVector.OfVector(c);

            var sms = new SimplexMethodSolver(CurrA, CurrB, CurrC);
            sms.Solve();
            sms.RoundResult();

            if (AreAllInteger(sms.ResultX))
            {
                RecordCost = sms.ResultCost;
                RecordX = DenseVector.OfVector(sms.ResultX);
                HasResult = true;
            }
            else
            {
                DeleteUnnecessaryRestrictions();

                AddNewRestrictions(sms);
            }


            ResultCost = RecordCost;
            ResultX = RecordX;
            return ResultX;
        }

        private void DeleteUnnecessaryRestrictions()
        {
        }

        private void AddNewRestrictions(SimplexMethodSolver solvedProblem)
        {
            var baseJ = solvedProblem.ResultBaseJ;
            var optimalX = solvedProblem.ResultX;

            foreach (var j in J.Intersect(baseJ).OrderBy(j => j))
            {
                if (!IsInteger(optimalX[j]))
                {
                    NonIntegerJ = j;
                    break;
                }
            }

            var identity = new DenseVector(baseJ.Count);
            identity[baseJ.IndexOf(NonIntegerJ)] = 1;
            var baseA = GetBaseMatrix(solvedProblem.A, baseJ);
            //var y = identity * 


        }

        private bool IsInteger(double x)
        {
            return Math.Abs(Math.Round(x, 0) - x) < Eps;
        }

        private DenseMatrix GetBaseMatrix(DenseMatrix matrix, IEnumerable<int> baseJ)
        {
            var resM = new DenseMatrix(matrix.RowCount, matrix.ColumnCount);
            int i = 0;
            foreach (var j in baseJ)
            {
                resM.SetColumn(i, matrix.Column(j));
                i++;
            }
            return resM;
        }
    }
}