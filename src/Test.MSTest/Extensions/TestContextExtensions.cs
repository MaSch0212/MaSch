namespace MaSch.Test.UnitTests.Extensions;

public static class TestContextExtensions
{
    public static MethodInfo GetTestMethod(this TestContext testContext)
    {
        var type = GetType(testContext.FullyQualifiedTestClassName);
        return type!
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Single(x => GetManagedName(x) == testContext.ManagedMethod);
    }

    private static Type? GetType(string typeName)
    {
        var type = Type.GetType(typeName);
        if (type != null)
            return type;

        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = a.GetType(typeName);
            if (type != null)
                return type;
        }

        return null;
    }

    private static string GetManagedName(MethodInfo m)
    {
        var p = m.GetParameters();
        if (p.Length == 0)
            return m.Name;
        else
            return $"{m.Name}({string.Join(", ", p.Select(x => x.ParameterType.ToString()))})";
    }
}
