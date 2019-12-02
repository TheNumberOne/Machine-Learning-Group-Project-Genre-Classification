using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MachineLearningAttempt1.Data;

namespace MachineLearningAttempt1.KDTree {
    internal class KDTreeNode {

        private string _splittingWord;
        private string SplittingWord { get { return _splittingWord; } }

        private int _splittingValue;
        private int SplittingValue { get { return _splittingValue; } }

        private List<int> _containedBookIndices;
        private List<int> ContainedBookIndices { get { return _containedBookIndices; } }

        private KDTreeNode _parent;
        private KDTreeNode Parent { get { return _parent; } }

        private KDTreeNode _leftChild;
        private KDTreeNode LeftChild { get { return _leftChild; } }

        private KDTreeNode _rightChild;
        private KDTreeNode RightChild { get { return _rightChild; } }

        internal KDTreeNode(List<BookData> books, List<string> splittingWords, int maxBooksPerLeaf) {
            var initialIndices = new List<int>();
            int index = -1;
            foreach (var book in books) {
                index++;
                initialIndices.Add(index);
            }

            // Not checking for max book indices because our expected size is great enough it should not be a problem.
            Split(books, splittingWords, initialIndices, 0, maxBooksPerLeaf);
        }
        private KDTreeNode (KDTreeNode parent, List<BookData> books, List<string> splittingWords, List<int> splittingBookIndices, int lastSplitIndex, int maxBooksPerLeaf) {
            _parent = parent;

            if (splittingBookIndices.Count > maxBooksPerLeaf) {
                // Last split index + 1 to get current index.
                Split(books, splittingWords, splittingBookIndices, lastSplitIndex + 1, maxBooksPerLeaf);
            }
            else {
                // Each indes remaining is a leaf.
                _containedBookIndices = splittingBookIndices;
            }
        }


        /// <summary>
        /// This should be the only function setting any internal variables other than parent.
        /// </summary>
        private void Split(List<BookData> books, List<string> splittingWords, List<int> splittingBookIndices, int splitIndex, int maxBooksPerLeaf) {
            _splittingWord = splittingWords[splitIndex];

            books.OrderBy(book => book.WordCounts[SplittingWord]);

            int median = books[books.Count / 2].WordCounts[SplittingWord];
            if (books.Count % 2 == 0) {
                median = (median + books[(books.Count - 1) / 2].WordCounts[SplittingWord]) / 2;
            }
            _splittingValue = median;

            var leftIndices = new List<int>();
            var rightIndices = new List<int>();

            foreach (int i in splittingBookIndices) {
                if (books[i].WordCounts[SplittingWord] <= SplittingValue) {
                    leftIndices.Add(i);
                }
                else { 
                    rightIndices.Add(i);
                }
            }

            _leftChild = new KDTreeNode(this, books, splittingWords, leftIndices, splitIndex, maxBooksPerLeaf);
            _rightChild = new KDTreeNode(this, books, splittingWords, rightIndices, splitIndex, maxBooksPerLeaf);
        }

        public List<int> GetPotentialNearestNeighborIndices(BookData toGet, int minNeighborCount) {
            int count = toGet.WordCounts[SplittingWord];

            if (ContainedBookIndices != null) {
                return Parent.Parent.GetLeaves(minNeighborCount);
            }
            else if (count <= SplittingValue) {
                return LeftChild.GetPotentialNearestNeighborIndices(toGet, minNeighborCount);
            }
            else {
                return RightChild.GetPotentialNearestNeighborIndices(toGet, minNeighborCount);
            }
        }

        private List<int> GetLeaves(int minLeafCount = 0) {
            List<int> leaves = null;
            if (ContainedBookIndices != null) {
                leaves = ContainedBookIndices;
            }
            else {
                leaves = LeftChild.GetLeaves();
                leaves.AddRange(RightChild.GetLeaves());
            }

            if (leaves.Count < minLeafCount) {
                leaves = Parent.GetLeaves(minLeafCount);
            }

            return leaves;
        }
    }
}
