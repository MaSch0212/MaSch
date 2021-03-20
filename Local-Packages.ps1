param(
    [Parameter(Mandatory=$true)]
    [string]$Mode,
    [Parameter(Mandatory=$true)]
    [string]$LocalRepoPath,
    [Parameter(Mandatory=$false)]
    [string]$MajorVersion = "1.100"
)

$ErrorActionPreference = 'Stop'

if ($Mode -eq "add") {
    $configPath = "$LocalRepoPath\.masch-config.json"
    if (Test-Path $configPath -PathType Leaf) {
        $config = Get-Content $configPath -Raw | ConvertFrom-Json
    }
    else {
        $config = @{ lastLocalVersion = 0 }
    }

    Set-Location $PSScriptRoot
    $config.lastLocalVersion++
    dotnet pack "-p:Version=$MajorVersion.$($config.lastLocalVersion)" -c Release --force
    ConvertTo-Json $config -Depth 99 | Set-Content $configPath

    Get-ChildItem "bin\Release\MaSch.*.$MajorVersion.$($config.lastLocalVersion).nupkg" | ForEach-Object { nuget add $_ -source $LocalRepoPath }

    Write-Host "Successfully added local packages" -ForegroundColor Green
    Write-Host "Run `"dotnet restore --force`" for the project that should use the local package"
}
elseif ($Mode -eq "cleanup") {
    Get-ChildItem "$LocalRepoPath\masch*" | ForEach-Object { Remove-Item $_ -Force -Recurse }
    Get-ChildItem "$env:USERPROFILE\.nuget\packages\MaSch*\*" | ForEach-Object { Remove-Item $_ -Force -Recurse }
    Write-Host "Local MaSch packages were removed" -ForegroundColor Green
}
else {
    Write-Host "The mode `"$Mode`" is unknwon. You can use `"add`" or `"cleanup`"" -ForegroundColor Red
}