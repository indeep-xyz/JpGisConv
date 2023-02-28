using JpGisConv.Source.Gis.JpGis03_0002_0001.Model;

namespace JpGisConv.Work.Model
{
    /// <summary>
    /// 区域コードごとの面積データを取得・管理する。
    /// 
    /// 行政区域コードは、
    /// 　日本の場合：　「都道府県名」「振興局名」「群・政令都市名」「市区町村名」で一意にならない。
    /// </summary>
    public class RegionArea
    {
        /// <summary>
        /// 値を文字列結合する際の結合文字。
        /// </summary>
        const string VALUE_DELIMITER = "_";

        /// <summary>
        /// 行政的な区域を表すコード。
        /// ※5桁0埋め、または空文字が入る想定
        /// </summary>
        public string Code { get; } = string.Empty;

        /// <summary>
        /// 都道府県名。
        /// </summary>
        public string PrefectureName { get; } = string.Empty;

        /// <summary>
        /// 行政区域の経緯度基準の面情報。
        /// </summary>
        public decimal? Area { get; }

        /// <summary>
        /// コンストラクタ。
        /// ※汎用
        /// </summary>
        /// <param name="code">行政区域コード</param>
        /// <param name="prefectureName">都道府県名</param>
        /// <param name="area">面積</param>
        public RegionArea(
            string code,
            string prefectureName,
            decimal area)
        {
            Code = code;
            PrefectureName = prefectureName;
            Area = area;
        }

        /// <summary>
        /// コンストラクタ。
        /// ※GISデータ用
        /// </summary>
        /// <param name="code">行政区域コード</param>
        /// <param name="prefectureName">都道府県名</param>
        public RegionArea(
            string code,
            string prefectureName)
        {
            Code = code;
            PrefectureName = prefectureName;
        }

        /// <summary>
        /// 他インスタンスを合成する。
        /// 
        /// ※自身はGISデータを取り込んだ直後のデータを想定している
        /// 　　・面積をもたない
        /// 　　・都道府県単位のデータに行政地域コードがない
        /// </summary>
        /// <param name="sourceAacList"></param>
        /// <returns></returns>
        public RegionArea MergeFrom(
            List<RegionArea> sourceAacList)
        {
            foreach (var sourceAac in sourceAacList)
            {
                if (Code == sourceAac.Code

                    // 都道府県単位のマージ
                    || (string.IsNullOrEmpty(Code)
                        && PrefectureName == sourceAac.PrefectureName))
                {
                    return new RegionArea(sourceAac.Code, PrefectureName, sourceAac.Area!.Value);
                }
            }

            return this;
        }

        /// <summary>
        /// 保持情報を規定の結合文字で結合する。
        /// </summary>
        public string JoinWithAreaAsString()
        {
            return $"{Code}{VALUE_DELIMITER}{Area}";
        }

        /// <summary>
        /// 出力ファイル名で使うための行政区域コードを作成する。
        /// </summary>
        public static string ToOutputFileName(
            List<RegionArea> areaList)
        {
            return string.Join('_',
                               areaList.Select(a => a.Code)
                                       .OrderBy(c => c)
                                       .ToArray());
        }
    }
}