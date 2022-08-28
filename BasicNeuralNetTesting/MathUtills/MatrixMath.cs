using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNetTesting.MathUtills {
    internal class MatrixMath {
        public static float[,] dot(float[,] a, float[,] b) {
            float[,] dot = new float[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++) {
                for (int j = 0; j < b.GetLength(1); j++) {
                    // the next loop looks way slow according to the profiler
                    for (int k = 0; k < b.GetLength(0); k++)
                        dot[i, j] += a[i, k] * b[k, j];
                }
            }
            return dot;
        }

        internal static float[,] add(float[,] a, float[,] b) {
            if(!(a.GetLength(0)==b.GetLength(0)&&a.GetLength(1)==b.GetLength(1))) {
                throw new ArgumentException("MatrixSizeMismatch");
            }
            float[,] sum = new float[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++) {
                for (int j = 0; j < b.GetLength(1); j++) {
                        sum[i, j] = a[i, j] + b[i, j];
                }
            }
            return sum;
        }
    }
}
