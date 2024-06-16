using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace SMS_Sender;


public class Sender
{
    private string ClientId;
    private string ClientSecret;
    private int Channel;
    private string SmsUrl;
    private string TokenUrl;

    public Sender(string clientId, string clientSecret, int channel, string smsUrl, string tokenUrl)
    {
        this.ClientId = clientId;
        this.ClientSecret = clientSecret;
        this.Channel = channel;
        this.SmsUrl = smsUrl;
        this.TokenUrl = tokenUrl;
    }

    public async Task<int> Send(string message, List<string> phoneNumbers)
    {
        string accessToken = await AccessTokenManager.GetAccessTokenAsync(this.ClientId, this.ClientSecret,this.TokenUrl);
        if (accessToken.Contains("400 Bad Request"))
        {
            return 1;
        }

        var clientHandler = new HttpClientHandler();
        using var client = new HttpClient(clientHandler);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var bodyData = new SenderRequestBody
        {
            message = message,
            channel = this.Channel,
            recipients = phoneNumbers
        };
        
        var jsonBody = JsonConvert.SerializeObject(bodyData);
        var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(SmsUrl, body);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"SMS byla úspěšně poslána, status: {(int)response.StatusCode}");
            return 0;
        }
        else
        {
            Console.WriteLine($"SMS nebyla poslána, status: {(int)response.StatusCode}");
            return 1;
        }
    }
}