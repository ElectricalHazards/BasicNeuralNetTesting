using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NNetTesting.DatasetTools {
    internal class DataPoint {
        public Vector2 pos;
        public Vector2 label;
        public DataPoint(Vector2 pos, Vector2 label) {
            this.pos = pos;
            this.label = label;
        }
        public bool Equals(DataPoint other) {
            return pos.Equals(other.pos) && label.Equals(other.label);
        }
        public override String ToString() {
            return "(" + pos.X + "," + pos.Y + ") : " + label.X + "," + label.Y;
        }
    }
}
