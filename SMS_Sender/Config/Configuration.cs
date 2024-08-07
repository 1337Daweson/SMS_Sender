namespace SMS_Sender.Config;

using Microsoft.Extensions.Configuration;
using System.Reflection;

public class Configuration
{
    public static string ClientId { get; set; }
    public static string ClientSecret { get; set; }
    public static string Channel { get; set; }
    public static string SmsUrl { get; set; }
    public static string TokenUrl { get; set; }

    public static void Load()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
            .AddJsonFile($"appsettings.json", true, true);


        var config = builder.Build();
        ClientId = config["ClientId"];
        ClientSecret = config["ClientSecret"];
        Channel = config["Channel"];
        SmsUrl = config["SmsUrl"];
        TokenUrl = config["TokenUrl"];
    }
}