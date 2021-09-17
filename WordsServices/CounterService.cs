using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordsServices
{
    public class CounterService : ICounterService
    {
        private readonly string[] _filipinoList  = { "at" };

        private readonly WordDictionary _dictionary = new WordDictionary();
        private readonly Spelling _spellChecker;

        public CounterService()
        {
            _dictionary.DictionaryFile = "en-AU.dic";
            _dictionary.Initialize();
            _spellChecker = new Spelling
            {
                Dictionary = _dictionary
            };
        }

        private bool IsEnglish(string word)
        {
            return _spellChecker.TestWord(word);
        }
        
        public int CountWords(Dictionary<string, int> counters, string text, List<string> ignoredWords)
        {
            text = Regex.Replace(text, "[^a-zA-Z% _']", string.Empty);

            var words = text.Split(' ');
            var totalWords = 0;

            foreach(var word in words)
            {
                var key = word.ToLower().Trim();
                if (key == string.Empty || key.Length == 1) continue;

                // ignore all English words
                if (!_filipinoList.Contains(key))
                {
                    if (IsEnglish(key) && !ignoredWords.Contains(key))
                    {
                        ignoredWords.Add(key);
                    }
                    continue;
                }
                

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
