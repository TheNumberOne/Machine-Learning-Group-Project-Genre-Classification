using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

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
                    _wordCounts = JsonConvert.DeserializeObject<Dictionary<string, int>>(value);
                }
                catch (JsonReaderException ex) {
                    Console.WriteLine(string.Format("Could not parse JSON for word frequencies. Message: {0}", ex.Message));
                }
            }
        }
    }
}
