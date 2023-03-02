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
        /// 値をグルーピングするためのキー情報。
        /// </summary>
        public string GroupKey
        {
            get => $"{PrefectureName}_{SubPrefectureName}_{CountyName}_{CityName}";
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="map"></param>
        public RegionNameMap(
            string prefectureName,
            string subPrefectureName,
            string countyName,
            string cityName)
        {
            PrefectureName = prefectureName;
            SubPrefectureName = subPrefectureName;
            CountyName = countyName;
            CityName = cityName;
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
                    map.CityName)
        { }
    }
}