namespace SMS_Sender.Config;

using Microsoft.Extensions.Configuration;

public class Configuration
{
    public static string ClientId { get; set; }
    public static string ClientSecret { get; set; }
    public static string Channel { get; set; }

    public static void Load()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", true, true);


        var config = builder.Build();
        ClientId = config["ClientId"];
        ClientSecret = config["ClientSecret"];
        Channel = config["Channel"];
    }
}