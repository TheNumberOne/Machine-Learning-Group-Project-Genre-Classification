using System;
using System.Collections.Generic;
using System.Text;

namespace MachineLearningAttempt1.Data {
    /// <summary>
    /// Holds data on a book. Includes word counts, genre, and name.
    /// </summary>
    class BookData {
        private string _genre;
        private string _name;

        private Dictionary<string, WordData> _words;
        
        public Dictionary<string, WordData> Words {
            get { return _words; }
        }
        public int TotalWordCount {
            get { return Words.Count; }
        }
    }
}
