using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests.BussinesLogicTests
{
    public class FileReaderTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void ReadFile_FileDoesNotExist_ReturnsEmptyArray()
        {
            IFileReader fileReader = new FileReader();

            var result = fileReader.ReadFile("");

            Assert.That(result, Is.Empty);
        }
    }
}
