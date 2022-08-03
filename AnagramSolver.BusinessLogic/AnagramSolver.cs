using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic
{
    public class AnagramSolver : IAnagramSolver
    {
        private readonly IWordRepository _wordRepository;

        public AnagramSolver(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        public IList<string> GetAnagrams(string myWords)
        {
            myWords = myWords.Replace(" ", "");

            var anagrams = FindAnagrams(myWords);

            var maxAnagrams = int.Parse(Settings.configuration.GetSection("MaxAnagrams").Value);

            return anagrams.Take(maxAnagrams).ToList();
        }

        public IEnumerable<string> FindAnagrams(string myWords)
        {
            var orderedWordChars = String.Concat(myWords.OrderBy(c => c));

            var words = _wordRepository.GetWords();
            var query = words
                .Where(word => word.BaseWord.Replace(" ", "").ToLower() != myWords 
                && String.Concat(word.BaseWord.Replace(" ", "").ToLower().OrderBy(c => c)).Equals(orderedWordChars));

            return query.Select(x => x.BaseWord).Distinct();
        }


    }
}
