using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace NNetTesting.DatasetTools {
    internal class SimpleDataset {

        private readonly Vector2 maxLocals = new Vector2(10, 10);
        private readonly Func<float, float> curve = (n1) => (2.03f * MathF.Pow((n1 - 2.9f), 2) + 3.6f) * 0.5f * MathF.Sin(0.6f * n1);

        private List<DataPoint> dataPoints = new List<DataPoint>();
        public List<DataPoint> trainingData { get; private set; } = new List<DataPoint>();
        public List<DataPoint> testingData { get; private set; } = new List<DataPoint>();
        private Random rand = new Random();
        public SimpleDataset(int setSize = 100, float trainTestPrecent = .8f) {
            createDataset(setSize);
            splitData(trainTestPrecent);
        }

        private void splitData(float split) {
            int splitPoint = (int)(dataPoints.Count * split);
            for(int i = 0; i < dataPoints.Count; i++) {
                if (i < splitPoint) {
                    trainingData.Add(dataPoints[i]);
                } else if(i >= splitPoint) {
                    testingData.Add(dataPoints[i]);
                }
            }
        }

        private void createDataset(int size) {
            for(int i = 0; i < size; i++) {
                bool flag = true;
                 
                while (flag) {
                    Vector2 pos = new Vector2((float)rand.NextDouble()*maxLocals.X, (float)rand.NextDouble()*maxLocals.Y);
                    DataPoint newPoint = new DataPoint(pos, new Vector2((pos.Y < curve(pos.X) ? 1 : 0), (pos.Y < curve(pos.X) ? 0 : 1)));
                    //X 1 if "Inside" equation
                    //Y 1 if "outside" equation
                    flag = false;
                    foreach(DataPoint d in dataPoints) {
                        flag = d.Equals(newPoint);
                    }
                    if (!flag)
                        dataPoints.Add(newPoint);
                }
                
            }
        }

        
    
    }
}
