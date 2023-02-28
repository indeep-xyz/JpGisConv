namespace JpGisConv.Source.LandArea.Interface
{
    public interface ILandAreaSourceRepository
    {
        /// <summary>
        /// ファイルからデータを読み込む。
        /// </summary>
        public ILandAreaSourceStorage Read();
    }
}