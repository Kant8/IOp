using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace IOpUtils
{
    public class BranchAndBoundMethod
    {
        #region Public Properties

        public DenseMatrix A { get; set; }
        public DenseVector b { get; set; }
        public DenseVector c { get; set; }
        public DenseVector dLower { get; set; }
        public DenseVector dUpper { get; set; }

        public List<int> IntegerJ { get; set; }
        
        public double StartRecord { get; set; }

        public DenseVector ResultX { get; set; }
        public double ResultCost { get; set; }

        public int MeaningDecimals { get; set; }

        #endregion

        #region Private Properties

        private Stack<BAndBTask> Tasks { get; set; }

        private int NonIntegerJ { get; set; }

        private double RecordCost { get; set; }
        private bool HasResult { get; set; }
        private DenseVector RecordX { get; set; }

        private double Eps { get; set; }

        #endregion

        #region Constructors

        public BranchAndBoundMethod(DenseMatrix a, DenseVector b, DenseVector c,
            DenseVector dLower, DenseVector dUpper, IEnumerable<int> integerJ,
            double startRecord)
        {
            A = DenseMatrix.OfMatrix(a);
            this.b = DenseVector.OfVector(b);
            this.c = DenseVector.OfVector(c);
            this.dLower = dLower;
            this.dUpper = dUpper;
            IntegerJ = new List<int>(integerJ);
            StartRecord = startRecord;
            MeaningDecimals = 8;
            HasResult = false;
        }

        #endregion

        #region Private Methods

        private bool AreAllInteger(DenseVector x)
        {
            NonIntegerJ = -1;
            foreach (var j in IntegerJ)
            {
                if (IsInteger(x[j])) 
                    continue;

                NonIntegerJ = j;
                return false;
            }
            return true;
        }

        private bool IsInteger(double x)
        {
            return Math.Abs(Math.Round(x, 0) - x) < Eps;
        }

        private BAndBTask MakeLowerTask(BAndBTask oldTask, DuplexMethodBoundsSolver solvedProblem)
        {
            var newStartX = DenseVector.OfVector(solvedProblem.ResultX);
            var newStartBaseJ = new List<int>(solvedProblem.ResultBaseJ);

            var newUpperBound = DenseVector.OfVector(oldTask.UpperBound);
            newUpperBound[NonIntegerJ] = Math.Floor(solvedProblem.ResultX[NonIntegerJ]);
            return new BAndBTask(oldTask.LowerBound, newUpperBound, newStartX, newStartBaseJ);
        }

        private BAndBTask MakeUpperTask(BAndBTask oldTask, DuplexMethodBoundsSolver solvedProblem)
        {
            var newStartX = DenseVector.OfVector(solvedProblem.ResultX);
            var newStartBaseJ = new List<int>(solvedProblem.ResultBaseJ);

            var newLowerBound = DenseVector.OfVector(oldTask.LowerBound);
            newLowerBound[NonIntegerJ] = Math.Floor(solvedProblem.ResultX[NonIntegerJ]) + 1;
            return new BAndBTask(newLowerBound, oldTask.UpperBound, newStartX, newStartBaseJ);
        }

        #endregion
        
        public DenseVector Solve()
        {
            Eps = Math.Round(Math.Pow(0.1, MeaningDecimals), MeaningDecimals);
            RecordCost = StartRecord;
            Tasks = new Stack<BAndBTask>();
            Tasks.Push(new BAndBTask(dLower, dUpper));

            while (Tasks.Any())
            {
                var task = Tasks.Pop();
                var dms = new DuplexMethodBoundsSolver(A, b, c, task.LowerBound, task.UpperBound,
                    task.StartX, task.StartBaseJ);
                
                try
                {
                    dms.Solve();
                    dms.RoundResult();
                }
                catch (ArithmeticException ex)
                {
                    continue;
                }

                if (dms.ResultCost <= RecordCost)
                    continue;

                if (AreAllInteger(dms.ResultX))
                {
                    RecordCost = dms.ResultCost;
                    RecordX = DenseVector.OfVector(dms.ResultX);
                    HasResult = true;
                }
                else
                {
                    Tasks.Push(MakeLowerTask(task, dms));
                    Tasks.Push(MakeUpperTask(task, dms));
                }
            }

            if (!HasResult)
                throw new ArithmeticException("Plans not found");

            ResultCost = RecordCost;
            ResultX = RecordX;
            return ResultX;
        }

        internal struct BAndBTask
        {
            public DenseVector LowerBound;
            public DenseVector UpperBound;
            public DenseVector StartX;
            public IEnumerable<int> StartBaseJ;

            public BAndBTask(DenseVector lowerBound, DenseVector upperBound)
            {
                LowerBound = DenseVector.OfVector(lowerBound);
                UpperBound = DenseVector.OfVector(upperBound);
                StartX = null;
                StartBaseJ = null;
            }

            public BAndBTask(DenseVector lowerBound, DenseVector upperBound, DenseVector startX, IEnumerable<int> startBaseJ)
            {
                LowerBound = DenseVector.OfVector(lowerBound);
                UpperBound = DenseVector.OfVector(upperBound);
                StartX = DenseVector.OfVector(startX);
                StartBaseJ = new List<int>(startBaseJ);
            }
        }
    }
}
