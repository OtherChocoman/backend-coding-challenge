using BackendChallenge.Application;
using Microsoft.AspNetCore.Mvc;

namespace BackendChallenge.Api
{
    [ApiController]
    public class SuggestionsController : ControllerBase
    {
        private readonly SuggestionsService _suggestionsService;

        public SuggestionsController(SuggestionsService suggestionsService)
        {
            this._suggestionsService = suggestionsService;
        }

        [HttpGet]
        [Route("suggestions")]
        public IActionResult GetSuggestions([FromQuery(Name = "q")] string query, [FromQuery] float? latitude, [FromQuery] float? longitude)
        {
            if (query == null)
            {
                return this.BadRequest("q cannot be null");
            }

            if (latitude > 90 || latitude < -90)
            {
                return this.BadRequest("latitude must be between -90 and 90");
            }

            if (longitude > 180 || longitude < -180)
            {
                return this.BadRequest("longitude must be between -180 and 180");
            }

            var suggestions =  this._suggestionsService.GetSuggestions(query.Trim(), latitude, longitude);

            return this.Ok(new SuggestionList
            {
                Suggestions = suggestions
            });
        }
    }
}