namespace JpGisConv.Source.LandArea.Reiwa01.Model
{
    /// <summary>
    /// 標準地域ごとのデータを取得・管理する。
    /// </summary>
    public class LandAreaSource
    {
        /// <summary>
        /// 標準地域コード。
        /// ※GISの行政区域コードとほぼ同一
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// 標準地域コード。
        /// ※GISの行政区域コードと同じ形式に変換して出力する
        /// </summary>
        public string GisCode
        {
            get => int.Parse(Code).ToString("00000"); 
        }

        /// <summary>
        /// 都道府県名。
        /// ※GISのデータでは、都道府県全体のデータに行政区域コードがないため保持
        /// </summary>
        public string PrefectureName { get; }

        /// <summary>
        /// 面積。（平方キロメートル）
        /// </summary>
        public decimal Area { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="csvRow"></param>
        public LandAreaSource(
            string[] csvRow)
        {
            Code = csvRow[0];
            PrefectureName = csvRow[1];
            Area = decimal.Parse(csvRow[4]);
        }
    }
}