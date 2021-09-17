using System.Collections.Generic;

namespace WordsServices
{
    public interface ICounterService
    {
        int CountWords(Dictionary<string, int> counters, string text, List<string> ignoredWords);
        void CreateCsv(Dictionary<string, int> counters, string pathName);
    }
}
