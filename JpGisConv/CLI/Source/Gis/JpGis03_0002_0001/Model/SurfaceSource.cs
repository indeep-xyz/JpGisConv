using System.Text.RegularExpressions;
using System.Xml;
using JpGisConv.Source.Gis.JpGis03_0002_0001.Helper;

namespace JpGisConv.Source.Gis.JpGis03_0002_0001.Model
{
    /// <summary>
    /// XML中の「図形の紐付け情報」を表すデータを取得・管理する。
    /// </summary>
    public class SurfaceSource
    {
        /// <summary>
        /// クラスで扱うXML要素タグ名。
        /// </summary>
        public const string TargetElementName = "gml:Surface";

        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 図形IDリスト。
        /// </summary>
        public List<string> CurveIdList { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="xml"></param>
        public SurfaceSource(
            string xml)
        {
            var xmlDoc = new XmlDocument();
            var xmlNamespace = XmlNamespaceManagerFactory.Create(xmlDoc);
            xmlDoc.LoadXml(xml);

            Id = ExtractId(xmlDoc, xmlNamespace);
            CurveIdList = ExtractCurveIdList(xmlDoc, xmlNamespace);
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
        private List<string> ExtractCurveIdList(
            XmlDocument xmlDoc,
            XmlNamespaceManager xmlNamespace)
        {
            const string xpath = $"/{TargetElementName}/gml:patches/gml:PolygonPatch";
            var exteriorList = xmlDoc.SelectNodes(xpath, xmlNamespace)![0]!.ChildNodes;
            var idList = new List<string>();
            var hrefRegex = new Regex("^#_?");

            foreach (XmlNode exterior in exteriorList)
            {
                var curveMember = exterior.ChildNodes[0]!.ChildNodes[0]!;
                var hrefAttribute = curveMember?.Attributes?["xlink:href"];

                if (hrefAttribute == null)
                {
                    continue;
                }

                idList.Add(hrefRegex.Replace(hrefAttribute.Value, string.Empty));
            }


            return idList;
        }
    }
}