param([Switch]$client, [Switch]$debug, [Switch]$deploy, [String]$username, [String]$password)

Add-Type -Path "C:\Program Files (x86)\WinSCP\WinSCPnet.dll"

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
    
    $p = Start-Process Unity.exe -ArgumentList '-quit', '-batchmode', '-projectPath PokemonTCGClient', '-executeMethod Builder.PerformBuild', '-logfile editor.log' -PassThru
    $p | Wait-Process

    Get-ChildItem -Path  'E:\PokemonBuild\Client\PokemonTCGClient_Data\StreamingAssets\Cards\' -Recurse |
    Select -ExpandProperty FullName |
    Where {$_ -notlike '*TCGCards*'} |
    sort length -Descending |
    Remove-Item -force 

    Get-ChildItem -Path  'E:\PokemonBuild\Client\PokemonTCGClient_Data\StreamingAssets\Decks\' -Recurse |
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

if ($debug.IsPresent) {
    Write-Output "Building debug server..."
    dotnet publish .\Server\Server.csproj -r linux-x64 -c Debug
    Copy-Item -Path ".\Server\bin\Debug\netcoreapp3.1\linux-x64\publish\*" -Destination "E:\PokemonBuild\Server" -Recurse
}
else {
    dotnet publish .\Server\Server.csproj -r linux-x64 -c Release
    Copy-Item -Path ".\Server\bin\Release\netcoreapp3.1\linux-x64\publish\*" -Destination "E:\PokemonBuild\Server" -Recurse
}

Copy-Item -Path ".\Data\*" -Destination "E:\PokemonBuild\Server" -Recurse

Write-Output "Building launcher..."
dotnet publish .\Launcher\Launcher.csproj
Copy-Item -Path ".\Launcher\bin\Debug\netcoreapp3.1\publish\*" -Destination "E:\PokemonBuild\Launcher" -Recurse

UpdateCardsVersion

if ($client.IsPresent) {
    
    BuildClient
}

if ($deploy.IsPresent) {
    Write-Output "Uploading to server..."
    
    Invoke-WebRequest -Uri "http://85.90.244.171:80/stop"
    $sessionOptions = New-Object WinSCP.SessionOptions -Property @{
        Protocol = [WinSCP.Protocol]::Sftp
        HostName = "sftp://root@85.90.244.171"
        SshHostKeyFingerprint = 'ssh-ed25519-uvkM44haU-oObtd8haavGeKQJdFOAMcJYGfxntcu2MY'
        UserName = $username
        Password = $password
    }

    $session = New-Object WinSCP.Session
    try {
        $session.Open($sessionOptions);
        $files = Get-ChildItem 'E:\PokemonBuild\Server\' -File

        foreach ($f in $files) {
            if ($f.Name.StartsWith('System.')) {
                continue;
            }
            if ($f.Name.StartsWith('libc')) {
                continue;
            }
            if ($f.Name.StartsWith('Microsoft.')) {
                continue;
            }

            $session.PutFiles($f.FullName, '/root/server/');
        }
    }
    finally {
        $session.Dispose();
        Write-Output "Restarting server...";
        Invoke-WebRequest -Uri "http://85.90.244.171:80/start"
    }
}

Write-Output "Done!"