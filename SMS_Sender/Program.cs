using Microsoft.Extensions.Configuration;
using SMS_Sender;

var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true);
var config = builder.Build();
var clientId = config["ClientId"];
var clientSecret = config["ClientSecret"];
var channel = config["Channel"];
var sender = new Sender(clientId, clientSecret, Convert.ToInt32(channel));

if (args.Length < 2 ||
    !args.Any(arg => arg.StartsWith("-message=")) ||
    !args.Any(arg => arg.StartsWith("-tolist=")))
{
    Console.WriteLine("Usage: MyApp -message=\"<message>\" -tolist=\"<filename>\"");
    Console.WriteLine("  -message: The message to write (enclose in quotes).");
    Console.WriteLine("  -tolist: The path to the file to read.");
    return;
}

string message = null;
string filePath = null;

// Extract message and file path from arguments
foreach (string arg in args)
{
    if (arg.StartsWith("-message="))
    {
        message = arg.Substring("-message=".Length);
    }
    else if (arg.StartsWith("-tolist="))
    {
        filePath = arg.Substring("-tolist=".Length);
    }
}

// Validate extracted values (optional)
if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(filePath))
{
    Console.WriteLine("Error: Missing required arguments.");
    return;
}

// Read the file content
string fileContent;
try
{
    fileContent = File.ReadAllText(filePath);
}
catch (FileNotFoundException)
{
    Console.WriteLine($"Error: File not found - {filePath}");
    return;
}
catch (Exception ex)
{
    Console.WriteLine($"Error reading file: {ex.Message}");
    return;
}

// Parse phone numbers (assuming each number is on a separate line)
List<string> phoneNumbers = fileContent.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList();

// Simulate processing message and phone numbers (replace with your logic)
Console.WriteLine($"Zpráva: {message}");
Console.WriteLine("příjemci:");
foreach (string number in phoneNumbers)
{
    Console.WriteLine($"- {number}");
}

int code = await sender.Send(message, phoneNumbers);
Console.WriteLine($"návratový kód: {code}");
Console.Read();