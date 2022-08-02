using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
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

        private readonly Dictionary<string, WordInfo> _words = new Dictionary<string, WordInfo>();

        public void LoadDictionary()
        {
            var lines = File.ReadAllLines(dictionaryPath);

            foreach (var line in lines)
            {
                var wordArr = line.Split('\t');

                if (!_words.ContainsKey(wordArr[0]))
                    _words.Add(wordArr[0], new WordInfo { PartOfSpeech = wordArr[1], Number = int.Parse(wordArr[3]) });

                if (!_words.ContainsKey(wordArr[2]))
                    _words.Add(wordArr[2], new WordInfo { PartOfSpeech = wordArr[1], Number = int.Parse(wordArr[3]) });
            }
        }

        public Dictionary<string, WordInfo> GetWords()
        {
            return _words;
        }
    }
}
