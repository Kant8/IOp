using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.General;

namespace IOpUtils
{
    public class FordFulkersonMethod
    {
        #region Public Properties

        public Graph<int> S { get; set; }
        public Vertex<int> StartVertex { get; set; }
        public Vertex<int> EndVertex { get; set; }

        public Dictionary<Edge<int>, double> StartX { get; set; }
        public double StartCost { get; set; }
        public Dictionary<Edge<int>, double> ResultX { get; set; }
        public double ResultCost { get; set; }
        public List<Vertex<int>> LastMarkedVerticies { get; set; }

        #endregion

        #region Private Properties

        private Dictionary<Edge<int>, double> CurrX { get; set; }
        private double CurrCost { get; set; }

        private List<Vertex<int>> MarkedVerticies { get; set; }
        private int IterationIndex { get; set; }
        private int MarkedCount { get; set; }
        private Dictionary<Vertex<int>, Vertex<int>> PathFrom { get; set; }
        private Dictionary<Vertex<int>, int> VertexMarkIndex { get; set; }

        private double CostIncrease { get; set; }

        #endregion

        #region Constructors

        public FordFulkersonMethod(Graph<int> orientedGraph, Vertex<int> startVertex, Vertex<int> endVertex)
        {
            S = orientedGraph;
            StartVertex = startVertex;
            EndVertex = endVertex;
            StartX = new Dictionary<Edge<int>, double>();
            foreach (var edge in S.Edges)
                StartX.Add(edge, 0.0);
            StartCost = 0.0;

            CurrX = new Dictionary<Edge<int>, double>(StartX);
            CurrCost = StartCost;
        }

        public FordFulkersonMethod(Graph<int> orientedGraph, Vertex<int> startVertex, Vertex<int> endVertex, 
            Dictionary<Edge<int>, double> startX, double startCost)
        {
            S = orientedGraph;
            StartVertex = startVertex;
            EndVertex = endVertex;
            StartX = startX;
            StartCost = startCost;

            CurrX = new Dictionary<Edge<int>, double>(StartX);
            CurrCost = startCost;
        }

        #endregion Constructors

        public Dictionary<Edge<int>, double> Solve()
        {
            while (true)
            {
                var iPath = FindIncreasingPath();

                if (iPath.Count == 0)
                {
                    ResultX = new Dictionary<Edge<int>, double>(CurrX);
                    ResultCost = CurrCost;
                    LastMarkedVerticies = new List<Vertex<int>>(MarkedVerticies);
                    return ResultX;
                }

                IncreaseFlow(iPath);
            }
        }

        #region Private Methods

        private List<Edge<int>> FindIncreasingPath()
        {
            IterationIndex = 1;
            MarkedCount = 1;
            MarkedVerticies = new List<Vertex<int>> {StartVertex};
            PathFrom = new Dictionary<Vertex<int>, Vertex<int>> {{StartVertex, null}};
            VertexMarkIndex = new Dictionary<Vertex<int>, int> {{StartVertex, 1}};
            var currVertex = StartVertex;

            while (true)
            {
                ProcessEmanatingEdges(currVertex);

                ProcessIncomingEdges(currVertex);

                if (MarkedVerticies.Contains(EndVertex))
                {
                    return BuildIncreasingPath();
                }

                IterationIndex++;
                var vertexMark = VertexMarkIndex.Select(vi => new {Vertex = vi.Key, Index = vi.Value})
                    .FirstOrDefault(vi => vi.Index == IterationIndex);
                if (vertexMark == null)
                {
                    return new List<Edge<int>>();
                }

                currVertex = vertexMark.Vertex;
            }
        }

        private void ProcessEmanatingEdges(Vertex<int> vertex)
        {
            foreach (var edge in vertex.EmanatingEdges)
            {
                var partnerV = edge.GetPartnerVertex(vertex);
                if (MarkedVerticies.Contains(partnerV)) continue;

                if (CurrX[edge] < edge.Weight)
                {
                    PathFrom[partnerV] = vertex;
                    VertexMarkIndex[partnerV] = ++MarkedCount;
                    MarkedVerticies.Add(partnerV);
                }
            }
        }

        private void ProcessIncomingEdges(Vertex<int> vertex)
        {
            foreach (var edge in vertex.IncomingEdges())
            {
                var partnerV = edge.GetPartnerVertex(vertex);
                if (MarkedVerticies.Contains(partnerV)) continue;

                if (CurrX[edge] > 0.0)
                {
                    PathFrom[partnerV] = vertex;
                    VertexMarkIndex[partnerV] = ++MarkedCount;
                    MarkedVerticies.Add(partnerV);
                }
            }
        }

        private List<Edge<int>> BuildIncreasingPath()
        {

            var path = new List<Edge<int>>();
            var currVertex = EndVertex;
            CostIncrease = Double.PositiveInfinity;

            while (currVertex != StartVertex)
            {
                var partnerV = PathFrom[currVertex];
                var edge = currVertex.GetIncidentEdgeWith(partnerV);
                CostIncrease = edge.FromVertex == partnerV 
                    ? Math.Min(CostIncrease, edge.Weight - CurrX[edge]) 
                    : Math.Min(CostIncrease, CurrX[edge]);
                path.Add(edge);
                currVertex = partnerV;
            }
            return path.AsEnumerable().Reverse().ToList();
        }

        private void IncreaseFlow(List<Edge<int>> path)
        {
            var currVertex = StartVertex;
            foreach (var edge in path)
            {
                if (edge.FromVertex == currVertex)
                {
                    CurrX[edge] += CostIncrease;
                    currVertex = edge.ToVertex;
                }
                else
                {
                    CurrX[edge] -= CostIncrease;
                    currVertex = edge.FromVertex;
                }
            }
            CurrCost += CostIncrease;
        }

        #endregion Private Methods
    }
}
