using System.Xml;
using JpGisConv.Source.Gis.JpGis03_0002_0001.Helper;
using JpGisConv.Work.Model;

namespace JpGisConv.Source.Gis.JpGis03_0002_0001.Model
{
    /// <summary>
    /// XML中の「図形」を表すデータを取得・管理する。
    /// </summary>
    public class CurveSource
    {
        /// <summary>
        /// クラスで扱うXML要素タグ名。
        /// </summary>
        public const string TargetElementName = "gml:Curve";

        /// <summary>
        /// 図形ID
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 図形を構成する地理座標のリスト。
        /// </summary>
        public List<GeoCoordinate> CoordinateList { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="xml"></param>
        public CurveSource(
            string xml)
        {
            var xmlDoc = new XmlDocument();
            var xmlNamespace = XmlNamespaceManagerFactory.Create(xmlDoc);
            xmlDoc.LoadXml(xml);

            Id = ExtractId(xmlDoc, xmlNamespace);
            CoordinateList = ExtractCoordinateList(xmlDoc, xmlNamespace);
        }

        /// <summary>
        /// IDを取得する。
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="positionName"></param>
        /// <returns></returns>
        /// <exception cref="FileLoadException"></exception>
        private string ExtractId(
            XmlDocument xmlDoc,
            XmlNamespaceManager xmlNamespace)
        {
            const string xpath = $"/{TargetElementName}";
            var node = xmlDoc.SelectSingleNode(xpath, xmlNamespace)!;
            var idAttribute = node.Attributes?["gml:id"];

            if (idAttribute == null)
            {
                throw new FileLoadException("Wrong XPath.");
            }

            return idAttribute.Value;
        }

        /// <summary>
        /// 座標系のデータを取得する。
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="positionName"></param>
        /// <returns></returns>
        /// <exception cref="FileLoadException"></exception>
        private List<GeoCoordinate> ExtractCoordinateList(
            XmlDocument xmlDoc,
            XmlNamespaceManager xmlNamespace)
        {
            const string xpath = $"/{TargetElementName}/gml:segments/gml:LineStringSegment/gml:posList";
            var node = xmlDoc.SelectSingleNode(xpath, xmlNamespace)!;

            return node.InnerText
                       .Trim()
                       .Split('\n')
                       .Select(src => new GeoCoordinate(src.Trim()))
                       .ToList();
        }
    }
}