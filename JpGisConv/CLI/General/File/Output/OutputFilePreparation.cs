namespace JpGisConv.General.File.Output
{
    /// <summary>
    /// 出力処理前のファイル関連処理群。
    /// </summary>
    static class OutputFilePreparation
    {
        /// <summary>
        /// ディレクトリの作成処理。
        /// </summary>
        /// <param name="dirPath"></param>
        public static void TryCreateDir(
            string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
    }
}

