using MailClassifier.Service.Dtos;

namespace MailClassifier.Service.Interfaces;

public interface IEmailClassifierService
{
    Task<GeneralResponse> ClassifyEmailsAsync(List<string> messages);
}
