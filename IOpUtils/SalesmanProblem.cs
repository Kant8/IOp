using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using NGenerics.DataStructures.General;

namespace IOpUtils
{
    public class SalesmanProblem
    {
        #region Public Properties
        
        public DenseMatrix Costs { get; set; }
        public SparseMatrix OptimalPath { get; set; }
        public double OptimalCost { get; set; }

        #endregion Public Properties

        #region Private Properties

        private int N { get; set; }
        private List<int> RecordChain { get; set; }
        private double Record { get; set; }
        private bool AnswerFound { get; set; }

        #endregion Private Properties

        public SalesmanProblem(DenseMatrix costs)
        {
            Costs = (DenseMatrix)costs.Clone();
            Record = Double.PositiveInfinity;
            AnswerFound = false;
        }

        public SalesmanProblem(DenseMatrix costs, double startRecord, IEnumerable<int> startChain)
            : this(costs)
        {
            Record = startRecord;
            RecordChain = new List<int>(startChain);
            AnswerFound = true;
        }

        public SparseMatrix Solve()
        {
            N = Costs.RowCount;
            ModifyInitialCosts();

            var tasks = new Queue<SalesmanTask>();
            tasks.Enqueue(new SalesmanTask());

            do
            {
                var currTask = tasks.Dequeue();
                var ap = PrepareAssignmentProblem(currTask);
                var resultPath = ap.Solve();

                if (ap.TotalCost >= Record)
                    continue;

                var chains = GetChains(resultPath);

                if (chains.Count == 0)
                    throw new ArithmeticException("Problem has no chains!");

                if (chains.Count == 1)
                {
                    AnswerFound = true;
                    Record = ap.TotalCost;
                    RecordChain = chains.First();
                    continue;
                }

                var shortestChain = chains.First();
                foreach (var chain in chains.Skip(1))
                {
                    if (chain.Count < shortestChain.Count)
                    {
                        shortestChain = chain;
                    }
                }

                for (int i = 0; i < shortestChain.Count - 1; i++)
                {
                    var newTask = new SalesmanTask(currTask.InfiniteRoads);
                    newTask.AddInfiniteRoad(shortestChain[i], shortestChain[i + 1]);
                    tasks.Enqueue(newTask);
                }
            } while (tasks.Count != 0);

            if (!AnswerFound)
                throw new ArithmeticException("Problem has no chains!");

            return BuildResult();
        }

        #region Private Methods

        private void ModifyInitialCosts()
        {
            for (int i = 0; i < N; i++)
            {
                Costs[i, i] = Double.PositiveInfinity;
            }
        }

        private AssingmentProblem PrepareAssignmentProblem(SalesmanTask task)
        {
            DenseMatrix newCosts = (DenseMatrix)Costs.Clone();
            foreach (var infRoad in task.InfiniteRoads)
            {
                newCosts[infRoad.Item1, infRoad.Item2] = Double.PositiveInfinity;
            }
            var ap = new AssingmentProblem(newCosts);
            return ap;
        }

        private List<List<int>> GetChains(SparseMatrix path)
        {
            List<List<int>> chains = new List<List<int>>();

            var unusedPoints = Enumerable.Range(0, N).ToList();
            int startPoint = unusedPoints.First();
            do
            {
                var chain = new List<int>();
                int prevPoint = startPoint, nextPoint;
                chain.Add(startPoint);
                do
                {
                    nextPoint = path.Row(prevPoint).GetIndexedEnumerator().Select(p => p.Item1).First();
                    chain.Add(nextPoint);
                    prevPoint = nextPoint;
                } while (nextPoint != startPoint);

                chains.Add(chain);

                unusedPoints = unusedPoints.Except(chain).ToList();
                startPoint = unusedPoints.FirstOrDefault();
            } while (unusedPoints.Count != 0);

            return chains;
        }

        private SparseMatrix BuildResult()
        {
            OptimalCost = Record;

            OptimalPath = new SparseMatrix(N);
            for (int i = 0; i < RecordChain.Count - 1; i++)
            {
                OptimalPath[RecordChain[i], RecordChain[i + 1]] = 1;
            }
            return OptimalPath;
        }

        #endregion Private Methods

        private class SalesmanTask
        {
            public List<Tuple<int, int>> InfiniteRoads { get; private set; }

            public SalesmanTask()
            {
                InfiniteRoads = new List<Tuple<int, int>>();
            }

            public SalesmanTask(IEnumerable<Tuple<int, int>> infRoads)
            {
                InfiniteRoads = new List<Tuple<int, int>>();
                foreach (var road in infRoads)
                {
                    InfiniteRoads.Add(new Tuple<int, int>(road.Item1, road.Item2));
                }
            }

            public SalesmanTask AddInfiniteRoad(int from, int to)
            {
                InfiniteRoads.Add(new Tuple<int, int>(from, to));
                return this;
            }

            public SalesmanTask AddInfiniteRoads(IEnumerable<Tuple<int, int>> infRoads)
            {
                foreach (var road in infRoads)
                {
                    InfiniteRoads.Add(new Tuple<int, int>(road.Item1, road.Item2));
                }
                return this;
            }
        }
    }
}
