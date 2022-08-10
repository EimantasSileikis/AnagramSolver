using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts.Interfaces;
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

        [SetUp]
        public void Setup()
        {
            _fileReader = new Mock<IFileReader>();
        }

        [Test]
        public void LoadDictionary_Always_ReturnsListWithoutDuplicates()
        {
            _fileReader.Setup(x => x.ReadFile("")).Returns(new string[] { "labas\tbdv\tlabas\t3", "labas\tbdv\tlabas\t3" });
            Mock<IFileWriter> _fileWriter = new Mock<IFileWriter>();
            var wordRepository = new WordRepository(_fileReader.Object, _fileWriter.Object);
            wordRepository.dictionaryPath = "";

            var words = wordRepository.LoadDictionary();

            Assert.That(words.Count, Is.EqualTo(1));
        }

    }
}
