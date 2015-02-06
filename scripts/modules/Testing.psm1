$vsTools = "$env:ProgramFiles\Microsoft Visual Studio 12.0\Common7\IDE"
$mstest = "$vsTools\MSTest.exe"

function Run-Tests (
        [string] $location,
        [string] $filter)
{
    Push-Location

    Set-Location $location

    $unitTestAssemblies = get-childitem -Include $filter -Recurse

    foreach($unitTestAssembly in $unitTestAssemblies)
    {
        $UnitTestResultsFile = "unitTestOutput.trx"
        $unitTestAssemblyFileName = split-path -leaf $unitTestAssembly

        # Need to remove previous test result file on disk
        if (Test-Path $UnitTestResultsFile)
        {
            Remove-Item $UnitTestResultsFile
        }

        # Run the tests
        $result = & $mstest /testcontainer:$unitTestAssembly /resultsFile:$UnitTestResultsFile

        if (Test-Path $UnitTestResultsFile)
        {
            $UnitTestResultsFileXml = [Xml](Get-Content -Path $UnitTestResultsFile)
            $ns = New-Object Xml.XmlNamespaceManager $UnitTestResultsFileXml.NameTable
            $ns.AddNamespace('dns','http://microsoft.com/schemas/VisualStudio/TeamTest/2010')
             
            $UnitTests = $UnitTestResultsFileXml.SelectNodes('//dns:TestRun/dns:TestDefinitions/dns:UnitTest',$ns)
            $UnitTestResults = $UnitTestResultsFileXml.SelectNodes('//dns:TestRun/dns:Results/dns:UnitTestResult',$ns)
            $ResultSummary = $UnitTestResultsFileXml.SelectSingleNode('//dns:TestRun/dns:ResultSummary/dns:Counters',$ns)

            $testsFailed += $ResultSummary.failed -as [int] 
            $testsPassed += $ResultSummary.passed -as [int] 
            $testsExecuted += $ResultSummary.executed -as [int] 
                     
            Write-Host "--------------------------------------------------------------------------------"
            Write-Host "Unit test results: $unitTestAssemblyFileName"
            Write-Host "--------------------------------------------------------------------------------"
 
            Foreach ($UnitTestResult in $UnitTestResults)
            {                     
                $ClassName = ($UnitTests | Where {$_.id -eq $UnitTestResult.testId}).TestMethod.ClassName.split(",")[0]             

                if ($UnitTestResult.outcome -eq "passed") { $Color = 'Green' }
                elseif ($UnitTestResult.outcome -eq "warning") { $Color = 'Yellow' }
                elseif ($UnitTestResult.outcome -eq "failed") { $Color = 'Red' }
                else { $Color = 'Gray' }
                     
                Write-Host $UnitTestResult.outcome"`t"$UnitTestResult.testName"`t"$ClassName -ForegroundColor $Color                     
                if ($UnitTestResult.outcome -eq "failed")
                {
                    Write-Host
                    Write-Host "`t"Error message: $UnitTestResult.Output.ErrorInfo.Message  -ForegroundColor Red 
                    Write-Host "`t"Stack trace: $UnitTestResult.Output.ErrorInfo.StackTrace -ForegroundColor Red
                }
            }
        }
    }
    
    Pop-Location

    Write-Host "--------------------------------------------------------------------------------"
    Write-Host "Unit test summary: $testsExecuted" tests where executed
    Write-Host "--------------------------------------------------------------------------------"

    if ($testsPassed -eq $testsExecuted)
    {
        Write-Host "All $testsPassed tests passed!" -ForegroundColor Green      
    }
    elseif ($testsPassed -gt 0)
    {
        Write-Host "$testsPassed test(s) passed" -ForegroundColor Green     
    }
    if ($testsFailed -gt 0)
    {
        Write-Host "$testsFailed test(s) failed" -ForegroundColor Red
    }

    $testsFailed
}
