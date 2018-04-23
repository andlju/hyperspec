param($task = "default", $buildNumber = "0", $changesetNumber = "0", $configuration = "Release")

$scriptPath = $MyInvocation.MyCommand.Path
$scriptDir = Split-Path $scriptPath

Write-Host '$scriptPath='$scriptPath
Write-Host '$scriptDir='$scriptDir

get-module psake | remove-module
get-module BuildBootstrap | remove-module

# The BuildBootstrap module is used to fetch initial NuGet packages
Import-Module "$scriptDir\BuildBootstrap.psm1"

# Import the psake module
$psakeFolder = Get-LatestPackageFolder("psake")
$psakeModulePath = "$psakeFolder\tools\psake\psake.psm1"

import-module $psakeModulePath

Write-Host ("Powershell {0} {1}" -f [string]$psversiontable.psversion, "32-bit")

# Run psake with our own build script
invoke-psake "$scriptDir\Default.ps1" $task -framework "4.5.1" -properties @{ "buildNumber"=$buildNumber; "changesetNumber"=$changesetNumber; "configuration"=$configuration;} 

