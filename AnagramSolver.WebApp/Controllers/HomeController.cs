using AnagramSolver.Contracts.Interfaces.Core;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IAnagramSolver anagramSolver, IUnitOfWork unitOfWork)
        {
            _anagramSolver = anagramSolver;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string word)
        {
            //_wordRepository.DeleteTableData("SearchHistory");

            if (word == null || word == string.Empty)
                return View(null);

            //Response.Cookies.Append("lastInput", word);

            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            IEnumerable<string> data = await _anagramSolver.GetAnagramsAsync(word);
            stopwatch.Stop();
            await _unitOfWork.SearchHistory
                .AddAsync(new SearchHistory
                {
                    IpAddress = remoteIpAddress ?? "",
                    SearchWord = word,
                    Anagrams = string.Join(",", data.ToArray()),
                    TimeSpent = (int)stopwatch.ElapsedMilliseconds
                });
            await _unitOfWork.CompleteAsync();

            return View(data);
        }

        public async Task<IActionResult> Anagrams(int? pageNumber)
        {
            int pageSize = 100;
            var data = await _unitOfWork.Words.GetAllAsync();

            return View(PaginatedList<WordModel>.Create(data, pageNumber ?? 1, pageSize));
        }

        public IActionResult SearchWord(string? word)
        {
            if (word == null || word == string.Empty)
            {
                return View(null);
            }
            else
            {
                var words = _unitOfWork.Words.GetSearchWords(word);

                return View(words);
            }
        }

        public IActionResult CreateWord()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateWord(
            [Bind("Word,PartOfSpeech,Number")] WordModel wordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var wordExists = _unitOfWork.Words.WordExists(wordModel);
            if (!wordExists)
            {
                await _unitOfWork.Words.AddAsync(wordModel);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest("Word already exists");
            }
        }

        public async Task<IActionResult> SearchHistory()
        {
            var history = await _unitOfWork.SearchHistory.GetAllAsync();
            if (history != null)
                return View(history);

            return new EmptyResult();
        }
    }
}