﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NGenerics.DataStructures.General;

namespace IOpUtils
{
    public class PotentialMethod
    {
        #region Public Properties

        public Graph<int> S { get; set; }
        public List<Edge<int>> StartBaseU { get; set; }
        public Dictionary<Edge<int>, double> StartX { get; set; }
        public Dictionary<Edge<int>, double> ResultX { get; set; }
        public double ResultCost { get; set; }
        public List<Edge<int>> ResultBaseU { get; set; }

        public List<int> Production { get; set; }
        public bool IsFirstPhaseNeeded { get; set; }

        #endregion

        #region Private Properties

        private Dictionary<Edge<int>, double> CurrX { get; set; }

        private List<Edge<int>> CurrBaseU { get; set; }

        private List<Edge<int>> PositiveLoop { get; set; }
        private List<Edge<int>> NegativeLoop { get; set; }


        private Dictionary<Edge<int>, double> Estimations { get; set; }
        private Edge<int> InoptimalEdge { get; set; }

        private double Theta0 { get; set; }
        private Edge<int> ThetaEdge { get; set; }

        #endregion

        #region Constructors

        public PotentialMethod(Graph<int> orientedGraph, List<int> production)
        {
            S = orientedGraph;

            Production = production;
            IsFirstPhaseNeeded = true;
        }

        public PotentialMethod(Graph<int> orientedGraph, Dictionary<Edge<int>, double> startX, List<Edge<int>> baseU)
        {
            S = orientedGraph;
            StartX = startX;
            StartBaseU = baseU;

            CurrX = new Dictionary<Edge<int>,double>(StartX);
            CurrBaseU = new List<Edge<int>>(StartBaseU);

            IsFirstPhaseNeeded = false;
        }

        #endregion Constructors

        public Dictionary<Edge<int>, double> Solve()
        {
            CheckRestictions();

            if (IsFirstPhaseNeeded)
            {
                FirstPhase();
            }

            while (true)
            {
                CalcEstimations();

                if (IsOptimal())
                {
                    ResultX = new Dictionary<Edge<int>, double>(CurrX);
                    ResultCost = ResultX.Sum(x => x.Key.Weight*x.Value);
                    ResultBaseU = CurrBaseU;
                    return ResultX;
                }

                CalcPosAndNegLoops();

                if (IsUnbounded())
                {
                    throw new ArithmeticException("Function is unbounded.");
                }

                CalcTheta();

                CalcNewPlan();
            }
        }

        public void FirstPhase()
        {
            IsFirstPhaseNeeded = false;

            if (Production.Sum() != 0)
                throw new ArithmeticException("Production sum != 0. Problem has no flows");

            var fakeGraph = S.Clone();
            var fakeVertex = new Vertex<int>(S.Vertices.Count);
            fakeGraph.AddVertex(fakeVertex);

            foreach (var e in fakeGraph.Edges)
            {
                e.Weight = 0;
            }

            var fakeEdges = new List<Edge<int>>();
            foreach (var v in fakeGraph.Vertices)
            {
                if (v == fakeVertex) continue;

                var fakeEdge = Production[v.Data] > 0 ? new Edge<int>(v, fakeVertex, true) : new Edge<int>(fakeVertex, v, true);
                fakeEdge.Weight = 1;
                fakeEdges.Add(fakeEdge);
                fakeGraph.AddEdge(fakeEdge);
            }

            var fakeStartX = fakeGraph.Edges.ToDictionary<Edge<int>, Edge<int>, double>(e => e, e => 0);
            foreach (var fe in fakeEdges)
            {
                if (fe.FromVertex == fakeVertex)
                    fakeStartX[fe] = -Production[fe.ToVertex.Data];
                else
                    fakeStartX[fe] = Production[fe.FromVertex.Data];
            }

            var fakePm = new PotentialMethod(fakeGraph, fakeStartX, new List<Edge<int>>(fakeEdges));
            var resX = fakePm.Solve();
            var resU = fakePm.ResultBaseU;


            if (fakeEdges.Any(fe => resX[fe] != 0.0))
            {
                throw new ArithmeticException("Restrictions are not compatible. " +
                                              "Problem has no flows");
            }

            StartX = new Dictionary<Edge<int>, double>();
            StartBaseU = new List<Edge<int>>();

            var realEdges = fakeGraph.Edges.Except(fakeEdges).ToList();
            var startXFromFake = resX.Where(pair => realEdges.Contains(pair.Key)).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var fakeX in startXFromFake)
            {
                StartX.Add(S.GetEdge(fakeX.Key.FromVertex.Data, fakeX.Key.ToVertex.Data), fakeX.Value);
            }

