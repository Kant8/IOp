using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using NGenerics.DataStructures.General;

namespace IOpUtils
{
    public class AssingmentProblem
    {
        #region Public Properties
        
        public DenseMatrix Costs { get; set; }
        public SparseMatrix Assingment {get; set; }
        public double TotalCost { get; set; }

        #endregion Public Properties

        #region Private Properties

        private DenseMatrix ReducedCosts { get; set; }
        private int N { get; set; }
        private Dictionary<Edge<int>, double> MaxFlow { get; set; }
        private double MaxFlowCost { get; set; }
        private List<Vertex<int>> Cut { get; set; }

        #endregion Private Properties

        public AssingmentProblem(DenseMatrix costs)
        {
            Costs = (DenseMatrix)costs.Clone();
        }

        public SparseMatrix Solve()
        {
            N = Costs.RowCount;
            ReduceCosts();
            while (true)
            {
                CalcMaxAssignmentFlow();

                if (MaxFlowCost == N)
                {
                    return BuildResult();
                }
                RebuildReducedCosts();
            }
        }

        #region Private Methods

        private void ReduceCosts()
        {
            ReducedCosts = (DenseMatrix)Costs.Clone();

            for (int i = 0; i < N; i++)
            {
                var min = ReducedCosts.Row(i).Minimum();
                for (int j = 0; j < N; j++)
                {
                    ReducedCosts[i, j] -= min;
                }
            }
            //todo: maybe round result
            for (int j = 0; j < N; j++)
            {
                var min = ReducedCosts.Column(j).Minimum();
                for (int i = 0; i < N; i++)
                {
                    ReducedCosts[i, j] -= min;
                }
            }
            //todo: maybe round result
        }

        private void RebuildReducedCosts()
        {
            var incRowIndicies = Cut.Where(v => v.Data <= N).Select(v => v.Data - 1).ToList();
            var decColIndicies = Cut.Where(v => v.Data > N && v.Data <= 2*N).Select(v => v.Data - N - 1).ToList();

            double min = Double.MaxValue;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (incRowIndicies.Contains(i) && !decColIndicies.Contains(j))
                    {
                        if (ReducedCosts[i, j] < min)
                        {
                            min = ReducedCosts[i, j];
                        }
                    }
                }
            }

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (incRowIndicies.Contains(i) && !decColIndicies.Contains(j))
                    {
                        ReducedCosts[i, j] -= min;
                    }

                    if (!incRowIndicies.Contains(i) && decColIndicies.Contains(j))
                    {
                        ReducedCosts[i, j] += min;
                    }
                }
            }
        }

        private void CalcMaxAssignmentFlow()
        {
            Vertex<int> startVertex, endVertex;
            Graph<int> graph = BuildFlowGraph(out startVertex, out endVertex);

            var ffm = new FordFulkersonMethod(graph, startVertex, endVertex); //todo: use previous result as start
            MaxFlow = ffm.Solve();
            MaxFlowCost = ffm.ResultCost;
            Cut = ffm.LastMarkedVerticies.Except(new[] {startVertex, endVertex}).ToList();
        }

        private Graph<int> BuildFlowGraph(out Vertex<int> startVertex, out Vertex<int> endVertex)
        {
            const int startVertexIndex = 1;
            int endVertexIndex = 2 * N + 2;

            var edges = new List<double[]>();
            for (int i = 2; i <= N + 1; i++)
            {
                edges.Add(new double[] {startVertexIndex, i, 1});
            }

            for (int i = 2; i <= N + 1; i++)
            {
                edges.Add(new double[] {i + N, endVertexIndex, 1});
            }

            foreach (var e in ReducedCosts.IndexedEnumerator())
            {
                int from = e.Item1 + 2;
                int to = e.Item2 + N + 2;
                bool hasEdge = e.Item3 == 0;

                if (hasEdge)
                {
                    edges.Add(new double[] {from, to, 2});
                }
            }

            double[,] data = new double[edges.Count, 3];
            for (int i = 0; i < edges.Count; i++)
            {
                data[i, 0] = edges[i][0];
                data[i, 1] = edges[i][1];
                data[i, 2] = edges[i][2];
            }

            var graph = GraphHelper.CreateGraph(data);
            startVertex = graph.GetVertex(startVertexIndex - 1);
            endVertex = graph.GetVertex(endVertexIndex - 1);
            return graph;
        }

        private SparseMatrix BuildResult()
        {
            Assingment = new SparseMatrix(N);

            foreach (var ex in MaxFlow.Where(kvp => kvp.Key.FromVertex.Degree != N && kvp.Key.ToVertex.IncomingEdgeCount != N))
            {
                if (ex.Value > 0 )
                {
                    int i = ex.Key.FromVertex.Data - 1;
                    int j = ex.Key.ToVertex.Data - N - 1;
                    Assingment[i, j] = 1;
                }
            }

            TotalCost = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (Assingment[i, j] != 0)
                        TotalCost += Costs[i, j]*Assingment[i, j];
                }
            }
            return Assingment;
        }

        #endregion Private Methods
    }
}
