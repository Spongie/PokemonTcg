Write-Output "Building server..."

dotnet clean .\Server\Server.csproj
dotnet build .\Server\Server.csproj

Copy-Item -Path ".\Server\bin\Debug\netcoreapp3.1\*" -Destination "E:\PokemonBuild\Server" -Recurse -Force

Write-Output "Building launcher..."
dotnet publish .\Launcher\Launcher.csproj
Copy-Item -Path ".\Launcher\bin\Debug\netcoreapp3.1\publish\*" -Destination "E:\PokemonBuild\Launcher" -Recurse

$raw = Get-Content -Path "E:\PokemonBuild\Server\version"
$versionNumbers = $raw.Split(".");

$buildVersion = $versionNumbers[2] -as [int]

$newBuild = $buildVersion + 1

$rawNewVersion = $versionNumbers[0] + "." + $versionNumbers[1] + "." + $newBuild

Set-Content -Path "E:\PokemonBuild\Server\version" -Value $rawNewVersion