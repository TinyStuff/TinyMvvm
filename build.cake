var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

Task("Build").Does(() => {
var settings = new DotNetCoreBuildSettings()
{
    Configuration = "Release",
    Sources = new List<string>() {"https ://api.nuget.org/v3/index.json",https://aka.ms/dotnet6/nuget/index.json","https://pkgs.dev.azure.com/dnceng/public/_packaging/darc-pub-dotnet-runtime-6f411658/nuget/v3/index.json", "https://pkgs.dev.azure.com/dnceng/public/_packaging/darc-pub-dotnet-emsdk-1ec2e17f/nuget/v3/index.json"}
};
    DotNetCoreBuild("src/TinyMvvm/TinyMvvm.csproj", settings);
    DotNetCoreBuild("src/TinyMvvm.Forms/TinyMvvm.Forms.csproj", settings);
    DotNetCoreBuild("src/TinyMvvm.Autofac/TinyMvvm.Autofac.csproj", settings);
    DotNetCoreBuild("src/TinyMvvm.TinyIoC/TinyMvvm.TinyIoC.csproj", settings);
    DotNetCoreBuild("src/TinyMvvm.Maui/TinyMvvm.Maui.csproj", settings);
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
    DotNetCorePack("src/TinyMvvm.Maui/TinyMvvm.Maui.csproj", settings);
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