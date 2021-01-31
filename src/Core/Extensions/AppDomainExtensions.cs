#if NETFRAMEWORK
using System;
using System.Linq;
using System.Xml.Linq;

namespace MaSch.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="AppDomain"/> class.
    /// </summary>
    public static class AppDomainExtensions
    {
        /// <summary>
        /// Gets the probing paths of this <see cref="AppDomain"/>.
        /// </summary>
        /// <param name="appDomain">The app domain from which to get the probing paths from.</param>
        /// <returns>The probing paths configured in this <see cref="AppDomain"/>.</returns>
        public static string[] GetProbingPaths(this AppDomain appDomain)
        {
            var configFile = XElement.Load(appDomain.SetupInformation.ConfigurationFile);
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