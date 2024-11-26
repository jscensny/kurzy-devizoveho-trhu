# C# .NET Web API Project

This project is a **C# .NET Web API** built using the ASP.NET Core framework. It is designed to expose RESTful endpoints for performing operations and can be customized for various use cases.

---

## Features

- **Minimal API**: A lightweight implementation using ASP.NET Core Minimal APIs.
- **Dependency Injection**: Built-in support for DI to manage services.
- **Routing**: Clean and structured routing for API endpoints.
- **Validation**: Custom validators for query parameters.
- **Error Handling**: Graceful handling of errors with meaningful HTTP responses.

---

## Prerequisites

Ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (Version 8.0 or higher recommended)
- [Git](https://git-scm.com/)
- An IDE like [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

---

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/jscensny/kurzy-devizoveho-trhu.git
cd your-repo-name
```

### 2. Install Dependencies

Run the following command to restore NuGet packages:

```bash
dotnet restore
```

### 3. Build the Project

Build the solution using:

```bash
dotnet build
```

### 4. Run the API

Start the API server using:

```bash
dotnet run
```

By default, the API will be accessible at `https://localhost:5001` and `http://localhost:5000`.

---

## Configuration

### appsettings.json

The application settings, including API URLs or database connection strings, can be configured in the `appsettings.json` file. Example:

```json
{
  ...
  "PublicApi": {
    "CNB": {
      "KurzyDevizovehoTrhuUrl": "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt"

    }
  }
}
```

### Environment-Specific Configuration

For development, use `appsettings.Development.json` to override default settings.

---

## API Endpoints

### Base URL

```
https://localhost:5001
```

### Example Endpoint: `/kurzy-devizoveho-trhu`

**Description**: Fetches exchange rates with optional filtering by date and currency code.

#### Query Parameters:

- `date` (optional): Date in the format `DD.MM.YYYY`.
- `currencyCode` (optional): Three-letter currency code (e.g., `USD`, `EUR`).

#### Example Request:

```
GET /kurzy-devizoveho-trhu?date=22.11.2024&currencyCode=USD
```

#### Example Response:

```json
{
  "Datum": "22.11.2024",
  "Tag": "#228",
  "SměnnéKurzy": [
    {
      "Země": "USA",
      "Měna": "dolar",
      "Množství": 1,
      "Kód": "USD",
      "Kurz": "22,456"
    }
  ]
}
```

---

## Testing

### API Testing

You can test the API using tools like [Postman](https://www.postman.com/) or [curl](https://curl.se/).

#### Example with curl:

```bash
curl -X GET "https://localhost:5001/kurzy-devizoveho-trhu?date=22.11.2024&currencyCode=USD"
```

---

## Deployment

### Publish the API

To publish the API for deployment:

```bash
dotnet publish -c Release -o ./publish
```

The output will be in the `./publish` directory.

### Deploy to a Server

Use your preferred deployment method (e.g., Azure, AWS, Docker, IIS).

---

## Contributing

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/YourFeature`).
3. Commit your changes (`git commit -m 'Add a new feature'`).
4. Push to the branch (`git push origin feature/YourFeature`).
5. Open a Pull Request.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Acknowledgments

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Minimal APIs in .NET 6](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)

---

