using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Kuchik.ORLab2.Interfaces
{
    public interface IGomoriInitializer
    {
        Matrix<Double> A { get; set; }
        Vector<Double> b { get; set; }
        Vector<Double> c { get; set; }
        Vector<Double> dLower { get; }
        Vector<Double> dUpper { get; }
        List<int> Jb { get; set; }
        Vector<double> xo { get; set; }

        IGomoriInitializer Clone();
    }
}
