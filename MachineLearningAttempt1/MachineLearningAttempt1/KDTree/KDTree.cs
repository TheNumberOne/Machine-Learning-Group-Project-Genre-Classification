using System;
using System.Collections.Generic;
using System.Text;

namespace MachineLearningAttempt1.KDTree {
    public class KDTree {
        protected const double UNKNOWN = double.MaxValue;

        private List<DataType> _columnDataTypes;
        private InternalNode _rootNode;




        enum DataType {
            continuous,
            nominal
        }
    }
}
