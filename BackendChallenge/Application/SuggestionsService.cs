using BackendChallenge.Domain;
using System.Collections.Generic;
using System.Linq;

namespace BackendChallenge.Application
{
    public class SuggestionsService
    {
        private readonly ICityRepository _cityRepository;

        public SuggestionsService(ICityRepository cityRepository)
        {
            this._cityRepository = cityRepository;
        }

        public IReadOnlyList<Suggestion> GetSuggestions(string name, float? latitude = null, float? longitude = null)
        {
            var cities = this._cityRepository.SearchCities(name);

            var suggestions = cities
                .Select(city => new Suggestion(city.DisplayName, city.Latitude, city.Longitude, city.CalculateScore(name, latitude, longitude)))
                .OrderByDescending(x => x.Score)
                .ToList();

            return suggestions;
        }
    }
}
