using BackendChallenge.Domain;
using System.Collections.Generic;
using System.Linq;

namespace BackendChallenge.Infrastructure
{
    public class CityRepository : ICityRepository
    {
        private readonly IReadOnlyList<City> _cities;

        public CityRepository(IReadOnlyList<City> cities)
        {
            this._cities = cities;
        }

        public IReadOnlyList<City> SearchCities(string name)
        {
            return this._cities.Where(x => x.Matches(name)).ToList();
        }
    }
}
