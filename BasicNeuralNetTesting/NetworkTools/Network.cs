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
            double sumIn = 0;
            foreach(double d in input) {
                sumIn += d;
            }
            double sumExp = 0;
            foreach(double d in expectedInput) {
                sumExp += d;
            }
            totalError = sumIn - sumExp;
            return Math.Pow(totalError,2);
        }
        public void Learn(SimpleDataset _dataset, double learnRate) {
            List<DataPoint> dataset = _dataset.trainingData;
            double h = 0.001f;
            foreach (Layer layer in layers) {
                for (int i = 0; i < layer.weights.GetLength(0); i++) {
                    for (int j = 0; j < layer.weights.GetLength(1); j++) {
                        double oldCost = 0;
                        foreach (DataPoint point in dataset) {
                            oldCost += cost(eval(new double[,] { { point.pos.X }, { point.pos.Y } }), new double[,] { { point.label.X }, { point.label.Y } });
                        }
                        oldCost /= dataset.Count;
                        layer.weights[i, j] += h;
                        double newCost = 0;
                        foreach (DataPoint point in dataset) {
                            newCost += cost(eval(new double[,] { { point.pos.X }, { point.pos.Y } }), new double[,] { { point.label.X }, { point.label.Y } });
                        }
                        newCost /= dataset.Count;
                        layer.weights[i, j] -= h;
                        double deltaCost = (newCost-oldCost)/h;
                        layer.weights[i, j] += deltaCost * learnRate;   
                        Console.WriteLine(oldCost + " : " + newCost + " : " + deltaCost*learnRate);
                    }
                }
                for (int i = 0; i < layer.biases.GetLength(0); i++) {
                    for (int j = 0; j < layer.biases.GetLength(1); j++) {
                        double oldCost = 0;
                        foreach (DataPoint point in dataset) {
                            oldCost += cost(eval(new double[,] { { point.pos.X }, { point.pos.Y } }), new double[,] { { point.label.X }, { point.label.Y } });
                        }
                        oldCost /= dataset.Count;
                        layer.biases[i, j] += h;
                        double newCost = 0;
                        foreach (DataPoint point in dataset) {
                            newCost += cost(eval(new double[,] { { point.pos.X }, { point.pos.Y } }), new double[,] { { point.label.X }, { point.label.Y } });
                        }
                        newCost /= dataset.Count;
                        layer.biases[i, j] -= h;
                        double deltaCost = (newCost-oldCost)/h;
                        layer.biases[i, j] += deltaCost * learnRate;
                        Console.WriteLine(oldCost + " : " + newCost + " : " + deltaCost*learnRate);
                    }
                }
            }
            Console.WriteLine("Itteration done, test accuracy: "+Test(_dataset)+"/"+_dataset.testingData.Count+" Training accuracy: "+trainAccuracy(_dataset)+"/" + _dataset.trainingData.Count);
            
        }
        public int Test(SimpleDataset dataset) {
            int accuracy = 0;
            foreach(DataPoint point in dataset.testingData) {
                double[,] guess = eval(new double[,] { { point.pos.X }, { point.pos.Y } });
                bool guessEval = guess[1, 0] > guess[0, 0];
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
