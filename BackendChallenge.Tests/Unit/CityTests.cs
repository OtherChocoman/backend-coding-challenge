using BackendChallenge.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BackendChallenge.Tests.Unit
{
    [TestClass]
    public class CityTests
    {
        [TestMethod]
        public void When_Initializing_City_Then_Fields_Are_Populated()
        {
            var city = new City("myCityName", "myCityAsciiName", new List<string> { "altname1" }, "CA", "Quebec",  42f, 13.37f);

            Assert.AreEqual("myCityName", city.Name);
            Assert.AreEqual(42f, city.Latitude);
            Assert.AreEqual(13.37f, city.Longitude);
        }

        [TestMethod]
        public void When_Initializing_City_Then_Display_Name_Is_Correct()
        {
            var city = new City("myCityName", "myCityAsciiName", new List<string> { "altname1" }, "CA", "Quebec", 42f, 13.37f);

            Assert.AreEqual("myCityName, Quebec, CA", city.DisplayName);
        }

        [DataTestMethod]
        [DataRow(null, "cityAsciiName", "country", "region")]
        [DataRow("cityName", null, "country", "region")]
        [DataRow("cityName", "cityAsciiName", null, "region")]
        [DataRow("cityName", "cityAsciiName", "country", null)]
        public void When_Initializing_City_With_Null_Parameters_Then_Argument_Null_Exception_Is_Thrown(
            string name, 
            string asciiName, 
            string country, 
            string region)
        {
            void action() => new City(name, asciiName, new List<string> { "altname1" }, country, region, 42f, 13.37f);

            Assert.ThrowsException<ArgumentNullException>(action);
        }

        [DataTestMethod]
        [DataRow("Quebec", "QuEbEc")]
        [DataRow("Quebec", "Que")]
        [DataRow("Quebec", "bec")]
        [DataRow("Quebec", "ebe")]
        public void When_City_Name_Matches_Then_It_Returns_True(string cityName, string match)
        {
            var city = new City(cityName, "myCityAsciiName", new List<string>(), "CA", "Quebec", 42f, 13.37f);

            Assert.IsTrue(city.Matches(match));
        }

        [DataTestMethod]
        [DataRow("Quebec", "QuEbEc")]
        [DataRow("Quebec", "Que")]
        [DataRow("Quebec", "bec")]
        [DataRow("Quebec", "ebe")]
        public void When_City_Ascii_Name_Matches_Then_It_Returns_True(string cityName, string match)
        {
            var city = new City("myCityName", cityName, new List<string>(), "CA", "Quebec", 42f, 13.37f);

            Assert.IsTrue(city.Matches(match));
        }

        [DataTestMethod]
        [DataRow("Quebec", "QuEbEc")]
        [DataRow("Quebec", "Que")]
        [DataRow("Quebec", "bec")]
        [DataRow("Quebec", "ebe")]
        public void When_City_Alternate_Name_Matches_Then_It_Returns_True(string cityName, string match)
        {
            var city = new City("myCityName", "myCityAsciiName", new List<string> { cityName }, "CA", "Quebec", 42f, 13.37f);

            Assert.IsTrue(city.Matches(match));
        }

        [DataTestMethod]
        [DataRow("Quebec", "Quebecc")]
        [DataRow("Quebec", "QQuebec")]
        [DataRow("Quebec", " ")]
        [DataRow("Quebec", "")]
        [DataRow("Quebec", null)]
        public void When_City_Does_Not_Match_Then_It_Returns_False(string cityName, string match)
        {
            var city = new City(cityName, "myCityAsciiName", new List<string>(), "CA", "Quebec", 42f, 13.37f);

            Assert.IsFalse(city.Matches(match));
        }

        [DataTestMethod]
        [DataRow("myCityName", null, null, 1f)]
        [DataRow("myCityName", 42f, 13.37f, 1f)]
        [DataRow("myCityName", 41.5f, 15f, 0.992707f)]
        [DataRow("myCityName", null, 20f, 0.9815834f)]
        [DataRow("myCityName", 75f, null, 0.8166667f)]
        [DataRow("CompletelyDifferentName", 42f, 13.37f, 0f)]
        [DataRow("city", 42f, 13.37f, 0.4f)]
        [DataRow("city", -90f, -180f, 0.04937185f)]
        public void When_Calculating_Score_Then_Returned_Score_Is_Correct(string cityName, float? latitude, float? longitude, float expectedScore)
        {
            var city = new City("myCityName", "myCityAsciiName", new List<string>(), "CA", "Quebec", 42f, 13.37f);

            Assert.AreEqual(expectedScore, city.CalculateScore(cityName, latitude, longitude), 0.0000001f);
        }
    }
}
