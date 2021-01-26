using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace MaSch.Presentation.Update
{
    internal static class XmlLoader
    {
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
