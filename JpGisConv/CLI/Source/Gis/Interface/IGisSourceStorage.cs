using JpGisConv.Work.Model;

namespace JpGisConv.Source.Gis.Interface
{
    public interface IGisSourceStorage
    {
        /// <summary>
        /// 共通モデルへの値の設定。
        /// </summary>
        /// <returns></returns>
        public RegionStorage ToWorkMaterial();
    }
}