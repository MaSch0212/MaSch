using DiffEngine;
using VerifyMSTest;
using VerifyTests;

namespace MaSch.CodeAnalysis.CSharp.UnitTests;

[TestClass]
public static class GlobalSettings
{
    [AssemblyInitialize]
    public static void InitializeAssembly(TestContext context)
    {
        VerifyBase.DerivePathInfo(DerivePathInfo);
        DiffTools.UseOrder(new[] { DiffTool.VisualStudioCode });
        DiffRunner.MaxInstancesToLaunch(50);
    }

    private static PathInfo DerivePathInfo(string sourceFile, string projectDirectory, Type type, MethodInfo method)
    {
        var subDir = type.Namespace!.Substring(typeof(GlobalSettings).Namespace!.Length + 1).Replace('.', Path.DirectorySeparatorChar);
        var typeName = type.Name;
        if (typeName.EndsWith("Tests"))
            typeName = typeName.Substring(0, typeName.Length - 5);

        return new(
            directory: Path.Combine(projectDirectory, ".verify", subDir, typeName),
            typeName: "Test",
            methodName: method.Name);
    }
}
