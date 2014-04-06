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

        #endregion



        public PotentialMethod(Graph<int> orientedGraph)
        {
            S = orientedGraph;

            IsFirstPhaseNeeded = true;
        }

        public PotentialMethod(Graph<int> orientedGraph, List<Edge<int>> baseU)
        {
            S = orientedGraph;
            StartBaseU = baseU;

            IsFirstPhaseNeeded = false;
        }

        public void Solve()
        {
            CheckRestictions();
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
            var potentials = new List<double>(Enumerable.Repeat(0.0, CurrBaseU.Count));

            var scannedVerticies = new List<Vertex<int>>();
            var verticiesToScan = new Queue<Vertex<int>>();
            
            verticiesToScan.Enqueue(S.GetVertex(0));
            while (verticiesToScan.Count != 0)
            {
                var v = verticiesToScan.Dequeue();
                foreach (var e in v.EmanatingEdges)
                {
                    var partnerV = e.GetPartnerVertex(v);
                    if (scannedVerticies.Contains(partnerV)) continue;
                    potentials[partnerV.Data] = -e.Weight;
                    verticiesToScan.Enqueue(partnerV);
                }

                foreach (var e in v.IncidentEdges)
                {
                    var partnerV = e.GetPartnerVertex(v);
                    if (scannedVerticies.Contains(partnerV)) continue;
                    potentials[partnerV.Data] = e.Weight;
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
            var loop = new List<Edge<int>>(CurrBaseU) {InoptimalEdge};
        }


    }
}
