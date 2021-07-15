using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

using Funda.Models;

namespace Funda.Services
{
    public class HouseFeedClient : IHouseFeedClient
    {
        private readonly HttpClient _httpClient;
        public HouseFeedClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<HouseFeedModel> GetHouseFeed(string type, string zo, int? page = 1, int? pageSize = 25)
        {
            string apiUrl = PrepareFeedUrl(type, zo, page, pageSize);

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                
                using (var response = await _httpClient.SendAsync(request))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (response.Content == null)
                        {
                            return new HouseFeedModel(false, "Response content was null");
                        }

                        var responseJson = await response.Content.ReadAsStringAsync();
                        var houseFeedModel = JsonConvert.DeserializeObject<HouseFeedModel>(responseJson);

                        if (houseFeedModel == null)
                        {
                            return new HouseFeedModel(false, "Failed to deserialize response");
                        }

                        return houseFeedModel;
                    }

                    return new HouseFeedModel(false,
                            response.StatusCode == System.Net.HttpStatusCode.NotFound ?
                                                   "Provided address not found" : "Did not receive 200 OK status code");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException when calling the API: {ex.Message}");
                return new HouseFeedModel(false, "HttpRequestException when calling the API");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"TimeoutException during call to API: {ex.Message}");
                return new HouseFeedModel(false, "TimeoutException during call to API");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Task was canceled during call to API: {ex.Message}");
                return new HouseFeedModel(false, "Task was canceled during call to API");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception when calling the API: {ex.Message}");
                return new HouseFeedModel(false, "Unhandled exception when calling the API");
            }
        }

        private string PrepareFeedUrl(string type, string zo, int? page, int? pageSize)
        {
            return $"?type={type}&zo={zo}&page={page}&pagesize={pageSize}";
        }
    }
}