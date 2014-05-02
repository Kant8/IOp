using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
namespace IOpUtils
{
    public class SimplexMethodSolver
    {
        #region J0Strategy enum
        public enum J0Strategy
        {
            MinimalJ0,
            Blend
        }
        #endregion

        #region Public Properties

        public DenseMatrix A { get; set; }
        public DenseVector b { get; set; }
        public DenseVector c { get; set; }

        public DenseVector StartX { get; set; }
        public List<int> StartBaseJ { get; set; }

        public J0Strategy Strategy { get; set; }

        public DenseVector ResultX { get; set; }
        public List<int> ResultBaseJ { get; set; }
        public double ResultCost { get; set; }

        public int MeaningDecimals { get; set; }

        #endregion

        #region Private Properties

        private bool IsFirstPhaseNeeded { get; set; }

        private DenseVector CurrX { get; set; }
        private List<int> CurrBaseJ { get; set; }
        private List<int> CurrNonBaseJ { get; set; }

        private DenseMatrix BaseA { get; set; }
        private DenseMatrix InvBaseA { get; set; }
        private DenseVector BaseC { get; set; }

        private DenseVector U { get; set; }
        private List<double> Estimations { get; set; }

        private int J0 { get; set; }
        private DenseVector Z { get; set; }
        private double Theta0 { get; set; }
        private int ThetaIndex { get; set; }

        private double Eps { get; set; }

        private DenseMatrix OldA { get; set; }
        private DenseVector Oldb { get; set; }
        private DenseVector Oldc { get; set; }

        #endregion

        #region Constructors

        public SimplexMethodSolver(DenseMatrix a, DenseVector b, DenseVector c,
            DenseVector startX, IEnumerable<int> startBaseJ, J0Strategy strategy = J0Strategy.Blend)
            : this(a, b, c, strategy)
        {
            this.StartX = DenseVector.OfVector(startX);
            this.StartBaseJ = new List<int>(startBaseJ);

            this.CurrX = DenseVector.OfVector(StartX);
            this.CurrBaseJ = new List<int>(StartBaseJ);

            this.IsFirstPhaseNeeded = false;
        }

        public SimplexMethodSolver(DenseMatrix a, DenseVector b, DenseVector c, J0Strategy strategy = J0Strategy.Blend)
        {
            this.A = DenseMatrix.OfMatrix(a);
            this.b = DenseVector.OfVector(b);
            this.c = DenseVector.OfVector(c);

            this.Strategy = strategy;

            this.MeaningDecimals = 8;

            this.IsFirstPhaseNeeded = true;
        }

        #endregion

        #region Private Methods

        private void CalcBaseAandC()
        {
            BaseA = new DenseMatrix(CurrBaseJ.Count);
            BaseC = new DenseVector(CurrBaseJ.Count);

            int i = 0;
            foreach (var j in CurrBaseJ)
            {
                BaseA.SetColumn(i, A.Column(j));
                BaseC[i] = c[j];
                i++;
            }

            InvBaseA = (DenseMatrix)BaseA.Inverse();

            CurrNonBaseJ = new List<int>(Enumerable.Range(0, A.ColumnCount).Except(CurrBaseJ));
        }

        private void CalcEstimations()
        {
            U = BaseC * InvBaseA;// (DenseVector)InvBaseA.LeftMultiply(BaseC);

            Estimations = new List<double>(Enumerable.Repeat(0.0, A.ColumnCount));
            foreach (var j in Enumerable.Range(0, A.ColumnCount))
            {
                Estimations[j] = U * A.Column(j) - c[j];
            }

            Estimations = Estimations.Select(x => Math.Round(x, MeaningDecimals)).ToList();
        }

        private bool IsOptimal()
        {
            foreach (var j in CurrNonBaseJ)
                if (Estimations[j] < 0)
                    return false;
            return true;
        }

