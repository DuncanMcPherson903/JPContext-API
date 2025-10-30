# JPContext-API

## Technologies Used

- **Backend**: .NET 8.0, ASP.NET Core
- **API Style**: Minimal API
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Identity with cookie authentication
- **Object Mapping**: AutoMapper
- **Data Format**: JSON

## Getting Started

### Cloning the Repository

Clone the project to the directory of your choice, and then:

```sh
cd JPContext.API
```

### Setting Up the Database

1. Create a connection string user secret. Remember to modify it by putting your password in there before running the commands.
   ```sh
   dotnet user-secrets init
   dotnet user-secrets set 'ConnectionStrings:JPContextDbConnectionString' 'Host=localhost;Port=5432;Username=postgres;Password=your_password;Database=JPContext'
   ```

2. Apply the database migrations:
   ```sh
   cd JPContext.API
   dotnet ef database update
   ```

### Running the API

There is a `launch.json` and `tasks.json` file already in the repostitory, so you can immediately start the program in debug mode.

1. The API will be available at `http://localhost:5000`

## API Endpoints

The API provides the following main endpoint groups:
