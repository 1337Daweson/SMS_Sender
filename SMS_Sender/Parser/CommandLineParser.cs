namespace SMS_Sender.Parser;

public static class CommandLineParser
{
    public static Dictionary<string, string> ParseArguments(string[] args)
    {
        var arguments = new Dictionary<string, string>();
        foreach (string arg in args)
        {
            var parts = arg.Split(new[] { '=' }, 2);
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                arguments[key] = value;
            }
        }

        return arguments;
    }
}