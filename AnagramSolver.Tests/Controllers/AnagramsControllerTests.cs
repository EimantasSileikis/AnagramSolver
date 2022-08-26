using AnagramSolver.Contracts.Interfaces.Core;
using AnagramSolver.WebApp.Controllers.Api;
using Moq;
using NSubstitute;

namespace AnagramSolver.Tests.Controllers
{
    public class AnagramsControllerTests
    {
        [Test]
        public async Task GetAnagrams_Always_ReturnsIEnumerableOfString()
        {
            IAnagramSolver _anagramSolver = Substitute.For<IAnagramSolver>();
            var controller = new AnagramsController(_anagramSolver);

            var result = await controller.GetAnagrams("a");

            Assert.That(result, Is.InstanceOf<IEnumerable<string>>());
        }
    }
}
