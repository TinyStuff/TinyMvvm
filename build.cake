var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var version = Argument("packageVersion", "1.0.0");

Task("Build").Does(() => {
var settings = new DotNetCoreBuildSettings()
{
    Configuration = "Release"
};

    DotNetCoreBuild("src/TinyMvvm/TinyMvvm.csproj", settings);
    DotNetCoreBuild("src/TinyMvvm.Forms/TinyMvvm.Forms.csproj", settings);
    DotNetCoreBuild("src/TinyMvvm.Autofac/TinyMvvm.Autofac.csproj", settings);
    DotNetCoreBuild("src/TinyMvvm.TinyIoC/TinyMvvm.TinyIoC.csproj", settings);
});

Task("Pack").IsDependentOn("Build").Does(() =>
{
    var settings = new DotNetCorePackSettings()
    {
        IncludeSymbols = true,
        ArgumentCustomization = args=>args.Append($"-p:PackageVersion={version}"),
        Configuration = "Release",
        OutputDirectory = ".packages"
    };

    DotNetCorePack("src/TinyMvvm/TinyMvvm.csproj", settings);
    DotNetCorePack("src/TinyMvvm.Forms/TinyMvvm.Forms.csproj", settings);
    DotNetCorePack("src/TinyMvvm.Autofac/TinyMvvm.Autofac.csproj", settings);
    DotNetCorePack("src/TinyMvvm.TinyIoC/TinyMvvm.TinyIoC.csproj", settings);
});


Task("Publish").IsDependentOn("Pack").Does(() =>{
    var settings = new DotNetCoreNuGetPushSettings
 {
     Source = "https://www.nuget.org/api/v2/package/",
     ApiKey = EnvironmentVariable<string>("NUGETKEY", ""),
     WorkingDirectory = ".packages",
     IgnoreSymbols = false
 };

 DotNetCoreNuGetPush("*.*pkg", settings);
});

RunTarget(target);