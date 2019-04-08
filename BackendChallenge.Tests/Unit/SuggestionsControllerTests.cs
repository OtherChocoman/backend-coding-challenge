using BackendChallenge.Api;
using BackendChallenge.Application;
using BackendChallenge.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net;

namespace BackendChallenge.Tests.Unit
{
    [TestClass]
    public class SuggestionsControllerTests
    {
        private SuggestionsController _suggestionsController;

        private Mock<ICityRepository> _cityRepositoryMock;

        [TestInitialize]
        public void SetUp()
        {
            this._cityRepositoryMock = new Mock<ICityRepository>();

            var suggestionsService = new SuggestionsService(this._cityRepositoryMock.Object);
            this._suggestionsController = new SuggestionsController(suggestionsService);
        }

        [TestMethod]
        public void When_Getting_Suggestions_Then_Matching_Cities_Are_Returned()
        {
            var city = new City("myCityName", "myCityAsciiName", new List<string>(), "CA", "Ontario", 90f, 180f);
            var otherCity = new City("myCityName", "myCityAsciiName", new List<string>(), "CA", "Quebec", 10f, 10f);

            this._cityRepositoryMock.Setup(x => x.SearchCities(It.IsAny<string>())).Returns(new List<City> { city, otherCity });

            var objectResult = (OkObjectResult) this._suggestionsController.GetSuggestions("myCityName", 10f, 10f);

            Assert.IsNotNull(objectResult);
            var suggestions = ((SuggestionList)objectResult.Value).Suggestions;

            Assert.AreEqual(2, suggestions.Count);
            Assert.AreEqual(otherCity.DisplayName, suggestions[0].Name);
            Assert.AreEqual(otherCity.Latitude, suggestions[0].Latitude);
            Assert.AreEqual(otherCity.Longitude, suggestions[0].Longitude);
            Assert.AreEqual(1f, suggestions[0].Score);

            Assert.AreEqual(city.DisplayName, suggestions[1].Name);
            Assert.AreEqual(city.Latitude, suggestions[1].Latitude);
            Assert.AreEqual(city.Longitude, suggestions[1].Longitude);
            Assert.AreNotEqual(3f, suggestions[0].Score, 0.1f);
        }

        [TestMethod]
        public void When_Getting_Suggestions_With_No_Match_Then_Suggestions_Are_Empty()
        {
            this._cityRepositoryMock.Setup(x => x.SearchCities(It.IsAny<string>())).Returns(new List<City>());

            var objectResult = (OkObjectResult)this._suggestionsController.GetSuggestions("cityName", null, null);

            Assert.IsNotNull(objectResult);
            var suggestions = ((SuggestionList)objectResult.Value).Suggestions;

            Assert.AreEqual(0, suggestions.Count);
        }

        [TestMethod]
        public void When_Query_Parameter_Is_Null_Then_Bad_Request_Is_Returned()
        {
            var objectResult = (ObjectResult) this._suggestionsController.GetSuggestions(null, null, null);

            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [DataTestMethod]
        [DataRow(-91)]
        [DataRow(91)]
        public void When_Latitude_Parameter_Is_Invalid_Then_Bad_Request_Is_Returned(float latitude)
        {
            var objectResult = (ObjectResult)this._suggestionsController.GetSuggestions("cityName", latitude, null);

            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        [DataRow(-181)]
        [DataRow(181)]
        public void When_Longitude_Parameter_Is_Invalid_Then_Bad_Request_Is_Returned(float longitude)
        {
            var objectResult = (ObjectResult)this._suggestionsController.GetSuggestions(null, null, longitude);

            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }
    }
}
