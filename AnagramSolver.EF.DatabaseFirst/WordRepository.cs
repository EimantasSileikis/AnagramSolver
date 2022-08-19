using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.DatabaseFirst.Data;
using AnagramSolver.EF.DatabaseFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.EF.DatabaseFirst
{
    public class WordRepository : IDbWordRepository
    {
        private readonly AnagramsContext _context;
        public HashSet<WordModel> Words { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public WordRepository(AnagramsContext context)
        {
            _context = context;
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
            
        }

        public List<string> GetCachedAnagrams(string inputWord)
        {
            var data = _context.CachedWords.Where(x => x.Word == inputWord).SelectMany(x => x.Anagrams).Select(x => x.Anagram1);
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
                if(anagrams.Count > 0)
                {
                    ICollection<Anagram> cache = new List<Anagram>();
                    foreach (var anagram in anagrams)
                    {
                        var anag = new Anagram { Anagram1 = anagram };
                        cache.Add(anag);
                    }

                    _context.CachedWords.Add(new CachedWord { Word = inputWord, Anagrams = cache });
                    _context.SaveChanges();
                }
                
                return true;
            }
        }

        public bool WordExists(WordModel word)
        {
            return _context.Words.Any(w => w.Word == (word.Word) & w.PartOfSpeech == (word.PartOfSpeech));
        }
    }
}
