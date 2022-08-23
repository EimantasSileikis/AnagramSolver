using AnagramSolver.Contracts.Interfaces.Core;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public HomeController(IAnagramSolver anagramSolver, IUnitOfWork unitOfWork, IConfiguration config)
        {
            _anagramSolver = anagramSolver;
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<IActionResult> Index(string word)
        {
            //_wordRepository.DeleteTableData("SearchHistory");

            if (word == null || word == string.Empty)
                return View(null);

            //Response.Cookies.Append("lastInput", word);

            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            if (ipAddress == null)
                return BadRequest();

            IEnumerable<string> data = new List<string>();

            if (await _unitOfWork.SearchLimit.Exist(x => x.Ip == ipAddress))
            {
                var ipSearchLimit = await _unitOfWork.SearchLimit.GetByIpAsync(ipAddress);
                if (_unitOfWork.SearchHistory.Find(x => x.IpAddress == ipAddress).Count() >= ipSearchLimit?.Limit)
                    return View("../Redirections/LimitReached");
            }
            else
                await _unitOfWork.SearchLimit.AddAsync(new SearchLimit { Ip = ipAddress, Limit = _config.GetValue<uint>("SearchLimit") });

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            data = await _anagramSolver.GetAnagramsAsync(word);
            stopwatch.Stop();

            await _unitOfWork.SearchHistory.AddSearchHistoryAsync(ipAddress, word, data, (int)stopwatch.ElapsedMilliseconds);
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
            if (!_unitOfWork.Words.WordExists(wordModel))
            {
                if(await ModifySearchLimit(1))
                {
                    await _unitOfWork.Words.AddAsync(wordModel);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                    return BadRequest("Could not create word");
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

        public async Task<IActionResult> DeleteWord(int id)
        {
            var word = await _unitOfWork.Words.GetAsync(id);
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            if (word != null && ipAddress != null)
            {
                if(await ModifySearchLimit(1, true))
                {
                    _unitOfWork.Words.Remove(word);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction("Anagrams");
                }
                else
                    return BadRequest("Cound not delete word");

            }
            else
            {
                return BadRequest("You can't delete word");

            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var word = await _unitOfWork.Words.GetAsync(id);

            return View(word);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditWord([Bind("Id,Word,PartOfSpeech,Number")] WordModel wordModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(await ModifySearchLimit(1))
            {
                _unitOfWork.Words.Edit(wordModel);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Anagrams");
            }
            else
                return BadRequest("Could not edit word");
        }

        private async Task<bool> ModifySearchLimit(uint increaseBy, bool checkSearchLimits = false)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ipAddress == null)
                return false;

            if (await _unitOfWork.SearchLimit.Exist(x => x.Ip == ipAddress))
            {
                var userIp = await _unitOfWork.SearchLimit.GetByIpAsync(ipAddress);

                if (checkSearchLimits && _unitOfWork.SearchHistory.Find(x => x.IpAddress == ipAddress).Count() >= userIp?.Limit)
                    return false;

                if (userIp != null)
                {
                    if (checkSearchLimits)
                        userIp.Limit -= increaseBy;
                    else
                        userIp.Limit += increaseBy;
                }
            }
            else
            {
                if (checkSearchLimits)
                    await _unitOfWork.SearchLimit.AddAsync(new SearchLimit
                    { Ip = ipAddress, Limit = _config.GetValue<uint>("SearchLimit") - increaseBy });
                else
                    await _unitOfWork.SearchLimit.AddAsync(new SearchLimit
                    { Ip = ipAddress, Limit = _config.GetValue<uint>("SearchLimit") + increaseBy });
            }
            return true;
        }
    }
}