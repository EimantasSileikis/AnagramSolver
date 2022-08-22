using AnagramSolver.Contracts.Interfaces.Files;
using AnagramSolver.Contracts.Interfaces.Repositories;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.Extensions.Configuration;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class DbWordRepository : Repository<WordModel>, IDbWordRepository
    {
        private readonly IFileManager _fileManager;
        private readonly IConfiguration _config;
        public CodeFirstContext CodeFirstContext { get { return (CodeFirstContext)Context; } }

        public DbWordRepository(CodeFirstContext context, IFileManager fileManager, IConfiguration config) : base(context)
        {
            _fileManager = fileManager;
            _config = config;
        }

        public IEnumerable<WordModel> GetSearchWords(string word)
        {
            return CodeFirstContext.Words.Where(x => x.Word.Contains(word)).ToList();
        }

        public async Task SeedData()
        {
            var lines = _fileManager.ReadFile(Path.Combine(Directory.GetCurrentDirectory(),
                _config.GetSection("DictionaryFilePath").Value));
            WordModel? lastWord = null;

            foreach (var line in lines)
            {
                var wordArr = line.Split('\t');

                WordModel word = new WordModel { Word = wordArr[0], PartOfSpeech = wordArr[1], Number = int.Parse(wordArr[3]) };

                if ((lastWord != null && lastWord.Word == word.Word && lastWord.PartOfSpeech != word.PartOfSpeech)
                    || (lastWord == null) || (lastWord != null && lastWord.Word != word.Word))
                {
                    if (word.Word.Contains("'"))
                    {
                        word.Word = word.Word.Replace("'", "''");
                    }

                    await CodeFirstContext.Words.AddAsync(word);
                    lastWord = word;
                }

                WordModel word2 = new WordModel { Word = wordArr[2], PartOfSpeech = wordArr[1], Number = int.Parse(wordArr[3]) };

                if ((word2.Word != word.Word)
                    || (word2.Word == word.Word && word2.PartOfSpeech != word.PartOfSpeech))
                {
                    if (word2.Word.Contains("'"))
                    {
                        word2.Word = word2.Word.Replace("'", "''");
                    }
                    await CodeFirstContext.Words.AddAsync(word2);
                }
            }
        }

        public void SaveWord(WordModel word)
        {
            if (word != null)
            {
                _fileManager.WriteLine(Path.Combine(Directory.GetCurrentDirectory(),
                _config.GetSection("DictionaryFilePath").Value), word.ToString() ?? "");
            }
        }

        public IEnumerable<WordModel> GetWordList(int pageIndex, int pageSize)
        {
            return CodeFirstContext.Words
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetDictionaryCount()
        {
            return CodeFirstContext.Words.Select(x => x).Count();
        }

        public bool WordExists(WordModel word)
        {
            return CodeFirstContext.Words.Any(w => w.Word == word.Word && w.PartOfSpeech == word.PartOfSpeech);
        }

        public HashSet<WordModel> LoadDictionary()
        {
            return CodeFirstContext.Words.ToHashSet();
        }
    }
}
