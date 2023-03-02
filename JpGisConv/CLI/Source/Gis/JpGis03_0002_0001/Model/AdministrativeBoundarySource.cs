using System.Xml;
using JpGisConv.Source.Gis.JpGis03_0002_0001.Helper;
using JpGisConv.Work.Model;

namespace JpGisConv.Source.Gis.JpGis03_0002_0001.Model
{
    /// <summary>
    /// XML中の「領域の基本情報」を表すデータを取得・管理する。
    /// </summary>
    public class AdministrativeBoundarySource
    {
        /// <summary>
        /// クラスで扱うXML要素タグ名。
        /// </summary>
        public const string TargetElementName = "ksj:AdministrativeBoundary";

        /// <summary>
        /// ID。
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 行政区域の範囲ID。
        /// </summary>
        public string SurfaceId { get; }

        /// <summary>
        /// 区域名称。
        /// </summary>
        public RegionNameMap Name { get; }

        /// <summary>
        /// 行政区域コード。
        /// </summary>
        public string AdministrativeAreaCode { get; }

        /// <summary>
        /// 値をグルーピングするためのキー情報。
        /// </summary>
        public string GroupKey
        {
            get
            {
                return Name.GroupKey;
            }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="xml"></param>
        public AdministrativeBoundarySource(
            string xml)
        {
            var xmlDoc = new XmlDocument();
            var xmlNamespace = XmlNamespaceManagerFactory.Create(xmlDoc);
            xmlDoc.LoadXml(xml);

            Id = ExtractId(xmlDoc, xmlNamespace);
            SurfaceId = ExtractSurfaceId(xmlDoc, xmlNamespace);

            Name = new(ExtractTextValue(xmlDoc, xmlNamespace, "prefectureName"),
                       ExtractTextValue(xmlDoc, xmlNamespace, "subPrefectureName"),
                       ExtractTextValue(xmlDoc, xmlNamespace, "countyName"),
                       ExtractTextValue(xmlDoc, xmlNamespace, "cityName"));

            AdministrativeAreaCode = ExtractTextValue(xmlDoc, xmlNamespace, "administrativeAreaCode");
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
        /// 行政区域の範囲IDを取得する。
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="positionName"></param>
        /// <returns></returns>
        /// <exception cref="FileLoadException"></exception>
        private string ExtractSurfaceId(
            XmlDocument xmlDoc,
            XmlNamespaceManager xmlNamespace)
        {
            const string xpath = $"/{TargetElementName}/ksj:bounds";
            var node = xmlDoc.SelectSingleNode(xpath, xmlNamespace)!;
            var hrefAttribute = node.Attributes?["xlink:href"];

            if (hrefAttribute == null)
            {
                throw new FileLoadException("Wrong XPath.");
            }

            return hrefAttribute.Value.Trim('#');
        }

        /// <summary>
        /// 座標系のデータを取得する。
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="positionName"></param>
        /// <returns></returns>
        /// <exception cref="FileLoadException"></exception>
        private string ExtractTextValue(
            XmlDocument xmlDoc,
            XmlNamespaceManager xmlNamespace,
            string elementName)
        {
            var xpath = $"/{TargetElementName}/ksj:{elementName}";
            var node = xmlDoc.SelectSingleNode(xpath, xmlNamespace)!;

            return node.InnerText;
        }

        /// <summary>
        /// 加工・出力処理で使うための行政区域コード単位の値を作成する。
        /// </summary>
        public static List<RegionArea> ToWorkInstance(
            List<AdministrativeBoundarySource> abSourceList)
        {
            var regionAreaList = new List<RegionArea>();

            // 都道府県～市区町村名でグルーピング
            foreach (var abSource in abSourceList)
            {
                var code = abSource.AdministrativeAreaCode;

                // 結果値への追加（コードの重複不可）
                if (!regionAreaList.Exists(ra => ra.Code == code))
                {
                    regionAreaList.Add(new RegionArea(code, abSource.Name.PrefectureName));
                }
            }

            return regionAreaList;
        }

        /// <summary>
        /// 出力ファイル名で使うための行政区域コードを作成する。
        /// </summary>
        public static string ToOutputFileName(
            List<AdministrativeBoundarySource> abSourceList)
        {
            var codeList = new List<string>();

            // 都道府県～市区町村名でグルーピング
            foreach (var abSource in abSourceList)
            {
                var code = abSource.AdministrativeAreaCode;

                // 結果値への追加（コードの重複不可）
                if (!codeList.Exists(c => c == code))
                {
                    codeList.Add(code);
                }
            }

            return string.Join('_',
                               codeList.OrderBy(c => c).ToArray());
        }
    }
}