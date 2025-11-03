# LibraryWeb

Web application for personal library management, built with ASP.NET Core 8.0 MVC. This is the frontend that consumes the LibraryAPI.

## Technologies Used

- .NET 8.0
- ASP.NET Core MVC
- Razor Views
- Bootstrap (for UI styling)
- HttpClient (for API consumption)
- Session-based authentication

## Architecture

The project follows the MVC (Model-View-Controller) pattern:

```
LibraryWeb/
├── Controllers/         # MVC controllers
├── Views/               # Razor views
├── Models/              # View models and DTOs
├── Services/            # API integration services
├── wwwroot/             # Static files (CSS, JS, images)
└── Program.cs           # Application configuration
```

## Features

### Authentication
- User registration
- Login with email and password
- Session management (2-hour timeout)
- Automatic logout

### Book Management
- View personal book library
- Add new books
- Edit existing books
- Delete books
- Filter and search books

### User Interface
- Responsive design
- Bootstrap-based styling
- Form validation
- Error handling

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- LibraryAPI running (see [LibraryAPI README](../LibraryAPI/README.md))
- IDE: Visual Studio 2022, VS Code, or Rider (optional)

## Installation

### 1. Clone the repository

```bash
git clone <repository-url>
cd Library/src/LibraryWeb
```

### 2. Configure the API connection

Edit the [appsettings.json](appsettings.json) file with the LibraryAPI URL:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:5001"
  }
}
```

**Note:** Make sure the URL matches the port where LibraryAPI is running.

### 3. Install dependencies

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
2. Set `LibraryWeb` as the startup project
3. Press F5 or click "Run"

### Option 3: Via script (together with LibraryAPI)

From the Library project root, run:

```bash
./run-projects.bat
```

This will start both LibraryAPI and LibraryWeb automatically.

## Application Access

After starting the project, the application will be available at:

- **HTTP**: http://localhost:5002
- **HTTPS**: https://localhost:5003

## Application Flow

1. **Home Page** (`/`)
   - If not logged in: Redirects to Login
   - If logged in: Redirects to Books

2. **Register** (`/Account/Register`)
   - Create a new user account
   - Automatic login after registration

3. **Login** (`/Account/Login`)
   - Authenticate with email and password
   - Session created on success

4. **Books** (`/Books`)
   - View all user's books
   - Add, edit, and delete books
   - Requires authentication

5. **Logout** (`/Account/Logout`)
   - Clear session
   - Redirect to Login

## Configuration

### Session

Sessions are configured with:
- **Timeout**: 2 hours
- **HttpOnly cookies**: Security enabled
- **Storage**: In-memory (for development)

Configuration in [Program.cs:13-18](Program.cs#L13-L18).

### Routing

All routes are automatically converted to lowercase via configuration in [Program.cs:9](Program.cs#L9).

## API Integration

The application consumes the LibraryAPI through the `ApiService` class:

- **Register User**: `POST /api/users/register`
- **Login**: `POST /api/users/login`
- **Get Books**: `GET /api/users/{userId}/books`
- **Get Book by ID**: `GET /api/users/{userId}/books/{id}`
- **Create Book**: `POST /api/users/{userId}/books`
- **Update Book**: `PUT /api/users/{userId}/books/{id}`
- **Delete Book**: `DELETE /api/users/{userId}/books/{id}`

## Project Structure

### Controllers

- **HomeController**: Home page routing
- **AccountController**: Registration, login, and logout
- **BooksController**: Book CRUD operations

### Views

- **Account/Login.cshtml**: Login page
- **Account/Register.cshtml**: Registration page
- **Books/Index.cshtml**: Book library page
- **Shared/_Layout.cshtml**: Main layout template

### Models

- **User**: User entity
- **Book**: Book entity
- **LoginViewModel**: Login form model
- **RegisterViewModel**: Registration form model

### Services

- **IApiService**: API service interface
- **ApiService**: HTTP client implementation for API calls

## Troubleshooting

### Cannot connect to API

Check:
- LibraryAPI is running
- API URL is correct in `appsettings.json`
- Port matches the LibraryAPI port
- No firewall blocking connections

### Session not working

Check:
- Cookies are enabled in browser
- Session is configured in `Program.cs`
- Application is using HTTPS (required for secure cookies)

### HTTPS certificate errors

For development, trust the .NET development certificate:

```bash
dotnet dev-certs https --trust
```

## Development

### Adding new pages

1. Create a new Controller in `Controllers/`
2. Add corresponding Views in `Views/[ControllerName]/`
3. Update navigation in `Views/Shared/_Layout.cshtml`

### Modifying styles

Static files are located in `wwwroot/`:
- CSS: `wwwroot/css/`
- JavaScript: `wwwroot/js/`
- Images: `wwwroot/images/`

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
