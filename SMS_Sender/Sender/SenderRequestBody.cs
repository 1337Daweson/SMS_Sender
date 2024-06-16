namespace SMS_Sender;

[Serializable]
public class SenderRequestBody
{
    public string message { get; set; }
    public List<string> recipients { get; set; }
    public int channel { get; set; }
}