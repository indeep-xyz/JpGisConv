using JpGisConv.Work.Model;

namespace JpGisConv.Source.LandArea.Interface
{
    public interface ILandAreaSourceStorage
    {
        /// <summary>
        /// 共通モデルへの値の設定。
        /// </summary>
        /// <returns></returns>
        public void Attach(RegionStorage administrativeStorage);
    }
}