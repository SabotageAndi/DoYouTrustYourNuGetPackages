using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// BaGet NuGet Server
builder.AddContainer("baget-nuget-server", "loicsharma/baget", "latest")
    .WithEnvironment("ApiKey", "NUGET-SERVER-API-KEY")
    .WithEnvironment("Storage__Type", "FileSystem")
    .WithEnvironment("Storage__Path", "/var/baget/packages")
    .WithEnvironment("Database__Type", "Sqlite")
    .WithEnvironment("Database__ConnectionString", "Data Source=/var/baget/baget.db")
    .WithEnvironment("Search__Type", "Database")
    .WithEnvironment("AllowPackageOverwrites", "true")
    .WithHttpEndpoint(5555,80)
    .WithVolume(target:"/var/baget")
    .WithLifetime(ContainerLifetime.Persistent);

// Dependency-Track PostgreSQL DB
var db = builder.AddContainer("dependencytrack-db", "postgres", "15")
    .WithEnvironment("POSTGRES_USER", "dependencytrack")
    .WithEnvironment("POSTGRES_PASSWORD", "dependencytrack")
    .WithEnvironment("POSTGRES_DB", "dependencytrack")
    .WithVolume(target: "/var/lib/postgresql/data")
    .WithEndpoint(5432, 5432)
    .WithLifetime(ContainerLifetime.Persistent);

// Dependency-Track API
var api = builder.AddContainer("dependencytrack-api", "dependencytrack/apiserver", "latest")
    .WithEnvironment("ALPINE_LOG_LEVEL", "info")
    .WithEnvironment("ALPINE_DATABASE_MODE", "external")
    .WithEnvironment("ALPINE_DATABASE_URL", "jdbc:postgresql://dependencytrack-db:5432/dependencytrack")
    .WithEnvironment("ALPINE_DATABASE_DRIVER", "org.postgresql.Driver")
    .WithEnvironment("ALPINE_DATABASE_USERNAME", "dependencytrack")
    .WithEnvironment("ALPINE_DATABASE_PASSWORD", "dependencytrack")
    .WithVolume(target: "/data")
    .WithHttpEndpoint(8081, 8080)
    .WaitFor(db)
    .WithLifetime(ContainerLifetime.Persistent);


// Dependency-Track Frontend
builder.AddContainer("dependencytrack-frontend", "dependencytrack/frontend", "latest")
    .WithEnvironment("API_BASE_URL", "http://localhost:8081")
    .WithHttpEndpoint(8080, 8080)
    .WaitFor(api)
    .WithLifetime(ContainerLifetime.Persistent);

builder.Build().Run();
