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

            List<Data.BookData> books = GetBooksByGenre(@"C:\Users\Jacob Marsden\source\repos\MachineLearningAttempt1\MachineLearningAttempt1\data\medium-dataset.csv");

            //this is per genre a list of the odds of words, per genre every word that appears in that genre can have it's odds caluclated now
            Dictionary<string, WordOddsBayes> wordOddsPerGenre = new Dictionary<string, WordOddsBayes>();
            //List<string> commonWords = MostCommonWords(@"C:\Users\Jacob Marsden\source\repos\MachineLearningAttempt1\MachineLearningAttempt1\data\medium-dataset.csv");

            setGenre(books, wordOddsPerGenre);
            getOddsOfAttributeGivenGenre(books, wordOddsPerGenre);
            makeListOfBestWords(wordOddsPerGenre);
            int counter = 0;
            foreach (Data.BookData book in books)
            {
                //string temp = getClassOfBookBayes(book, wordOddsPerGenre);
                string temp = findMostLikelyGenre(book,books, 21);
                if (temp == book.Genre)
                {
                    counter++;
                }
            }
            Console.WriteLine(counter + "/" + books.Count);


        }

        //for each word find the mean for that genre
        //for each word sum all the words devation from the mean that is (x- mean )^2 / n-1 where n is the total number of the values in the data set
        //

        static string[] arrayOfBestWords;

        //specifically for when we do bayes learning classified data
        //this is basically what the code has learned
        //the first index is the output, the second is the actual attribute we're judging, the last is which class the attribute had, that is if it was class zero we'd placed it on the zero index class one at first index and so on
        //the value that's held here will be the odds of the given outcome occuring in reference to the given output
        double[,,] oddsAttributesGivenOutputs;


        //this is going to get the totals of each output. I originally thought set the odds now, but if we wait it's only a little more complicated here but we have the total of outputs to divide by
        static public void setGenre(List<Data.BookData> books, Dictionary<string, WordOddsBayes> wordOddsPerGenre)
        {
            foreach(Data.BookData book in books) 
            {
                if (!wordOddsPerGenre.ContainsKey(book.Genre)) 
                {
                    wordOddsPerGenre.Add(book.Genre, new WordOddsBayes());
                }
            }

        }

        //outputs in this context means the outcome of the instance
        //attribute is the thing leading to the outcoome

        //double[][] oddsAttributesGivenOutputs = new double[numberOfOutputs][numberOfAttribute];

        static public void getOddsOfAttributeGivenGenre(List<Data.BookData> bookList, Dictionary<string, WordOddsBayes> odds)
        {
            System.Collections.IList books = bookList;
            foreach (Data.BookData book in books)
            {
                foreach (KeyValuePair<string, int> instance in book.WordCounts)
                {
                    //we now have the word to run the rest by
                    string word = instance.Key;
                    //if we've already run the odds on this particular word we can ignore it
                    bool seenThisBefore = false;
                    //we're going to go through each genre and see if the odds have been set if not we run it
                    foreach (KeyValuePair<string, WordOddsBayes> genre in odds)
                    {
                        if (genre.Value.containsKey(word))
                        {
                            seenThisBefore = true;
                        }
                    }

                    //if we haven't seen this before
                    if (!seenThisBefore)
                    {
                        //now we run the actual odds
                        //this first one gets the mean
                        foreach (Data.BookData bookSecondLevel in books)
                        {
                            if (bookSecondLevel.WordCounts.ContainsKey(word))
                            {
                                bool getByPercent = false;
                                int TotalWordCount = 1;
                                //a simple fix ensures we can get the percent or absolute value easily
                                if (getByPercent)
                                {
                                    TotalWordCount = book.TotalWordCount;
                                }
                                //add the word counts for that word in that book
                                odds[bookSecondLevel.Genre].addToMean(word, bookSecondLevel.WordCounts[word] / TotalWordCount);
                            }
                            else
                            {
                                //if it's not found there's a book with zero instances of that word
                                odds[bookSecondLevel.Genre].addToMean(word, 0);
                            }
                        }

                        //this second one gets the variance
                        foreach (Data.BookData bookSecondLevel in books)
                        {
                            if (bookSecondLevel.WordCounts.ContainsKey(word))
                            {
                                bool getByPercent = false;
                                int TotalWordCount = 1;
                                //a simple fix ensures we can get the percent or absolute value easily
                                if (getByPercent)
                                {
                                    TotalWordCount = book.TotalWordCount;
                                }
                                //add the word counts for that word in that book
                                odds[bookSecondLevel.Genre].addToVariance(word, bookSecondLevel.WordCounts[word] / TotalWordCount);
                            }
                            else
                            {
                                //if it's not found there's a book with zero instances of that word
                                odds[bookSecondLevel.Genre].addToVariance(word, 0);
                            }

                        }
                        foreach (KeyValuePair<string, WordOddsBayes> odd in odds)
                        {
                            //incidentally ths will also set standard deviation at this point we can simply use the z score function in the wordOddsBayes to find the odds of a specific incident being in  genre
                            odd.Value.getFinalVariance(word);
                        }

                    }

                }
            }
        }

        static public string getClassOfBookBayes(Data.BookData bookTested, Dictionary<string, WordOddsBayes> wordOddsPerGenre)
        {
            double[] OddsPerOutcome = new double[wordOddsPerGenre.Count];
            //set everthing to 1 as we need to multiply times this to increment it
            for (int i = 0; i < OddsPerOutcome.Length; i++) 
            {
                OddsPerOutcome[i] = 1;
            }
            //we use the foreach because it's eaaser to itterate through a dictinary with that. But we need to get the index per word
            int counter = 0;
            string bestGuess = "";
            foreach(KeyValuePair<string, WordOddsBayes>Genre in wordOddsPerGenre)
            {
                //foreach (KeyValuePair<string, int> word in bookTested.WordCounts)
                //{
                //    OddsPerOutcome[counter] = OddsPerOutcome[counter] * Genre.Value.OneSampleZTest(word.Value, word.Key);
                //}

                for (int i = 0; i<arrayOfBestWords.Length;i++) 
                {
                    string word = arrayOfBestWords[i];
                    double occuranceInBook=0;
                    if (bookTested.WordCounts.ContainsKey(word))
                    {
                        occuranceInBook = bookTested.WordCounts[word];
                    }


                    OddsPerOutcome[counter] = OddsPerOutcome[counter] * Genre.Value.OneSampleZTest(occuranceInBook, word);
                }
                bool shouldStateCurrentGenreAsBest = true;
                if (counter == 0)
                {
                    bestGuess = Genre.Key;
                }
                for (int altCount = 0; altCount < counter; altCount++) 
                {

                    if (OddsPerOutcome[altCount] > OddsPerOutcome[counter]) 
                    {
                        shouldStateCurrentGenreAsBest = false;
                    }
                    if (shouldStateCurrentGenreAsBest) 
                    {
                        bestGuess = Genre.Key;
                    }
                }
                counter++;
            }

            return bestGuess;
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


        static public string findMostLikelyGenre(Data.BookData bookTested, List<Data.BookData> books, int n)
        {
            Data.BookData[] ListOfClosestBooks = getNClosestPoints(bookTested, books, n);
            Dictionary<string, double> oddsByGenre = new Dictionary<string, double>();
            for (int i = 0; i < ListOfClosestBooks.Length; i++)
            {
                if (!oddsByGenre.ContainsKey(ListOfClosestBooks[i].Genre))
                {
                    oddsByGenre.Add(ListOfClosestBooks[i].Genre, 0);
                }
                oddsByGenre[ListOfClosestBooks[i].Genre]++;
            }
            string bestGuess = "";
            double highest = -1;
            foreach (KeyValuePair<string, double> element in oddsByGenre)
            {
                if (element.Value > highest)
                {
                    highest = element.Value;
                    bestGuess = element.Key;
                }
            }

            return bestGuess;
        }

        //n is the number of closest points you want, always set it to something odd
        static public Data.BookData[] getNClosestPoints(Data.BookData bookTested, List<Data.BookData> books, int n)
        {

            double[] distances = new double[n];
            Data.BookData[] nClosestBooks = new Data.BookData[n];
            //if there are more than one hundred million of the same words in a book we can establish that something is wrong in the world
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = 100000000;
            }

            foreach (Data.BookData bookDataPoint in books)
            {
                //if the book data point has the same memory address we know this will be the same and we don't want to test it
                if (bookDataPoint != bookTested)
                {
                    double distanceBetweenBooks = getEucDistance(bookTested, bookDataPoint);
                    //this is for manhatten distance
                    //double distanceBetweenBooks = getManhattenDistance(bookTested, bookDataPoint);

                    //a simple loop to find the highest distance so far. This is the one to test against and replace
                    int indexOfHighestDistance = 0;
                    for (int i = 0; i < distances.Length; i++)
                    {
                        if (distances[indexOfHighestDistance] < distances[i])
                        {
                            indexOfHighestDistance = i;
                        }
                    }
                    if (distances[indexOfHighestDistance] > distanceBetweenBooks)
                    {
                        //if the furthest out point so far is further out than the newest book point we can assume 
                        distances[indexOfHighestDistance] = distanceBetweenBooks;
                        nClosestBooks[indexOfHighestDistance] = bookDataPoint;
                    }
                }
            }
            return nClosestBooks;
        }


        static public double getEucDistance(Data.BookData firstBook, Data.BookData secondBook)
        {
            double sum = 0;
            //foreach (KeyValuePair<string, int> word in firstBook.WordCounts)
            //{
            //    int firstx = word.Value;
            //    int secondx = 0;

            //    if (secondBook.WordCounts.ContainsKey(word.Key))
            //    {
            //        secondx = secondBook.WordCounts[word.Key];
            //    }
            //    double localTotal = firstx - secondx;
            //    sum += Math.Pow(localTotal, 2);
            //}

            ////this second foreach loop has some issues, specifically it might not help to test against words not in the books, then again it might help so I set this up
            //foreach (KeyValuePair<string, int> word in secondBook.WordCounts)
            //{
            //    //if the first book did not have this key we didn't check it so we need to check it, 
            //    //by default it will have the value zero
            //    if (!firstBook.WordCounts.ContainsKey(word.Key))
            //    {
            //        int secondx = secondBook.WordCounts[word.Key];
            //        sum += Math.Pow(secondx, 2);
            //    }
            //}

            for (int i = 0; i < arrayOfBestWords.Length; i++) 
            {
                string word = arrayOfBestWords[i];
                double firstx = 0;
                double secondx = 0;

                if (firstBook.WordCounts.ContainsKey(word))
                {
                    firstx = firstBook.WordCounts[word];
                }
                if (secondBook.WordCounts.ContainsKey(word))
                {
                    secondx = secondBook.WordCounts[word];
                }
                double localTotal = firstx - secondx;
                sum += Math.Pow(localTotal, 2);
            }
            sum = Math.Sqrt(sum);
            return sum;
        }

        //almost the exact same save that this is for manhatten distance so we absolute the value as opposed to squaring it
        static public double getManhattenDistance(Data.BookData firstBook, Data.BookData secondBook)
        {
            double sum = 0;
            foreach (KeyValuePair<string, int> word in firstBook.WordCounts)
            {
                int firstx = word.Value;
                int secondx = 0;

                if (secondBook.WordCounts.ContainsKey(word.Key))
                {
                    secondx = secondBook.WordCounts[word.Key];
                }

                double localTotal = firstx - secondx;
                sum += Math.Abs(localTotal);
            }

            //this second foreach loop has some issues, specifically it might not help to test against words not in the books, then again it might help so I set this up
            foreach (KeyValuePair<string, int> word in secondBook.WordCounts)
            {
                //if the first book did not have this key we didn't check it so we need to check it, 
                //by default it will have the value zero
                if (!firstBook.WordCounts.ContainsKey(word.Key))
                {
                    int secondx = secondBook.WordCounts[word.Key];
                    sum += Math.Abs(secondx);
                }
            }
            sum = Math.Sqrt(sum);
            return sum;
        }


        static public void makeListOfBestWords(Dictionary<string, WordOddsBayes> odds) 
        {
            List<string> bestWords=new List<string>();
            List<string> fullListOfWords = new List<string>();
            foreach (KeyValuePair<string, WordOddsBayes>genre  in odds) 
            {
                List<string> listOfWords =genre.Value.getDictionary();
                fullListOfWords = fullListOfWords.Union(listOfWords).ToList();
            }

            int lengthWordList = 50;
            double[] wordAverages = new double[lengthWordList];
            string[] words = new string[lengthWordList];
            foreach (string word in fullListOfWords)
            {
                double sum = 0;
                foreach (KeyValuePair<string, WordOddsBayes> genre in odds)
                {
                    sum+=genre.Value.getAverage(word);

                    //I can also see the following
                    //does it have a different mean with a low variance

                }
                Boolean found = false;
                for (int i =0;i< wordAverages.Length ; i++) 
                {
                    if (!found && sum > wordAverages[i]) 
                    {
                        wordAverages[i] = sum;
                        words[i] = word;
                        found = true;
                    }
                }
            }
            if (5 == 0) 
            {
            }
            arrayOfBestWords= words;
        }


        public static List<Data.BookData> GetBooksByGenre(string csvPath)
        {
            List<Data.BookData> records = null;
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvHelper.CsvReader(reader))
            {
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.Replace(" ", "").ToLower();
                // Casting to list loads all records into memory.
                records = csv.GetRecords<Data.BookData>().ToList();
            }
            return records;
        }


        public static List<string> MostCommonWords(string csvPath)
        {
            List<string> words = null;
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvHelper.CsvReader(reader))
            {
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => index == 0 ? header = "word" : null;
                words = csv.GetRecords<string>().ToList();
            }
            return words;
        }
    }








}

