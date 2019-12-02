using MachineLearningAttempt1.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MachineLearningAttempt1
{
    class WordOddsBayes
    {
        public WordOddsBayes() 
        {
            oddsPerWord = new Dictionary<string, double[]>();
        }

        //first index is total words, second is total books sampled, third is variance
        Dictionary<string, double[]> oddsPerWord;

        public void addWord(string word) 
        {
            double[] temp = new double[3];
            for (int i = 0; i < temp.Length; i++) 
            {
                temp[i] = 0;
            }
            oddsPerWord.Add(word, temp);
        }


        public double getMeanOfWord(string word) 
        {
            double mean = 0;
            if (oddsPerWord.ContainsKey(word)) 
            {
                double totalWords = oddsPerWord[word][0];
                double numberOfBooks = oddsPerWord[word][1];
                //if the number of books =0 then we'd get an error
                if (numberOfBooks != 0)
                {
                    mean = totalWords / numberOfBooks;
                }
            }
            return mean;
        }

        //it will be assumed that the number of values we add is equal to the number of values added before we don't need to update anything on the book count
        public void addToVariance(string word, double numberBeforeCalculation) 
        {
            double mean = numberBeforeCalculation;
            double difference = mean - numberBeforeCalculation;
            oddsPerWord[word][2] += Math.Pow(difference, 2);
        }

        //run this after you have done the clever bits
        public void getFinalVariance (string word)
        {
            //simply we divide the odds per word by the value we need for variance, the total number of groups sampled from -1
            oddsPerWord[word][2] = oddsPerWord[word][2] /(oddsPerWord[word][1]-1) ;
        
        }

        //just assume that if they add a value it's from a different book and increment the appropriate counter as seen below
        public void addToMean(string word, double meanable) 
        {
            if (!oddsPerWord.ContainsKey(word)) 
            {
                addWord(word);
            }
            oddsPerWord[word][0] += meanable;
            oddsPerWord[word][1] += 1;
        }

        public Boolean containsKey(string word) 
        {
            return oddsPerWord.ContainsKey(word);
        }

    }
}
