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
            string[] wordsArr = myWords.Split(" ");
            myWords = myWords.Replace(" ", "");

            var anagrams = FindAnagrams(myWords, wordsArr);

            var maxAnagrams = int.Parse(Settings.configuration.GetSection("MaxAnagrams").Value);

            return anagrams.Take(maxAnagrams).ToList();
        }

        private IEnumerable<string> FindAnagrams(string myWords, string[] wordsArr)
        {
            var words = _wordRepository.GetWords();
            var orderedWordChars = String.Concat(myWords.OrderBy(c => c));
            IEnumerable<Word?> query;

            if (wordsArr.Length < 2)
            {
                query = words
                    .Where(word => word.BaseWord.Replace(" ", "").ToLower() != myWords
                    && String.Concat(word.BaseWord.Replace(" ", "").ToLower().OrderBy(c => c)).Equals(orderedWordChars));

                return query.Select(x => x.BaseWord).Distinct();
            }

            var anagramList = FindAnagramsWithFewWords(words, wordsArr, orderedWordChars, myWords);

            return anagramList.Distinct();
        }

        private IList<string> FindAnagramsWithFewWords(HashSet<Word> words, string[]? wordsArr, string orderedWordChars, string myWords)
        {
            IList<string> anagramList = new List<string>();

            var dktWords = words
                .Where(word => (word.PartOfSpeech == "dkt")
                && String.Concat(word.BaseWord.Replace(" ", "").ToLower().OrderBy(c => c)).All(orderedWordChars.Contains));

            var bdvWords = words
                .Where(word => (word.PartOfSpeech == "bdv")
                && String.Concat(word.BaseWord.Replace(" ", "").ToLower().OrderBy(c => c)).All(orderedWordChars.Contains));

            foreach (var bdvWord in bdvWords)
            {
                foreach (var dktWord in dktWords)
                {
                    if (bdvWord.BaseWord.Length + dktWord.BaseWord.Length == orderedWordChars.Length
                        && String.Concat((bdvWord.BaseWord + dktWord.BaseWord).OrderBy(c => c)).Equals(orderedWordChars)
                        && (bdvWord.BaseWord + dktWord.BaseWord) != myWords
                        && !wordsArr.Contains(dktWord.BaseWord) && !wordsArr.Contains(bdvWord.BaseWord))
                    {
                        anagramList.Add(bdvWord.BaseWord + " " + dktWord.BaseWord);
                    }
                }
            }

            return anagramList;
        }
    }
}
