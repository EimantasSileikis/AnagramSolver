using AnagramSolver.BusinessLogic.Data;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using Moq;
using NSubstitute;

namespace AnagramSolver.Tests.Data
{
    public class SearchLimitRepositoryTests
    {
        private SearchLimitRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var queryable = GetSampleData().AsQueryable();
            var context = Substitute.For<CodeFirstContext>(new DbContextOptions<CodeFirstContext>());
            var searchLimitsDbSet = queryable.BuildMockDbSet();
            var searchHistoryDbSet = GetSearchHistoryData().AsQueryable().BuildMockDbSet();
            context.Set<SearchLimit>().Returns(searchLimitsDbSet); 
            context.SearchLimits.Returns(searchLimitsDbSet);
            context.SearchHistories.Returns(searchHistoryDbSet);

            _repository = new SearchLimitRepository(context);
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
