
$packagesDir = "$PSScriptRoot\packages"
$nuget_exe = "$PSScriptRoot\NuGet.exe"

function Get-LatestPackageFolder ([string]$package) {
  Get-ChildItem "$packagesDir\$package.*" | Select-Object -Last 1
}

# Find or download nuget.exe
if (-not (Test-Path $nuget_exe)) {
    Write-Host "Downloading Nuget.exe"
    (new-object System.Net.WebClient).DownloadFile( "http://www.nuget.org/nuget.exe", $nuget_exe)
}

Write-Host "Restoring packages"

# Download all nuget packages needed. They are downloaded to src\packages dir
$env:EnableNuGetPackageRestore="true"
try {
  & $nuget_exe install "$PSScriptRoot\packages.config" -OutputDirectory "$packagesDir"
} catch [Exception]{
  Write-Host "Unable to install nuget packages $($_.Exception.Message)"
  exit 1
}
if ($lastexitcode -ne 0) {
  Write-Host "`nNuget install failed"
  exit 1
}
