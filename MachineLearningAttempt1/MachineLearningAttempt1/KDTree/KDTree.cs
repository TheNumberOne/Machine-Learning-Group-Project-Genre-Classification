using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using MachineLearningAttempt1.Data;

namespace MachineLearningAttempt1.KDTree {
    public class KDTree {

        private List<BookData> _books;
        private List<BookData> Books { get { return _books; } }

        private List<string> _wordsToSplitOn;
        private List<string> SplittingWords { get { return _wordsToSplitOn; } }

        private KDTreeNode _root;
        private KDTreeNode Root { get { return _root; } }

        /// <summary>
        /// Creates a KDTree to speed up the K Nearest Neighbor algorithm.
        /// </summary>
        /// <param name="books">Books contained in the tree.</param>
        /// <param name="wordsToSplitOn">Words (most common words) used in the algorithm.</param>
        public KDTree(List<BookData> books, List<string> wordsToSplitOn, int maxBooksPerLeaf = 4) {
            _books = books;
            _wordsToSplitOn = wordsToSplitOn;
            _root = new KDTreeNode(books, wordsToSplitOn, maxBooksPerLeaf);
        }

        public List<BookData> GetPotentialNearestNeighbors(BookData book, int minNeighborCount = 0) {
            var neighborIndices = Root.GetPotentialNearestNeighborIndices(book, minNeighborCount);
            return neighborIndices.Select(i => Books[i]).ToList();
        }
    }
}
