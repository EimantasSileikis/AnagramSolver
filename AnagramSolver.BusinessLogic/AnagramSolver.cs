﻿using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AnagramSolver.BusinessLogic
{
    public class AnagramSolver : IAnagramSolver
    {
        private readonly IDbWordRepository _wordRepository;
        private readonly IConfiguration _config;
        private readonly string url = "https://localhost:7127/api/anagrams/";

        public AnagramSolver(IDbWordRepository wordRepository, IConfiguration config)
        {
            _wordRepository = wordRepository;
            _config = config;
        }

        public IList<string> GetAnagrams(string myWords)
        {
            myWords = myWords.Trim();
            var words = myWords.Split(" ");

            var minLength = _config.GetValue<int>("MinWordLength");

            foreach (var word in words)
            {
                if (word.Length < minLength)
                {
                    Console.Clear();
                    Console.WriteLine($"Minimum length of each word is {minLength}");
                    return new List<string>();
                }
            }

            var maxAnagrams = _config.GetValue<int>("MaxAnagrams");

            if (_wordRepository.AnagramsFound(myWords))
            {
                var anagrams = _wordRepository.GetCachedAnagrams(myWords);
                return anagrams.Take(maxAnagrams).ToList();
            }
            else
            {
                var anagrams = FindAnagrams(myWords);
                _wordRepository.StoreToCachedTable(myWords, anagrams.ToList());
                return anagrams.Take(maxAnagrams).ToList();
            }
        }

       

        public async Task<List<string>> RequestAnagrams(string myWords)
        {
            using (var client = new HttpClient())
            {
                var responseBody = await client.GetStringAsync($"{url}{myWords}");
                var anagrams = JsonConvert.DeserializeObject<List<string>>(responseBody);

                if (anagrams != null)
                    return anagrams;
            }

            return new List<string>();
        }

        private IEnumerable<string> FindAnagrams(string myWords)
        {
            string[] wordsArray = myWords.Split(" ");
            var words = _wordRepository.LoadDictionary();
            myWords = myWords.Replace(" ", "").ToLower();
            var orderedWordChars = String.Concat(myWords.OrderBy(c => c));

            if (wordsArray.Length < 2)
            {
                var query =
                    words
                    .Where(word => word.Word.Replace(" ", "").ToLower() != myWords
                    && string.Concat(word.Word.Replace(" ", "").ToLower().OrderBy(c => c)).Equals(orderedWordChars));

                return query.Select(x => x.Word).Distinct();
            }
            else
            {
                var anagramList = FindAnagramsWithFewWords(words, wordsArray, orderedWordChars, myWords);

                return anagramList.Distinct();
            }
        }

        private IList<string> FindAnagramsWithFewWords(HashSet<WordModel> words, string[] wordsArr, string orderedWordChars, string myWords)
        {
            IList<string> anagramList = new List<string>();

            var dktWords = words
                .Where(word => (word.PartOfSpeech == "dkt")
                && String.Concat(word.Word.Replace(" ", "").ToLower().OrderBy(c => c)).All(orderedWordChars.Contains));

            var bdvWords = words
                .Where(word => (word.PartOfSpeech == "bdv")
                && String.Concat(word.Word.Replace(" ", "").ToLower().OrderBy(c => c)).All(orderedWordChars.Contains));

            foreach (var bdvWord in bdvWords)
            {
                foreach (var dktWord in dktWords)
                {
                    if (bdvWord.Word.Length + dktWord.Word.Length == orderedWordChars.Length
                        && String.Concat((bdvWord.Word + dktWord.Word).OrderBy(c => c)).Equals(orderedWordChars)
                        && (bdvWord.Word + dktWord.Word) != myWords
                        && !wordsArr.Contains(dktWord.Word) && !wordsArr.Contains(bdvWord.Word))
                    {
                        anagramList.Add(bdvWord.Word + " " + dktWord.Word);
                    }
                }
            }

            return anagramList;
        }
    }
}
