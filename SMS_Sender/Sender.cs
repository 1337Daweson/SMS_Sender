using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace SMS_Sender;

[Serializable]
public class RequestData
{
    public string message { get; set; }
    public List<string> recipients { get; set; }
    public int channel { get; set; }
}
public class Sender
{
    private string ClientId;
    private string ClientSecret;
    private int Channel;
    private string SmsUrl = "https://app.gosms.eu/api/v1/messages";
    
    public Sender(string clientId, string clientSecret, int channel)
    {
        this.ClientId = clientId;
        this.ClientSecret = clientSecret;
        this.Channel = channel;
    }

    public async Task<int> Send(string message, List<string> phoneNumbers)
    {
        string accessToken = await AccessTokenManager.GetAccessTokenAsync(this.ClientId, this.ClientSecret);
        if (accessToken.Contains("400 Bad Request"))
        {
            return 1;
        }

        var clientHandler = new HttpClientHandler();
        using var client = new HttpClient(clientHandler);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var bodyData = new RequestData
        {
            message = message,
            channel = this.Channel,
            recipients = phoneNumbers
        };
        

            var jsonContent = JsonConvert.SerializeObject(bodyData);
            var body = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
        var response = await client.PostAsync(SmsUrl, body);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"POST request successful {(int)response.StatusCode}");
            return 0;
        }
        else
        {
            Console.WriteLine($"POST request failed with status code: {(int)response.StatusCode}");
            return 1;
        }

        
    }

    

}