using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace IOpUtils
{
    public class DynamicMethod
    {

        #region Public Properties

        public int[,] F { get; set; }
        public int C { get; set; }


        public int[] ResultX { get; set; }
        public int ResultCost { get; set; }

        #endregion

        #region Private Properties

        private int[,] B { get; set; }
        private int[,] BestXResult { get; set; }

        private int ProductsCount { get; set; }
        private int ResourcesCount { get; set; }

        #endregion

        #region Constructors

        public DynamicMethod(int[,] Fj, int c)
        {
            this.F = (int[,]) Fj.Clone();
            this.C = c;
            ProductsCount = Fj.GetLength(0);
            ResourcesCount = Fj.GetLength(1);
        }

        #endregion

        

        public int[] Solve()
        {
            B = new int[ProductsCount, ResourcesCount];
            BestXResult = new int[ProductsCount, ResourcesCount];
            for (int j = 0; j < ResourcesCount; j++)
                B[0, j] = F[0, j];


            for (int i = 1; i < ProductsCount; i++)
            {
                for (int y = 0; y < ResourcesCount; y++)
                {
                    int maxCost = F[i, 0] + B[i - 1, y - 0];
                    int maxCostIndex = 0;
                    for (int z = 1; z <= y; z++)
                    {
                        int cost = F[i, z] + B[i - 1, y - z];
                        if (cost > maxCost)
                        {
                            maxCost = cost;
                            maxCostIndex = z;
                        }
                    }
                    B[i, y] = maxCost;
                    BestXResult[i, y] = maxCostIndex;
                }
            }

            ResultCost = B[ProductsCount - 1, ResourcesCount - 1];

            int remainResources = C;
            ResultX = new int[ProductsCount];
            for (int i = ProductsCount - 1; i > 0; i--)
            {
                ResultX[i] = BestXResult[i, remainResources];
                remainResources -= ResultX[i];
            }
            ResultX[0] = remainResources;

            return ResultX;
        }
    }
}
