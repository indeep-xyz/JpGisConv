namespace JpGisConv.Work.Model
{
    /// <summary>
    /// 経緯度座標で構成された面情報を管理する。
    /// </summary>
    public class GeoCoordinate
    {
        #region 定数

        /// <summary>
        /// 座標を文字列結合する際の結合文字。
        /// </summary>
        const string COORDINATE_DELIMITER = "-";

        #endregion 定数
        #region フィールド、プロパティ

        /// <summary>
        /// 緯度。
        /// </summary>
        public decimal Latitude { get; }

        /// <summary>
        /// 経度。
        /// </summary>
        public decimal Longitude { get; }

        #endregion フィールド、プロパティ
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="latitude">緯度</param>
        /// <param name="longitude">経度</param>
        public GeoCoordinate(
            decimal latitude,
            decimal longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="coordinateSource">経緯度を半角空白で区切った文字列</param>
        public GeoCoordinate(
            string coordinateSource)
        {
            var p = coordinateSource.Split(' ');

            Latitude = decimal.Parse(p[0]);
            Longitude = decimal.Parse(p[1]);
        }

        #endregion コンストラクタ
        #region 値の変換処理

        /// <summary>
        /// double型の値に変換した座標を返却する。
        /// </summary>
        public double[] ToDoubleArrayCoordinate()
        {
            return new double[] {
                decimal.ToDouble(Longitude),
                decimal.ToDouble(Latitude)
            };
        }

        /// <summary>
        /// 経緯度を規定の結合文字で結合する。
        /// </summary>
        /// <returns></returns>
        public string JoinAsString()
        {
            return $"{Latitude}{COORDINATE_DELIMITER}{Longitude}";
        }

        #endregion 値の変換処理
        #region 比較処理

        /// <summary>
        /// 等価比較処理。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not GeoCoordinate)
            {
                return false;
            }

            var another = (GeoCoordinate)obj;

            return Latitude == another.Latitude
                && Longitude == another.Longitude;
        }

        /// <summary>
        /// ハッシュ値の生成。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Tuple.Create(Latitude, Longitude).GetHashCode();
        }

        #endregion 比較処理
    }
}