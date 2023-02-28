using JpGisConv.Source.Gis.Interface;
using JpGisConv.Work.Model;

namespace JpGisConv.Source.Gis.Factory
{
    /// <summary>
    /// GISの読込処理および、処理用の値の作成処理を扱うファクトリ。
    /// </summary>
    static class GisSourceFactory
    {
        /// <summary>
        /// GISの読込処理および、処理用の値の作成処理。
        /// </summary>
        /// <param name="sourcePath">読込対象のファイルパス</param>
        /// <returns></returns>
        public static RegionStorage Read(
            string sourcePath)
        {
            var repository = CreateRepository(sourcePath);

            try
            {
                var storage = repository.Read();
                return storage.ToWorkMaterial();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// GISの読込処理用のインスタンス生成。
        /// </summary>
        /// <param name="sourcePath">読込対象のファイルパス</param>
        /// <returns></returns>
        private static IGisSourceRepository CreateRepository(
            string sourcePath)
        {
            if (JpGis03_0002_0001.Repository.GisSourceRepository.IsReadable(sourcePath))
            {
                return new JpGis03_0002_0001.Repository.GisSourceRepository(sourcePath);
            }

            throw new FileLoadException();
        }
    }
}

