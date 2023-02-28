namespace JpGisConv.Work.Model
{
    /// <summary>
    /// 経緯度の座標をもとにした面情報を管理する。
    /// </summary>
    public class GeoPolygon
    {
        /// <summary>
        /// 座標を文字列結合する際の結合文字。
        /// </summary>
        const string COORDINATE_DELIMITER = "_";

        /// <summary>
        /// 順に線を引くと面になる経緯度のリスト。
        /// </summary>
        public List<GeoCoordinate> GeoCoordinateList { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="geoCoordinateList">順に線を引くと面になる経緯度のリスト</param>
        public GeoPolygon(
            IEnumerable<GeoCoordinate> geoCoordinateList)
        {
            GeoCoordinateList = geoCoordinateList.ToList();
        }

        #region 値の変換処理

        /// <summary>
        /// 面情報を規定の結合文字で結合する。
        /// </summary>
        public string JoinGeoCoordinateAsString()
        {
            return string.Join(
                COORDINATE_DELIMITER,
                GeoCoordinateList.Select(gc => gc.JoinAsString()));
        }

        #endregion 値の変換処理
    }
}