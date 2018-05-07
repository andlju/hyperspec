#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=ReportUnit"
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var buildNumber = Argument<int>("buildnumber", 0);

///////////////////////////////////////////////////////////////////////////////
// PREPARATION
///////////////////////////////////////////////////////////////////////////////

var projectName = "Hyperspec";

// Get whether or not this is a local build.
var local = BuildSystem.IsLocalBuild;
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;

// Parse release notes.
var releaseNotes = ParseReleaseNotes("./ReleaseNotes.md");

// Get version.
var semanticVersion = releaseNotes.Version.ToString();

// Get build number
if (isRunningOnAppVeyor) {
  buildNumber = AppVeyor.Environment.Build.Number;
}

// Define directories.
var sourceDirectory = Directory("./src");

var outputDirectory = Directory("./output");
var testResultsDirectory = outputDirectory + Directory("tests");
var artifactsDirectory = outputDirectory + Directory("artifacts");
var solutions = GetFiles("./**/*.sln");
var solutionPaths = solutions.Select(solution => solution.GetDirectory());


///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
  // Executed BEFORE the first task.
  Information("Target: " + target);
  Information("Configuration: " + configuration);
  Information("Is local build: " + local.ToString());
  Information("Is running on AppVeyor: " + isRunningOnAppVeyor.ToString());
  Information("Semantic Version: " + semanticVersion);
  Information("NuGet Api Key: " + EnvironmentVariable("NuGetApiKey"));
});

Teardown(context =>
{
  // Executed AFTER the last task.
  Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
  .Does(() =>
{
  // Clean solution directories.
  foreach(var path in solutionPaths)
  {
    Information("Cleaning {0}", path);
    CleanDirectories(artifactsDirectory.Path + "/");
    CleanDirectories(path + "/**/bin/" + configuration);
    CleanDirectories(path + "/**/obj/" + configuration);
  }

  CleanDirectories(outputDirectory);
});

Task("Create-Directories")
  .IsDependentOn("Clean")
  .Does(() =>
{
  var directories = new List<DirectoryPath>{ outputDirectory, testResultsDirectory, artifactsDirectory };
  directories.ForEach(directory => 
  {
    if (!DirectoryExists(directory))
    {
      CreateDirectory(directory);
    }
  });
});

Task("Restore-NuGet-Packages")
  .IsDependentOn("Create-Directories")
  .Does(() =>
{
  // Restore all NuGet packages.
  foreach(var solution in solutions)
  {
    Information("Restoring {0}...", solution);
    NuGetRestore(solution);
  }
});

Task("Patch-Assembly-Info")
  .IsDependentOn("Restore-NuGet-Packages")
  .WithCriteria(() => !local)
  .Does(() =>
{
  var assemblyInfoFiles = GetFiles("./**/AssemblyInfo.cs");
  var assemblyVersion = semanticVersion + "." + buildNumber;

  foreach(var assemblyInfoFile in assemblyInfoFiles)
  {
    var parsedAssemblyInfo = ParseAssemblyInfo(assemblyInfoFile);
    CreateAssemblyInfo(assemblyInfoFile, new AssemblyInfoSettings {
      // Copy everything except version from the old AssemblyInfo file
      Product = parsedAssemblyInfo.Product,
      Company = parsedAssemblyInfo.Company,
      Title = parsedAssemblyInfo.Title,
      Description = parsedAssemblyInfo.Description,
      Guid = parsedAssemblyInfo.Guid,
      Copyright = parsedAssemblyInfo.Copyright,
      Trademark = parsedAssemblyInfo.Trademark,
      ComVisible = parsedAssemblyInfo.ComVisible,
      CLSCompliant = parsedAssemblyInfo.ClsCompliant,
      InternalsVisibleTo = parsedAssemblyInfo.InternalsVisibleTo,
      Configuration = parsedAssemblyInfo.Configuration,

      Version = assemblyVersion,
      FileVersion = assemblyVersion,
      InformationalVersion = assemblyVersion
    });
  }
});

Task("Build")
  .IsDependentOn("Patch-Assembly-Info")
  .Does(() =>
{
  // Build all solutions.
  foreach(var solution in solutions)
  {
    Information("Building {0}", solution);
    MSBuild(solution, settings => 
      settings
        .WithTarget("Build")
        .SetConfiguration(configuration));
  }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
  var testAssemblies = GetFiles(sourceDirectory.Path + "/**/bin/" + configuration + "/*.Tests.dll");
  if(testAssemblies.Count() > 0)
  {
    XUnit2(sourceDirectory.Path + "/**/bin/" + configuration + "/*.Tests.dll", 
      new XUnit2Settings
      { 
        OutputDirectory = testResultsDirectory.Path,
        XmlReport = true
      }
    );
  }
}).
Finally(() => {
  ReportUnit(testResultsDirectory.Path);
});

Task("Create-NuGet-Packages")
  .IsDependentOn("Run-Unit-Tests")
  .Does(() =>
{
  var nugetVersion = semanticVersion;
  if (local) {
    nugetVersion = string.Format("{0}-build{1:0000}", nugetVersion, buildNumber);
  }
  var nuspecFiles = GetFiles(sourceDirectory.Path + "/**/*.nuspec");
  foreach(var nuspecFile in nuspecFiles)
  {
    NuGetPack(nuspecFile,
      new NuGetPackSettings
      {
        BasePath = nuspecFile.GetDirectory() + "/bin/" + configuration,
        OutputDirectory = artifactsDirectory.Path,
        Version = nugetVersion
      }
    );
  }
});

Task("Update-AppVeyor-Build-Number")
  .WithCriteria(() => isRunningOnAppVeyor)
  .Does(() =>
{
  var appVeyorVersion = semanticVersion + "." + buildNumber;
  AppVeyor.UpdateBuildVersion(appVeyorVersion);
});

Task("Upload-AppVeyor-Artifacts")
  .IsDependentOn("Package")
  .WithCriteria(() => isRunningOnAppVeyor)
  .Does(() =>
{
  var artifacts = GetFiles(artifactsDirectory.Path + "/**/*.nupkg");
  foreach(var artifact in artifacts)
  {
    AppVeyor.UploadArtifact(artifact);
  }
});

Task("Publish-NuGet-Packages")
  .IsDependentOn("Upload-AppVeyor-Artifacts")
  .WithCriteria(() => !local)
  .WithCriteria(() => !isPullRequest)
  .Does(() =>
{
  // Resolve the API key.
  var apiKey = EnvironmentVariable("NuGetApiKey");
  if(string.IsNullOrEmpty(apiKey)) {
    throw new InvalidOperationException("Could not resolve NuGet API key.");
  }

  var nugetPackages = GetFiles(artifactsDirectory.Path + "/**/*.nupkg");
  foreach(var nugetPackage in nugetPackages)
  {
    NuGetPush(nugetPackage, new NuGetPushSettings {
      ApiKey = apiKey
    });
  }
});

///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
  .IsDependentOn("Run-Unit-Tests");

Task("Package")
  .IsDependentOn("Create-NuGet-Packages");

Task("Publish")
  .IsDependentOn("Update-AppVeyor-Build-Number")
  .IsDependentOn("Upload-AppVeyor-Artifacts")
  /*.IsDependentOn("Publish-NuGet-Packages")*/;

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
