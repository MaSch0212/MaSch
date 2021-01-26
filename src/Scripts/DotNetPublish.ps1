param (
    [Parameter(Mandatory=$true)]
    [Alias("s")]
    [string] $SolutionDir,

    [Parameter(Mandatory=$true)]
    [Alias("p")]
    [string] $ProjectPath,

    [Parameter(Mandatory=$true)]
    [Alias("t")]
    [string[]] $TargetPlatforms,

    [Alias("o")]
    [string] $OutputPath = [IO.Path]::Combine("bin", '$(Configuration)', "publish"),

    [Alias("c")]
    [string] $Configuration = "Release"
)

$projectToPublish = [IO.Path]::Combine($SolutionDir, $ProjectPath)
$outputDir = [IO.Path]::Combine($SolutionDir, $OutputPath.Replace('$(Configuration)', $Configuration))
$projectName = [IO.Path]::GetFileNameWithoutExtension($projectToPublish);

foreach ($runtime in $TargetPlatforms)
{
    $output = [IO.Path]::Combine($outputDir, $runtime)

    Write-Output "`nPublishing `"$projectName`" ($Configuration|$runtime) to `"$("." + $output.SubString($SolutionDir.Length))`"..."
    dotnet publish -o $output -r $runtime -c $Configuration $projectToPublish
}
