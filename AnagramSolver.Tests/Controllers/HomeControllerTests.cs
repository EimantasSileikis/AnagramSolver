using AnagramSolver.Contracts.Interfaces.Core;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Controllers;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Linq.Expressions;
using System.Text;
using NSubstitute;

namespace AnagramSolver.Tests.Controllers
{
    public class HomeControllerTests
    {
        IAnagramSolver _anagramSolver;
        IUnitOfWork _unitOfWork;
        IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            var appSettings = "{\"MinWordLength\": 1,\"MaxAnagrams\": 3, \"SearchLimit\" : 1}";

            _configuration = new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(appSettings)))
                .Build();

            _anagramSolver = Substitute.For<IAnagramSolver>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
        }

        [Test]
        public async Task Index_WithInput_ReturnsAViewResult()
        {
            _anagramSolver.GetAnagramsAsync("abc").Returns(Task.FromResult(new List<string> { "bac", "cab" }.AsEnumerable()));
            //_unitOfWork.Setup(x => x.SearchHistory.AddAsync(Arg.Any<SearchHistory>()));
            _unitOfWork.SearchLimit.Exist(Arg.Any<Expression<Func<SearchLimit, bool>>>()).Returns(Task.FromResult(true));
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);
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
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);

            var result = await controller.Index("");

            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = (ViewResult)result;
            Assert.That(viewResult.ViewData.Model, Is.Null);
        }

        [Test]
        public async Task Index_IpAddressIsNull_ReturnsAEmptyViewResult()
        {
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);

            var result = await controller.Index("a");

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Index_UserReachedSearchLimit_ReturnsAEmptyView()
        {
            _unitOfWork.SearchLimit.Exist(Arg.Any<Expression<Func<SearchLimit, bool>>>()).Returns(Task.FromResult(true));
            _unitOfWork.SearchHistory.Find(Arg.Any<Expression<Func<SearchHistory, bool>>>())
                .Returns(new List<SearchHistory>() { new SearchHistory(), new SearchHistory(), new SearchHistory() });
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);
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

            var result = await controller.Index("a");

            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = (ViewResult)result;
            Assert.That(viewResult.ViewData.Model, Is.Empty);

        }

        [Test]
        public async Task Anagrams_WithAHashSet_ReturnsAViewResult()
        {
            var list = new HashSet<WordModel> { new WordModel { Word = "abc", PartOfSpeech = "dkt", Number = 1 } };
            _unitOfWork.Words.GetAllAsync().Returns(Task.FromResult(list.AsEnumerable()));

            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);

            var result = await controller.Anagrams(null);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = (ViewResult)result;
            Assert.IsAssignableFrom<PaginatedList<WordModel>>(viewResult.ViewData.Model);
        }

        [Test]
        public void SearchWord_InputIsNullOrEmpty_ReturnsViewWithNullObject()
        {
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);

            var result = controller.SearchWord(null);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = (ViewResult)result;
            Assert.That(viewResult.ViewData.Model, Is.Null);
        }

        [Test]
        public void SearchWord_ValidInput_ReturnsViewWithWords()
        {
            _unitOfWork.Words.GetSearchWords(Arg.Any<string>()).Returns(new List<WordModel> { new WordModel() });
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);

            var result = controller.SearchWord("a");

            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = (ViewResult)result;
            Assert.IsAssignableFrom<List<WordModel>>(viewResult.ViewData.Model);
        }

        [Test]
        public async Task CreateWord_WhenModelStateIsInvalid_ReturnsBadRequestResult()
        {
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);
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
            _unitOfWork.Words.WordExists(word).Returns(true);
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);

            var result = await controller.CreateWord(word);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task CreateWord_WhenUserReachedSearchLimit_ReturnsBadRequestResult()
        {
            var word = new WordModel
            {
                Word = "balas",
                Number = 1,
                PartOfSpeech = "dkt"
            };
            _unitOfWork.Words.WordExists(word).Returns(false);
            _unitOfWork.SearchLimit.ModifySearchLimit(
                Arg.Any<string>(), Arg.Any<uint>(), Arg.Any<uint>(), Arg.Any<bool>())
                .Returns(Task.FromResult(false));

            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);
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

            var result = await controller.CreateWord(word);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task CreateWord_WhenModelIsValid_ReturnsARedirectAndAddsWord()
        {
            var word = new WordModel
            {
                Word = "balas",
                Number = 1,
                PartOfSpeech = "dkt"
            };
            _unitOfWork.Words.WordExists(word).Returns(false);
            _unitOfWork.SearchLimit.ModifySearchLimit(
                Arg.Any<string>(), Arg.Any<uint>(), Arg.Any<uint>(), Arg.Any<bool>())
                .Returns(Task.FromResult(true));
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);
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

            var result = await controller.CreateWord(word);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            _unitOfWork.Received();
        }

        [Test]
        public async Task Delete_WhenWordDoesNotExist_ReturnsBadRequestResult()
        {
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);

            var result = await controller.DeleteWord(0);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Delete_SearchLimitReached_ReturnsBadRequestResult()
        {
            _unitOfWork.Words.GetAsync(Arg.Any<int>()).Returns(Task.FromResult<WordModel?>(new WordModel()));
            _unitOfWork.SearchLimit.ModifySearchLimit(
                Arg.Any<string>(), Arg.Any<uint>(), Arg.Any<uint>(), Arg.Any<bool>())
                .Returns(Task.FromResult(false));
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);
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

            var result = await controller.DeleteWord(0);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Delete_SearchLimitReached_ReturnsARedirectAndDeletesWord()
        {
            _unitOfWork.Words.GetAsync(Arg.Any<int>()).Returns(Task.FromResult<WordModel?>(new WordModel()));
            _unitOfWork.SearchLimit.ModifySearchLimit(
                Arg.Any<string>(), Arg.Any<uint>(), Arg.Any<uint>(), Arg.Any<bool>())
                .Returns(Task.FromResult(true));
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);
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

            var result = await controller.DeleteWord(0);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        }

        [Test]
        public async Task EditWord_WhenModelStateIsInvalid_ReturnsBadRequestResult()
        {
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);
            controller.ModelState.AddModelError("BaseWord", "Required");
            var word = new WordModel();

            var result = await controller.EditWord(word);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task EditWord_WhenModelIsValid_ReturnsARedirectAndEditsWord()
        {
            var word = new WordModel
            {
                Word = "balas",
                Number = 1,
                PartOfSpeech = "dkt"
            };
            _unitOfWork.SearchLimit.ModifySearchLimit(
                Arg.Any<string>(), Arg.Any<uint>(), Arg.Any<uint>(), Arg.Any<bool>())
                .Returns(Task.FromResult(true));
            _unitOfWork.Words.Edit(Arg.Any<WordModel>());
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);
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

            var result = await controller.EditWord(word);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        }

        [Test]
        public async Task EditWord_WhenModificationOfSearchLimitsDidNotWork_ReturnsARedirectAndEditsWord()
        {
            var word = new WordModel
            {
                Word = "balas",
                Number = 1,
                PartOfSpeech = "dkt"
            };
            _unitOfWork.SearchLimit.ModifySearchLimit(
                Arg.Any<string>(), Arg.Any<uint>(), Arg.Any<uint>(), Arg.Any<bool>())
                .Returns(Task.FromResult(false));
            var controller = new HomeController(_anagramSolver, _unitOfWork, _configuration);
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

            var result = await controller.EditWord(word);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}