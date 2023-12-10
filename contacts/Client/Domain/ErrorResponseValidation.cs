namespace contacts.Client.Domain;

public class ErrorResponseValidation
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public Dictionary<string, IEnumerable<string>> Errors { get; set; }
    public string TraceId { get; set; }
}