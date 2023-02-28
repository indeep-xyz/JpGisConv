namespace JpGisConv.Work.Model
{
    /// <summary>
    /// 区域ごとのポリゴンデータ。（地球座標用）
    /// </summary>
    public class GeoRectangularPolygonStorage
    {
        /// <summary>
        /// 面情報。
        /// 経緯度情報の変換後そのままの状態のため、座標が負数の可能性あり。
        /// </summary>
        public List<RectangularPolygon> PolygonList { get; }

        /// <summary>
        /// 図形の縦幅。
        /// </decimaly>
        public decimal RectangularHeight
        {
            get => PolygonList.SelectMany(p => p.CoordinateList).Max(rc => rc.Y) -
                   PolygonList.SelectMany(p => p.CoordinateList).Min(rc => rc.Y);
        }

        /// <summary>
        /// 図形の横幅。
        /// </summary>
        public decimal RectangularWidth
        {
            get => PolygonList.SelectMany(p => p.CoordinateList).Max(rc => rc.X) -
                   PolygonList.SelectMany(p => p.CoordinateList).Min(rc => rc.X);
        }

        /// <summary>
        /// 親要素基準での左上の頂点座標。
        /// </summary>
        public RectangularCoordinate LeftTopOnParent { get;  }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="geoPolygonList">経緯度基準の面情報</param>
        public GeoRectangularPolygonStorage(
            IEnumerable<GeoPolygon> geoPolygonList)
        {
            // 経緯度情報から直交座標に変換する
            PolygonList = geoPolygonList.Select(gp => new RectangularPolygon(gp.GeoCoordinateList)).ToList();

            // 自身の座標に従って座標を設定する
            LeftTopOnParent = RectangularCoordinate.CreateEdgeRect(PolygonList).LeftTop;
        }
    }
}