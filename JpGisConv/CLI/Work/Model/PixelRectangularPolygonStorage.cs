namespace JpGisConv.Work.Model
{
    /// <summary>
    /// 区域ごとのポリゴンデータ。（ピクセル座標用）
    /// </summary>
    public class PixelRectangularPolygonStorage
    {
        /// <summary>
        /// 行政区域の直交座標基準の面情報。
        /// </summary>
        public List<RectangularPolygon> PolygonList { get; private set; } = new();

        /// <summary>
        /// 図形の縦幅。
        /// </summary>
        public decimal Height
        {
            get => PolygonList.SelectMany(p => p.CoordinateList).Max(rc => rc.Y) -
                   PolygonList.SelectMany(p => p.CoordinateList).Min(rc => rc.Y);
        }

        /// <summary>
        /// 図形の横幅。
        /// </summary>
        public decimal Width
        {
            get => PolygonList.SelectMany(p => p.CoordinateList).Max(rc => rc.X) -
                   PolygonList.SelectMany(p => p.CoordinateList).Min(rc => rc.X);
        }

        /// <summary>
        /// 親要素基準での直交座標観点の左上の頂点座標。
        /// </summary>
        public RectangularCoordinate LeftTopOnParent { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="geoPolygonStorage">地球座標基準の面情報</param>
        /// <param name="scale">変換倍率</param>
        /// <param name="mostLeftTopOnSource">ソース内の全ての面情報のうち最も左上の座標</param>
        public PixelRectangularPolygonStorage(
            GeoRectangularPolygonStorage geoPolygonStorage,
            decimal scale,
            RectangularCoordinate mostLeftTopOnSource)
        {
            // 親要素から見た図形の左上位置を設定する
            LeftTopOnParent = geoPolygonStorage.LeftTopOnParent
                                               .Move(mostLeftTopOnSource.Negate())
                                               .ScaleWithInt(scale);

            // 図形の設定
            PolygonList = CreatePolygon(geoPolygonStorage, scale);
        }

        private static List<RectangularPolygon> CreatePolygon(
            GeoRectangularPolygonStorage geoPolygonStorage,
            decimal scale)
        {
            // 自身の座標に従って左上寄せした面情報を保持する
            var polygonList = geoPolygonStorage.PolygonList.Select(p => p.MoveToLeftTop(geoPolygonStorage.LeftTopOnParent)).ToList();

            foreach (var p in polygonList)
            {
                p.ScaleCoordinateList(scale);
            }

            return RectangularPolygon.Minimize(polygonList)
                                     .ToList();
        }

        #region 直交座標基準の面情報を扱う処理

        public void MoveToLeftTop()
        {
            var edgeRect = RectangularCoordinate.CreateEdgeRect(PolygonList);

            // 自身の座標に従って左上寄せした面情報を保持する
            PolygonList = PolygonList.Select(p => p.MoveToLeftTop(edgeRect.LeftTop)).ToList();
        }

        /// <summary>
        /// 直交座標系の位置情報を上下ひっくり返す。
        /// </summary>
        /// <param name="sourceHeight">座標が使われる領域全体の縦幅</param>
        public void UpsideDown(
            decimal sourceHeight)
        {
            PolygonList = PolygonList.Select(p => p.UpsideDownCoordinate(sourceHeight))
                                     .ToList();

            LeftTopOnParent = new(LeftTopOnParent!.X, sourceHeight - Height - LeftTopOnParent!.Y);
        }

        #endregion 直交座標基準の面情報を扱う処理
    }
}