            while (true)
            {
                var edgesToDelete = resU.Intersect(fakeEdges).ToList();
                if (edgesToDelete.Count == 1)
                {
                    var startBaseUFromFake = resU.Except(edgesToDelete).ToList();
                    foreach (var fakeU in startBaseUFromFake)
                    {
                        StartBaseU.Add(S.GetEdge(fakeU.FromVertex.Data, fakeU.ToVertex.Data));
                    }
                    break;
                }
                foreach (var edge in realEdges.Except(resU))
                {
                    var loop = GetLoop(new List<Edge<int>>(resU) {edge});

                    var fakeEdgesInLoop = loop.Intersect(fakeEdges).ToList();

                    if (fakeEdgesInLoop.Count == 2)
                    {
                        resU.Remove(fakeEdgesInLoop.First());
                        resU.Add(edge);
                        break;
                    }

                }
            }

            CurrX = new Dictionary<Edge<int>,double>(StartX);
            CurrBaseU = new List<Edge<int>>(StartBaseU);
        }

        #region Private Methods

        private void CheckRestictions()
        {
            double sum = S.Vertices.Sum(v => v.Weight);
            if (sum != 0)
                throw new ArithmeticException("Restrictions are not compatible. " +
                                              "Problem has no flows");
        }

        private void CalcEstimations()
        {
            var potentials = new List<double>(Enumerable.Repeat(0.0, CurrBaseU.Count + 1));

            var scannedVerticies = new List<Vertex<int>>();
            var verticiesToScan = new Queue<Vertex<int>>();
            
            verticiesToScan.Enqueue(S.Vertices.First());
            while (verticiesToScan.Count != 0)
            {
                var v = verticiesToScan.Dequeue();
                foreach (var e in v.BaseEmanatingEdges(CurrBaseU))
                {
                    var partnerV = e.GetPartnerVertex(v);
                    if (scannedVerticies.Contains(partnerV)) continue;
                    potentials[partnerV.Data] = potentials[v.Data] - e.Weight;
                    verticiesToScan.Enqueue(partnerV);
                }

                foreach (var e in v.BaseIncomingEdges(CurrBaseU))
                {
                    var partnerV = e.GetPartnerVertex(v);
                    if (scannedVerticies.Contains(partnerV)) continue;
                    potentials[partnerV.Data] = potentials[v.Data] + e.Weight;
                    verticiesToScan.Enqueue(partnerV);
                }
                scannedVerticies.Add(v);
            }

            Estimations = new Dictionary<Edge<int>, double>();
            foreach (var e in S.Edges)
            {
                int i = e.FromVertex.Data;
                int j = e.ToVertex.Data;
                Estimations[e] = potentials[i] - potentials[j] - e.Weight;
            }
        }

        private bool IsOptimal()
        {
            foreach (var est in Estimations)
            {
                if (est.Value > 0)
                {
                    InoptimalEdge = est.Key;
                    return false;
                }
            }
            return true;
        }

        private void CalcPosAndNegLoops()
        {
            var loopedTree = new List<Edge<int>>(CurrBaseU) {InoptimalEdge};
            var loop = GetLoop(loopedTree);
            var posLoop = new List<Edge<int>>();
            var negLoop = new List<Edge<int>>();

            var currEdge = InoptimalEdge;
            bool isPositiveDirection = true;
            do
            {
                var currVertex = isPositiveDirection ? currEdge.ToVertex : currEdge.FromVertex;
                var outEdges = currVertex.BaseEmanatingEdges(loop);
                var inEdges = currVertex.BaseIncomingEdges(loop);

                if (isPositiveDirection)
                {
                    if (outEdges.Count == 1)
                    {
                        currEdge = outEdges.First();
                        posLoop.Add(currEdge);
                    }
                    else
                    {
                        currEdge = inEdges.First(e => e != currEdge);
                        negLoop.Add(currEdge);
                        isPositiveDirection = false;
                    }
                }
                else 
                {
                    if (inEdges.Count == 1)
                    {
                        currEdge = inEdges.First();
                        negLoop.Add(currEdge);
                    }
                    else
                    {
                        currEdge = outEdges.First(e => e != currEdge);
                        posLoop.Add(currEdge);
                        isPositiveDirection = true;
                    }
                }
            } while (currEdge != InoptimalEdge);

            PositiveLoop = posLoop;
            NegativeLoop = negLoop;
        }

