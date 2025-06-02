# Mail Classifier Backend

This is the backend API for the Mail Classifier app, built using ASP.NET Core.

## Features

- REST API endpoint: `/api/EmailClassifier/classify`
- Accepts a JSON array of email messages
- Returns classification/tags for each message
- CORS enabled for frontend development

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)

### Installation

```sh
git clone https://github.com/masum-rayhan/MailClassifier.git
cd mail-classifier-be
dotnet restore
```

### Development

```sh
dotnet run
```

- API will run at `https://localhost:7250` by default

### API Usage

#### Request

`POST /api/EmailClassifier/classify`

```json
{
  "messages": [
    "Your email message here",
    "Another message"
  ]
}
```

#### Response

```json
{
  "data": [
    {
      "message": "Your email message here",
      "tags": ["Tag1", "Tag2"]
    },
    {
      "message": "Another message",
      "tags": []
    }
  ]
}
```

### CORS

- The API is configured for CORS in development.
- Edit `Program.cs` to restrict allowed origins for production.

### Swagger

- Swagger UI is available at `/swagger` in development.

## License

MIT
