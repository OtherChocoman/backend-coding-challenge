using BackendChallenge.Domain;
using System.Collections.Generic;

namespace BackendChallenge.Domain
{
    public interface ICityRepository
    {
        IReadOnlyList<City> SearchCities(string name);
    }
}
