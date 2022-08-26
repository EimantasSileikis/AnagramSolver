using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Interfaces.Files;
using AnagramSolver.Contracts.Models;
using Moq;

namespace AnagramSolver.Tests.BussinesLogicTests
{
    public class WordRepositoryTests
    {
        Mock<IFileManager> _fileManager;

        [SetUp]
        public void Setup()
        {
            _fileManager = new Mock<IFileManager>();
        }

        [Test]
        public void LoadDictionary_Always_ReturnsListWithoutDuplicates()
        {
            _fileManager.Setup(x => x.ReadFile("")).Returns(new string[] { "labas\tbdv\tlabas\t3", "labas\tbdv\tlabas\t3" });
            var wordRepository = new WordRepository(_fileManager.Object);
            wordRepository.dictionaryPath = "";

            var result = wordRepository.LoadDictionary();

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void LoadDictionary_Always_ReturnsList()
        {
            _fileManager.Setup(x => x.ReadFile("")).Returns(new string[] { "labas\tbdv\tlabas\t3", "balas\tdkt\tbalas\t1", "stalas\tdkt\tstalas\t2" });
            var wordRepository = new WordRepository(_fileManager.Object);
            wordRepository.dictionaryPath = "";
            var expected = new HashSet<WordModel> {  new WordModel { Word = "labas", PartOfSpeech = "bdv", Number = 3 },
                                                new WordModel { Word = "balas", PartOfSpeech = "dkt", Number = 1 },
                                                new WordModel { Word = "stalas", PartOfSpeech = "dkt", Number = 2 }};

            var result = wordRepository.LoadDictionary();

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void WordExists_WhenExists_ReturnsTrue()
        {
            var wordRepository = new WordRepository(_fileManager.Object);
            wordRepository.Words.Add(new WordModel { Word = "labas", PartOfSpeech = "bdv", Number = 3 });

            var result = wordRepository.WordExists(new WordModel { Word = "labas", PartOfSpeech = "bdv", Number = 3 });

            Assert.That(result, Is.True);
        }

        [Test]
        public void WordExists_WhenDoesNotExist_ReturnsFalse()
        {
            var wordRepository = new WordRepository(_fileManager.Object);

            var result = wordRepository.WordExists(new WordModel { Word = "labas", PartOfSpeech = "bdv", Number = 3 });

            Assert.That(result, Is.False);
        }

        [Test]
        public void AddWord_WhenWordIsNotNull_AddsWordToHashSet()
        {
            var wordRepository = new WordRepository(_fileManager.Object);
            var word = new WordModel { Word = "labas", PartOfSpeech = "bdv", Number = 3 };
            var expected = new HashSet<WordModel> { word };

            wordRepository.SaveWord(word);
            var result = wordRepository.Words;

            Assert.That(result, Is.EquivalentTo(expected));
        }
    }
}
