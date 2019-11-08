using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MachineLearningAttempt1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            GetBooksByGenre(@"C:\ComputerScience\CS4478\GroupProject\Machine-Learning-Group-Project-Genre-Classification\data\small-dataset.csv");


        }
        //this will hold the actual number of possible outcomes or genres
        int numberOfOutputs;
        //this will hold the actual set of outputs per book
        int[] outputs;
        //we're going to have a set number of bins that a word count can fit into say ten. So if you're in the bottom ten you get one set, if you're in the top ten you're in another set
        int numberOfAttributeOutcomes;
        //attributes will be a sequence of classes if it's passed in this way, as at least 2 of our methods require classes this is how we'll do it
        int[,] attributes;
        //assuming we need to try another method this will be used probably for knn if we get that far
        double[,] continuousAttributes;

        //specifically for when we do bayes learning classified data
        //this is basically what the code has learned
        //the first index is the output, the second is the actual attribute we're judging, the last is which class the attribute had, that is if it was class zero we'd placed it on the zero index class one at first index and so on
        //the value that's held here will be the odds of the given outcome occuring in reference to the given output
        double[,,] oddsAttributesGivenOutputs;


        //this is going to get the totals of each output. I originally thought set the odds now, but if we wait it's only a little more complicated here but we have the total of outputs to divide by
        public double[] getOutputTotals(int[] outputs, int numberOfOutputs)
        {
            int[] individualCounts = new int[numberOfOutputs];
            double[] totalsPerOutcome = new double[numberOfOutputs];
            int total = 0;
            //total bot the full total of given outputs and each total counts
            for (int i = 0; i < outputs.Length; i++)
            {
                //increment the count of each output
                individualCounts[outputs[i]]++;
                //increment the total outputs
                total++;
            }

            //find all the odds of a specific outcome happening.
            for (int i = 0; i < individualCounts.Length; i++)
            {
                totalsPerOutcome[i] = individualCounts[i];
            }


            return totalsPerOutcome;
        }

        //outputs in this context means the outcome of the instance
        //attribute is the thing leading to the outcoome

        //double[][] oddsAttributesGivenOutputs = new double[numberOfOutputs][numberOfAttribute];

        public double[,,] getOddsOfAttributeGivenOutput(int[,] attributes, int[] outputs, int numberOfOutputs, int numberOfAttributeOutcomes)
        {
            int numberOfAttribute = attributes.GetLength(0);
            //this is going to store the odds for each outcome
            double[,,] oddsAttributesGivenOutputs = new double[numberOfOutputs, numberOfAttribute, numberOfAttributeOutcomes];
            // we itterate through each attribute first
            for (int eachAttribute = 0; eachAttribute < attributes.GetLength(0); eachAttribute++)
            {
                //then we move down the instances
                for (int eachInstance = 0; eachInstance < attributes.GetLength(1); eachInstance++)
                {
                    //so we go down first
                    //at each we need to
                    //find the output from the outputs array
                    int currentOutput = outputs[eachInstance];
                    //find the attribute from the attribute array
                    int currentAttributeOutcome = attributes[eachAttribute, eachInstance];
                    oddsAttributesGivenOutputs[currentOutput, eachAttribute, currentAttributeOutcome]++;
                }
            }
            for (int currentOutput = 0; currentOutput < oddsAttributesGivenOutputs.GetLength(0); currentOutput++)
            {
                for (int currentAttribute = 0; currentAttribute < oddsAttributesGivenOutputs.GetLength(1); currentOutput++)
                {
                    for (int currentAttributeOutcome = 0; currentAttributeOutcome < numberOfAttributeOutcomes; currentAttributeOutcome++)
                    {
                        oddsAttributesGivenOutputs[currentOutput, currentAttribute, currentAttributeOutcome] = oddsAttributesGivenOutputs[currentOutput, currentAttribute, currentAttributeOutcome] / outputs[currentOutput];
                    }
                }
            }



            //for the next we need to divide the number of an attribute by the total number of that outcome

            return oddsAttributesGivenOutputs;
        }

        public double[] getOutputOdds(double[] totalsPerOutcome)
        {
            int totals = 0;
            for (int i = 0; i < totalsPerOutcome.Length; i++)
            {
                totals += (int)totalsPerOutcome[i];
            }
            for (int i = 0; i < totalsPerOutcome.Length; i++)
            {
                totalsPerOutcome[i] = totalsPerOutcome[i] / totals;
            }


            return totalsPerOutcome;
        }

        //this is a wrapper allowing easy use of the 1d array function for if we have  2d array instead 
        public int returnClassOfBook(int[,] attributes, int instance)
        {
            //accept an array or a 2d array with index to see what output is
            //the classes found in the array will point to the indexes where the values are stored


            return returnClassOfBook(turnLayerIn2dInto1d(attributes,instance));
        }
        public int  returnClassOfBook(int[] attributes)
        {
            //accept an array or a 2d array with index to see what output is
            //the classes found in the array will point to the indexes where the values are stored
            double biggest = 0;
            int bestClass = -1;
            for (int output = 0; output < numberOfOutputs; output++) {
                double sum = 1;
                for (int attribute = 0; attribute < attributes.Length; attribute++)
                {
                    sum = sum * oddsAttributesGivenOutputs[output, attribute, attributes[attribute]];
                }
                if (biggest <= sum) 
                {
                    biggest = sum;
                    bestClass = 0;
                }
            }


            return bestClass;
        }


        public int[] turnLayerIn2dInto1d(int[,] arrayTo1d, int instance)
        {
            int[] newArray = new int[arrayTo1d.GetLength(0)];
            for (int attribute = 0; attribute < arrayTo1d.GetLength(0); attribute++) 
            {
                newArray[attribute] = arrayTo1d[attribute, instance];
            }
            return newArray;
        }



        public void fillArraysWithAppropriateData(int[,] ListOfWordCounts, string[,] ListOfWords, double[] listOfNumberOfDistinctWordsUsed, int[] genresBrokenIntoTypes, string[] indexesConnectGenreNamesToClassInts, int numberTodivideDataSetByIntCrossFold, int[] TotalWordCountsPerBook, string pathToActualFiles, int[,] ListOfWordCountsCrossFold, string[,] ListOfWordsCrossFold, double[] listOfNumberOfDistinctWordsUsedCrossFold, int[] genresBrokenIntoTypesCrossFold, string[] indexesConnectGenreNamesToClassIntsCrossFold, int numberTodivideDataSetByIntCrossFoldCrossFold, int[] TotalWordCountsPerBookCrossFold, string pathToActualFilesCrossFold) 
        {
            //clarifying notes
            //I need the list of word coutns and list of words to sink up, that is if I look at the word "jingo" in the listOfWords I can go to the corresponding index in listOfWordCounts to see which word that was
            // list of distinct words used just keeps track of if the word "the" or "inteligence" were each used at least once in the text if any word is used it ups the count by one
            //genresBrokenIntoTypes is going to be a list of ints, each int is going to be connected to the associated index of that instance, So while itterating down the second dimsnion of listOfWordCounts I can go to the same index and see which genre it came out of
            //IndexesConnectGenreNamesToClassInts just means that the associated index for the genre name say "mystery" will be the number used in the genresBrokenIntoTypes to represent that genre
            //TotalWordCountsPerBook is the total word counts of a book. For dividing the word counts
            //pathToActualFiles  this is going to be the path to the files we use. I don't know if we will need a list or not for this
            //I don't know how you want to handle cross fold or if you want to pass out by reference or even use the toolkit the teacher provided. I just need the ability to access these elementas I ask for
            //For cross fold I do ask that the data be divided into two sets one to work on, and the other to test with
            //If you want to have a visually more pleasing function you can give me the names to initiate as class level variables and just assume those class level variables are initaited in the appropriate scope
            //I also have no problems with multiple smaller functions either called by this master function
            //Any questions conserns or ideas on this I'm happy to work with you on
        }

        public static List<Data.BookData> GetBooksByGenre(string csvPath) {
            List<Data.BookData> records = null;
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvHelper.CsvReader(reader)) {
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.Replace(" ", "").ToLower();
                // Casting to list loads all records into memory.
                records = csv.GetRecords<Data.BookData>().ToList();
            }
            return records;
        }
    }








}

