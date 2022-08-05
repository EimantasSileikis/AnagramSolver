using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text;

namespace AnagramSolver.Tests
{
    public class AnagramSolverTests
    {
        Mock<IWordRepository> _wordRepository;
        IConfiguration _configuration;
        AnagramSolver.BusinessLogic.AnagramSolver anagramSolver;

        [SetUp]
        public void Setup()
        {
            var appSettings = "{\"MinWordLength\": 1,\"MaxAnagrams\": 3}";

            _configuration = new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(appSettings)))
                .Build();

            _wordRepository = new Mock<IWordRepository>();
            _wordRepository.Setup(x => x.Words).Returns(GetSampleWords());

            anagramSolver = new AnagramSolver.BusinessLogic.AnagramSolver(_wordRepository.Object, _configuration);
        }

        [TestCase("solo", "Oslo")]
        [TestCase("Oslo", "solo")]
        public void GetAnagrams_CapitalLettersInInputOrDictionary_ReturnsAllResults(string input, string expectedResult)
        {
            var expected = new List<string> { expectedResult };

            var result = anagramSolver.GetAnagrams(input);

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void GetAnagrams_OneInputWords_ReturnsAnagramsWithOneWord()
        {
            var expected = new List<string> { "balas" };

            var result = anagramSolver.GetAnagrams("labas");

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void GetAnagrams_FewInputWords_ReturnsListWithStringPairs()
        {
            var expected = new List<string> { "tyras balas" };

            var result = anagramSolver.GetAnagrams("la bas rytas");

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void GetAnagrams_FindsMoreAnagramsThanMax_ReturnsListWithSpecifiedCount()
        {

            var result = anagramSolver.GetAnagrams("tops");

            Assert.That(result.Count, Is.EqualTo(_configuration.GetValue<int>("MaxAnagrams")));
        }

        [Test]
        public void GetAnagrams_FindsLessAnagramsThanMax_ReturnsListWithAllSolutions()
        {
            var expected = new List<string> { "Oslo", "solo"};

            var result = anagramSolver.GetAnagrams("loso");

            Assert.That(result.Count, Is.EqualTo(expected.Count));
        }

        private HashSet<Word> GetSampleWords()
        {
            HashSet<Word> output = new HashSet<Word>
            {
                new Word
                {
                    BaseWord = "balas",
                    Number = 1,
                    PartOfSpeech = "dkt"
                },
                new Word
                {
                    BaseWord = "labas",
                    Number = 1,
                    PartOfSpeech = "bdv"
                },
                new Word
                {
                    BaseWord = "Oslo",
                    Number = 1,
                    PartOfSpeech = "tikr. dkt"
                },
                new Word
                {
                    BaseWord = "solo",
                    Number = 1,
                    PartOfSpeech = "bdv"
                },
                new Word
                {
                    BaseWord = "tyras",
                    Number = 1,
                    PartOfSpeech = "bdv"
                },
                new Word
                {
                    BaseWord = "stop", 
                    Number = 1,
                    PartOfSpeech = "dkt"
                },
                new Word
                {
                    BaseWord = "post",
                    Number = 1,
                    PartOfSpeech = "dkt"
                },
                new Word
                {
                    BaseWord = "pots",
                    Number = 1,
                    PartOfSpeech = "dkt"
                },
                new Word
                {
                    BaseWord = "spot",
                    Number = 1,
                    PartOfSpeech = "dkt"
                }
            };

            return output;
        }
    }
}