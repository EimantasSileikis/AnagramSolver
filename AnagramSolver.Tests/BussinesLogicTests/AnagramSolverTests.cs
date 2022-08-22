using AnagramSolver.Contracts.Interfaces.Core;
using AnagramSolver.Contracts.Interfaces.Repositories;
using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text;

namespace AnagramSolver.Tests.BussinesLogicTests
{
    public class AnagramSolverTests
    {
        Mock<IUnitOfWork> _unitOfWork;
        IConfiguration _configuration;
        BusinessLogic.Core.AnagramSolver anagramSolver;

        [SetUp]
        public void Setup()
        {
            var appSettings = "{\"MinWordLength\": 1,\"MaxAnagrams\": 3}";

            _configuration = new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(appSettings)))
                .Build();

            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.Setup(x => x.Words.GetAllAsync()).ReturnsAsync(GetSampleWords());
            _unitOfWork.Setup(x => x.CachedWords.WordExists(It.IsAny<string>())).Returns(false);

            anagramSolver = new BusinessLogic.Core.AnagramSolver(_unitOfWork.Object, _configuration);
        }

        [TestCase("solo", "Oslo")]
        [TestCase("Oslo", "solo")]
        public async Task GetAnagrams_CapitalLettersInInputOrDictionary_ReturnsAllResults(string input, string expectedResult)
        {
            var expected = new List<string> { expectedResult };

            var result = await anagramSolver.GetAnagramsAsync(input);

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task GetAnagrams_OneInputWords_ReturnsAnagramsWithOneWord()
        {
            var expected = new List<string> { "balas" };

            var result = await anagramSolver.GetAnagramsAsync("labas");

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task GetAnagrams_FewInputWords_ReturnsListWithStringPairs()
        {
            var expected = new List<string> { "tyras balas" };
            var result = await anagramSolver.GetAnagramsAsync("la bas rytas");

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public async Task GetAnagrams_FindsMoreAnagramsThanMax_ReturnsListWithSpecifiedCount()
        {
            var result = await anagramSolver.GetAnagramsAsync("tops");

            Assert.That(result.Count, Is.EqualTo(_configuration.GetValue<int>("MaxAnagrams")));
        }

        [Test]
        public async Task GetAnagrams_FindsLessAnagramsThanMax_ReturnsListWithAllSolutions()
        {
            var expected = new List<string> { "Oslo", "solo" };

            var result = await anagramSolver.GetAnagramsAsync("loso");

            Assert.That(result.Count, Is.EqualTo(expected.Count));
        }

        private HashSet<WordModel> GetSampleWords()
        {
            HashSet<WordModel> output = new HashSet<WordModel>
            {
                new WordModel
                {
                    Word = "balas",
                    Number = 1,
                    PartOfSpeech = "dkt"
                },
                new WordModel
                {
                    Word = "labas",
                    Number = 1,
                    PartOfSpeech = "bdv"
                },
                new WordModel
                {
                    Word = "Oslo",
                    Number = 1,
                    PartOfSpeech = "tikr. dkt"
                },
                new WordModel
                {
                    Word = "solo",
                    Number = 1,
                    PartOfSpeech = "bdv"
                },
                new WordModel
                {
                    Word = "tyras",
                    Number = 1,
                    PartOfSpeech = "bdv"
                },
                new WordModel
                {
                    Word = "stop",
                    Number = 1,
                    PartOfSpeech = "dkt"
                },
                new WordModel
                {
                    Word = "post",
                    Number = 1,
                    PartOfSpeech = "dkt"
                },
                new WordModel
                {
                    Word = "pots",
                    Number = 1,
                    PartOfSpeech = "dkt"
                },
                new WordModel
                {
                    Word = "spot",
                    Number = 1,
                    PartOfSpeech = "dkt"
                }
            };

            return output;
        }
    }
}