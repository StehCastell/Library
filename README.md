# Library Management System

A full-stack personal library management application built with ASP.NET Core 8.0. The system consists of a RESTful API backend and an MVC web frontend.

## Project Structure

```
Library/
├── src/
│   ├── LibraryAPI/          # RESTful API backend
│   │   └── README.md        # API documentation
│   └── LibraryWeb/          # MVC web frontend
│       └── README.md        # Web app documentation
├── DBScripts/               # Database scripts
├── run-projects.bat         # Script to run both projects
└── README.md               # This file
```

## Technologies

### LibraryAPI (Backend)
- .NET 8.0 / ASP.NET Core Web API
- Entity Framework Core 9.0
- MySQL Database
- BCrypt.Net-Next (password hashing)
- Swagger/OpenAPI

### LibraryWeb (Frontend)
- .NET 8.0 / ASP.NET Core MVC
- Razor Views
- Bootstrap
- Session-based authentication
- HttpClient (API consumption)

## Features

- User registration and authentication
- Personal book library management
- CRUD operations for books
- Session management
- Responsive web interface
- RESTful API with Swagger documentation

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL 8.0+](https://dev.mysql.com/downloads/mysql/) or MariaDB
- IDE: Visual Studio 2022, VS Code, or Rider (optional)

## Quick Start

### 1. Clone the repository

```bash
git clone <repository-url>
cd Library
```

### 2. Configure the database

Edit `src/LibraryAPI/appsettings.json` with your MySQL credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=library_db;user=YOUR_USER;password=YOUR_PASSWORD"
  }
}
```

### 3. Create the database

```bash
cd src/LibraryAPI
dotnet ef database update
cd ../..
```

### 4. Configure the API URL

Edit `src/LibraryWeb/appsettings.json` to match the LibraryAPI URL:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:5001"
  }
}
```

### 5. Run both projects

**Option 1: Using the batch script (Windows)**

From the project root:

```bash
./run-projects.bat
```

This will automatically:
- Start LibraryAPI in a new terminal window
- Wait 5 seconds for the API to initialize
- Start LibraryWeb in another terminal window

**Option 2: Manual (separate terminals)**

Terminal 1 - Start the API:
```bash
cd src/LibraryAPI
dotnet run
```

Terminal 2 - Start the Web app:
```bash
cd src/LibraryWeb
dotnet run
```

**Option 3: Using Visual Studio**

1. Open `Library.sln`
2. Right-click the solution → Properties
3. Select "Multiple startup projects"
4. Set both `LibraryAPI` and `LibraryWeb` to "Start"
5. Press F5

## Access the Application

After starting both projects:

- **Web Application**: https://localhost:5003
- **API**: https://localhost:5001
- **Swagger UI**: https://localhost:5001/swagger

## Project Documentation

Each project has its own detailed README with specific instructions:

- **[LibraryAPI README](src/LibraryAPI/README.md)** - API documentation, endpoints, database structure, and troubleshooting
- **[LibraryWeb README](src/LibraryWeb/README.md)** - Web application flow, features, and development guide

## API Endpoints

### Users
- `POST /api/users/register` - Register new user
- `POST /api/users/login` - Authenticate user
- `GET /api/users/{id}` - Get user data

### Books
- `GET /api/users/{userId}/books` - List all user's books
- `GET /api/users/{userId}/books/{id}` - Get book details
- `POST /api/users/{userId}/books` - Create new book
- `PUT /api/users/{userId}/books/{id}` - Update existing book
- `DELETE /api/users/{userId}/books/{id}` - Delete book

For detailed API documentation, visit the Swagger UI at https://localhost:5001/swagger when the API is running.

## Architecture

### LibraryAPI (Backend)
Follows a layered architecture pattern:
- **Controllers**: API endpoints
- **Services**: Business logic
- **Repositories**: Data access layer
- **Models**: Domain entities
- **DTOs**: Data transfer objects

### LibraryWeb (Frontend)
Follows the MVC (Model-View-Controller) pattern:
- **Controllers**: Handle HTTP requests
- **Views**: Razor templates for UI
- **Models**: View models and DTOs
- **Services**: API integration layer

## Database

The application uses MySQL with the following main tables:

- **Users**: User accounts with BCrypt password hashing
- **Books**: Book records linked to users

Database schema is managed through Entity Framework Core migrations.

## Development Workflow

1. Make changes to LibraryAPI (backend)
2. Test endpoints using Swagger UI
3. Update LibraryWeb (frontend) to consume new/updated endpoints
4. Test the full flow in the web application

## Troubleshooting

### Cannot connect to database
- Verify MySQL is running
- Check credentials in `src/LibraryAPI/appsettings.json`
- Ensure database migrations are applied: `dotnet ef database update`

### Web app cannot connect to API
- Ensure LibraryAPI is running
- Verify API URL in `src/LibraryWeb/appsettings.json`
- Check firewall/antivirus settings

### HTTPS certificate errors
Trust the .NET development certificate:
```bash
dotnet dev-certs https --trust
```

### Port already in use
Change the port in `Properties/launchSettings.json` for the affected project.

For more specific troubleshooting, check the individual project READMEs:
- [LibraryAPI Troubleshooting](src/LibraryAPI/README.md#troubleshooting)
- [LibraryWeb Troubleshooting](src/LibraryWeb/README.md#troubleshooting)

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