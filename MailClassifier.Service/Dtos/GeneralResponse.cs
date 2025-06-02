namespace MailClassifier.Service.Dtos;

public class GeneralResponse
{
    public long Id { get; set; }
    public object Data { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}
