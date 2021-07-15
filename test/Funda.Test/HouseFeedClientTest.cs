
using Funda.Models;
using Funda.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Funda.Test
{
    public class HouseFeedClientTest
    {

        [Fact]
        public async Task GetHouseFeed_ShoudReturnHouseFeeds()
        {
            // Arrange
            var responseBody = new HouseFeedModel
            {
                Objects = new List<HouseModel>() {
                    new HouseModel { Id = System.Guid.NewGuid(), MakelaarId = 1, MakelaarNaam = "Makelaar 1"}
                },
                TotaalAantalObjecten = 1,
                Success = true,
                Message = "Houses fetched successfully"
            };
            
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(responseBody))
            };

            var httpClient = new HttpClient(new MockedHttpMessageHandler(response)){
                BaseAddress = new Uri("http://localhost")
            };
            
            // Act
            var houseFeedClient = new HouseFeedClient(httpClient);
            var result = await houseFeedClient.GetHouseFeed("type", "zo");

            // Assert
            Console.Write("Result => ", result.Success);
            Assert.NotNull(result);
            Assert.IsType<HouseFeedModel>(result);
            Assert.Equal<int>(1, result.Objects.Count);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetHouseFeed_ShoudReturnFailResult_WhenResponseContentIsNull()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var httpClient = new HttpClient(new MockedHttpMessageHandler(response)){
                BaseAddress = new Uri("http://localhost")
            };

            // Act
            var houseFeedClient = new HouseFeedClient(httpClient);
            var result = await houseFeedClient.GetHouseFeed("type", "zo");

            // Assert
            Assert.False(result.Success);
            Assert.Equal<string>(result.Message, "Response content was null");
        }

        [Fact]
        public async Task GetHouseFeed_ShoudReturnFailResult_WhenResponseContentIsEmpty()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK){
                Content = new StringContent(string.Empty)
            };
            var httpClient = new HttpClient(new MockedHttpMessageHandler(response)){
                BaseAddress = new Uri("http://localhost")
            };

            // Act
            var houseFeedClient = new HouseFeedClient(httpClient);
            var result = await houseFeedClient.GetHouseFeed("type", "zo");

            // Assert
            Assert.False(result.Success);
            Assert.Equal<string>(result.Message, "Failed to deserialize response");
        }

        [Fact]
        public async Task GetHouseFeed_ShoudReturnException_WhenHttpRequestExceptionOccurs()
        {
            // Arrange
            var httpClient = new HttpClient(new MockedHttpMessageHandler(() => throw new HttpRequestException("Http Exception"))){
                BaseAddress = new Uri("http://localhost")
            };

            // Act
            var houseFeedClient = new HouseFeedClient(httpClient);
            var result = await houseFeedClient.GetHouseFeed("type", "zo");

            // Assert
            Assert.False(result.Success);
            Assert.Equal<string>(result.Message, "HttpRequestException when calling the API");
        }

        [Fact]
        public async Task GetHouseFeed_ShoudReturnFailResult_WhenNotFoundResultFromApi()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.NotFound){
                Content = new StringContent("{\"error\": \"Address was not found\"}")
            };
            var httpClient = new HttpClient(new MockedHttpMessageHandler(response)){
                BaseAddress = new Uri("http://localhost")
            };

            // Act
            var houseFeedClient = new HouseFeedClient(httpClient);
            var result = await houseFeedClient.GetHouseFeed("type", "zo");

            // Assert
            Assert.False(result.Success);
            Assert.Equal<string>(result.Message, "Provided address not found");
        }

        [Fact]
        public async Task GetHouseFeed_ShoudReturnFailResult_WhenTimeoutOccurs()
        {
            // Arrange
            var httpClient = new HttpClient(new MockedHttpMessageHandler(() => throw new TimeoutException("Timeout Exception"))){
                BaseAddress = new Uri("http://localhost"),
                Timeout = TimeSpan.FromMilliseconds(300)
            };

            // Act
            var houseFeedClient = new HouseFeedClient(httpClient);
            var result = await houseFeedClient.GetHouseFeed("type", "zo");

            // Assert
            Assert.False(result.Success);
            Assert.Equal<string>(result.Message, "TimeoutException during call to API");
        }
    }
}