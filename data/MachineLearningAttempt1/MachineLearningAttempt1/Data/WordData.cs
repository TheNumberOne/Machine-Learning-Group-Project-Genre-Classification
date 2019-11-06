using System;
using System.Collections.Generic;
using System.Text;

namespace MachineLearningAttempt1.Data {
    /// <summary>
    /// Word count and frequency for a word in a book.
    /// </summary>
    public class WordData {
        private string _word;
        private int _count;
        private BookData _bookData;

        public string Word {
            get { return _word; }
        }
        public int Count {
            get { return _count; }
        }
        public double Frequency {
            get { return _count / _bookData.TotalWordCount; }
        }
    }
}