        private List<Edge<int>> GetLoop(List<Edge<int>> edges)
        {
            var loop = new List<Edge<int>>(edges);
            var nextLoop = new List<Edge<int>>(loop);

            bool wasRemoved = true;
            while (wasRemoved)
            {
                wasRemoved = false;
                foreach (var edge in loop)
                {
                    if (edge.FromVertex.BaseIncidentEdges(loop).Count == 1)
                    {
                        nextLoop.Remove(edge);
                        wasRemoved = true;
                    }

                    if (edge.ToVertex.BaseIncidentEdges(loop).Count == 1)
                    {
                        nextLoop.Remove(edge);
                        wasRemoved = true;
                    }
                }
                loop = new List<Edge<int>>(nextLoop);
            }
            return loop;
        }

        private bool IsUnbounded()
        {
            if (NegativeLoop.Count == 0)
            {
                return true;
            }
            return false;
        }

        private void CalcTheta()
        {
            var negX = CurrX.Where(x => NegativeLoop.Contains(x.Key)).ToList();
            var min = negX.First();
            foreach (var x in negX)
            {
                if (x.Value < min.Value)
                {
                    min = x;
                }
            }
            Theta0 = min.Value;
            ThetaEdge = min.Key;
        }

        private void CalcNewPlan()
        {
            foreach (var e in NegativeLoop)
            {
                CurrX[e] -= Theta0;
            }

            foreach (var e in PositiveLoop)
            {
                CurrX[e] += Theta0;
            }

            CurrBaseU.Remove(ThetaEdge);
            CurrBaseU.Add(InoptimalEdge);
        }

        #endregion Private Methods

        #region Helper Methods

        public static string PrintX(Dictionary<Edge<int>, double> x)
        {
            var sb = new StringBuilder();
            foreach (var pair in x)
            {
                sb.AppendFormat("{0} -> {1} : {2}\n", pair.Key.FromVertex.Data + 1,
                                pair.Key.ToVertex.Data + 1, pair.Value);
            }
            return sb.ToString();
        }

