using AnagramSolver.Contracts.Interfaces.Files;
using AnagramSolver.Contracts.Interfaces.Repositories;
using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class WordRepository : IWordRepository
    {
        public string dictionaryPath = Path.Combine(Directory.GetCurrentDirectory(), "zodynas.txt");
        private readonly IFileManager _fileManager;

        public HashSet<WordModel> Words { get; set; }
        public WordRepository(IFileManager fileManager)
        {
            _fileManager = fileManager;
            Words = LoadDictionary();
        }

        public HashSet<WordModel> LoadDictionary()
        {
            var wordSet = new HashSet<WordModel>();

            var lines = _fileManager.ReadFile(dictionaryPath);
            WordModel? lastWord = null;

            foreach (var line in lines)
            {
                var wordArr = line.Split('\t');

                WordModel word = new WordModel { Word = wordArr[0], PartOfSpeech = wordArr[1], Number = int.Parse(wordArr[3]) };

                if (lastWord != null && lastWord.Word == word.Word && lastWord.PartOfSpeech != word.PartOfSpeech
                    || lastWord == null || lastWord != null && lastWord.Word != word.Word)
                {
                    wordSet.Add(word);
                    lastWord = word;
                }

                WordModel word2 = new WordModel { Word = wordArr[2], PartOfSpeech = wordArr[1], Number = int.Parse(wordArr[3]) };

                if (word2.Word != word.Word
                    || word2.Word == word.Word && word2.PartOfSpeech != word.PartOfSpeech)
                {
                    wordSet.Add(word2);
                }
            }

            return wordSet;
        }

        public bool WordExists(WordModel word)
        {
            return Words.Any(w => w.Word == word.Word && w.PartOfSpeech == word.PartOfSpeech);
        }

        public void SaveWord(WordModel word)
        {
            if (word != null)
            {
                _fileManager.WriteLine(dictionaryPath, word.ToString() ?? "");
                Words.Add(word);
            }
        }
    }
}
