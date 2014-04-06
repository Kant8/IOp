using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOpUtils;
using NGenerics.DataStructures.General;
using NUnit.Framework;
namespace IOpUtils.Tests
{
    [TestFixture()]
    public class PotentialMethodTests
    {
        [Test()]
        public void PotentialMethodTest()
        {
            var pm = new PotentialMethod(null);

            var v1 = new Vertex<int>(1);
            var v2 = new Vertex<int>(2);
            var e = new Edge<int>(v1, v2, 10, true);

            var g = new Graph<int>(true);
            g.AddVertex(v1);
            g.AddVertex(v2);
            g.AddEdge(e);
        }
    }
}
