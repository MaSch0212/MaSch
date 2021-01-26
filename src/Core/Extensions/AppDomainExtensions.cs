#if NETFX
using System;
using System.Xml.Linq;
using System.Linq;

namespace MaSch.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="AppDomain"/> class.
    /// </summary>
    public static class AppDomainExtensions
    {
        public static string[] GetProbingPaths(this AppDomain appDomain)
        {
            var configFile = XElement.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            var probingElement = (
                    from runtime
                    in configFile.Descendants("runtime")
                    from assemblyBinding
                    in runtime.Elements(XName.Get("assemblyBinding", "urn:schemas-microsoft-com:asm.v1"))
                    from probing
                    in assemblyBinding.Elements(XName.Get("probing", "urn:schemas-microsoft-com:asm.v1"))
                    select probing)
                .FirstOrDefault();

            return probingElement?.Attribute("privatePath")?.Value.Split(';');
        }
    }
}
#endif