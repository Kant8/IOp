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

        public DenseVector ResultX { get; set; }
        public double ResultCost { get; set; }

        public int MeaningDecimals { get; set; }

        public int ResultMeaningDecimals { get; set; }

        #endregion

        #region Private Properties

        private DenseMatrix CurrA { get; set; }
        private DenseVector CurrB { get; set; }
        private DenseVector CurrC { get; set; }

        private bool IsFirstPhaseNeeded { get; set; }

        private int NonIntegerJ { get; set; }

        private List<int> J { get; set; }
        private List<int> FakeJ { get; set; }

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
            ResultMeaningDecimals = 4;
            IsFirstPhaseNeeded = true;
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
            CurrC = DenseVector.OfVector(-c); // !!! to min

            while (true)
            {
                var sms = new SimplexMethodSolver(CurrA, CurrB, CurrC) {MeaningDecimals = MeaningDecimals};
                sms.Solve();
                sms.RoundResult();

                if (AreAllInteger(sms.ResultX))
                {
                    RoundResult(sms);
                    ResultCost = sms.ResultCost;
                    ResultX = (DenseVector)sms.ResultX.SubVector(0, J.Count);
                    return ResultX;
                }
                else
                {
                    //DeleteUnnecessaryRestrictions(sms);
                    AddNewRestrictions(sms);
                }
            }
        }

        private void DeleteUnnecessaryRestrictions(SimplexMethodSolver solvedProblem)
        {
            var baseJ = solvedProblem.ResultBaseJ;
            foreach (var j0 in baseJ.Intersect(FakeJ))
            {
                var i0 = GetFakeRowIndex(j0);
                for (int i = i0 + 1; i < CurrA.RowCount; i++)
                {
                    for (int j = 0; j < j0; j++)
                    {
                        CurrA[i, j] = CurrA[i, j] - CurrA[i, j0]*CurrA[i0, j];
                    }
                    CurrA[i, j0] = 0;
                    CurrB[i] = CurrB[i] - CurrA[i, j0]*CurrB[i0];
                }
                DeleteRow(CurrA, i0);
                // delete from b
            }
        }

        private int GetFakeRowIndex(int j0)
        {
            return 0;
        }

        private void DeleteRow(DenseMatrix matrix, int rowIndex)
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
            
            var y = identity*baseA.Inverse();

            var a = y * CurrA;
            a = RoundVector((DenseVector)a, MeaningDecimals);
            var f = a - DenseVector.OfEnumerable(a.Select(Math.Floor));
            f = RoundVector((DenseVector)f, MeaningDecimals);

            //f = f - DenseVector.OfEnumerable(Enumerable.Repeat(Eps*2, f.Count));

            var b = y * CurrB;
            b = Math.Round(b, MeaningDecimals);
            b = b - Math.Floor(b);
            b = Math.Round(b, MeaningDecimals);


            InsertNewRestrictionRow((DenseVector)f, b);
            FakeJ.Add(a.Count);
        }

        private void InsertNewRestrictionRow(DenseVector f, double b)
        {
            CurrA = (DenseMatrix) CurrA.InsertRow(CurrA.RowCount, -f); // TODO: test this
            var newColumn = new DenseVector(CurrA.RowCount);
            newColumn[CurrA.RowCount - 1] = 1;
            CurrA = (DenseMatrix) CurrA.InsertColumn(CurrA.ColumnCount, newColumn); 

            var newB = new DenseVector(CurrB.Count + 1);
            newB.SetSubVector(0, CurrB.Count, CurrB);
            newB[CurrB.Count] = -b;
            CurrB = newB;

            var newC = new DenseVector(CurrC.Count + 1);
            newC.SetSubVector(0, CurrC.Count, CurrC);
            CurrC = newC;
        }

        private bool IsInteger(double x)
        {
            var localEps = Eps = Math.Round(Math.Pow(0.1, 4), ResultMeaningDecimals);
            return Math.Abs(Math.Round(x, 0) - x) < localEps;
        }

        private void RoundResult(SimplexMethodSolver sms)
        {
            sms.ResultX = RoundVector(sms.ResultX, ResultMeaningDecimals);
            sms.ResultCost = Math.Round(sms.ResultCost, ResultMeaningDecimals);
        }

        private DenseMatrix GetBaseMatrix(DenseMatrix matrix, IEnumerable<int> baseJ)
        {
            var resM = new DenseMatrix(baseJ.Count());
            int i = 0;
            foreach (var j in baseJ)
            {
                resM.SetColumn(i, matrix.Column(j));
                i++;
            }
            return resM;
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