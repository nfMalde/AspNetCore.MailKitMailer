
# Check if we are on a version branch or release branch (eg. 1.1.x or release-1.1.x)
$currentBranch = (git branch --show-current)
$isReleaseBranch = ("$($currentBranch)").StartsWith("release-");

$parsedBranchName = $currentBranch;

if ($isReleaseBranch) {
    Write-Host "We are on release branch. Versioning will be permanent"
    $parsedBranchName = ("$($currentBranch)").Substring(("release-").Length)

}

if ($parsedBranchName.StartsWith('v')) {
    $parsedBranchName = $parsedBranchName.Substring(1)

}

$isVersionBranch = $parsedBranchName -match "(\d+.\d+.x)"

if (!$isVersionBranch) {
    Write-Host "Canceling. Not a version branch"
}
$versionParts = $parsedBranchName.Split('.')
$versionPrefix = "$($versionParts[0]).$($versionParts[1])" 

Write-Host $currentBranch


$startVersion = [version]::Parse("$($versionPrefix).0")
# Get Git Tags
$gitTags = git tag --list --sort=-version:refname "v$($versionPrefix).*"
$newVersion = [version]::Parse("$($startVersion.Major).$($startVersion.Minor).$($startVersion.Build+1)")
Write-Host "Tags: $($gitTags)"
if ($gitTags.Length -gt 0) {
    if ($gitTags[0][0] -eq "v") {
        $latest =  ("$($gitTags[0])").Substring(1)

        Write-Host "Latest  Tag is $($latest)"
        $versionInfo = [Version]::Parse($latest)
    
        if ($versionInfo) {
            Write-Host "Version Parsed: $($versionInfo.Major).$($versionInfo.Minor).$($versionInfo.Build)."
            $newVersion = [version]::Parse("$($versionInfo.Major).$($versionInfo.Minor).$($versionInfo.Build+1)")
            
        }
    }
}


Write-Host "New Version is $($newVersion.Major).$($newVersion.Minor).$($newVersion.Build)"

Write-Output "::set-output name=version::$($newVersion.Major).$($newVersion.Minor).$($newVersion.Build)"