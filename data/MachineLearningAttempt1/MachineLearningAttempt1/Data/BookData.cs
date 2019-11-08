using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace MachineLearningAttempt1.Data {
    public class BookData {
        private Dictionary<string, int> _wordCounts;

        public int GutenbergId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int TotalWordCount { get; set; }
        public Dictionary<string, int> WordCounts {
            get { return _wordCounts; }
        }
        public string WordFrequencies { 
            set {
                try {
                    _wordCounts = Regex.Replace(value, @"[{}\s']", "")
                        .Split(',').Select((string s) => s.Split(':'))
                        .ToDictionary(arr => arr[0], arr => int.Parse(arr[1]));
                }
                catch (System.FormatException ex) {
                    Console.WriteLine(string.Format("Word Frequencies formatted incorrectly. Internal error: {0}. Stack Trace: {1}"
                        , ex.Message, ex.StackTrace));
                }
            }
        }
    }
}
