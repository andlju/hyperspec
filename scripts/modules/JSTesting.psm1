if (-not (Get-Module "BuildBootstrap"))
{
    throw "BuildBootstrap module must be loaded"
}

$chutzpahFolder = Get-LatestPackageFolder("Chutzpah")
$chutzpah_exe = "$chutzpahFolder\tools\chutzpah.console.exe"

function Run-JavascriptTests($location, $folder, $ignoreCodeCoverage) {

    Push-Location

    Set-Location $location

	if ($ignoreCodeCoverage)
	{
		$result = & $chutzpah_exe /path $folder /timeoutMilliseconds 25000 /testMode JavaScript /junit javascript.testresult.xml 
	}
    else
	{
		$result = & $chutzpah_exe /path $folder /timeoutMilliseconds 25000 /testMode JavaScript /junit javascript.testresult.xml  /coverage /coverageExcludes "*Scripts_Tests*,*Scripts\**.*" 
	}
    
    foreach($testResult in $result)
    {
        Write-Host $testResult

        if ($testResult.Contains("[FAIL]")) {
            $failedTests += "`n$testResult"
        }
    }

    Pop-Location

    if ($failedTests)
    {
        Write-Host "JS tests that failed:"
        Write-Host $failedTests
    }

    ($failedTests | Measure-Object -Line).Lines
}