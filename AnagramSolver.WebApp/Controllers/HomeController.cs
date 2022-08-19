using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Models;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly IDbWordRepository _wordRepository;

        public HomeController(IAnagramSolver anagramSolver, IDbWordRepository wordRepository)
        {
            _anagramSolver = anagramSolver;
            _wordRepository = wordRepository;
        }

        public IActionResult Index(string word)
        {
            //_wordRepository.DeleteTableData("SearchHistory");

            if (word == null || word == string.Empty)
                return View(null);

            //Response.Cookies.Append("lastInput", word);

            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var data = _anagramSolver.GetAnagrams(word);
            stopwatch.Stop();
            _wordRepository.StoreSearchData(remoteIpAddress ?? "", word, data.ToList(), (int)stopwatch.ElapsedMilliseconds);

            return View(data);
        }

        public IActionResult Anagrams(int? pageNumber)
        {
            var data = _wordRepository.LoadDictionary();
            int pageSize = 100;

            return View(PaginatedList<WordModel>.Create(data, pageNumber ?? 1, pageSize));
        }

        public IActionResult SearchWord(string? word)
        {
            if(word == null || word == string.Empty)
            {
                return View(null);
            }
            else
            {
                var words = _wordRepository.SearchWord(word);

                return View(words);
            }
        }

        public IActionResult CreateWord()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateWord(
            [Bind("Word,PartOfSpeech,Number")] WordModel wordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var condition = !_wordRepository.WordExists(wordModel);
            if (condition)
            {
                _wordRepository.AddWord(wordModel);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest("Word already exists");
            }
        }

        public IActionResult SearchHistory()
        {
            var history = _wordRepository.GetSearchHistory();
            if(history != null)
                return View(history);

            return new EmptyResult();
        }
    }
}