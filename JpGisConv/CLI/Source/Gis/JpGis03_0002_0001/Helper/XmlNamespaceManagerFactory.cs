using System.Xml;

namespace JpGisConv.Source.Gis.JpGis03_0002_0001.Helper
{
    /// <summary>
    /// GML形式（JPGIS2014準拠）のXML読み込み用の名前空間マネージャ作成処理。
    /// </summary>
    static class XmlNamespaceManagerFactory
    {
        /// <summary>
        /// 名前空間マネージャ作成処理。
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static XmlNamespaceManager Create(XmlDocument xmlDoc)
        {
            var xmlNamespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            xmlNamespaceManager.AddNamespace("ksj", "http://nlftp.mlit.go.jp/ksj/schemas/ksj-app");
            xmlNamespaceManager.AddNamespace("gml", "http://schemas.opengis.net/gml/3.2.1/gml.xsd");
            xmlNamespaceManager.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
            xmlNamespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            return xmlNamespaceManager;
        }
    }
}