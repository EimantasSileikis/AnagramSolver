using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace AnagramSolver.Tests.BussinesLogicTests
{
    public class SearchLimitRepositoryTests
    {
        private SearchLimitRepository _repository;

        [SetUp]
        public void SetUp()
        { 
            var queryable = GetSampleData().AsQueryable();
            var context = new Mock<CodeFirstContext>(new DbContextOptions<CodeFirstContext>());
            var searchLimitsDbSet = queryable.BuildMockDbSet();
            var searchHistoryDbSet = GetSearchHistoryData().AsQueryable().BuildMockDbSet();
            searchLimitsDbSet.Setup(d => d.Add(It.IsAny<SearchLimit>())).Callback<SearchLimit>((s) => GetSampleData().Add(s));
            context.Setup(x => x.Set<SearchLimit>()).Returns(searchLimitsDbSet.Object);
            context.Setup(x => x.SearchLimits).Returns(searchLimitsDbSet.Object);
            context.Setup(x => x.SearchHistories).Returns(searchHistoryDbSet.Object);
            _repository = new SearchLimitRepository(context.Object);
        }

        [Test]
        public async Task ModifySearchLimit_WhenIpIsNull_ReturnsFalse()
        {
            var result = await _repository.ModifySearchLimit(null, It.IsAny<uint>(), It.IsAny<uint>());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ModifySearchLimit_WhenIpExistButReachedLimit_ReturnsFalse()
        {
            var result = await _repository.ModifySearchLimit("1", 1, 1, true);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ModifySearchLimit_WhenLimitNotReachedOrIpDoesNotExist_ReturnsTrue()
        {
            var result = await _repository.ModifySearchLimit("abc", 1, 1, true);

            Assert.That(result, Is.True);
        }

        private List<SearchLimit> GetSampleData()
        {
            var list = new List<SearchLimit>(){
                new SearchLimit() { Ip = "1", Limit = 0}
            };

            return list;
        }

        private List<SearchHistory> GetSearchHistoryData()
        {
            var list = new List<SearchHistory>() { new SearchHistory { IpAddress = "1" } };
            return list;
        }
    }
}
