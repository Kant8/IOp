using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kuchik.ORLab2.Initializers;
using Kuchik.ORLab2.Initializers.GomoriTasks;

namespace Kuchik.ORLab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var method = new GomoriMethod();
            method.Solve(new Test7(), Console.Out);

            //var method = new SimplexMethod(Console.Out);
            //method.Solve(new SimplexInit3());
            Console.ReadLine();
        }
    }
}
