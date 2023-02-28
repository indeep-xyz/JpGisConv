namespace JpGisConv.Work.Model
{
    /// <summary>
    /// 日本の地区ごとの名称。
    /// 
    /// 地区は、
    /// 　都道府県名、振興局名、群・政令都市名、市区町村名、町字
    /// で一意とする。
    /// </summary>
    public class RegionNameMap
    {
        /// <summary>
        /// 都道府県名。
        /// </summary>
        public string PrefectureName { get; set; } = string.Empty;

        /// <summary>
        /// 振興局名。
        /// </summary>
        public string SubPrefectureName { get; } = string.Empty;

        /// <summary>
        /// 郡・政令都市名。
        /// </summary>
        public string CountyName { get; } = string.Empty;

        /// <summary>
        /// 市区町村名。
        /// </summary>
        public string CityName { get; } = string.Empty;

        /// <summary>
        /// 町字。
        /// </summary>
        public string TownName { get; } = string.Empty;

        /// <summary>
        /// 値をグルーピングするためのキー情報。
        /// </summary>
        public string GroupKey
        {
            get => $"{PrefectureName}_{SubPrefectureName}_{CountyName}_{CityName}_{TownName}";
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="map"></param>
        public RegionNameMap(
            string prefectureName,
            string subPrefectureName,
            string countyName,
            string cityName,
            string townName)
        {
            PrefectureName = prefectureName;
            SubPrefectureName = subPrefectureName;
            CountyName = countyName;
            CityName = cityName;
            TownName = townName;
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="map"></param>
        public RegionNameMap(
            RegionNameMap map)
                :this(
                    map.PrefectureName,
                    map.SubPrefectureName,
                    map.CountyName,
                    map.CityName,
                    map.TownName)
        { }

        ///// <summary>
        ///// 行政区域コードまわりの情報を合成して更新する。
        ///// ※既に保持しているコードにのみ更新を行う。処理後に件数は増減しない
        ///// </summary>
        ///// <param name="sourceAacList">行政区域コードまわりの情報</param>
        ///// <returns></returns>
        //public void UpdateExistenceCodeList(
        //    List<DistrictYomi> sourceAacList)
        //{
        //    // TODO マージする必要ないのでは（仮名側に都道府県～仮名まであるので、抽出だけでよい）
        //    var aycList = new List<DistrictYomi>();

        //    foreach(var ayc in AdministrativeYomiCodeList)
        //    {
        //        aycList.Add(ayc.MergeFrom(sourceAacList));
        //    }

        //    AdministrativeYomiCodeList = aycList;
        //}

        //#endregion 保持データの加工・更新処理
    }
}