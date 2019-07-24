using FreeWheel.Logic.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FreeWheel.Acceptance
{
    [TestClass]
    public class MoviesApiAcceptanceTests
    {
        private HttpClient _client;

        private IWebHostBuilder CreateWebHostBuilder()
        {
            var config = new ConfigurationBuilder().Build();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseStartup<Startup>();

            return host;
        }

        public MoviesApiAcceptanceTests() {
            var testServer = new TestServer(CreateWebHostBuilder());
            _client = testServer.CreateClient();
        }

        [TestMethod]
        public async Task GivenRequestWithoutFiltersThenApiReturns400()
        {
            var results = await _client.GetAsync("api/movies");
            Assert.AreEqual(400, (int)results.StatusCode);
        }

        [TestMethod]
        public async Task GivenRequestNotMatchingMovingThenApiReturns404()
        {
            var results = await _client.GetAsync("api/movies?title=3");
            Assert.AreEqual(404, (int)results.StatusCode);
        }

        [TestMethod]
        public async Task GivenMatchingQueryThenApiReturns200()
        {
            var results = await _client.GetAsync("api/movies?title=1");
            Assert.AreEqual(200, (int)results.StatusCode);
        }

        [TestMethod]
        public async Task GivenGlobalRatingsQueryThenApiReturnsAsExpected()
        {
            var results = await (await _client.GetAsync("api/movies/byRating"))
                .Content
                .ReadAsStringAsync();

            var dResults = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MovieProjection>>(results);
            Assert.AreEqual("Test1", dResults.First().Title);
        }

        [TestMethod]
        public async Task GivenUserScopedRatingQueryThenApiReturnsOnlyScopedResults()
        {
            var jwt = new JwtSecurityTokenHandler()
                .WriteToken(
                    new JwtSecurityToken(claims: new List<Claim> {
                        new Claim("uid", "1"),
                    }));
            
            var request = new HttpRequestMessage(HttpMethod.Get, "api/movies/byRating");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

            var results = await (await _client.SendAsync(request))
                .Content
                .ReadAsStringAsync();

            var dResults = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MovieProjection>>(results);
            Assert.AreEqual(3, dResults.Count);
        }
    }
}
