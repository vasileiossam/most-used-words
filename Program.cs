using System;
WikipediaWordFrequencyCounter

WikipediaMostUsedWords

namespace MostUsedWords
{
    class Program
    {
        static string GetUsage()
        {
            return @"Outputs a CSV file with the most frequently used words in  a list of most used words Usage: MostUsedWords <path name>";
        }

        static void Main(string[] args)
        {
            Console.WriteLine(args.Length);

            if (args.Length == 0)
            {
                throw new Exception(GetUsage());

            }
        }
    }
}
