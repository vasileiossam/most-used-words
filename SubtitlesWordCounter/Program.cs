using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using WordsServices;

namespace SubtitlesWordCounter
{
    class Program
    {
        static string GetUsageString()
        {
            return @"
Creates a CSV file with the most frequently used words in subtitle files.

usage: SubtitlesWordCounter <source directory> <output file>

    source directory: directory with .srt files
    output file     : .csv path name
";
        }

        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<ICounterService, CounterService>();
            var serviceProvider = services.BuildServiceProvider(true);

            if (args.Length != 2)
            {
                Console.WriteLine(GetUsageString());
                Environment.Exit(-1);
            }

            var sourceDir = args[0];
            if (!Directory.Exists(sourceDir))
            {
                Console.WriteLine($"Error: directory '{sourceDir}' not exists");
                Console.WriteLine(GetUsageString());
                Environment.Exit(-1);
            }

            var outputFile = args[1];
            var outputPath = Path.GetDirectoryName(outputFile);
            if (outputPath != string.Empty && !Directory.Exists(outputPath))
            {
                Console.WriteLine($"Error: path name '{outputFile}' not exists");
                Console.WriteLine(GetUsageString());
                Environment.Exit(-1);
            }

            var files = Directory.GetFiles(sourceDir, "*.srt");
            Console.WriteLine($"Found {files.Length} .srt files in '{sourceDir}'.");

            var totalWords = 0;
            var counters = new Dictionary<string, int>();
            var counterService = serviceProvider.GetService<ICounterService>();
     
            foreach (var file in files)
            {
                Console.Write($"Counting words in '{Path.GetFileName(file)}'...");
                totalWords = counterService.CountWords(counters, File.ReadAllText(file));
                Console.WriteLine($"found {totalWords} words!");
            }

            if (totalWords > 0)
            {
                counterService.CreateCsv(counters, outputFile);
                Console.WriteLine($"Created file {outputFile}.");
            }
        }
    }
}
