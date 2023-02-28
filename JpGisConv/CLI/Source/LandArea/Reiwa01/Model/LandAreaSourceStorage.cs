using JpGisConv.Source.LandArea.Interface;
using JpGisConv.Work.Model;
using System.Text;

namespace JpGisConv.Source.LandArea.Reiwa01.Model
{
    /// <summary>
    /// 面積情報の取込データを保持・変換する。
    /// </summary>
    public class LandAreaSourceStorage : ILandAreaSourceStorage
    {
        #region フィールド、プロパティ

        /// <summary>
        /// 全国面積。
        /// </summary>
        public decimal? WholeCountryArea { get; private set; }

        public List<LandAreaSource> LandAreaSourceList { get; } = new();

        #endregion フィールド、プロパティ
        #region コンストラクタ、初期化処理

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="wholeCountryArea"></param>
        public LandAreaSourceStorage(
            decimal wholeCountryArea)
        {
            WholeCountryArea = wholeCountryArea;
        }

        /// <summary>
        /// 領域単位の面積情報データの追加
        /// </summary>
        /// <param name="landAreaSource"></param>
        public void Add(
            LandAreaSource landAreaSource)
        {
            LandAreaSourceList.Add(landAreaSource);
        }

        #endregion コンストラクタ、初期化処理
        #region 処理用の共通モデルへの割当処理

        /// <summary>
        /// 全区域のデータに、保持している面積情報を割り当てる。
        /// </summary>
        /// <param name="administrativeStorage">全行政区域のデータ</param>
        public void Attach(RegionStorage administrativeStorage)
        {
            administrativeStorage.WholeArea = WholeCountryArea;
            administrativeStorage.UpdateRegionList(ToDistrict());
        }

        /// <summary>
        /// 保持しているデータを、処理用の区域コード単位データに変換する。
        /// </summary>
        /// <returns></returns>
        private List<RegionArea> ToDistrict()
        {
            return LandAreaSourceList
                       .Select(las => new RegionArea(las.GisCode, las.PrefectureName, las.Area))
                       .ToList();
        }

        #endregion 処理用の共通モデルへの割当処理
    }
}
