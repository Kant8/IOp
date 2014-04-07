using System;
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



        public PotentialMethod(Graph<int> orientedGraph)
        {
            S = orientedGraph;

            IsFirstPhaseNeeded = true;
        }

        public PotentialMethod(Graph<int> orientedGraph, Dictionary<Edge<int>, double> startX, List<Edge<int>> baseU)
        {
            S = orientedGraph;
            StartX = startX;
            StartBaseU = baseU;

            CurrX = StartX;
            CurrBaseU = StartBaseU;

            IsFirstPhaseNeeded = false;
        }

        public Dictionary<Edge<int>, double> Solve()
        {
            CheckRestictions();

            while (true)
            {
                CalcEstimations();

                if (IsOptimal())
                {
                    ResultX = new Dictionary<Edge<int>, double>(CurrX);
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
    }
}
