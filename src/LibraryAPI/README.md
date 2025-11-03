# LibraryAPI

RESTful API for personal library management, built with ASP.NET Core 8.0 and Entity Framework Core with MySQL.

## Technologies Used

- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core 9.0
- MySQL (via Pomelo.EntityFrameworkCore.MySql)
- BCrypt.Net-Next (for password hashing)
- Swagger/OpenAPI (documentation)

## Architecture

The project follows a layered architecture:

```
LibraryAPI/
├── Controllers/          # API endpoints
├── Services/            # Business logic
├── Repositories/        # Data access
├── Models/              # Domain entities
├── DTOs/                # Data Transfer Objects
├── Data/                # Database context
├── Migrations/          # EF Core migrations
└── Program.cs           # Application configuration
```

## Features

### Authentication
- User registration
- Email and password login
- BCrypt password hashing

### Book Management
- List books by user
- Get book details
- Create new book
- Update existing book
- Delete book

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL 8.0+](https://dev.mysql.com/downloads/mysql/) or MariaDB
- IDE: Visual Studio 2022, VS Code, or Rider (optional)

## Installation

### 1. Clone the repository

```bash
git clone <repository-url>
cd Library/src/LibraryAPI
```

### 2. Configure the database

Edit the [appsettings.json](appsettings.json) file with your MySQL credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=library_db;user=YOUR_USER;password=YOUR_PASSWORD"
  }
}
```

### 3. Create the database

Run the following commands to create the database and apply migrations:

```bash
dotnet ef database update
```

If you need to create new migrations:

```bash
dotnet ef migrations add MigrationName
```

### 4. Install dependencies

```bash
dotnet restore
```

## How to Run

### Option 1: Via .NET CLI

```bash
dotnet run
```

### Option 2: Via Visual Studio

1. Open the `Library.sln` solution
2. Set `LibraryAPI` as the startup project
3. Press F5 or click "Run"

### Option 3: Via script (together with LibraryWeb)

From the Library project root, run:

```bash
./run-projects.bat
```

## API Access

After starting the project, the API will be available at:

- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001
- **Swagger UI**: https://localhost:5001/swagger

## Endpoints

### Users

| Method | Endpoint | Description |
|--------|----------|-----------|
| POST | `/api/users/register` | Register new user |
| POST | `/api/users/login` | Authenticate user |
| GET | `/api/users/{id}` | Get user data |

### Books

| Method | Endpoint | Description |
|--------|----------|-----------|
| GET | `/api/users/{userId}/books` | List all user's books |
| GET | `/api/users/{userId}/books/{id}` | Get book details |
| POST | `/api/users/{userId}/books` | Create new book |
| PUT | `/api/users/{userId}/books/{id}` | Update existing book |
| DELETE | `/api/users/{userId}/books/{id}` | Delete book |

## Request Examples

### Register User

```bash
POST /api/users/register
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "password123"
}
```

### Login

```bash
POST /api/users/login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "password123"
}
```

### Create Book

```bash
POST /api/users/1/books
Content-Type: application/json

{
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "isbn": "9780132350884",
  "publicationYear": 2008,
  "status": "ToRead"
}
```

## Additional Settings

### CORS

The project is configured with a permissive CORS policy (`AllowAll`) for development. For production, configure appropriately in [Program.cs:27-36](Program.cs#L27-L36).

### Routing

All routes are automatically converted to lowercase via configuration in [Program.cs:14](Program.cs#L14).

## Database Structure

### Table: Users

| Field | Type | Description |
|-------|------|-----------|
| Id | int | Primary key |
| Name | varchar(100) | User name |
| Email | varchar(100) | Email (unique) |
| PasswordHash | varchar(255) | BCrypt hashed password |

### Table: Books

| Field | Type | Description |
|-------|------|-----------|
| Id | int | Primary key |
| Title | varchar(200) | Book title |
| Author | varchar(100) | Author |
| ISBN | varchar(20) | ISBN (optional) |
| PublicationYear | int | Publication year |
| Status | varchar(20) | Status (ToRead, Reading, Read) |
| UserId | int | Foreign key to Users |

## Tests

To run tests (when implemented):

```bash
dotnet test
```

## Troubleshooting

### MySQL Connection Error

Check:
- MySQL is running
- Correct credentials in `appsettings.json`
- Port 3306 is available
- Firewall is not blocking

### Migration Execution Error

```bash
# Remove all migrations
dotnet ef migrations remove

# Recreate migrations
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Contributing

1. Fork the project
2. Create a feature branch (`git checkout -b feature/new-feature`)
3. Commit your changes (`git commit -m 'Add new feature'`)
4. Push to the branch (`git push origin feature/new-feature`)
5. Open a Pull Request

## License

This project is under the MIT license.

## Contact

For questions or suggestions, contact through the project repository.
