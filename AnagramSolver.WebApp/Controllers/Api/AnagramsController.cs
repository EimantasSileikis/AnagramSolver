using AnagramSolver.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnagramsController : ControllerBase
    {
        private readonly IAnagramSolver _anagramSolver;

        public AnagramsController(IAnagramSolver anagramSolver)
        {
            _anagramSolver = anagramSolver;
        }

        [HttpGet("{word}")]
        public IList<string> GetAnagrams(string word)
        {
            var anagrams = _anagramSolver.GetAnagrams(word);

            return anagrams;
        }
    }
}
