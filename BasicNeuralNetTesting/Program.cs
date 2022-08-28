using NNetTesting.DatasetTools;
using NNetTesting.NetworkTools;

namespace NNetTesting {
    public class Program {
        static readonly float TRAINING_TEST_PRECENTAGE = .8f;
        static readonly int DATASET_SIZE = 10;
        static readonly int[] NETWORK_SIZE = { 2, 3, 2 };
        
        public static void Main(string[] args) {

            SimpleDataset dataset = new SimpleDataset(DATASET_SIZE,TRAINING_TEST_PRECENTAGE);
            Network network = new Network(NETWORK_SIZE);
            //            Layer layer = new Layer(2,3);
            

        }
    }
}