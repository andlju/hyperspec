
function Build-Solution (
    $codeDir,
    $solutionToBuild,
    $outDir,
	$configuration="Release"
    )
{
    Write-Host "Now building $solutionToBuild" -ForegroundColor Green
    Exec { msbuild "$codeDir\$solutionToBuild" /t:Build /p:Configuration=$configuration /v:quiet /p:OutDir=$outDir /p:GenerateProjectSpecificOutputFolder=true } 
}

function Clean-Solution (
    $codeDir,
    $solutionToBuild,
    $outDir,
	$configuration="Release"
    )
{
    Write-Host "Creating BuildArtifacts directory" -ForegroundColor Green
    Write-Host "BuildArtifacts path: $outDir"
    if (Test-Path $outDir) 
    {   
        rd $outDir -rec -force | out-null
    }
    
    mkdir $outDir | out-null
    
    Write-Host "Cleaning $solutionToBuild" -ForegroundColor Green

    Exec { msbuild "$codeDir\$solutionToBuild" /t:Clean /p:Configuration=$configuration /v:quiet } 
}
