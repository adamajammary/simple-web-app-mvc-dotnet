# simple-web-app-mvc-dotnet

Simple Web App MVC (C#, ASP.NET 9.0, MVC, Entity Framework ORM, Identity)

## GCP (Google Cloud Platform)

<https://dotnet.gcp.jammary.com/>

## How to get started locally

Clone the git repo.

```shell
git clone https://github.com/adamajammary/simple-web-app-mvc-dotnet.git
```

Create a new appsettings file for local development.

```shell
cp -f "SimpleWebAppMVC\appsettings.json" "SimpleWebAppMVC\appsettings.Development.json"
```

Update your new appsettings file.

```json
{
  "UseMySQL": false,
  "ConnectionStrings": {
    "DbConnection": "Server=(localdb)\\mssqllocaldb;Trusted_Connection=True;Database=SimpleWebAppMVC"
  },
  "Token": {
    "Audience": "http://localhost:57968/",
    "Issuer": "http://localhost:57968/",
    "Key": "d8fd8fb0-2dcb-4b12-892f-2eab015f246a"
  },
}
```

Change working directory to the `SimpleWebAppMVC` project.

```shell
cd SimpleWebAppMVC
```

Check if dependent nuget packages are installed.

```shell
dotnet list package
```

Install dependent nuget packages (if necessary).

```shell
dotnet restore
```

### Database Migration

<https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli#create-your-database-and-schema>

#### Visual Studio Package Manager Console (PMC)

Apply the Database migration in PMC.

```ps1
Update-Database
```

#### Entity Framework Core .NET CLI

Install the CLI tools (if not already installed).

<https://learn.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools>

```shell
dotnet tool install --global dotnet-ef
```

Apply the Database migration using the CLI.

```shell
dotnet ef database update
```

### Visual Studio

When you start the project in Visual Studio you will be asked to trust the IIS Express SSL certificate for Hot Reload purposes. You can choose to install the root certificate if you want, but it's not necessary to run the project in Development mode (since it uses HTTP and no SSL certificate is needed).
