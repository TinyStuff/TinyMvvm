namespace TinyMvvm;

internal interface ITinyApplication
{
    event EventHandler ApplicationResume;
    event EventHandler ApplicationSleep;
}

internal static class ApplicationResolver
{
    public static ITinyApplication? Current { get; set; }
}

