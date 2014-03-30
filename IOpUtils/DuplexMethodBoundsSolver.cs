using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace IOpUtils
{
    public class DuplexMethodBoundsSolver
    {
        #region Public Properties

        public DenseMatrix A { get; set; }
        public DenseVector b { get; set; }
        public DenseVector c { get; set; }
        public DenseVector dLower { get; set; }
        public DenseVector dUpper { get; set; }


        public DenseVector StartY { get; set; }
        public List<int> StartBaseJ { get; set; }


        public DenseVector ResultX { get; set; }
        public List<int> ResultBaseJ { get; set; }
        public double ResultCost { get; set; }

        public int MeaningDecimals { get; set; }

        #endregion

        #region Private Properties

        private bool IsFirstPhaseNeeded { get; set; }

        private DenseVector CurrY { get; set; }
        private List<int> CurrBaseJ { get; set; }
        private List<int> CurrNonBaseJ { get; set; }
        private List<int> CurrNonBaseJPos { get; set; }
        private List<int> CurrNonBaseJNeg { get; set; }

        private DenseMatrix BaseA { get; set; }
        private DenseMatrix InvBaseA { get; set; }
        private DenseVector BaseC { get; set; }

        private DenseVector Kappas { get; set; }
        private DenseVector Estimations { get; set; }
        private DenseVector Mus { get; set; }
        private int MuJk { get; set; }

        private int Jk { get; set; }
        private double Sigma0 { get; set; }
        private int SigmaIndex { get; set; }

        private double Eps { get; set; }

        #endregion

        #region Constructors
        
        public DuplexMethodBoundsSolver(DenseMatrix a, DenseVector b, DenseVector c, DenseVector dLower, DenseVector dUpper,
            DenseVector startY, IEnumerable<int> startBaseJ)
            :this(a, b, c, dLower, dUpper)
        {
            if (startY == null || startBaseJ == null)
            {
                IsFirstPhaseNeeded = true;
                return;
            }
            this.StartY = DenseVector.OfVector(startY);
            this.StartBaseJ = new List<int>(startBaseJ);

            this.CurrY = DenseVector.OfVector(StartY);
            this.CurrBaseJ = new List<int>(StartBaseJ);

            this.IsFirstPhaseNeeded = false;
        }


        public DuplexMethodBoundsSolver(DenseMatrix a, DenseVector b, DenseVector c, DenseVector dLower, DenseVector dUpper)
        {
            this.A = DenseMatrix.OfMatrix(a);
            this.b = DenseVector.OfVector(b);
            this.c = DenseVector.OfVector(c);
            this.dLower = DenseVector.OfVector(dLower);
            this.dUpper = DenseVector.OfVector(dUpper);

            MeaningDecimals = 8;

            IsFirstPhaseNeeded = true;
        }

        #endregion

        #region Private Methods

        private void CalcBaseAandC()
        {
            BaseA = new DenseMatrix(CurrBaseJ.Count);
            BaseC = new DenseVector(A.RowCount);

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

        private void CalcFirstEstimations()
        {
            Kappas = (DenseVector)InvBaseA.Multiply(b);
            DenseVector y = BaseC*InvBaseA;

            Estimations = new DenseVector(A.ColumnCount);
            for (int j = 0; j < A.ColumnCount; j++)
            {
                Estimations[j] = y * A.Column(j) - c[j];
            }

            Estimations = RoundVector(Estimations, MeaningDecimals);            // ROUND

            CurrNonBaseJPos = new List<int>();
            CurrNonBaseJNeg = new List<int>();
            foreach (var j in CurrNonBaseJ)
            {
                if (Estimations[j] >= 0)
                {
                    CurrNonBaseJPos.Add(j);
                }
                else
                {
                    CurrNonBaseJNeg.Add(j);
                }
            }
        }

        private void CalcKappas()
        {
            Kappas = new DenseVector(A.ColumnCount);

            foreach (var j in CurrNonBaseJPos)
            {
                Kappas[j] = dLower[j];
            }

            foreach (var j in CurrNonBaseJNeg)
            {
                Kappas[j] = dUpper[j];
            }

            DenseVector AjKappaJSumm = new DenseVector(A.RowCount);
            foreach (var j in CurrNonBaseJ)
            {
                AjKappaJSumm += (DenseVector) A.Column(j)*Kappas[j];
            }
            DenseVector nonBaseKappa = InvBaseA*(b - AjKappaJSumm);

            int i = 0;
            foreach (var j in CurrBaseJ)
            {
                Kappas[j] = nonBaseKappa[i];
                i++;
            }

            Kappas = RoundVector(Kappas, MeaningDecimals);            // ROUND
        }

        private bool IsOptimal()
        {
            foreach (var j in CurrBaseJ)
                if (!(dLower[j] <= Kappas[j] && Kappas[j] <= dUpper[j]))
                {
                    Jk = j;
                    return false;
                }
            return true;
        }

        private void CalcMus()
        {
            if (Kappas[Jk] < dLower[Jk])
                MuJk = 1;
            else
                MuJk = -1;

            var identityVector = new DenseVector(A.RowCount);
            identityVector[CurrBaseJ.IndexOf(Jk)] = 1;
            var deltaY = MuJk*identityVector*InvBaseA;
            Mus = deltaY * A;

            Mus = RoundVector(Mus, MeaningDecimals);            // ROUND
        }

        private void CalcSigma()
        {
            List<double> sigmas = new List<double>(CurrNonBaseJ.Count);
            foreach (var j in CurrNonBaseJ)
            {
                if ((CurrNonBaseJNeg.Contains(j) && Mus[j] > 0)
                    || (CurrNonBaseJPos.Contains(j) && Mus[j] < 0))
                {
                    sigmas.Add(-Estimations[j]/Mus[j]);
                }
                else
                    sigmas.Add(Double.PositiveInfinity);
            }


            Sigma0 = sigmas.Min();
            SigmaIndex = CurrNonBaseJ[sigmas.IndexOf(Sigma0)];
        }

        private bool IsUnbounded()
        {
            if (Double.IsPositiveInfinity(Sigma0))
                return true;
            return false;
        }

        private void CalcNewBasePlan()
        {
            foreach (var j in CurrNonBaseJ.Concat(new[] { Jk }))
            {
                Estimations[j] = Estimations[j] + Sigma0*Mus[j];
            }

            foreach (var j in CurrBaseJ.Except(new[] { Jk }))
            {
                Estimations[j] = 0;
            }

            Estimations = RoundVector(Estimations, MeaningDecimals);            // ROUND

            CurrBaseJ = CurrBaseJ.Except(new[] {Jk}).Concat(new[] {SigmaIndex}).ToList();

            BaseA = new DenseMatrix(CurrBaseJ.Count);
            int i = 0;
            foreach (var j in CurrBaseJ)
            {
                BaseA.SetColumn(i, A.Column(j));
                BaseC[i] = c[j];
                i++;
            }
            InvBaseA = (DenseMatrix)BaseA.Inverse();

            CurrNonBaseJ = Enumerable.Range(0, A.ColumnCount).Except(CurrBaseJ).ToList();

            if (CurrNonBaseJPos.Contains(SigmaIndex))
            {
                if (MuJk == 1)
                {
                    CurrNonBaseJPos = CurrNonBaseJPos.Except(new[] { SigmaIndex }).Concat(new[] { Jk }).ToList();
                }
                else
                {
                    CurrNonBaseJPos = CurrNonBaseJPos.Except(new[] { SigmaIndex }).ToList();
                }
            }
            else
            {
                if (MuJk == 1)
                {
                    CurrNonBaseJPos = CurrNonBaseJPos.Concat(new[] { Jk }).ToList();
                }
                else
                {
                    CurrNonBaseJPos = CurrNonBaseJPos;
                }
            }

            CurrNonBaseJNeg = CurrNonBaseJ.Except(CurrNonBaseJPos).ToList();
        }

        private void BuildResult()
        {
            ResultX = DenseVector.OfVector(Kappas);

            ResultBaseJ = new List<int>(CurrBaseJ); 

            ResultCost = 0;
            for (int j = 0; j < A.ColumnCount; j++)
            {
                ResultCost += c[j] * ResultX[j];
            }
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
            int m = A.RowCount;
            int n = A.ColumnCount;
            bool isFound = false;
            int[] allJ = Enumerable.Range(0, n).ToArray();
            var subsets = allJ.Combinations(m);

            DenseVector y = null;
            List<int> baseJ = null;
            foreach (var subset in subsets)
            {
                isFound = true;
                baseJ = subset.ToList();
                DenseMatrix baseA = new DenseMatrix(m);
                DenseVector baseC = new DenseVector(m);

                int columnIndex = 0;
                foreach (var j in baseJ)
                {
                    baseA.SetColumn(columnIndex, A.Column(j));
                    baseC[columnIndex] = c[j];
                    columnIndex++;
                }
                DenseMatrix invA = (DenseMatrix)baseA.Inverse();
                if (invA.Values.Any(Double.IsNaN))
                {
                    continue;
                }
                y = baseC * invA;

                var testedC = A.Transpose() * y;
                for (int j = 0; j < c.Count; j++)
                {
                    if (testedC[j] < c[j])
                    {
                        isFound = false;
                        break;
                    }
                }
                if (isFound)
                    break;
            }

            if (!isFound)
            {
                throw new ArithmeticException("Restrictions are not compatible. Problem has no plans.");
            }

            StartY = (DenseVector)y.Clone();
            StartBaseJ = new List<int>(baseJ);

            CurrY = DenseVector.OfVector(StartY);
            CurrBaseJ = new List<int>(StartBaseJ);
        }

        #endregion

        public DenseVector Solve()
        {
            Eps = Math.Round(Math.Pow(0.1, MeaningDecimals), MeaningDecimals);

            if (IsFirstPhaseNeeded)
            {
                FirstPhase();
            }

            CalcBaseAandC();
            CalcFirstEstimations();

            while (true)
            {
                CalcKappas();

                if (IsOptimal())
                {
                    BuildResult();
                    return ResultX;
                }

                CalcMus();

                CalcSigma();

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

    public static class CombinationsHelper
    {
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0
                       ? new[] { new T[0] }
                       : elements.SelectMany((e, i) =>
                                             elements.Skip(i + 1).Combinations(k - 1)
                                                 .Select(c => (new[] { e }).Concat(c)));
        }
    }
}
