using BackendChallenge.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackendChallenge.EndToEnd
{
    [TestClass]
    public class SuggestionsTests
    {
        private string _apiEndpoint;

        private HttpClient _httpclient;

        [TestInitialize]
        public void SetUp()
        {
            this._httpclient = new HttpClient();
            this._apiEndpoint = ConfigurationFactory.GetConfiguration()["ApiEndpoint"];
        }

        [TestMethod]
        public async Task When_Getting_Suggestions_Then_Suggestions_Are_Returned()
        {
            var query = "london";
            var result = await this._httpclient.GetAsync($"{_apiEndpoint}suggestions?q={query}");

            var suggestions = JsonConvert.DeserializeObject<SuggestionList>(await result.Content.ReadAsStringAsync()).Suggestions;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(7, suggestions.Count);
            Assert.IsTrue(suggestions[0].Name.Contains(query, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(suggestions[0].Score > 0.3f); // Score should be high because we do not specify latitude and longitude
        }

        [TestMethod]
        public async Task When_Getting_Suggestions_With_Latitude_And_Longitude_Then_Suggestions_Are_Returned()
        {
            var query = "london";
            var result = await this._httpclient.GetAsync($"{_apiEndpoint}suggestions?q={query}&latitude=-90&longitude=-180");

            var suggestions = JsonConvert.DeserializeObject<SuggestionList>(await result.Content.ReadAsStringAsync()).Suggestions;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(7, suggestions.Count);
            Assert.IsTrue(suggestions[0].Name.Contains(query, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsTrue(suggestions[0].Score < 0.3f); // Score should be low because specified latitude and longitude are very far away
        }

        [TestMethod]
        public async Task When_Getting_Suggestions_With_Not_Existing_City_Then_No_Suggestions_Are_Returned()
        {
            var query = "Minas Tirith";
            var result = await this._httpclient.GetAsync($"{_apiEndpoint}suggestions?q={query}");

            var suggestions = JsonConvert.DeserializeObject<SuggestionList>(await result.Content.ReadAsStringAsync()).Suggestions;

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(0, suggestions.Count);
        }
    }
}
