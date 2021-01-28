using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace MaSch.Presentation.Update
{
    /// <summary>
    /// Provides methods to load xml files.
    /// </summary>
    internal static class XmlLoader
    {
        /// <summary>
        /// Downloads an XML file from an endpoint an deserializes it.
        /// </summary>
        /// <typeparam name="T">The type in which the xml should be deserialized to.</typeparam>
        /// <param name="uri">The endpoint uri to download the xml from.</param>
        /// <param name="webClient">The web client to use.</param>
        /// <returns>The deserialized xml from the defined endoint.</returns>
        internal static T DownloadXml<T>(Uri uri, WebClient webClient = null)
        {
            if (webClient == null)
                webClient = new WebClient();
            var xml = webClient.DownloadString(uri);
            var reader = new StringReader(xml);
            var ser = new XmlSerializer(typeof(T));
            return (T)ser.Deserialize(reader);
        }
    }
}
