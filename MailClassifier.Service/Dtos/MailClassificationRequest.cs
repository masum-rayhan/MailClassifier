using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailClassifier.Service.Dtos;

public class MailClassificationRequest
{
    public List<string> Messages { get; set; } = [];
}
