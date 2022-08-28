using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNetTesting.NetworkTools {
    internal class Network {

        private List<Layer> layers = new List<Layer>();
        public Network(int[] layers) {
            for (int i = 1; i < layers.Length; i++) {
                this.layers.Add(new Layer(layers[i - 1], layers[i]));
            }
        }
        public float[,] eval(float[,] input) {
            float[,] lastEval = input;
            foreach(Layer l in layers) {
                lastEval = l.eval(lastEval);
            }
            return lastEval;
        }
    }
}
