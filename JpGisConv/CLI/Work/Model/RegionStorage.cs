namespace JpGisConv.Work.Model
{
    /// <summary>
    /// 複数の区域データを管理する。
    /// </summary>
    public class RegionStorage
    {
        #region フィールド、プロパティ

        /// <summary>
        /// 取扱い中の素材データにおける左上端の経緯度座標。
        /// </summary>
        public GeoCoordinate LeftTopCorner { get; }

        /// <summary>
        /// 取扱い中の素材データにおける右下端の経緯度座標。
        /// </summary>
        public GeoCoordinate RightBottomCorner { get; }

        /// <summary>
        /// 取扱い中の素材データにおける全体面積。
        /// </summary>
        public decimal? WholeArea { get; set; }

        /// <summary>
        /// 区域データ。
        /// </summary>
        public List<Region> RegionList { get; }

        #endregion フィールド、プロパティ
        #region コンストラクタ、初期化処理

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="leftTopCorner">左上端の経緯度座標</param>
        /// <param name="rightBottomCorner">右下端の経緯度座標</param>
        /// <param name="administrativeBoundaryList">行政区域データ</param>
        public RegionStorage(
            GeoCoordinate leftTopCorner,
            GeoCoordinate rightBottomCorner,
            List<Region> administrativeBoundaryList
        )
        {
            LeftTopCorner = leftTopCorner;
            RightBottomCorner = rightBottomCorner;
            RegionList = administrativeBoundaryList;
        }

        #endregion コンストラクタ、初期化処理
        #region 保持データの加工・更新処理

        /// <summary>
        /// 全行政区域の面情報を指定幅に収めるため、サイズを整える。
        /// </summary>
        /// <param name="afterMaxWidth">調整後の最大幅</param>
        public void InitializePixelRectangularCoordinateCoordinate(
            int afterMaxWidth)
        {
            decimal scale = CalclateScale(afterMaxWidth);

            var mostLeftTopOnTheEarth = new RectangularCoordinate(RegionList.Min(ab => ab.GeoRectangularPolygonStorage.LeftTopOnParent.X),
                                                                  RegionList.Min(ab => ab.GeoRectangularPolygonStorage.LeftTopOnParent.Y));

            foreach (var ab in RegionList)
            {
                ab.InitializePixelRectangularPolygon(scale, mostLeftTopOnTheEarth);
            }

            UpdateCoordinateOnWholeSource();
        }

        /// <summary>
        /// 読込データ全体での図形ごとの相対座標を更新する。
        /// </summary>
        private void UpdateCoordinateOnWholeSource()
        {
            // 保持している全区域データ中の最大縦幅。
            var maxPixelHeight = RegionList.Max(ab => ab.PixelRectangularPolygonStorage!.Height);

            foreach (var ab in RegionList){
                ab.UpsideDownPixelRectangularPolygon(maxPixelHeight);
                ab.PixelRectangularPolygonStorage!.MoveToLeftTop();
            }
        }

        /// <summary>
        /// 全区域の面情報を指定幅に収めるための拡大率を計算する。
        /// </summary>
        /// <param name="afterMaxWidth">調整後の最大幅</param>
        /// <returns></returns>
        private decimal CalclateScale(
            long afterMaxWidth)
        {
            // 取り扱う全区域内の最大の幅と高さを取得
            var geoMaxWidth = RegionList.Max(ab => ab.GeoRectangularPolygonStorage.RectangularWidth);
            var getMaxHeight = RegionList.Max(ab => ab.GeoRectangularPolygonStorage.RectangularHeight);

            // 横幅と縦幅のうち、大きい方を基準に拡大率を計算する
            return geoMaxWidth > getMaxHeight
                ? decimal.Divide(afterMaxWidth, geoMaxWidth)
                : decimal.Divide(afterMaxWidth, getMaxHeight);
        }

        /// <summary>
        /// 全区域に対し、コードまわりの情報を合成して更新する。
        /// ※既に保持しているコードにのみ更新を行う。処理後に件数は増減しない
        /// </summary>
        /// <param name="codeList">行政区域コードまわりの情報</param>
        /// <returns></returns>
        public void UpdateRegionList(
            List<RegionArea> codeList)
        {
            foreach(var ab in RegionList)
            {
                ab.UpdateExistenceCodeList(codeList);
            }
        }

        #endregion 保持データの加工・更新処理
    }
}