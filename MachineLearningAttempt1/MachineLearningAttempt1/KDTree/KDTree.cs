using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using MachineLearningAttempt1.Data;

namespace MachineLearningAttempt1.KDTree {
    public class KDTree {
        private InternalNode _rootNode;

        public List<BookData> Books { get; set; }
        public int ColumnCount {
            get {
                return Books.ElementAt(0).WordCounts.Count;
            }
        }

        public KDTree(List<BookData> books) {
            Books = books;
            _rootNode = new InternalNode(this);
        }

        public List<BookData> GetPotentialNearestNeighbors(BookData book) {
            List<int> indices = _rootNode.GetPotentialNearestIndices(book);
            return indices.Select(index => Books.ElementAt(index)).ToList();
        }
    }
}
