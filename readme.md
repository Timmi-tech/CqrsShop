# CqrsShop 🧩
A lightweight CQRS + Clean Architecture sample for building a simple e‑commerce backend.

![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=c-sharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=flat-square&logo=.net&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![FluentValidation](https://img.shields.io/badge/FluentValidation-6E4C13?style=flat-square&logo=checkmarx&logoColor=white)
![MediatR](https://img.shields.io/badge/MediatR-FF6F61?style=flat-square&logo=messenger&logoColor=white)


## About

CqrsShop is an educational, production-oriented sample backend demonstrating CQRS (via MediatR), Clean Architecture, EF Core with PostgreSQL, and ASP.NET Core Identity. It's ideal for teams and learners who want a realistic, well-structured starting point for e-commerce backends.

### Why This Project?
- Realistic domain modeling and separation of concerns
- Demonstrates MediatR-driven commands/queries and repository patterns  
- Ready-to-run API with Swagger — great for experimentation and teaching

## 🔥 Features
- 🛍️ Product CRUD with inventory support
- 🧾 Order placement, cancellation, and lifecycle
- 🔐 ASP.NET Core Identity-based authentication & refresh tokens
- ✉️ MediatR handlers for commands/queries (CQRS)
- 🐘 EF Core + Npgsql (Postgres) persistence with migrations
- 🧾 Serilog structured logging
- 🧩 Clean Architecture with clear layer separation

## ⚡ Getting Started

### Prerequisites
- .NET 7.0 SDK or later
- PostgreSQL database
- Visual Studio Code or Visual Studio 2022

### Installation

1. Clone the repository
```bash
git clone https://github.com/your-org/CqrsShop.git
cd CqrsShop
```

2. Configure the database connection
```bash
cd Src/Presentation
cp appsettings.Example.json appsettings.Development.json
```
Edit appsettings.Development.json with your PostgreSQL connection string.

3. Build and run
```bash
dotnet restore
dotnet build
cd Src/Infrastructure
dotnet ef database update --project Infrastructure --startup-project ../Presentation
cd ../Presentation
dotnet run
```

## 🚀 Usage

Access the Swagger UI at `http://localhost:5180/swagger`

Example API call:
```bash
curl -X POST http://localhost:5180/api/Products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Product",
    "description": "Description",
    "price": 29.99,
    "category": "electronics",
    "initialStock": 100
  }'
```

## 🛠️ Tech Stack
- .NET 7.0+ ⚙️
- ASP.NET Core Web API 🕸️
- MediatR (CQRS) ✉️
- Entity Framework Core 🗄️
- PostgreSQL 🐘
- ASP.NET Core Identity 🔐
- Serilog 📝
- Swagger/OpenAPI 📘

## Project Structure
```
CqrsShop/
├── src/
│   ├── Application/        # Business logic, CQRS handlers
│   ├── Domain/            # Entities, value objects
│   ├── Infrastructure/    # Data access, external services
│   └── Presentation/      # API endpoints, middleware
├── tests/                 # Unit & integration tests
└── README.md
```

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch
```bash
git checkout -b feature/amazing-feature
```
3. Commit your changes
```bash
git commit -m 'Add some amazing feature'
```
4. Push to the branch
```bash
git push origin feature/amazing-feature
```
5. Open a Pull Request

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

Project Link: [https://github.com/timmi-tech/CqrsShop](https://github.com/your-org/CqrsShop)

---
⭐ Star us on GitHub — it helps!