        private void CalcJ0()
        {
            switch (Strategy)
            {
                case J0Strategy.MinimalJ0:
                    J0 = Estimations.IndexOf(Estimations.Min());
                    break;
                case J0Strategy.Blend:
                    J0 = Estimations.Select((e, j) => new { e, j })
                        .First(ej => ej.e < 0).j;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void CalcTheta()
        {
            Z = (DenseVector)InvBaseA.Multiply(A.Column(J0));

            List<double> thetas = new List<double>(A.RowCount);
            for (int i = 0; i < A.RowCount; i++)
            {
                if (Z[i] <= 0)
                    thetas.Add(Double.PositiveInfinity);
                else
                    thetas.Add(CurrX[CurrBaseJ[i]] / Z[i]);
            }


            switch (Strategy)
            {
                case J0Strategy.MinimalJ0:
                    Theta0 = thetas.Min();
                    ThetaIndex = thetas.IndexOf(Theta0);
                    break;
                case J0Strategy.Blend:
                    Theta0 = thetas.Min();
                    ThetaIndex = thetas.Select((t, i) => new { t, i })
                        .Where(theta => Math.Abs(theta.t - Theta0) < Eps)
                        .OrderBy(ti => CurrBaseJ[ti.i])
                        .First().i;
                    break;
                default:
                    throw new NotSupportedException();
            }

        }

        private bool IsUnbounded()
        {
            if (Double.IsPositiveInfinity(Theta0))
                return true;
            return false;
        }

        private void CalcNewBasePlan()
        {
            foreach (var j in CurrNonBaseJ)
            {
                if (j == J0)
                    CurrX[j] = Theta0;
                else
                    CurrX[j] = 0;
            }

            for (int i = 0; i < A.RowCount; i++)
            {
                CurrX[CurrBaseJ[i]] = CurrX[CurrBaseJ[i]] - Theta0 * Z[i];
            }

            CurrBaseJ[ThetaIndex] = J0;
        }

        private void BuildResult()
        {
            ResultX = (DenseVector)CurrX.Clone();
            ResultBaseJ = new List<int>(CurrBaseJ);

            double cost = 0;
            for (int j = 0; j < A.ColumnCount; j++)
            {
                cost += ResultX[j]*c[j];
            }
            ResultCost = cost;
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

        private void FirstPhase()
        {
            IsFirstPhaseNeeded = false;
            int m = b.Count;
            int n = A.ColumnCount;
            OldA = DenseMatrix.OfMatrix(A);
            Oldb = DenseVector.OfVector(b);
            Oldc = DenseVector.OfVector(c);

            for (int i = 0; i < m; i++)
            {
                if (b[i] < 0)
                {
                    b[i] = -b[i];
                    var newRow = A.Row(i).Select(number => -number).ToArray();
                    A.SetRow(i, newRow);
                }
            }

            StartX = new DenseVector(n + m);
            StartBaseJ = new List<int>(m);
            c = new DenseVector(n + m);
            for (int i = 0; i < m; i++)
            {
                StartX[n + i] = b[i];
                StartBaseJ.Add(n + i);
                c[n + i] = -1;
            }

            CurrX = DenseVector.OfVector(StartX);
            CurrBaseJ = new List<int>(StartBaseJ);

            A = (DenseMatrix)A.Append(DenseMatrix.Identity(b.Count));


            Solve();

            for (int i = 0; i < m; i++)
            {
                if (CurrX[n + i] > 0)
                    throw new ArithmeticException("Restrictions are not compatible. Problem has no plans.");
            }
            if (CurrBaseJ.Intersect(Enumerable.Range(n, m)).Count() != 0)
                throw new NotImplementedException();


            A = DenseMatrix.OfMatrix(OldA);
            b = DenseVector.OfVector(Oldb);
            c = DenseVector.OfVector(Oldc);
            StartX = (DenseVector)CurrX.SubVector(0, n);
            StartBaseJ = CurrBaseJ.Take(m).ToList(); // can be deleted

            CurrX = DenseVector.OfVector(StartX);
            CurrBaseJ = new List<int>(StartBaseJ);
        }

        #endregion

        public void FirstPhaseOnly()
        {
            Eps = Math.Round(Math.Pow(0.1, MeaningDecimals), MeaningDecimals);

            FirstPhase();
            ResultX = (DenseVector)StartX.Clone();
            ResultBaseJ = new List<int>(StartBaseJ);
        }

        public DenseVector Solve()
        {
            Eps = Math.Round(Math.Pow(0.1, MeaningDecimals), MeaningDecimals);

            if (IsFirstPhaseNeeded)
            {
                FirstPhase();
            }

            while (true)
            {
                CalcBaseAandC();

                CalcEstimations();

                if (IsOptimal())
                {
                    BuildResult();
                    return ResultX;
                }

                CalcJ0();

                CalcTheta();

                if (IsUnbounded())
                {
                    throw new ArithmeticException("Can't find optimal plan. Function is unbounded.");
                }

                CalcNewBasePlan();
            }
        }

        public void RoundResult()
        {
            ResultX = ResultX.Select(x => Math.Round(x, MeaningDecimals)).ToArray();
            ResultCost = Math.Round(ResultCost, MeaningDecimals);
        }
    }
}