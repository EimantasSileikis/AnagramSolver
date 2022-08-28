using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AnagramSolver.Tests.BussinesLogicTests
{
    public class CachedWordRepositoryTests
    {
        private CachedWordRepository _repository;

        [SetUp]
        public void SetUp()
        {
            
            var queryable = GetSampleData().AsQueryable();

            var context = new Mock<CodeFirstContext>(new DbContextOptions<CodeFirstContext>());
            var cachedWordsDbSet = new Mock<DbSet<CachedWord>>();
            cachedWordsDbSet.As<IQueryable<CachedWord>>().Setup(m => m.Provider).Returns(queryable.Provider);
            cachedWordsDbSet.As<IQueryable<CachedWord>>().Setup(m => m.Expression).Returns(queryable.Expression);
            cachedWordsDbSet.As<IQueryable<CachedWord>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            cachedWordsDbSet.As<IQueryable<CachedWord>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            context.Setup(x => x.CachedWords).Returns(cachedWordsDbSet.Object);
            _repository = new CachedWordRepository(context.Object);
        }

        [Test]
        public void GetCachedWordAnagrams_CachedWordFound_ReturnsIEnumerableOfAnagrams()
        {
            var expexted = new List<string?> {"tea", "eta"};

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
