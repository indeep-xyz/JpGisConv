namespace JpGisConv.Work.Model
{
    /// <summary>
    /// 日本の地区ごとのデータを取得・管理する。
    /// 
    /// 地区は、
    /// 　都道府県名、振興局名、群・政令都市名、市区町村名、町字
    /// で一意とする。
    /// </summary>
    public class Region
    {
        /// <summary>
        /// 領域の名称群。
        /// </summary>
        public RegionNameMap Name { get; set; }

        /// <summary>
        /// 領域の名称群。（かな）
        /// </summary>
        public RegionNameMap NameKana { get; set; }

        /// <summary>
        /// 行政区域ごとの面積。
        /// </summary>
        public List<RegionArea> AreaList { get; private set; } = new();

        /// <summary>
        /// 区域の経緯度基準の面情報。
        /// </summary>
        public List<GeoPolygon> GeoPolygonList { get; } = new();

        /// <summary>
        /// 区域の直交座標基準の面情報。（地球基準）
        /// </summary>
        public GeoRectangularPolygonStorage GeoRectangularPolygonStorage { get; }


        /// <summary>
        /// 区域の直交座標基準の面情報。（ピクセル画像表現用）
        /// </summary>
        public PixelRectangularPolygonStorage? PixelRectangularPolygonStorage { get; private set; }

        /// <summary>
        /// 出力ファイル名。
        /// </summary>
        public string OutputFileName { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="map"></param>
        public Region(
            RegionNameMap map,
            IEnumerable<RegionArea> administrativeAreaCodeList,
            IEnumerable<GeoPolygon> geoPolygonList,
            string outputFileName)
        {
            Name = new(map);
            NameKana = new(map);

            AreaList = administrativeAreaCodeList.ToList();
            GeoPolygonList = geoPolygonList.ToList();

            GeoRectangularPolygonStorage = new(GeoPolygonList);

            OutputFileName = outputFileName;
        }

        #region 直交座標基準の面情報を扱う処理

        /// <summary>
        /// ピクセル直交座標系の座標情報を拡縮する。
        /// </summary>
        /// <param name="scale">拡大率</param>
        /// <param name="mostLeftTopOnSource">ソース内の全ての面情報のうち最も左上の座標</param>
        public void InitializePixelRectangularPolygon(
            decimal scale,
            RectangularCoordinate mostLeftTopOnSource)
        {
            PixelRectangularPolygonStorage = new(GeoRectangularPolygonStorage, scale, mostLeftTopOnSource);
        }

        /// <summary>
        /// 直交座標系の位置情報を上下ひっくり返す。
        /// </summary>
        /// <param name="maxPixelHeight">全区域データ中の最大縦幅</param>
        public void UpsideDownPixelRectangularPolygon(
            decimal maxPixelHeight)
        {
            PixelRectangularPolygonStorage!.UpsideDown(maxPixelHeight);
        }

        #endregion 直交座標基準の面情報を扱う処理
        #region 保持データの加工・更新処理

        /// <summary>
        /// 行政区域コードまわりの情報を合成して更新する。
        /// ※既に保持しているコードにのみ更新を行う。処理後に件数は増減しない
        /// </summary>
        /// <param name="sourceAacList">行政区域コードまわりの情報</param>
        /// <returns></returns>
        public void UpdateExistenceCodeList(
            List<RegionArea> sourceAacList)
        {
            AreaList = AreaList.Select(a => a.MergeFrom(sourceAacList)).ToList();
            OutputFileName = RegionArea.ToOutputFileName(AreaList);
        }

        #endregion 保持データの加工・更新処理
    }
}