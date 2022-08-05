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
        public string dictionaryPath = Path.GetFullPath(Path.Combine(@"zodynas.txt", "..", "..", "..", "..", "..", "zodynas.txt"));
        private readonly IFileReader _fileReader;

        public HashSet<Word> Words { get; set; }

        public WordRepository(IFileReader fileReader)
        {
            _fileReader = fileReader;
            Words = LoadDictionary();
        }

        public HashSet<Word> LoadDictionary()
        {
            var wordSet = new HashSet<Word>();

            var lines = _fileReader.ReadFile(dictionaryPath);
            Word? lastWord = null; 

            foreach (var line in lines)
            {
                var wordArr = line.Split('\t');

                Word word = new Word { BaseWord = wordArr[0], PartOfSpeech = wordArr[1], Number = int.Parse(wordArr[3]) };

                if((lastWord != null && lastWord.BaseWord == word.BaseWord && lastWord.PartOfSpeech != word.PartOfSpeech) 
                    || (lastWord == null) || (lastWord != null && lastWord.BaseWord != word.BaseWord) )
                {
                    wordSet.Add(word);
                    lastWord = word;
                }

                Word word2 = new Word { BaseWord = wordArr[2], PartOfSpeech = wordArr[1], Number = int.Parse(wordArr[3]) };

                if ((word2.BaseWord != word.BaseWord) 
                    || (word2.BaseWord == word.BaseWord && word2.PartOfSpeech != word.PartOfSpeech) )
                {
                    wordSet.Add(word2);
                }
            }

            return wordSet;
        }
    }
}
