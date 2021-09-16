using System;

namespace WikipediaWordCounter
{
    class Program
    {
        static string GetUsageString()
        {
            return @"
Creates a CSV file with the most frequently used words in Wikipedia pages.

usage: WikipediaWordCounter <xml file> <output file>

    xml file    : path name to a wikipedia xml dump file
    output file : .csv path name
";
        }

        static void Main(string[] args)
        {

            if (args.Length != 2)
            {
                Console.WriteLine(GetUsageString());
                Environment.Exit(-1);
            }
        }
    }
}
