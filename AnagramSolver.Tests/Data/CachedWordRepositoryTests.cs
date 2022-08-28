using AnagramSolver.BusinessLogic.Data;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using MockQueryable.NSubstitute;

namespace AnagramSolver.Tests.Data
{
    public class CachedWordRepositoryTests
    {
        private CachedWordRepository _repository;

        [SetUp]
        public void SetUp()
        {

            var queryable = GetSampleData().AsQueryable();

            var context = Substitute.For<CodeFirstContext>(new DbContextOptions<CodeFirstContext>());
            var cachedWordsDbSet = queryable.BuildMockDbSet();
            context.CachedWords.Returns(cachedWordsDbSet);
            _repository = new CachedWordRepository(context);
        }

        [Test]
        public void GetCachedWordAnagrams_CachedWordFound_ReturnsIEnumerableOfAnagrams()
        {
            var expexted = new List<string?> { "tea", "eta" };

            var result = _repository.GetCachedWordAnagrams("ate");

            Assert.That(result, Is.EquivalentTo(expexted));
        }

        [Test]
        public void GetCachedWordAnagrams_CachedWordNotFound_ReturnsEmptyIEnumerable()
        {
            var result = _repository.GetCachedWordAnagrams("");

            Assert.That(result, Is.Empty);
        }

        private List<CachedWord> GetSampleData()
        {
            var list = new List<CachedWord>(){
                new CachedWord {
                    Word = "ate", Anagrams = new List<Anagram> {
                        new Anagram { Word = "tea" },
                        new Anagram { Word = "eta"}
                    }
                }
            };

            return list;
        }
    }
}
