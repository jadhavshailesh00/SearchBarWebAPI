using Interview.Controllers;
using Interview.Entity.Response;
using Interview.Repository;
using Interview.Repository.Search;
using Interview.Service.Search;
using Microsoft.Extensions.Logging;
using Moq;

namespace Interview.Test
{
    [TestFixture]
    public class SearchBarControllerTests
    {
        private Mock<IRepository> _mockRepository;
        private Mock<ISearchHistoryRepository> _mockSearchHistoryRepository;
        private Mock<ISearchService> _mockSearchService;
        private Mock<ILogger<SearchBarController>> _mockLogger;
        private SearchBarController _controller;
        private SearchService _SearchService;


        [SetUp]
        public void SetUp()
        {
            _mockSearchService = new Mock<ISearchService>();
            _mockLogger = new Mock<ILogger<SearchBarController>>();
            _controller = new SearchBarController(_mockSearchService.Object, _mockLogger.Object);

            _mockRepository = new Mock<IRepository>();
            _mockSearchHistoryRepository = new Mock<ISearchHistoryRepository>();

            _SearchService = new SearchService(_mockRepository.Object, _mockSearchHistoryRepository.Object);

        }

        [Test]
        public void SearchBar_ReturnsBadRequest_WhenQueryIsNull()
        {

            var expectedResult = new ErrorResponse
            {
                Message = "No results were found for your search query.",
                ErrorCode = "NO_RESULTS_FOUND",
                Resolution = "Please try adjusting your search parameters.",
                ErrorId = Guid.NewGuid().ToString()
            };
            var result = _controller.SearchBar(null) as Microsoft.AspNetCore.Mvc.NotFoundObjectResult;
            var actualresult = result.Value as ErrorResponse;
            Assert.AreEqual(actualresult.Message, expectedResult.Message);
            Assert.AreEqual(expectedResult.ErrorCode, actualresult.ErrorCode);
            Assert.AreEqual(expectedResult.Resolution, actualresult.Resolution);
        }

        [Test]
        public void SearchBar_ReturnsNotFound_WhenNoResultsFound()
        {
            _mockSearchService.Setup(service => service.SearchData("1", "query", "filter", "sort"))
                     .Returns((List<SearchResponse>)null);

            var result = _controller.SearchBar("query", "filter", "sort");

            Assert.IsNotNull(result);

        }

        [Test]
        public void SearchBar_SearchDataByID_WhenNoResultsFound()
        {
            var expectedResult = new SearchResponse();
            expectedResult.ID = "1";
            expectedResult.Title = "Title";
            expectedResult.Date = System.DateTime.Now;
            expectedResult.Category = "Category";
            expectedResult.Description = "Description";

            var result = _SearchService.SearchDataByID(" ", " ");
            Assert.AreNotEqual(result, expectedResult);

        }

        [Test]
        public void SearchBar_SearchData_WhenNoResultsFound()
        {
            var data = new List<SearchResponse>();
            var expectedResult = new SearchResponse();
            expectedResult.ID = "1";
            expectedResult.Title = "Title";
            expectedResult.Date = System.DateTime.Now;
            expectedResult.Category = "Category";
            expectedResult.Description = "Description";
            data.Add(expectedResult);
            var result = _SearchService.SearchData(" ", " ", " ", " ");
            Assert.AreNotEqual(result, data);

        }

        [Test]
        public void SearchBar_SearchDataByID_Success()
        {
            var expectedResult = new SearchResponse { ID = "1", Title = "Test Result" };
            _mockRepository.Setup(repo => repo.SearchDataByID(It.IsAny<string>())).Returns(expectedResult);

            var result = _SearchService.SearchDataByID("UserID", "query");

            Assert.AreEqual(expectedResult, result);
            _mockSearchHistoryRepository.Verify(repo => repo.SaveSearchDataByID(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockSearchHistoryRepository.Verify(repo => repo.SaveSearchHistroy(It.IsAny<string>(), It.IsAny<SearchResponse>()), Times.Once);
        }

        [Test]
        public void SearchBar_SearchData_Success()
        {

            var expectedSearchResults = new List<SearchResponse>
            {
                new SearchResponse { ID = "1", Title = "Result 1", Date = new DateTime(2023, 11, 11) },
                new SearchResponse { ID = "2", Title = "Result 2", Date = new DateTime(2023, 12, 12) },
                new SearchResponse { ID = "3", Title = "Result 3", Date = new DateTime(2023, 10, 10) }
            };

            _mockRepository.Setup(repo => repo.SearchData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expectedSearchResults);

            var results = _SearchService.SearchData("1", "in", "", "");

            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual("1", results[0].ID);
            Assert.AreEqual("2", results[1].ID);
        }
    }

}