using JpGisConv.General.Extension;
using JpGisConv.Work.Model;
using System.Runtime.Serialization;

namespace JpGisConv.Work.File.Output.InformationJson.Model
{
    /// <summary>
    /// SVG補足用のJSONファイルの構造。（全体）
    /// </summary>
    [DataContract]
    public class Region
    {
        /// <summary>
        /// 都道府県名。
        /// </summary>
        [DataMember]
        public string? PrefectureName { get; set; }

        /// <summary>
        /// 振興局名。
        /// </summary>
        [DataMember]
        public string? SubPrefectureName { get; set; }

        /// <summary>
        /// 郡・政令都市名。
        /// </summary>
        [DataMember]
        public string? CountyName { get; set; }

        /// <summary>
        /// 市区町村名。
        /// </summary>
        [DataMember]
        public string? CityName { get; set; }

        /// <summary>
        /// 町字。
        /// </summary>
        [DataMember]
        public string? TownName { get; set; }

        /// <summary>
        /// 都道府県～市区町村の末端にあたる部分の読み仮名。
        /// </summary>
        [DataMember]
        public string? Yomi { get; set; }

        /// <summary>
        /// 区域のピクセルサイズ。（横幅）
        /// </summary>
        [DataMember]
        public int Width { get; set; }

        /// <summary>
        /// 区域のピクセルサイズ。（縦幅）
        /// </summary>
        [DataMember]
        public int Height { get; set; }

        /// <summary>
        /// 行政区域コード関連情報。
        /// </summary>
        [DataMember]
        public List<Distinct> DistinctList { get; set; } = new();

        /// <summary>
        /// 取込データ全体の中での直交座標の位置情報。
        /// </summary>
        [DataMember]
        public Coordinate CoordinateOnWholeSource { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="ab"></param>
        public Region(
            Work.Model.Region ab)
        {
            // 都道府県～市区町村名
            PrefectureName = ab.Name.PrefectureName.TryConvertEmptyToNull();
            SubPrefectureName = ab.Name.SubPrefectureName.TryConvertEmptyToNull();
            CountyName = ab.Name.CountyName.TryConvertEmptyToNull();
            CityName = ab.Name.CityName.TryConvertEmptyToNull();
            TownName = ab.Name.TownName.TryConvertEmptyToNull();

            // ピクセル基準の縦横幅
            Width = decimal.ToInt16(ab.PixelRectangularPolygonStorage!.Width);
            Height = decimal.ToInt16(ab.PixelRectangularPolygonStorage!.Height);

            // 行政区域単位のデータ
            DistinctList = ab.AreaList
                             .Where(c => !string.IsNullOrEmpty(c.Code))
                             .Select(c => new Distinct(c))
                             .ToList();

            // 端の座標情報
            CoordinateOnWholeSource = new Coordinate(ab.PixelRectangularPolygonStorage!.LeftTopOnParent!);
        }
    }
}