using NNetTesting.MathUtills;
using NNetTesting.DatasetTools;
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
        public double[,] eval(double[,] input) {
            double[,] lastEval = input;
            foreach(Layer l in layers) {
                lastEval = l.eval(lastEval);
            }
            return lastEval;
        }
        private double cost(double[,] input, double[,] expectedInput) {
            double totalError = 0;
            double[,] difference = MatrixMath.subtract(expectedInput, input);
            totalError += difference[1, 0];
            totalError -= difference[0, 0];

            return Math.Pow(totalError,2);
        }
        public void Learn(SimpleDataset _dataset, double learnRate) {
            List<DataPoint> dataset = _dataset.trainingData;
            double h = 0.001f;
            double itterationCost = 0;
            int _batchSize = 50;
            int totalBatches = (int)(((double)dataset.Count / _batchSize) + .5);
            List<List<DataPoint>> batches = new List<List<DataPoint>>();
            for(int i = 0; i<totalBatches; i++){
                List<DataPoint> batch = new List<DataPoint>();
                for(int k = 0; k < _batchSize; k++) {
                    batch.Add(dataset[k]);
                }
                batches.Add(batch);
            }
            foreach (List<DataPoint> batch in batches) {
                foreach (Layer layer in layers) {
                    for (int i = 0; i < layer.weights.GetLength(0); i++) {
                        for (int j = 0; j < layer.weights.GetLength(1); j++) {
                            double oldCost = 0;
                            foreach (DataPoint point in batch) {
                                oldCost += cost(eval(new double[,] { { point.pos.X }, { point.pos.Y } }), new double[,] { { point.label.X }, { point.label.Y } });
                            }
                            oldCost /= batch.Count;
                            layer.weights[i, j] += h;
                            double newCost = 0;
                            foreach (DataPoint point in batch) {
                                newCost += cost(eval(new double[,] { { point.pos.X }, { point.pos.Y } }), new double[,] { { point.label.X }, { point.label.Y } });
                            }
                            newCost /= batch.Count;
                            layer.weights[i, j] -= h;
                            double deltaCost = (newCost - oldCost) / h;
                            layer.weights[i, j] -= deltaCost * learnRate;
                            // Console.WriteLine(oldCost + " : " + newCost + " : " + deltaCost * learnRate);
                        }
                    }
                    for (int i = 0; i < layer.biases.GetLength(0); i++) {
                        for (int j = 0; j < layer.biases.GetLength(1); j++) {
                            double oldCost = 0;
                            foreach (DataPoint point in batch) {
                                oldCost += cost(eval(new double[,] { { point.pos.X }, { point.pos.Y } }), new double[,] { { point.label.X }, { point.label.Y } });
                            }
                            oldCost /= batch.Count;
                            layer.biases[i, j] += h;
                            double newCost = 0;
                            foreach (DataPoint point in batch) {
                                newCost += cost(eval(new double[,] { { point.pos.X }, { point.pos.Y } }), new double[,] { { point.label.X }, { point.label.Y } });
                            }
                            newCost /= batch.Count;
                            layer.biases[i, j] -= h;
                            double deltaCost = (newCost - oldCost) / h;
                            itterationCost = (newCost > oldCost ? newCost : oldCost);
                            layer.biases[i, j] -= deltaCost * learnRate;
                            // Console.WriteLine(oldCost + " : " + newCost + " : " + deltaCost * learnRate);
                        }
                    }
                }
            }
            Console.WriteLine("Itteration done, test accuracy: "+Test(_dataset)+"/"+_dataset.testingData.Count+" Training accuracy: "+trainAccuracy(_dataset)+"/" + _dataset.trainingData.Count+". Learn rate: "+learnRate+". Cost: "+itterationCost);
            
        }
        public int Test(SimpleDataset dataset) {
            int accuracy = 0;
            foreach(DataPoint point in dataset.testingData) {
                double[,] guess = eval(new double[,] { { point.pos.X }, { point.pos.Y } });
                bool guessEval = guess[1, 0] < guess[0, 0];
                bool labelEval = point.label.X > point.label.Y;
                if (guessEval == labelEval) {
                    accuracy++;
                }
            }
            return accuracy;
        }
        public int trainAccuracy(SimpleDataset dataset) {
            int accuracy = 0;
            foreach (DataPoint point in dataset.trainingData) {
                double[,] guess = eval(new double[,] { { point.pos.X }, { point.pos.Y } });
                bool guessEval = guess[1, 0] < guess[0, 0];
                bool labelEval = point.label.X > point.label.Y;
                if (guessEval == labelEval) {
                    accuracy++;
                }
            }
            return accuracy;
        }
    }
}
