using CsvHelper.Configuration.Attributes;
using JpGisConv.Work.Model;

namespace JpGisConv.Work.File.Output.Csv.Model
{
    /// <summary>
    /// XML文字列中の「属性」を表すデータを取得・管理する。
    /// </summary>
    public class CsvRow 
    {
        /// <summary>
        /// CSV出力時にリスト要素同士を繋ぐ文字列。
        /// </summary>
        const string LIST_DELIMITER = "|";

        /// <summary>
        /// 都道府県名。
        /// </summary>
        [Index(0)]
        [Name("都道府県名")]
        public string PrefectureName { get; } = string.Empty;

        /// <summary>
        /// 振興局名。
        /// </summary>
        [Index(1)]
        [Name("振興局名")]
        public string SubPrefectureName { get; } = string.Empty;

        /// <summary>
        /// 郡・政令都市名。
        /// </summary>
        [Index(2)]
        [Name("郡・政令都市名")]
        public string CountyName { get; } = string.Empty;

        /// <summary>
        /// 市区町村名。
        /// </summary>
        [Index(3)]
        [Name("市区町村名")]
        public string CityName { get; } = string.Empty;

        /// <summary>
        /// 行政区域コード。
        /// </summary>
        [Index(4)]
        [Name("行政区域コード")]
        public string AdministrativeAreaCodeList { get; } = string.Empty;

        /// <summary>
        /// 経緯度基準の面情報。
        /// </summary>
        [Index(5)]
        [Name("経緯度基準の面情報")]
        public string PolygonList { get; } = string.Empty;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="ab"></param>
        public CsvRow(
            Region ab)
        {
            // 都道府県～市区町村名
            PrefectureName = ab.Name.PrefectureName;
            SubPrefectureName = ab.Name.SubPrefectureName;
            CountyName = ab.Name.CountyName;
            CityName = ab.Name.CityName;

            // 行政区域コード
            AdministrativeAreaCodeList = JoinAsString(ab.AreaList.Select(aac => aac.JoinWithAreaAsString()));

            // 面情報
            PolygonList = JoinAsString(ab.GeoPolygonList.Select(gc => gc.JoinGeoCoordinateAsString()));
        }

        /// <summary>
        /// 列挙可能な文字列群を規定の結合文字で結合する。
        /// </summary>
        /// <param name="list">列挙可能な文字列群</param>
        /// <returns></returns>
        private string JoinAsString(
            IEnumerable<string> list)
        {
            return string.Join(LIST_DELIMITER, list);
        }

        public static List<CsvRow> CreateFrom(
            IEnumerable<Region> administrativeBoundaryList)
        {
            return administrativeBoundaryList
                       .Select(ab => new CsvRow(ab))
                       .ToList();
        }
    }
}