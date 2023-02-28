using System.Globalization;
using JpGisConv.Work.File.Output.Csv.Model;

namespace JpGisConv.Work.File.Output.Csv
{
    /// <summary>
    /// CSVデータの出力処理を扱うクラス。
    /// </summary>
    class CsvWriter
    {
        /// <summary>
        /// 出力対象データ。
        /// </summary>
        public List<CsvRow> RowList { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="rowList">出力対象データ</param>
        public CsvWriter(
            List<CsvRow> rowList)
        {
            RowList = rowList;
        }

        /// <summary>
        /// 出力処理。
        /// </summary>
        /// <param name="outputPath"></param>
        public void ExportCsv(
            string outputPath)
        {
            using var sw = new StreamWriter(outputPath, false);
            using var cw = new CsvHelper.CsvWriter(sw, new CultureInfo("ja-JP", false));

            cw.WriteRecords(RowList);
        }

        public static string GetOutputPath(
            string ExportFilePath,
            string ImportFilePath)
        {
            var dirPath = string.IsNullOrWhiteSpace(ExportFilePath)
                ? Path.GetDirectoryName(ImportFilePath)
                : ExportFilePath;

            return dirPath
                 + Path.DirectorySeparatorChar
                 + Path.GetFileNameWithoutExtension(ImportFilePath)
                 + $".csv";
        }
    }
}

