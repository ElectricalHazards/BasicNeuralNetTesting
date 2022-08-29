using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NNetTesting.MathUtills;


namespace NNetTesting.NetworkTools {
    internal class Layer {
        public double[,] weights;
        public double[,] biases;
        Random rand = new Random();
        public Layer(int inputNodes, int outputNodes) {
            weights = new double[outputNodes, inputNodes];
            biases = new double[outputNodes,1];
            for (int i = 0; i < weights.GetLength(0); i++) {
                for (int j = 0; j < weights.GetLength(1); j++) {
                    weights[i, j] = (double) ((double)rand.NextDouble()) - .5;
                }
            }
            for (int i = 0; i < biases.GetLength(0); i++) {
                for (int j = 0; j < biases.GetLength(1); j++) {
                    biases[i, j] = (double)((double)rand.NextDouble())-.5;
                }
            }
        }


        public double[,] eval(double[,] input) {
            double[,] weightedInputs = MatrixMath.dot(weights,input);
            double[,] biasedWeightedInputs = MatrixMath.add(weightedInputs, biases);
            double[,] activatedBiasedWeightedInputs = Node.activateNodes(biasedWeightedInputs);
            return activatedBiasedWeightedInputs;
        }
    }
}