        public static string PrintTree(List<Edge<int>> tree)
        {
            var sb = new StringBuilder();
            foreach (var node in tree)
            {
                sb.AppendFormat("{0} -> {1}\n", node.FromVertex.Data + 1, node.ToVertex.Data + 1);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates graph from template array
        /// </summary>
        /// <param name="data">row is one edge with format "from, to, cost"</param>
        /// <remarks>indexing is started from 1</remarks>
        /// <returns>oriented graph</returns>
        public static Graph<int> CreateGraph(double[,] data)
        {
            var g = new Graph<int>(true);

            var vs = new List<int>(data.GetLength(0)*2+1);
            vs.Add(-1);
            for (int i = 0; i < data.GetLength(0); i++)
            {
                vs.Add((int)data[i, 0] - 1);
                vs.Add((int)data[i, 1] - 1);
            }
            vs = vs.Distinct().OrderBy(v => v).ToList();
            foreach (var v in vs.Skip(1))
            {
                g.AddVertex(v);
            }

            for (int i = 0; i < data.GetLength(0); i++)
            {
                var from = g.GetVertex(vs[(int)data[i, 0]]);
                var to = g.GetVertex(vs[(int)data[i, 1]]);
                g.AddEdge(from, to, data[i, 2]);
            }
            return g;
        }

        /// <summary>
        /// Creates graph from template array
        /// </summary>
        /// <param name="data">row is one edge with format "from, to, cost, startX, isBaseU"</param>
        /// <param name="startX">startX</param>
        /// <param name="baseU">baseU</param>
        /// <remarks>indexing is started from 1, isBaseU should be 0 or 1</remarks>
        /// <returns></returns>
        public static Graph<int> CreateGraph(double[,] data, out Dictionary<Edge<int>, double> startX,
            out List<Edge<int>> baseU)
        {
            var g = CreateGraph(data);

            startX = new Dictionary<Edge<int>, double>(data.GetLength(0));
            baseU = new List<Edge<int>>(data.GetLength(0));
            for (int i = 0; i < data.GetLength(0); i++)
            {
                var edge = g.GetEdge((int)data[i, 0] - 1, (int)data[i, 1] - 1);
                startX.Add(edge, data[i, 3]);
                if ((int)data[i, 4] == 1)
                {
                    baseU.Add(edge);
                }
            }
            return g;
        }

        /// <summary>
        /// Creates graph from template array
        /// </summary>
        /// <param name="data">row is one edge with format "from, to, cost, expectedX"</param>
        /// <param name="expectedX">expectedX</param>
        /// <remarks>indexing is started from 1</remarks>
        /// <returns></returns>
        public static Graph<int> CreateGraph(double[,] data, out Dictionary<Edge<int>, double> expectedX)
        {
            var g = CreateGraph(data);

            expectedX = new Dictionary<Edge<int>, double>(data.GetLength(0));
            for (int i = 0; i < data.GetLength(0); i++)
            {
                var edge = g.GetEdge((int)data[i, 0] - 1, (int)data[i, 1] - 1);
                expectedX.Add(edge, data[i, 3]);
            }
            return g;
        }

        /// <summary>
        /// Creates graph from template array
        /// </summary>
        /// <param name="data">row is one edge with format "from, to, cost, startX, isBaseU, expectedX"</param>
        /// <param name="startX">startX</param>
        /// <param name="baseU">baseU</param>
        /// <param name="expectedX">expectedX</param>
        /// <remarks>indexing is started from 1, isBaseU should be 0 or 1</remarks>
        /// <returns></returns>
        public static Graph<int> CreateGraph(double[,] data, out Dictionary<Edge<int>, double> startX,
            out List<Edge<int>> baseU, out Dictionary<Edge<int>, double> expectedX)
        {
            var g = CreateGraph(data, out startX, out baseU);
            
            expectedX = new Dictionary<Edge<int>, double>(data.GetLength(0));
            for (int i = 0; i < data.GetLength(0); i++)
            {
                var edge = g.GetEdge((int)data[i, 0] - 1, (int)data[i, 1] - 1);
                expectedX.Add(edge, data[i, 5]);
            }
            return g;
        }

        

        #endregion Helper Methods
    }

    public static class GraphHelper
    {
        public static IList<Edge<T>> IncomingEdges<T>(this Vertex<T> v)
        {
            return v.IncidentEdges.Except(v.EmanatingEdges).ToList();
        }

        public static IList<Edge<T>> BaseIncomingEdges<T>(this Vertex<T> v, List<Edge<T>> baseU)
        {
            return v.IncomingEdges().Where(baseU.Contains).ToList();
        }

        public static IList<Edge<T>> BaseEmanatingEdges<T>(this Vertex<T> v, List<Edge<T>> baseU)
        {
            return v.EmanatingEdges.Where(baseU.Contains).ToList();
        }

        public static IList<Edge<T>> BaseIncidentEdges<T>(this Vertex<T> v, List<Edge<T>> baseU)
        {
            return v.IncidentEdges.Where(baseU.Contains).ToList();
        }

        public static Graph<T> Clone<T>(this Graph<T> graph)
        {
            var res = new Graph<T>(graph.IsDirected);

            foreach (var v in graph.Vertices)
            {
                var newV = new Vertex<T>(v.Data, v.Weight);
                res.AddVertex(newV);
            }

            foreach (var e in graph.Edges)
            {
                var newE = new Edge<T>(res.GetVertex(e.FromVertex.Data), res.GetVertex(e.ToVertex.Data), e.Weight, true);
                newE.Tag = e.Tag;
                res.AddEdge(newE);
            }

            return res;
        }
    }
}
