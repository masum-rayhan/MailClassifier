using MailClassifier.Service.Dtos;
using MailClassifier.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MailClassifier.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailClassifierController(IEmailClassifierService emailClassifierService) : ControllerBase
{
    private readonly IEmailClassifierService _emailClassifierService = emailClassifierService;

    [HttpPost("classify")]
    public async Task<ActionResult<GeneralResponse>> ClassifyEmails([FromBody] MailClassificationRequest request)
    {
        if(request.Messages == null || request.Messages.Count == 0)
        {
            return BadRequest(new GeneralResponse
            {
                IsSuccess = false,
                Message = "No messages provided for classification."
            });
        }

        var result = await _emailClassifierService.ClassifyEmailsAsync(request.Messages);

        if (!result.IsSuccess)
        {
            return BadRequest(new GeneralResponse
            {
                IsSuccess = false,
                Message = result.Message
            });
        }

        return Ok(result);
    }
}
