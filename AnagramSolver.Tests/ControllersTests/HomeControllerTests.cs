using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Controllers;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests.ControllersTests
{
    public class HomeControllerTests
    {
        Mock<IAnagramSolver> _anagramSolver;
        Mock<IDbWordRepository> _wordRepository;

        [SetUp]
        public void Setup()
        {
            _anagramSolver = new Mock<IAnagramSolver>();
            _wordRepository = new Mock<IDbWordRepository>();
        }

        [Test]
        public void Index_WithInput_ReturnsAViewResult()
        {
            _anagramSolver.Setup(x => x.GetAnagrams("abc")).Returns(new List<string> { "bac", "cab" });
            var controller = new HomeController(_anagramSolver.Object, _wordRepository.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    Connection =
                {
                    RemoteIpAddress = new System.Net.IPAddress(16885952)
                }
                }
            };
            var result = controller.Index("abc");

            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = (ViewResult)result;
            Assert.IsAssignableFrom<List<string>>(viewResult.ViewData.Model);
        }

        [Test]
        public void Index_InputIsNullOrEmpty_ReturnsAEmptyViewResult()
        {
            var controller = new HomeController(_anagramSolver.Object, _wordRepository.Object);

            var result = controller.Index("");

            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = (ViewResult)result;
            Assert.That(viewResult.ViewData.Model, Is.Null);
        }

        [Test]
        public void Anagrams_WithAHashSet_ReturnsAViewResult()
        {
            _wordRepository.Setup(x => x.LoadDictionary()).Returns(GetSampleWords());
            var controller = new HomeController(_anagramSolver.Object, _wordRepository.Object);

            var result = controller.Anagrams(null);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            ViewResult viewResult = (ViewResult)result;
            Assert.IsAssignableFrom<PaginatedList<WordModel>>(viewResult.ViewData.Model);
        }

        [Test]
        public void CreateWord_WhenModelStateIsInvalid_ReturnsBadRequestResult()
        {
            var controller = new HomeController(_anagramSolver.Object, _wordRepository.Object);
            controller.ModelState.AddModelError("BaseWord", "Required");
            var word = new WordModel();

            var result = controller.CreateWord(word);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void CreateWord_WhenWordAlreadyExists_ReturnsBadRequestResult()
        {
            var word = new WordModel
            {
                Word = "balas",
                Number = 1,
                PartOfSpeech = "dkt"
            };
            _wordRepository.Setup(x => x.WordExists(word)).Returns(true);
            var controller = new HomeController(_anagramSolver.Object, _wordRepository.Object);

            var result = controller.CreateWord(word);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void CreateWord_WhenInputIsVlaid_ReturnsARedirectAndAddsSession()
        { 
            var controller = new HomeController(_anagramSolver.Object, _wordRepository.Object);
            var word = new WordModel
            {
                Word = "balas",
                Number = 1,
                PartOfSpeech = "dkt"
            };

            var result = controller.CreateWord(word);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            _wordRepository.Verify();
        }

        private HashSet<WordModel> GetSampleWords()
        {
            HashSet<WordModel> output = new HashSet<WordModel>
            {
                new WordModel
                {
                    Word = "balas",
                    Number = 1,
                    PartOfSpeech = "dkt"
                },
                new WordModel
                {
                    Word = "labas",
                    Number = 1,
                    PartOfSpeech = "bdv"
                }
            };

            return output;
        }
    }
}
