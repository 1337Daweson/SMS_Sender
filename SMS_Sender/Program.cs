using SMS_Sender;
using SMS_Sender.Config;
using SMS_Sender.FileReader;


Configuration.Load();
var sender = new Sender(Configuration.ClientId, Configuration.ClientSecret, Convert.ToInt32(Configuration.Channel));

if (args.Length < 2 ||
    !args.Any(arg => arg.StartsWith("-message=")) ||
    !args.Any(arg => arg.StartsWith("-tolist=")))
{
    Console.WriteLine("Usage: SMS_Sender.exe -message=\"<message>\" -tolist=\"<filename>\"");
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

// Validate extracted values
if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(filePath))
{
    Console.WriteLine("Error: Missing required arguments.");
    return;
}

// Parse phone numbers
List<string> phoneNumbers = FileReader.ReadPhoneNumbers(filePath);

if (phoneNumbers.Count == 0)
{
    Console.WriteLine($"návratový kód: 1");
}

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