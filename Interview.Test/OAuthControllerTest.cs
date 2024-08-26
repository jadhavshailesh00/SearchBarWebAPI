//using Interview.Controllers;
//using Interview.Entity.History;
//using Interview.Entity.Response;
//using Interview.Repository.Token;
//using Interview.Service.Search;
//using Interview.Service.Token;
//using Microsoft.Extensions.Logging;
//using Moq;
//using System.Web.Http.Results;

//namespace Interview.Test
//{
//    [TestFixture]
//    public class OAuthControllerTest
//    {

//        private Mock<ITokenRepository> _mockTokenRepository;
//        private Mock<ILogger<SearchBarController>> _mockLogger;
//        private TokenService _TokenService;

//        [SetUp]
//        public void SetUp()
//        {
//            _mockTokenService = new Mock<ITokenService>();
//            _mockLogger = new Mock<ILogger<SearchBarController>>();
//            _TokenService = new SearchBarController(_mockSearchService.Object, _mockLogger.Object);
//        }

//        [Test]
//        public void SearchBar_ReturnsBadRequest_WhenQueryIsNull()
//        {

//            var expectedResult = new ErrorResponse
//            {
//                Message = "No results were found for your search query.",
//                ErrorCode = "NO_RESULTS_FOUND",
//                Resolution = "Please try adjusting your search parameters.",
//                ErrorId = Guid.NewGuid().ToString()
//            };
//            var result = _controller.SearchBar(null) as Microsoft.AspNetCore.Mvc.NotFoundObjectResult;
//            var actualresult = result.Value as ErrorResponse;
//            Assert.AreEqual(actualresult.Message, expectedResult.Message);
//            Assert.AreEqual(expectedResult.ErrorCode, actualresult.ErrorCode);
//            Assert.AreEqual(expectedResult.Resolution, actualresult.Resolution);
//        }

//        [Test]
//        public void SearchBar_ReturnsNotFound_WhenNoResultsFound()
//        {
//            _mockSearchService.Setup(service => service.SearchData("1", "query", "filter", "sort"))
//                     .Returns((List<SearchResponse>)null);

//            var result = _controller.SearchBar("query", "filter", "sort");

//            Assert.IsNotNull(result);

//        }

//        [Test]
//        public void SearchBar_ReturnsOk_WhenResultsAreFound()
//        {
//            var ExpectedResult = new List<SearchResponse>();
//            var _data = new SearchResponse
//            {
//                ID = "9",
//                Category = "Books",
//                Date = System.DateTime.Now,
//                Description = "New Books",
//                Title = "Title Books",
//            };
//            ExpectedResult.Add(_data);

//            _mockSearchService.Setup(service => service.SearchData("1", "ok","",""))
//                              .Returns((List<SearchResponse>)null);

//            var result = _controller.SearchBar("1","ok","") as  List<SearchResponse>;

//            Assert.IsNotNull(result);
//            Assert.AreEqual(result.Count, ExpectedResult.Count);
//        }

//        [Test]
//        public void SearchBar_LogsError_WhenExceptionIsThrown()
//        {
//            _mockSearchService.Setup(service => service.SearchData("1", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
//                              .Throws(new Exception("Test exception"));

//            var result = _controller.SearchBar("query") as StatusCodeResult;

//            Assert.IsInstanceOf<StatusCodeResult>(result);
//            _mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
//        }

//        [Test]
//        public void GetSearchById_ReturnsNotFound_WhenNoResultFound()
//        {
//            _mockSearchService.Setup(service => service.SearchDataByID("1", It.IsAny<string>()))
//                              .Returns((SearchResponse)null);

//            var result = _controller.GetSearchById("id") as NotFoundResult;

//            Assert.IsNotNull(result);
//            _mockLogger.Verify(logger => logger.LogWarning(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
//        }

//        [Test]
//        public void GetSearchById_ReturnsOk_WhenResultIsFound()
//        {
//            var resultData = "result";
//            _mockSearchService.Setup(service => service.SearchDataByID("1", It.IsAny<string>()))
//                              .Returns((SearchResponse)null);

//            var result = _controller.GetSearchById("id") as OkNegotiatedContentResult<string>;

//            Assert.IsNotNull(result);
//            Assert.AreEqual(resultData, result.Content);
//        }

//        [Test]
//        public void GetSearchById_LogsError_WhenExceptionIsThrown()
//        {
//            _mockSearchService.Setup(service => service.SearchDataByID("1", It.IsAny<string>()))
//                              .Throws(new Exception("Test exception"));

//            var result = _controller.GetSearchById("id") as StatusCodeResult;

//            Assert.IsInstanceOf<StatusCodeResult>(result);
//            _mockLogger.Verify(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
//        }

//        [Test]
//        public void GetSearchHistory_ReturnsOk_WhenDataIsFound()
//        {
//            var history = new List<string> { "history1", "history2" };
//            _mockSearchService.Setup(service => service.GetSearchHistoryAsync(It.IsAny<string>()))
//                              .Returns((List<SearchHistory>)null);

//            var result = _controller.GetSearchHistory("userId") as OkNegotiatedContentResult<IEnumerable<string>>;

//            Assert.IsNotNull(result);
//            Assert.AreEqual(history, result.Content);
//        }
//    }

//}