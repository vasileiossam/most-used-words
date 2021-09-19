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
        private readonly string[] _filipinoList  = { "at", "pa", "ale", "bag", "din", "man", "may"};

        private readonly string[] _ignoreList = { 
            "bfsfnarabic", 
            "maria", 
            "steve", 
            "john", 
            "miguel", 
            "james", 
            "okey", 
            "mr", 
            "pedro",
            "ok", 
            "telepono", 
            "inconvenienceyoursbr", 
            "foundationdiv", 
            "english", 
            "dirltrhellothe",
            "sideeffects",
            "informationunfortunately",
            "insorry",
            "noincludenoinclude",
            "wikimedia",
            "wikipedia",
            "px",
            "uri",
            "nowikinowiki",
            "tv",
            "lang",
            "del",
            "dec",
            "datasortvalue",
            "crosswiki",
            "pob",
            "noinclude",
            "roma",
            "frazioni",
            "jose",
            "st",
            "korea",
            "sur",
            "amerikano",
            "anime",
            "ce",
            "uk",
            "dr",
            "jr",
            "interwikis",
            "latin",
            "david",
            "cd",
            "inc",
            "aaa",
            "thenreturn",
            "timezone",
            "el",
            "internet",
            "blg",
            "wiki",
            "nd",
            "etc",
            "israel",
            "william",
            "tokyo",
            "australia",
            "subpage",
            "da",
            "kim",
            "platinumref",
            "jesus",
            "nbspkw",
            "refmga",
            "fm",
            "asia",
            "'s",
            "vs",
            "encyclopedia",
            "xf",
            "ll",
            "xd",
            "french",
            "greek",
            "iso",
            "george",
            "italian",
            "ko",
            "mwcontentltr",
            "sul",
            "amerika",
            "isbn",
            "wikis",
            "york",
            "ba",
            "bce",
            "japan",
            "italya",
            "japanese",
            "june",
            "january",
            "december",
            "india",
            "british",
            "september",
            "ed",
            "november",
            "october",
            "anderson",
            "includeonly",
        };

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

        private bool IsCss(string word)
        {

            string[] cssShort = { "br", "td", "tr", "div", "nbsp", "args", "th" };

            string[] css = {"color", "font", "fff", "align", "style", "text", "center", "background", "space", "%", "_", "''",
                            "refref", "ndash", "unit", "span", "scope","class", "name", "code", "cell", "spacing", "padding","small", 
                            "url", "plain", "border", "site", "this", "var"};

            if (cssShort.Contains(word)) return true;
            if (css.Any(x => word.Contains(x))) return true;
            return false;
        }

        public int CountWords(Dictionary<string, int> counters, string text, List<string> ignoredWords)
        {
            text = Regex.Replace(text, "[^a-zA-Z% _']", string.Empty);

            var words = text.Split(' ');
            var totalWords = 0;

            foreach(var word in words)
            {
                var key = word.ToLower().Trim();
                if (key == string.Empty || key.Length == 1 || _ignoreList.Contains(key)) continue;

                // ignore all CSS 
                if (IsCss(key)) continue;

                // ignore all English words
                if (IsEnglish(key) && !_filipinoList.Contains(key))
                {
                    if (!ignoredWords.Contains(key))
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
                        .Select(d => $"{counter++},{d.Key},{d.Value},{Math.Round((double)d.Value/total*100, 2):0.00}")
                ); 

            File.WriteAllText(pathName, csv);
        }
    }
}
