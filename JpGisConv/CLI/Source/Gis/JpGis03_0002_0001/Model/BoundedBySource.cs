using System.Xml;
using JpGisConv.Source.Gis.JpGis03_0002_0001.Helper;
using JpGisConv.Work.Model;

namespace JpGisConv.Source.Gis.JpGis03_0002_0001.Model
{
    /// <summary>
    /// XML中の「データ提供範囲」を表すデータを取得・管理する。
    /// </summary>
    public class BoundedBySource
    {
        /// <summary>
        /// クラスで扱うXML要素タグ名。
        /// </summary>
        public const string TargetElementName = "gml:boundedBy";

        /// <summary>
        /// XML中の下限となる左端、上端の座標。
        /// </summary>
        public GeoCoordinate LowerCorner { get; }

        /// <summary>
        /// XML中の上限となる右端、下端の座標。
        /// </summary>
        public GeoCoordinate UpperCorner { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="xml"></param>
        public BoundedBySource(
            string xml)
        {
            var xmlDoc = new XmlDocument();
            var xmlNamespace = XmlNamespaceManagerFactory.Create(xmlDoc);
            xmlDoc.LoadXml(xml);

            LowerCorner = ExtractCoordinate(xmlDoc, xmlNamespace, "lowerCorner");
            UpperCorner = ExtractCoordinate(xmlDoc, xmlNamespace, "upperCorner");
        }

        /// <summary>
        /// 座標系のデータを取得する。
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="coordinateName"></param>
        /// <returns></returns>
        /// <exception cref="FileLoadException"></exception>
        private GeoCoordinate ExtractCoordinate(
            XmlDocument xmlDoc,
            XmlNamespaceManager xmlNamespace,
            string coordinateName)
        {
            var xpath = $"/{TargetElementName}/gml:EnvelopeWithTimePeriod/gml:{coordinateName}";
            var node = xmlDoc.SelectSingleNode(xpath, xmlNamespace)!;

            return new GeoCoordinate(node.InnerText);
        }
    }
}