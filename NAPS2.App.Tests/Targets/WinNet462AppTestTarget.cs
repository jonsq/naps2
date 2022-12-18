namespace NAPS2.App.Tests.Targets;

public class WinNet462AppTestTarget : IAppTestTarget
{
    public AppTestExe Console => GetAppTestExe("NAPS2.Console.exe");
    public AppTestExe Gui => GetAppTestExe("NAPS2.exe");
    public AppTestExe Worker => GetAppTestExe("NAPS2.Worker.exe");

    private AppTestExe GetAppTestExe(string exeName)
    {
        return new AppTestExe(
            Path.Combine(AppTestHelper.SolutionRoot, "NAPS2.App.WinForms", "bin", "Debug", "net462"),
            exeName,
            TestRootSubPath: "lib");
    }

    public override string ToString() => "Windows (net462)";
}