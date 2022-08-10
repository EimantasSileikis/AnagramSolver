using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic
{
    public class WordRepository : IWordRepository
    {
        public string dictionaryPath = Path.Combine(Directory.GetCurrentDirectory(), "zodynas.txt");
        private readonly IFileReader _fileReader;
        private readonly IFileWriter _fileWriter;

        public HashSet<Word> Words { get; set; }

        public WordRepository(IFileReader fileReader, IFileWriter fileWriter)
        {
            _fileReader = fileReader;
            _fileWriter = fileWriter;
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

        public bool WordExists(Word word)
        {
            return Words.Contains(word);
        }

        public void AddWord(Word word)
        {
            if(word != null)
            {
                Words.Add(word);
                _fileWriter.WriteLine(dictionaryPath, word.ToString());
            }
        }
    }
}
