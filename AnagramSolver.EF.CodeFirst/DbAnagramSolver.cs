using AnagramSolver.Contracts.Interfaces.Core;
using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AnagramSolver.EF.CodeFirst
{
    public class DbAnagramSolver : IAnagramSolver
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly string url = "https://localhost:7127/api/anagrams/";

        public DbAnagramSolver(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<IEnumerable<string>> GetAnagramsAsync(string myWords)
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

            if (await _unitOfWork.CachedWords.Exist(x => x.Word == myWords))
            {
                var anagrams = _unitOfWork.CachedWords.GetCachedWordAnagrams(myWords);
                return anagrams.Take(maxAnagrams);
            }
            else
            {
                var anagrams = await FindAnagrams(myWords);
                if (anagrams.Count() > 0)
                {
                    List<Anagram> anagramList = new List<Anagram>();
                    anagrams.ToList().ForEach(word => anagramList.Add(new Anagram { Word = word }));
                    await _unitOfWork.CachedWords.AddAsync(new CachedWord { Word = myWords, Anagrams = anagramList });
                    await _unitOfWork.CompleteAsync();
                }

                return anagrams.Take(maxAnagrams);
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

        private async Task<IEnumerable<string>> FindAnagrams(string myWords)
        {
            string[] wordsArray = myWords.Split(" ");
            var words = await _unitOfWork.Words.GetAllAsync();
            myWords = myWords.Replace(" ", "").ToLower();
            var orderedWordChars = string.Concat(myWords.OrderBy(c => c));

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

        private IList<string> FindAnagramsWithFewWords(IEnumerable<WordModel> words, string[] wordsArr, string orderedWordChars, string myWords)
        {
            IList<string> anagramList = new List<string>();

            var dktWords = words
                .Where(word => word.PartOfSpeech == "dkt"
                && string.Concat(word.Word.Replace(" ", "").ToLower().OrderBy(c => c)).All(orderedWordChars.Contains));

            var bdvWords = words
                .Where(word => word.PartOfSpeech == "bdv"
                && string.Concat(word.Word.Replace(" ", "").ToLower().OrderBy(c => c)).All(orderedWordChars.Contains));

            foreach (var bdvWord in bdvWords)
            {
                foreach (var dktWord in dktWords)
                {
                    if (bdvWord.Word.Length + dktWord.Word.Length == orderedWordChars.Length
                        && string.Concat((bdvWord.Word + dktWord.Word).OrderBy(c => c)).Equals(orderedWordChars)
                        && bdvWord.Word + dktWord.Word != myWords
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
