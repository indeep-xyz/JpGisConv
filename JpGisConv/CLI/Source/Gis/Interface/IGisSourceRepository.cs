namespace JpGisConv.Source.Gis.Interface
{
    public interface IGisSourceRepository
    {
        /// <summary>
        /// ファイルからデータを読み込む。
        /// </summary>
        public IGisSourceStorage Read();
    }
}