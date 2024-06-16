using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SMS_Sender;

public static class AccessTokenManager
{
    private const string grantType = "client_credentials";
    public static async Task<string> GetAccessTokenAsync(string clientId, string clientSecret, string tokenUrl)
    {
        using var client = new HttpClient();
        var bodyData = new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "grant_type", grantType }
        };
        
        var body = new FormUrlEncodedContent(bodyData);
        var response = await client.PostAsync(tokenUrl, body);
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
            var accessToken = jsonResponse.access_token;
            Console.WriteLine($"Access Token: {accessToken}");
            
            return accessToken;
        }
        else
        {
            Console.WriteLine("Klient ID a Klient Secret jsou neplatné.");
            return $"{(int)response.StatusCode} Bad Request";
        }
    }
}