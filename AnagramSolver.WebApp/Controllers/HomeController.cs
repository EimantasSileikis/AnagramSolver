using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly IWordRepository _wordRepository;

        public HomeController(IAnagramSolver anagramSolver, IWordRepository wordRepository)
        {
            _anagramSolver = anagramSolver;
            _wordRepository = wordRepository;
        }

        public IActionResult Index(string word)
        {
            if (word == null || word == string.Empty)
                return View(null);

            var data = _anagramSolver.GetAnagrams(word);

            return View(data);
        }

        public IActionResult Anagrams(int? pageNumber)
        {
            var data = _wordRepository.Words;
            int pageSize = 100;

            return View(PaginatedList<Word>.Create(data, pageNumber ?? 1, pageSize));
        }

        public IActionResult CreateWord()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateWord(
            [Bind("BaseWord,PartOfSpeech,Number")] Word word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var condition = !_wordRepository.WordExists(word);
            if (condition)
            {
                _wordRepository.AddWord(word);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest("Word already exists");
            }
        }
    }
}