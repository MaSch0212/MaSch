using System.Reflection;

namespace MaSch.Presentation.Update
{
    public enum AssemblyVersionType
    {
        AssemblyVersion,
        AssemblyFileVersion,
        AssemblyInformationalVersion
    }

    public static class AssemblyVersionTypeExtensions
    {
        public static string GetVersion(this AssemblyVersionType type, Assembly assembly)
        {
            return type switch
            {
                AssemblyVersionType.AssemblyFileVersion => assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version,
                AssemblyVersionType.AssemblyInformationalVersion => assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion,
                _ => assembly.GetName().Version.ToString(),
            };
        }

        public static string GetVersion(this AssemblyVersionType type)
        {
            return GetVersion(type, Assembly.GetEntryAssembly());
        }
    }
}
