
function Update-AssemblyInfoFiles ([string] $version, [string] $codeDir) {

    Write-Host "Versioning files to version " $version

    if ($version -notmatch "[0-9]+(\.([0-9]+|\*)){1,3}") {
        Write-Error "Version number incorrect format: $version"
    }
    
    $versionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $versionAssembly = 'AssemblyVersion("' + $version + '")';
    $versionFilePattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
    $versionAssemblyFile = 'AssemblyFileVersion("' + $version + '")';
 

    Get-ChildItem -Path $codeDir -r -filter *AssemblyInfo.cs | % {
        $filename = $_.fullname
        
        $tmp = ($filename + ".tmp")
        if (test-path ($tmp)) { remove-item $tmp }

        (get-content $filename) | % {$_ -replace $versionFilePattern, $versionAssemblyFile } | % {$_ -replace $versionPattern, $versionAssembly }  > $tmp
        write-host Updating file AssemblyInfo and AssemblyFileInfo: $filename --> $versionAssembly / $versionAssemblyFile

        if (test-path ($filename)) { remove-item $filename -force}
        move-item $tmp $filename -force 
    }
}
