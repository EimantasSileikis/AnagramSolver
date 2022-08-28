using AnagramSolver.Contracts.Interfaces.Core;
using AnagramSolver.WebApp.Controllers.Api;
using Moq;

namespace AnagramSolver.Tests.ControllersTests
{
    public class AnagramsControllerTests
    {
        [Test]
        public async Task GetAnagrams_Always_ReturnsIEnumerableOfString()
        {
            Mock<IAnagramSolver> _anagramSolver = new Mock<IAnagramSolver>();
            var controller = new AnagramsController(_anagramSolver.Object);

            var result = await controller.GetAnagrams("a");

            Assert.That(result, Is.InstanceOf<IEnumerable<string>>());
        }
    }
}
