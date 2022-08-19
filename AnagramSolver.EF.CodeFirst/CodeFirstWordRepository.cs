using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;
using AnagramSolver.EF.CodeFirst.Models;
using Microsoft.Extensions.Configuration;

namespace AnagramSolver.EF.CodeFirst
{
    public class CodeFirstWordRepository : IDbWordRepository
    {
        private readonly CodeFirstContext _context;
        private readonly IFileReader _fileReader;
        private readonly IConfiguration _config;
        private readonly string dictionaryPath;

        public HashSet<WordModel> Words { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public CodeFirstWordRepository(CodeFirstContext context, IFileReader fileReader, IConfiguration config)
        {
            _context = context;
            _fileReader = fileReader;
            _config = config;
            dictionaryPath = Path.Combine(Directory.GetCurrentDirectory(),
                _config.GetSection("DictionaryFilePath").Value);
            //SeedDatabase();
        }

        public void AddWord(WordModel word)
        {
            _context.Words.Add(word);
            _context.SaveChanges();
        }

        public bool AnagramsFound(string word)
        {
            return _context.CachedWords.Any(w => w.Word == word);
        }

        public void DeleteTableData(string tableName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetCachedAnagrams(string inputWord)
        {
            var data = _context.CachedWords.Where(x => x.Word == inputWord).SelectMany(x => x.Anagrams).Select(x => x.Anagram);
            return data.ToList();
        }

        public List<SearchHistory> GetSearchHistory()
        {
            return _context.SearchHistories.ToList();
        }

        public HashSet<WordModel> LoadDictionary()
        {
            return _context.Words.ToHashSet();
        }

        public List<WordModel> SearchWord(string word)
        {
            return _context.Words.Where(w => w.Word.Contains(word)).ToList();
        }

        public void StoreSearchData(string ipAddress, string inputWord, List<string> anagrams, int timeSpent)
        {
            _context.SearchHistories.Add(new SearchHistory
            {
                IpAddress = ipAddress,
                SearchWord = inputWord,
                Anagrams = string.Join(",", anagrams),
                TimeSpent = timeSpent
            });
            _context.SaveChanges();
        }

        public bool StoreToCachedTable(string inputWord, List<string> anagrams)
        {
            if (AnagramsFound(inputWord))
            {
                return false;
            }
            else
            {
                if (anagrams.Count > 0)
                {
                    ICollection<AnagramsEntity> cache = new List<AnagramsEntity>();
                    foreach (var anagram in anagrams)
                    {
                        var anag = new AnagramsEntity { Anagram = anagram };
                        cache.Add(anag);
                    }

                    _context.CachedWords.Add(new CachedWordEntity { Word = inputWord, Anagrams = cache });
                    _context.SaveChanges();
                }

                return true;
            }
        }

        public bool WordExists(WordModel word)
        {
            return _context.Words.Any(w => w.Word == (word.Word) & w.PartOfSpeech == (word.PartOfSpeech));
        }

        public void SeedDatabase()
        {
            var lines = _fileReader.ReadFile(dictionaryPath);

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

                    _context.Words.Add(word);
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
                    _context.Words.Add(word2);
                }
            }
            _context.SaveChanges();
        }
    }
}
