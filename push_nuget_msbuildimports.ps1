# Push all NuGet packages (*.nupkg) found in subdirectories

$source = "http://localhost:5555/v3/index.json"
$apiKey = "NUGET-SERVER-API-KEY"

# Find all .nupkg files recursively
$packages = Get-ChildItem -Path . -Recurse -Filter *.nupkg

foreach ($pkg in $packages) {
    Write-Host "Pushing package: $($pkg.FullName)"
    dotnet nuget push -s $source $pkg.FullName --allow-insecure-connections -k $apiKey
}