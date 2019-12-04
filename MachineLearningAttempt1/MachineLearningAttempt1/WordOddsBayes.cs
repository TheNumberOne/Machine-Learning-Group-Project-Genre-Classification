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

        //first index is total words, second is total books sampled, third is variance, fourth is the standard deviation
        Dictionary<string, double[]> oddsPerWord;

        public void addWord(string word)
        {
            double[] temp = new double[4];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = 0;
            }
            oddsPerWord.Add(word, temp);
        }


        public List<string> getDictionary()
        {


            List<string> listOfWords = new List<string>(oddsPerWord.Keys);


            return listOfWords;
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
            double totalOccurances = oddsPerWord[word][0];
            double outOFNumberOfBooks = oddsPerWord[word][1];
            double mean = totalOccurances / outOFNumberOfBooks;
            double difference = mean - numberBeforeCalculation;
            oddsPerWord[word][2] += Math.Pow(difference, 2);
        }

        //run this after you have done the clever bits
        public void getFinalVariance(string word)
        {
            //simply we divide the odds per word by the value we need for variance, the total number of groups sampled from -1
            oddsPerWord[word][2] = oddsPerWord[word][2] / (oddsPerWord[word][1] - 1);
            //set standard deviation
            oddsPerWord[word][3] = Math.Sqrt(oddsPerWord[word][2]);

        }

        public double getStandardDeviation(string word)
        {
            double returnable = 0;
            if (oddsPerWord.ContainsKey(word))
            {
                returnable = oddsPerWord[word][3];
            }
            return returnable;
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

        public double getAverage(string word) 
        {
            double returnable = 0;
            if (oddsPerWord.ContainsKey(word))
            {
                double total = oddsPerWord[word][0];
                double divdedBy = oddsPerWord[word][1];
                returnable = total / divdedBy;
            }
            return returnable;
        }

        public double OneSampleZTest(double valueTested, string word)
        {
            //function looks like this (x-mean)/standardDevation/sqrt(numberSampled)
            double numerator = valueTested - getMeanOfWord(word);
            //numerator = 1;

            double denominator = 1;
            if (oddsPerWord.ContainsKey(word))
            {
                double standardDeviation = getStandardDeviation(word);
                if (standardDeviation != 0)
                {
                    denominator = getStandardDeviation(word); /// Math.Sqrt(oddsPerWord[word][1]);
                }
            }
            return  numerator/denominator;
        }
    }
}
