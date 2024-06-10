namespace SMS_Sender.FileReader;

public static class FileReader
{
    public static List<string> ReadPhoneNumbers(string filePath)
    {
        try
        {
            string fileContent = File.ReadAllText(filePath);
            return fileContent.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Error: Seznam s příjemci nebyl nalezen - {filePath}");
            return new List<string>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problém se čtením souboru: {ex.Message}");
            return new List<string>();
        }
    }
}