using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace AnagramSolver.Tests
{
    public class WordRepositoryTestsShouldly
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
            var wordRepository = new WordRepository(_fileReader.Object);
            wordRepository.dictionaryPath = "";

            var words = wordRepository.LoadDictionary();

            words.Count.ShouldBe(1);
        }

    }
}
