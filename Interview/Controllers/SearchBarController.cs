using Interview.App_Start.Filter;
using Interview.Entity.Response;
using Interview.Service.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Controllers
{
    /// <summary>
    /// Handles search-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ApiController]
    public class SearchBarController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchBarController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBarController"/> class.
        /// </summary>
        /// <param name="searchService">The search service.</param>
        /// <param name="logger">The logger.</param>
        public SearchBarController(ISearchService searchService, ILogger<SearchBarController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        /// <summary>
        /// Searches for data based on the query, filter, and sort parameters.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="filter">The optional filter parameter.</param>
        /// <param name="sort">The optional sort parameter.</param>
        /// <returns>A list of search results or an error response.</returns>
        /// <response code="200">Returns the list of search results.</response>
        /// <response code="404">If no results are found.</response>
        /// <response code="401">Unauthorized client error found.</response>
        /// <response code="403">HTTP 403 Forbidden client error.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpGet]
        [Authorize(Policy = "Developer")]
        public IActionResult SearchBar([FromQuery] string query, [FromQuery] string filter = null, [FromQuery] string sort = null)
        {
            try
            {
                _logger.LogInformation("SearchBar endpoint called with query: {query}, filter: {filter}, sort: {sort}", query, filter, sort);
                var results = _searchService.SearchData("1", query, filter, sort);
                if (results == null || results.Count() == 0 )
                {
                    _logger.LogWarning("No results found for query: {query}", query);
                    return NotFound(new ErrorResponse
                    {
                        Message = "No results were found for your search query.",
                        ErrorCode = "NO_RESULTS_FOUND",
                        Resolution = "Please try adjusting your search parameters.",
                        ErrorId = Guid.NewGuid().ToString()
                    });
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for data.");
                return StatusCode(500, new ErrorResponse
                {
                    Message = "An unexpected error occurred while processing your search.",
                    ErrorCode = "SEARCH_ERROR",
                    Resolution = "Please contact support with the error ID for further assistance.",
                    ErrorId = Guid.NewGuid().ToString()
                });
            }
        }

        /// <summary>
        /// Retrieves a specific search result by its ID.
        /// </summary>
        /// <param name="id">The ID of the search result.</param>
        /// <returns>The search result corresponding to the provided ID or an error response.</returns>
        /// <response code="200">Returns the search result.</response>
        /// <response code="404">If no result is found with the provided ID.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        /// 
        [HttpGet("SearchById")]
        [Authorize(Policy = "Developer")]
        public IActionResult GetSearchById([FromQuery] string id)
        {
            try
            {
                _logger.LogInformation("GetSearchById endpoint called with ID: {id}", id);

                var result = _searchService.SearchDataByID("1", id);
                if (result == null || result.ID == null )
                {
                    _logger.LogWarning("No result found with ID: {id}", id);
                    return NotFound(new ErrorResponse
                    {
                        Message = "No result was found with the provided ID.",
                        ErrorCode = "ID_NOT_FOUND",
                        Resolution = "Please verify the ID and try again.",
                        ErrorId = Guid.NewGuid().ToString()
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the search result by ID.");
                return StatusCode(500, new ErrorResponse
                {
                    Message = "An unexpected error occurred while processing your request.",
                    ErrorCode = "SEARCH_BY_ID_ERROR",
                    Resolution = "Please contact support with the error ID for further assistance.",
                    ErrorId = Guid.NewGuid().ToString()
                });
            }
        }



        [HttpGet("GetSearchHistory")]
        [Authorize(Policy = "Admin")]
        public IActionResult GetSearchHistory(string userId)
        {
            if (!int.TryParse(userId, out int numericUserId))
            {
                return BadRequest("Invalid user ID. User ID must be a numeric value.");
            }
            var history = _searchService.GetSearchHistoryAsync(userId);
            if (history == null || history.Count() == 0)
            {
                _logger.LogWarning("No result found with ID: {id}", userId);
                return NotFound(new ErrorResponse
                {
                    Message = "No result was found with the provided ID.",
                    ErrorCode = "ID_NOT_FOUND",
                    Resolution = "Please verify the ID and try again.",
                    ErrorId = Guid.NewGuid().ToString()
                });
            }
            return Ok(history);
        }
    }
}
