using BackendChallenge.Domain;
using BackendChallenge.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BackendChallenge.Tests.Unit
{
    [TestClass]
    public class CityRepositoryTests
    {
        [TestMethod]
        public void When_Searching_Cities_Then_Returns_All_Cities_That_Match()
        {
            var city1 = new City("myCityName", "myCityAsciiName", new List<string> { "altname1" }, "CA", "Quebec", 42f, 13.37f);
            var city2 = new City("myCityName2", "myCityAsciiName2", new List<string> { "altname2" }, "CA", "Quebec", 42f, 13.37f);

            var cityRepository = new CityRepository(new List<City> { city1, city2 });

            var foundCities = cityRepository.SearchCities("myCityName2");

            Assert.AreEqual(1, foundCities.Count);
            Assert.AreEqual(city2.Name, foundCities[0].Name);
        }
    }
}
