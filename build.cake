#tool "nuget:?package=GitVersion.CommandLine"

var entryPoint = "EntryPoint";
var defaultTarget = "unknown";
var versionInfoTarget = "GetVersionInfo";

Task(entryPoint)
  .Does(() =>
  {
    var target = Argument("target", defaultTarget);
    if(target.Equals(defaultTarget, StringComparison.InvariantCultureIgnoreCase))
    {
      #addin "nuget:?package=Cake.Dialog"
      InputDialog(options => {
        options.Title = "Enter Build Target";
        options.Text = "Default";
        options.OnOk = (txt) =>
        {
          target = txt;
        };
      });
    }

    Information("Build Target: {0}", target);
    RunTarget(target);
  });

Task(versionInfoTarget)
  .Does(() =>
  {
    var result = GitVersion(new GitVersionSettings {
      NoFetch = false
    });

    Information("FullSemVer: {0}, FullBuildMetaData: {1}", result.FullSemVer, result.FullBuildMetaData);
  });

Task("Default")
  .IsDependentOn(versionInfoTarget)
  .Does(() => 
  {
    Information("Hello World!");
  });
  
RunTarget(entryPoint);
