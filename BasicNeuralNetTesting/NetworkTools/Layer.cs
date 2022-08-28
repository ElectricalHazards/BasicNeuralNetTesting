using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NNetTesting.MathUtills;

namespace NNetTesting.NetworkTools {
    internal class Layer {
        private float[,] weights;
        private float[,] biases;
        public Layer(int inputNodes, int outputNodes) {
            weights = new float[inputNodes, outputNodes];
            biases = new float[1,outputNodes];
            

        }


        public float[,] eval(float[,] input) {
            float[,] weightedInputs = MatrixMath.dot(weights,input);
            float[,] biasedWeightedInputs = MatrixMath.add(weightedInputs, biases);
            float[,] activatedBiasedWeightedInputs = Node.activate(biasedWeightedInputs);
            return activatedBiasedWeightedInputs;
        }
    }
}
