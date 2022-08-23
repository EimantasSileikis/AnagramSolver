using AnagramSolver.Contracts.Interfaces.Core;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Controllers;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text;

namespace AnagramSolver.Tests.ControllersTests
{
    public class HomeControllerTests
    {
        Mock<IAnagramSolver> _anagramSolver;
        Mock<IUnitOfWork> _unitOfWork;
        IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            var appSettings = "{\"MinWordLength\": 1,\"MaxAnagrams\": 3}";

            _configuration = new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(appSettings)))
                .Build();

            _anagramSolver = new Mock<IAnagramSolver>();
            _unitOfWork = new Mock<IUnitOfWork>();
        }

        [Test]
        public async Task Index_WithInput_ReturnsAViewResult()
        {
            _anagramSolver.Setup(x => x.GetAnagramsAsync("abc")).ReturnsAsync(new List<string> { "bac", "cab" });
            _unitOfWork.Setup(x => x.SearchHistory.AddAsync(It.IsAny<SearchHistory>()));
            var controller = new HomeController(_anagramSolver.Object, _unitOfWork.Object, _configuration);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    Connection =
                    {
                        RemoteIpAddress = new System.Net.IPAddress(123456789)
                    }
                }
            };
            var result = await controller.Index("abc");

            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = (ViewResult)result;
            Assert.IsAssignableFrom<List<string>>(viewResult.ViewData.Model);
        }

        [Test]
        public async Task Index_InputIsNullOrEmpty_ReturnsAEmptyViewResult()
        {
            var controller = new HomeController(_anagramSolver.Object, _unitOfWork.Object, _configuration);

            var result = await controller.Index("");

            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = (ViewResult)result;
            Assert.That(viewResult.ViewData.Model, Is.Null);
        }

        [Test]
        public async Task Anagrams_WithAHashSet_ReturnsAViewResult()
        {
            var list = new HashSet<WordModel> { new WordModel { Word = "abc", PartOfSpeech = "dkt", Number = 1 } };
            _unitOfWork.Setup(x => x.Words.GetAllAsync()).ReturnsAsync(list);

            var controller = new HomeController(_anagramSolver.Object, _unitOfWork.Object, _configuration);

            var result = await controller.Anagrams(null);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = (ViewResult)result;
            Assert.IsAssignableFrom<PaginatedList<WordModel>>(viewResult.ViewData.Model);
        }

        [Test]
        public async Task CreateWord_WhenModelStateIsInvalid_ReturnsBadRequestResult()
        {
            var controller = new HomeController(_anagramSolver.Object, _unitOfWork.Object, _configuration);
            controller.ModelState.AddModelError("BaseWord", "Required");
            var word = new WordModel();

            var result = await controller.CreateWord(word);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task CreateWord_WhenWordAlreadyExists_ReturnsBadRequestResult()
        {
            var word = new WordModel
            {
                Word = "balas",
                Number = 1,
                PartOfSpeech = "dkt"
            };
            _unitOfWork.Setup(x => x.Words.WordExists(word)).Returns(true);
            var controller = new HomeController(_anagramSolver.Object, _unitOfWork.Object, _configuration);

            var result = await controller.CreateWord(word);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task CreateWord_WhenInputIsValid_ReturnsARedirectAndAddsWord()
        {
            var word = new WordModel
            {
                Word = "balas",
                Number = 1,
                PartOfSpeech = "dkt"
            };
            _unitOfWork.Setup(x => x.Words.WordExists(word)).Returns(false);
            var controller = new HomeController(_anagramSolver.Object, _unitOfWork.Object, _configuration);

            var result = await controller.CreateWord(word);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            _unitOfWork.Verify();
        }
    }
}