using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNetTesting.MathUtills {
    internal class MatrixMath {
        public static double[,] dot(double[,] a, double[,] b) {
            double[,] dot = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++) {
                for (int j = 0; j < b.GetLength(1); j++) {
                    // the next loop looks way slow according to the profiler
                    for (int k = 0; k < b.GetLength(0); k++)
                        dot[i, j] += a[i, k] * b[k, j];
                }
            }
            return dot;
        }

        internal static double[,] add(double[,] a, double[,] b) {
            if(!(a.GetLength(0)==b.GetLength(0)&&a.GetLength(1)==b.GetLength(1))) {
                throw new ArgumentException("MatrixSizeMismatch");
            }
            double[,] sum = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++) {
                for (int j = 0; j < b.GetLength(1); j++) {
                        sum[i, j] = a[i, j] + b[i, j];
                }
            }
            return sum;
        }
        internal static double[,] subtract(double[,] a, double[,] b) {
            if (!(a.GetLength(0) == b.GetLength(0) && a.GetLength(1) == b.GetLength(1))) {
                throw new ArgumentException("MatrixSizeMismatch");
            }
            double[,] difference = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++) {
                for (int j = 0; j < b.GetLength(1); j++) {
                    difference[i, j] = a[i, j] - b[i, j];
                }
            }
            return difference;
        }
    }
}
