using SMS_Sender;
using SMS_Sender.Config;
using SMS_Sender.FileReader;
using SMS_Sender.Parser;

Configuration.Load();

var sender = new Sender(
    Configuration.ClientId, 
    Configuration.ClientSecret, 
    Convert.ToInt32(Configuration.Channel), 
    Configuration.SmsUrl, 
    Configuration.TokenUrl);

if (args.Length != 2 ||
    !args.Any(arg => arg.StartsWith(Constants.MESSAGE_ARGUMENT + "=")) ||
    !args.Any(arg => arg.StartsWith(Constants.FILE_PATH_ARGUMENT + "=")))
{
    Console.WriteLine("Nebyli zadané správné parametry!");
    Console.WriteLine("Použití: SMS_Sender.exe -message=\"<message>\" -tolist=\"<filename>\"");
    Console.WriteLine("  -message=: Zpráva kterou chceš poslat, uzavřená v uvozovkách: -message=\"TEST MESSAGE\"");
    Console.WriteLine("  -tolist=: Cesta k souboru s příjemci, uzavřená v uvozovkách: -tolist=\"C:\\Users\\Test\\Desktop\\senders.txt\"");
    Console.WriteLine($"návratový kód: 1");
    Console.Read();
    return;
}

var arguments = CommandLineParser.ParseArguments(args);
string message = arguments.GetValueOrDefault(Constants.MESSAGE_ARGUMENT);
string filePath = arguments.GetValueOrDefault(Constants.FILE_PATH_ARGUMENT);
if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(filePath))
{
    Console.WriteLine("Chyba v parametrech");
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
Console.WriteLine("Příjemci:");
foreach (string number in phoneNumbers)
{
    Console.WriteLine($"- {number}");
}

Console.WriteLine("SMSUrl: " + Configuration.SmsUrl);
Console.WriteLine("TokenUrl: " + Configuration.TokenUrl);
int code = await sender.Send(message, phoneNumbers);
//int code = 123;
Console.WriteLine($"návratový kód: {code}");