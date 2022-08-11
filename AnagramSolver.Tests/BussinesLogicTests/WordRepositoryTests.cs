using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests.BussinesLogicTests
{
    public class WordRepositoryTests
    {
        Mock<IFileReader> _fileReader;
        Mock<IFileWriter> _fileWriter;

        [SetUp]
        public void Setup()
        {
            _fileReader = new Mock<IFileReader>();
            _fileWriter = new Mock<IFileWriter>();
        }

        [Test]
        public void LoadDictionary_Always_ReturnsListWithoutDuplicates()
        {
            _fileReader.Setup(x => x.ReadFile("")).Returns(new string[] { "labas\tbdv\tlabas\t3", "labas\tbdv\tlabas\t3" });
            var wordRepository = new WordRepository(_fileReader.Object, _fileWriter.Object);
            wordRepository.dictionaryPath = "";

            var result = wordRepository.LoadDictionary();

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void LoadDictionary_Always_ReturnsList()
        {
            _fileReader.Setup(x => x.ReadFile("")).Returns(new string[] { "labas\tbdv\tlabas\t3", "balas\tdkt\tbalas\t1", "stalas\tdkt\tstalas\t2" });
            var wordRepository = new WordRepository(_fileReader.Object, _fileWriter.Object);
            wordRepository.dictionaryPath = "";
            var expected = new HashSet<Word> {  new Word { BaseWord = "labas", PartOfSpeech = "bdv", Number = 3 },
                                                new Word { BaseWord = "balas", PartOfSpeech = "dkt", Number = 1 },
                                                new Word { BaseWord = "stalas", PartOfSpeech = "dkt", Number = 2 }};

            var result = wordRepository.LoadDictionary();

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void WordExists_WhenExists_ReturnsTrue()
        {
            var wordRepository = new WordRepository(_fileReader.Object, _fileWriter.Object);
            wordRepository.Words.Add(new Word { BaseWord = "labas", PartOfSpeech = "bdv", Number = 3 });

            var result = wordRepository.WordExists(new Word { BaseWord = "labas", PartOfSpeech = "bdv", Number = 3});

            Assert.That(result, Is.True);
        }

        [Test]
        public void WordExists_WhenDoesNotExist_ReturnsFalse()
        {
            var wordRepository = new WordRepository(_fileReader.Object, _fileWriter.Object);

            var result = wordRepository.WordExists(new Word { BaseWord = "labas", PartOfSpeech = "bdv", Number = 3 });

            Assert.That(result, Is.False);
        }

        [Test]
        public void AddWord_WhenWordIsNotNull_AddsWordToHashSet()
        {
            var wordRepository = new WordRepository(_fileReader.Object, _fileWriter.Object);
            var word = new Word { BaseWord = "labas", PartOfSpeech = "bdv", Number = 3 };
            var expected = new HashSet<Word> { word };

            wordRepository.AddWord(word);
            var result = wordRepository.Words;

            Assert.That(result, Is.EquivalentTo(expected));
        }
    }
}
