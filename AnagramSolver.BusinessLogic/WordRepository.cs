using AnagramSolver.BusinessLogic.Models;
using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public class WordRepository : IWordRepository
    {
        private readonly string dictionaryPath = Path.GetFullPath(Path.Combine(@"zodynas.txt", "..", "..", "..", "..", "..", "zodynas.txt"));

        //public IDictionary<string, List<Word>> dictionary = new Dictionary<string, List<Word>>();
        private readonly Dictionary<string, string> _words = new Dictionary<string, string>();

        public void LoadDictionary()
        {
            var lines = File.ReadAllLines(dictionaryPath);

            foreach (var line in lines)
            {
                var wordArr = line.Split('\t');

                if (!_words.ContainsKey(wordArr[0]))
                    _words.Add(wordArr[0], wordArr[1]);

                if (!_words.ContainsKey(wordArr[2]))
                    _words.Add(wordArr[2], wordArr[1]);
            }
        }

        public Dictionary<string, string> GetWords()
        {
            return _words;
        }
    }
}
