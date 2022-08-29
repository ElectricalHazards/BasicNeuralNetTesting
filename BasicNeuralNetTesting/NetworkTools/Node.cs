using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNetTesting.NetworkTools {
    internal class Node {
        internal static double[,] activateNodes(double[,] a) {
            double[,] activatedInputs = new double[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++) {
                for (int j = 0; j < a.GetLength(1); j++) {
                    activatedInputs[i, j] = activation(a[i, j]);
                }
            }
            return activatedInputs;
        }

        private static double activation(double v) {
            return (1 / (1+Math.Exp(-1*v)));
        }
    }
}
