using Polly;
using Polly.Retry;

namespace ResilientAPIsWithPolly;

public class ReqresAPIClient
{
    private const string _reqresAPIBaseUrl = "https://reqres.in/api";
    private readonly HttpClient _httpClient;
    private const int _maxApiCallRetries = 3;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _asyncRetryPolicy =
        Policy.
            HandleResult<HttpResponseMessage>(response => !response.IsSuccessStatusCode).
            WaitAndRetryAsync(
                _maxApiCallRetries,
                retryAttempt => TimeSpan.FromSeconds(retryAttempt),
                (exception, timeSpan, retryCount, context) =>
                {

                });


    public ReqresAPIClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> GetUser(int userId)
    {
        var userUrl = $"{_reqresAPIBaseUrl}/users/{userId}";        
        var httpResponseMessage = await _asyncRetryPolicy.ExecuteAsync(
            () => 
            { 
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, userUrl);
                return _httpClient.SendAsync(httpRequestMessage);
            });
        //var httpResponseMessage = await _asyncRetryPolicy.ExecuteAsync(() => _httpClient.GetAsync(userUrl));
        return httpResponseMessage;
    }
}