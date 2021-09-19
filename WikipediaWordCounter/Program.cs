using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using WordsServices;

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

        /// <summary>
        /// https://stackoverflow.com/questions/2441673/reading-xml-with-xmlreader-in-c-sharp
        /// </summary>
        /// <param name="inputUrl"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        static IEnumerable<XElement> SimpleStreamAxis(string inputUrl, string elementName)
        {
            using (XmlReader reader = XmlReader.Create(inputUrl))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == elementName)
                        {
                            XElement el = XNode.ReadFrom(reader) as XElement;
                            if (el != null)
                            {
                                yield return el;
                            }
                        }
                    }
                }
            }
        }

        static bool ContainsHTML(string checkString)
        {
            return Regex.IsMatch(checkString, "<(.|\n)*?>");
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

            var xmlFile = args[0];
            if (!File.Exists(xmlFile))
            {
                Console.WriteLine($"Error: file '{xmlFile}' not exists");
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

            var totalWords = 0;
            var counters = new Dictionary<string, int>();
            var counterService = serviceProvider.GetService<ICounterService>();
            var ignoredWords = new List<string>();
            var i = 1;
            var totalPages = 0;

            var xmlReader = XmlReader.Create(xmlFile);
            while (xmlReader.ReadToFollowing("page"))
            {
                xmlReader.ReadToDescendant("title");
                xmlReader.Read();
                var title = xmlReader.Value;
                if (title.Contains("MediaWiki:") || title.StartsWith("Wikipedia")) continue;

                xmlReader.ReadToFollowing("text");
                xmlReader.Read();
                var text = xmlReader.Value;

                if (text.StartsWith("#REDIRECT")) continue;
                //if (ContainsHTML(text)) continue;

                // remove wiki markup, anyting enclosed in [[ ]]
                var rgx = new Regex(@"(?<=\[\[)(?s).+?(?=\]\])");
                text = rgx.Replace(text, "");
                text = text.Replace("[[]]", "");

                // remove wiki markup, anyting enclosed in {{ }}
                rgx = new Regex(@"(?<=\{\{)(?s).+?(?=\}\})");
                text = rgx.Replace(text, "");
                text = text.Replace("{{}}", "");

                // remove links
                text = Regex.Replace(text, @"((http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)", "");

                Console.Write($"{i++}. Counting words in page with title '{title}'...");
                
                totalWords = counterService.CountWords(counters, text, ignoredWords);
                
                Console.WriteLine($"found {totalWords} words!");
                if (totalWords > 0) totalPages++;
            }

            counterService.CreateCsv(counters, outputFile);
            File.WriteAllLines(outputFile + "-ignoredWords.txt", ignoredWords);
            Console.WriteLine($"---------------------------------");
            Console.WriteLine($"Scanned pages: {i}, pages with words: {totalPages}");
            Console.WriteLine($"Created file {outputFile}.");
        }
    }
}
 