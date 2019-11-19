using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MachineLearningAttempt1.KDTree {
    internal class InternalNode {
        private static int MAX_PER_LEAF = 4;

        private string SplittingWord { get; set; }
        private int SplittingValue { get; set; }
        private List<int> LeafNode { get; set; }

        private KDTree Tree { get; set; }

        private InternalNode LeftNode { get; set; }
        private InternalNode RightNode { get; set; }

        internal InternalNode(KDTree tree, List<int> subSection) {
            Tree = tree;
            if (subSection.Count < MAX_PER_LEAF) {
                LeafNode = new List<int>(subSection);
            }
            else {
                Split(subSection);
            }
        }

        private void Split(List<int> subSection) {
            SplittingValue = GetMedianCount();
            LeftNode = new InternalNode(Tree, subSection.Where(wordCount => wordCount <= SplittingValue).ToList());
            RightNode = new InternalNode(Tree, subSection.Where(wordCount => wordCount > SplittingValue).ToList());
        }

        private int GetMedianCount() {
            List<int> countsForSplittingWord 
                = Tree.Books.Select(book => book.WordCounts[SplittingWord]).OrderBy(i => i).ToList();

            if (countsForSplittingWord.Count % 2 == 0) {
                int median = (countsForSplittingWord[countsForSplittingWord.Count / 2]
                    + countsForSplittingWord[countsForSplittingWord.Count / 2 - 1]) / 2;
                return median;
            }
            else {
                return countsForSplittingWord[countsForSplittingWord.Count / 2];
            }
        }
    }
}
