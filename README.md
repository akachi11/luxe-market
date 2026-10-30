# LuxeMarket

A luxury e-commerce REST API built with ASP.NET Core — for watches, jewelry, and pre-owned designer goods.

Built as a hands-on learning project to explore the .NET ecosystem from the perspective of a MERN stack developer.

## Tech Stack

- **Runtime:** .NET 10 / C#
- **Framework:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Database:** SQLite (development)
- **API Docs:** OpenAPI / Swagger

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [EF Core CLI tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet): `dotnet tool install --global dotnet-ef`

### Setup

```bash
# Clone the repo
git clone https://github.com/akachi11/LuxeMarket.git
cd LuxeMarket

# Apply database migrations
dotnet ef database update

# Run the API
dotnet run
```

The server starts at `http://localhost:5106` (check console output for the exact port).

## API Endpoints

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | List all products |
| GET | `/api/products/{id}` | Get a product by ID |
| POST | `/api/products` | Create a product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |

### Example Request

```bash
curl -X POST http://localhost:5106/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Rolex Submariner","brand":"Rolex","price":45000,"category":"Watch"}'
```

## Project Structure

```
LuxeMarket/
├── Controllers/        # API endpoints (route handlers)
├── Models/             # Data models (DB table definitions)
├── Data/               # Database context
├── Migrations/         # EF Core schema migrations
├── Program.cs          # App entry point & middleware config
└── appsettings.json    # App configuration
```

## Roadmap

- [x] Product CRUD API
- [ ] Brand & Category models with relationships
- [ ] Filtering, sorting & pagination
- [ ] User authentication (ASP.NET Identity + JWT)
- [ ] Role-based authorization (admin, customer)
- [ ] Image uploads
- [ ] Shopping cart & orders
- [ ] React/TypeScript frontend

## License

MIT
