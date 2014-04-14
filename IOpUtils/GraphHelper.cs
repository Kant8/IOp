using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NGenerics.DataStructures.General;

namespace IOpUtils
{
    public static class GraphHelper
    {
        #region Extensions
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
        #endregion Extensions

        #region Print

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

        #endregion Print

        #region Creators

        /// <summary>
        /// Creates graph from template array
        /// </summary>
        /// <param name="data">row is one edge with format "from, to, weight"</param>
        /// <remarks>indexing is started from 1, but in graph will be from 0</remarks>
        /// <returns>oriented graph</returns>
        public static Graph<int> CreateGraph(double[,] data)
        {
            var g = new Graph<int>(true);

            var vs = new List<int>(data.GetLength(0) * 2 + 1);
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
                g.AddEdge(@from, to, data[i, 2]);
            }
            return g;
        }

        /// <summary>
        /// Creates graph from template array
        /// </summary>
        /// <param name="data">row is one edge with format "from, to, weight, startX, isBaseU"</param>
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
        /// <param name="data">row is one edge with format "from, to, weight, expectedX"</param>
        /// <param name="expectedX">expectedX or startX</param>
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
        /// <param name="data">row is one edge with format "from, to, weight, startX, isBaseU, expectedX"</param>
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

        #endregion Creators
    }
}
