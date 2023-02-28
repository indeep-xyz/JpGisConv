using JpGisConv.Source.LandArea.Interface;
using JpGisConv.Work.Model;

namespace JpGisConv.Source.LandArea.Factory
{
    /// <summary>
    /// 面積情報の読込処理および、処理用の値の作成処理を扱うファクトリ。
    /// </summary>
    static class LandAreaSourceFactory
    {
        /// <summary>
        /// 面積情報の読込処理および、処理用の値の作成処理。
        /// </summary>
        /// <param name="sourcePath">読込対象のファイルパス</param>
        /// <param name="administrativeStorage">読み込んだ値を割り当てる先の共通処理用インスタンス</param>
        /// <returns></returns>
        public static void ReadAndAttach(
            string sourceFilePath,
            RegionStorage administrativeStorage)
        {
            var repository = CreateStorage(sourceFilePath);

            try
            {
                var storage = repository.Read();
                storage.Attach(administrativeStorage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 面積情報の読込処理用のインスタンス生成。
        /// </summary>
        /// <param name="sourcePath">読込対象のファイルパス</param>
        /// <returns></returns>
        private static ILandAreaSourceRepository CreateStorage(
            string sourceFilePath)
        {
            if (Reiwa01.Repository.LandAreaSourceRepository.IsReadable(sourceFilePath))
            {
                return new Reiwa01.Repository.LandAreaSourceRepository(sourceFilePath);
            }

            throw new FileLoadException();
        }
    }
}

