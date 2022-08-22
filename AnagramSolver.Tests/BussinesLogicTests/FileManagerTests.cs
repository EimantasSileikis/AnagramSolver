using AnagramSolver.BusinessLogic.Files;
using AnagramSolver.Contracts.Interfaces.Files;

namespace AnagramSolver.Tests.BussinesLogicTests
{
    public class FileManagerTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void ReadFile_FileDoesNotExist_ReturnsEmptyArray()
        {
            IFileManager fileManager = new FileManager();

            var result = fileManager.ReadFile("");

            Assert.That(result, Is.Empty);
        }
    }
}
