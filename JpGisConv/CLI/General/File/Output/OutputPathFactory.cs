namespace JpGisConv.General.File.Output
{
    /// <summary>
    /// 出力先パスの生成処理群。
    /// </summary>
    static class OutputPathFactory
    {
        /// <summary>
        /// 取込元ファイルを使ってファイルパスを生成する。
        /// </summary>
        /// <param name="exportDirPath">出力先ディレクトリパス。ない場合は取込元ファイルのディレクトリパスを用いる</param>
        /// <param name="importFilePath">取込元ファイルパス</param>
        /// <param name="extension">ファイルの拡張子</param>
        /// <returns></returns>
        public static string CreateFilePathOnImportPath(
            string exportDirPath,
            string importFilePath,
            string extension)
        {
            var dirPath = CreateDirPathOnImportPath(exportDirPath, importFilePath);

            return dirPath
                 + Path.DirectorySeparatorChar
                 + Path.GetFileNameWithoutExtension(importFilePath)
                 + $".{extension}";
        }

        /// <summary>
        /// 取込元ファイルを使ってファイルパスを生成する。
        /// </summary>
        /// <param name="exportDirPath">出力先ディレクトリパス。ない場合は取込元ファイルのディレクトリパスを用いる</param>
        /// <param name="importFilePath">取込元ファイルパス</param>
        /// <param name="extension">ファイルの拡張子</param>
        /// <returns></returns>
        public static string CreateDirPathOnImportPath(
            string exportDirPath,
            string importFilePath)
        {
            if (string.IsNullOrWhiteSpace(exportDirPath))
            {
                var dir = Path.GetDirectoryName(importFilePath);

                if (string.IsNullOrEmpty(dir))
                {
                    throw new ArgumentException();
                }

                return dir;
            }

            return exportDirPath;
        }
    }
}

