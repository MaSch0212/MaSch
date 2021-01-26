param (
    [Parameter(Mandatory=$true)]
    [string] $AssemblyInfoPath
)

if (-not (Test-Path $AssemblyInfoPath -PathType Leaf)) {
    Write-Error "The assembly info file does not exist at '$AssemblyInfoPath'"
    exit 1
}

$dateFilePath = [IO.Path]::Combine([IO.Path]::GetDirectoryName($AssemblyInfoPath), 
    [IO.Path]::GetFileNameWithoutExtension($AssemblyInfoPath) + "_increment.tmp");

$mutex = New-Object System.Threading.Mutex($false, "922101f1-590e-5c7a-86c9-a39bde57e7fc")
$mutex.WaitOne() | Out-Null
try {
    $data = @{}
    $doIncrement = $true
    if (Test-Path $dateFilePath -PathType Leaf) {
        $data = Get-Content $dateFilePath | ConvertFrom-Json
        if (([DateTime]::UtcNow - $data.lastIncrement).TotalSeconds -lt 30) {
            $doIncrement = $false
        }
    }
    else {
        New-Item $dateFilePath -ItemType File | Out-Null
        attrib +h $dateFilePath
    }

    $data.lastIncrement = [DateTime]::UtcNow
    Set-Content $dateFilePath ($data | ConvertTo-Json)
}
finally {
    $mutex.ReleaseMutex();
    $mutex.Dispose();
}

if ($doIncrement) {
    $fileContent = Get-Content $AssemblyInfoPath -Raw
    $match = [Text.RegularExpressions.Regex]::Match($fileContent, "\[assembly:\s*AssemblyFileVersion\(\""(?<version>[0-9\.]+)\""\)\]")

    if (-not $match.Success) {
        Write-Error "No AssemblyFileVersion attribute found in assembly info file '$AssemblyInfoPath'"
        exit 2
    } else {
        $versionGroup = $match.Groups["version"]
        $version = [Version]::Parse($versionGroup.Value)
        $version = New-Object Version $version.Major, $version.Minor, ($version.Build + 1), $version.Revision
        $fileContent = ($fileContent.SubString(0, $versionGroup.Index) + $version + $fileContent.SubString($versionGroup.Index + $versionGroup.Length)).TrimEnd()
    
        if ([string]::IsNullOrWhiteSpace($fileContent)) {
            Write-Error "File Content is empty after processing. Aborting."
            exit 3
        } else {
            Set-Content $AssemblyInfoPath $fileContent
            Write-Host "Assembly file version has been incremented to $version."
        }
    }
}