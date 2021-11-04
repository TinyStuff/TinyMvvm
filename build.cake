var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

Task("Build").Does(() => {
var settings = new DotNetCoreBuildSettings()
{
    Configuration = "Release"
};
    DotNetCoreBuild("src/TinyMvvm/TinyMvvm.csproj", settings);
    DotNetCoreBuild("src/TinyMvvm.Forms/TinyMvvm.Forms.csproj", settings);
    DotNetCoreBuild("src/TinyMvvm.Autofac/TinyMvvm.Autofac.csproj", settings);
    DotNetCoreBuild("src/TinyMvvm.TinyIoC/TinyMvvm.TinyIoC.csproj", settings);
    DotNetCoreBuild("src/TinyMvvm.TinyIoC/TinyMvvm.Maui.csproj", settings);
});

Task("Pack").IsDependentOn("Build").Does(() =>
{
    var version = EnvironmentVariable<string>("GITHUB_REF", "").Split("/").Last();

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
    DotNetCorePack("src/TinyMvvm.TinyIoC/TinyMvvm.Maui.csproj", settings);
});


Task("Publish").IsDependentOn("Pack").Does(() =>{

var apiKey = EnvironmentVariable<string>("NUGETKEY", "");

    var settings = new DotNetCoreNuGetPushSettings
 {
     Source = "https://www.nuget.org/api/v2/package/",
     ApiKey = apiKey,
     IgnoreSymbols = false
 };
    DotNetCoreNuGetPush(".packages/*.nupkg", settings);

    var symbolSettings = new DotNetCoreNuGetPushSettings
    {
        Source = "https://www.nuget.org/api/v2/symbolpackage/",
        ApiKey = apiKey,
        IgnoreSymbols = false
    };



    DotNetCoreNuGetPush(".packages/*.snupkg", symbolSettings);
});

RunTarget(target);