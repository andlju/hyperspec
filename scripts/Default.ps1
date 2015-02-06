#This build assumes the following directory structure
#
#  \scripts   - This is where the project build code lives
#  \BuildArtifacts - This folder is created if it is missing and contains output of the build
#  \src         - This folder contains the source code or solutions you want to build
#

Properties {
    $mainVersion = "0.1"        # Change the main version number here
    $buildNumber = 0            # This should be set by the build server
	$configuration = "Release"
}

$solutionToBuild = "Hyperspec.sln"

$build_dir = Split-Path $psake.build_script_file
$repository_root_dir = "$build_dir\..\"
$build_artifacts_dir = "$repository_root_dir\BuildArtifacts"
$code_dir = "$repository_root_dir\src"
$nugetOutputFolder = "$build_artifacts_dir\Nuget"
$moduleFolder = "$build_dir\modules"

Write-Host "Using modules from: $moduleFolder"

Import-Module "$moduleFolder\Versioning.psm1"
Import-Module "$moduleFolder\Building.psm1"
Import-Module "$moduleFolder\Testing.psm1"
Import-Module "$moduleFolder\Packaging.psm1"

FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task Default -Depends Local

Task Local -Depends Clean, Build, Package

Task LocalWithTests -Depends Clean, Build, UnitTest, Package

Task VersionFiles {
    $packageVersion = "$mainVersion.$buildNumber"
    $assemblyVersion = "$packageVersion.0"
    Update-AssemblyInfoFiles $assemblyVersion $code_dir
}

Task Clean {
    Clean-Solution $code_dir $solutionToBuild $build_artifacts_dir $configuration
}

Task Build -Depends Clean { 
    Build-Solution $code_dir $solutionToBuild $build_artifacts_dir $configuration
}

Task UnitTest -Depends Build {
    if ((Run-Tests $build_artifacts_dir '*.Test.dll') -gt 0)
    {
        exit 1
    }
}

Task Package -Depends Build {
    $packageVersion = "$mainVersion.$buildNumber"
    Build-Packages -location:$build_artifacts_dir -output:$nugetOutputFolder -defaultNugetVersion:$packageVersion
}

Task Publish -Depends Package {
    Publish-Packages -location:$nugetOutputFolder
}
