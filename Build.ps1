function UpdateClientVersion {
    $raw = Get-Content -Path "E:\PokemonBuild\Server\client.version"
    $versionNumbers = $raw.Split(".");
    
    $buildVersion = $versionNumbers[2] -as [int]
    
    $newBuild = $buildVersion + 1
    
    $rawNewVersion = $versionNumbers[0] + "." + $versionNumbers[1] + "." + $newBuild
    
    Set-Content -Path "E:\PokemonBuild\Server\client.version" -Value $rawNewVersion
}

function UpdateCardsVersion {
    $raw = Get-Content -Path "E:\PokemonBuild\Server\cards.version"
    $versionNumbers = $raw.Split(".");
    
    $buildVersion = $versionNumbers[2] -as [int]
    
    $newBuild = $buildVersion + 1
    
    $rawNewVersion = $versionNumbers[0] + "." + $versionNumbers[1] + "." + $newBuild
    
    Set-Content -Path "E:\PokemonBuild\Server\cards.version" -Value $rawNewVersion
}

function BuildClient {
    Write-Output "Building client..."
    
    $p = Start-Process Unity.exe -ArgumentList '-quit', '-batchmode', '-projectPath PokemonTCGClient', '-executeMethod Builder.PerformBuild', '-logfile editor.log' -Wait

    Get-ChildItem -Path  'E:\PokemonBuild\Client\PokemonTCGClient_Data\StreamingAssets\Cards\' -Recurse -exclude somefile.txt |
    Select -ExpandProperty FullName |
    Where {$_ -notlike '*TCGCards*'} |
    sort length -Descending |
    Remove-Item -force 

    Remove-Item -Path "E:\PokemonBuild\Client\PokemonTCGClient_Data\StreamingAssets\Cards\*.*" -Force -Exclude "*TCGCards\*" -Recurse

    Write-Output "Updating version number..."

    UpdateClientVersion

    Write-Output "Zipping client..."

    Compress-Archive -Path "E:\PokemonBuild\Client\*" -DestinationPath "E:\PokemonBuild\Server\Client.zip" -CompressionLevel Optimal -Force
}

Write-Output "Building server..."

dotnet publish .\Server\Server.csproj -r linux-x64 -c Release

Copy-Item -Path ".\Server\bin\Release\netcoreapp3.1\linux-x64\publish\*" -Destination "E:\PokemonBuild\Server" -Recurse
Copy-Item -Path ".\Data\*" -Destination "E:\PokemonBuild\Server" -Recurse

Write-Output "Building launcher..."
dotnet publish .\Launcher\Launcher.csproj
Copy-Item -Path ".\Launcher\bin\Debug\netcoreapp3.1\publish\*" -Destination "E:\PokemonBuild\Launcher" -Recurse

UpdateCardsVersion

if ($args[0] -eq '-client') {
    
    BuildClient
}