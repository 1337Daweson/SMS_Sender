using SMS_Sender;
using SMS_Sender.Config;
using SMS_Sender.FileReader;
using SMS_Sender.Parser;

Configuration.Load();
var sender = new Sender(Configuration.ClientId, Configuration.ClientSecret, Convert.ToInt32(Configuration.Channel));

if (args.Length < 2 ||
    !args.Any(arg => arg.StartsWith(Constants.MESSAGE_ARGUMENT + "=")) ||
    !args.Any(arg => arg.StartsWith(Constants.FILE_PATH_ARGUMENT + "=")))
{
    Console.WriteLine("Usage: SMS_Sender.exe -message=\"<message>\" -tolist=\"<filename>\"");
    Console.WriteLine("  -message: The message to write (enclose in quotes).");
    Console.WriteLine("  -tolist: The path to the file to read.");
    return;
}

var arguments = CommandLineParser.ParseArguments(args);
string message = arguments.TryGetValue(Constants.MESSAGE_ARGUMENT, out string messageValue) ? messageValue : null;
string filePath = arguments.TryGetValue(Constants.FILE_PATH_ARGUMENT, out string filePathValue) ? filePathValue : null;

if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(filePath))
{
    Console.WriteLine("Error: Missing required arguments.");
    Console.Read();
    return;
}

List<string> phoneNumbers = FileReader.ReadPhoneNumbers(filePath);
if (phoneNumbers.Count == 0)
{
    Console.WriteLine($"návratový kód: 1");
    Console.Read();
    return;
}

Console.WriteLine($"Zpráva: {message}");
Console.WriteLine("příjemci:");
foreach (string number in phoneNumbers)
{
    Console.WriteLine($"- {number}");
}

int code = await sender.Send(message, phoneNumbers);
Console.WriteLine($"návratový kód: {code}");
Console.Read();