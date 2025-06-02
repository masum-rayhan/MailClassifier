namespace MailClassifier.Service.Dtos;

public class MailClassificationResult
{
    public string Message { get; set; }
    public List<string> Tags { get; set; } = [];
}
