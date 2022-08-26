using AnagramSolver.Contracts.Interfaces.Core;
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
        public async Task<IEnumerable<string>> GetAnagrams(string word)
        {
            var anagrams = await _anagramSolver.GetAnagramsAsync(word);

            return anagrams;
        }
    }
}
