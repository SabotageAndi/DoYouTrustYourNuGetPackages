var builder = DistributedApplication.CreateBuilder(args);

builder.AddContainer("baget-nuget-server", "loicsharma/baget", "latest")
    .WithEnvironment("ApiKey", "NUGET-SERVER-API-KEY")
    .WithEnvironment("Storage__Type", "FileSystem")
    .WithEnvironment("Storage__Path", "/var/baget/packages")
    .WithEnvironment("Database__Type", "Sqlite")
    .WithEnvironment("Database__ConnectionString", "Data Source=/var/baget/baget.db")
    .WithEnvironment("Search__Type", "Database")
    .WithHttpEndpoint(5555,80)
    .WithVolume(target:"/var/baget")
    ;

builder.Build().Run();
