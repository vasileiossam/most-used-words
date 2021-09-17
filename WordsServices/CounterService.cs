using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordsServices
{
    public class CounterService : ICounterService
    {
        private readonly string[] _ignoreList  = 
            { "hi", "ok", "hey", "it's", "by", "his", "one", "two", "oh", "okay"};

        public int CountWords(Dictionary<string, int> counters, string text)
        {
            text = Regex.Replace(text, "[^a-zA-Z% _']", string.Empty);

            var words = text.Split(' ');
            var totalWords = 0;

            foreach(var word in words)
            {
                var key = word.ToLower().Trim();
                if (key == string.Empty || key.Length == 1 || _ignoreList.Contains(key)) continue;

                if (counters.ContainsKey(key)) 
                {
                    counters[key] += 1; 
                
                }
                else
                {
                    counters.Add(key, 1);
                }

                totalWords++;
            }

            return totalWords;
        }

        public void CreateCsv(Dictionary<string, int> counters, string pathName)
        {
            var total = counters.Sum(x => x.Value);
            var counter = 1;

            var csv = "#,Word,Count,Percentage\n" +
                String.Join(
                    Environment.NewLine,
                    counters
                        .OrderByDescending(x => x.Value)
                        .Select(d => $"{counter++},{d.Key},{d.Value},{Math.Round((double)d.Value/total*100, 2)}")
                ); 

            File.WriteAllText(pathName, csv);
        }
    }
}
