using JpGisConv.General.File.Output;
using JpGisConv.Work.File.Output.Csv;
using JpGisConv.Work.File.Output.Csv.Model;
using JpGisConv.Work.File.Output.InformationJson;
using JpGisConv.Work.File.Output.InformationJson.Model;
using JpGisConv.Work.File.Output.Svg;
using JpGisConv.Work.File.Output.Svg.Model;
using JpGisConv.Work.File.Output.SvgPath;
using JpGisConv.Work.File.Output.SvgPath.Model;
using JpGisConv.Work.Model;

namespace JpGisConv.Work.File.Output
{
    /// <summary>
    /// 出力処理の実行処理。
    /// </summary>
    public class OutputRunner
    {
        /// <summary>
        /// 出力モード。
        /// </summary>
        public string OutputMode { get; }
        
        /// <summary>
        /// 出力先のディレクトリパス。
        /// </summary>
        public string OutputDirPath { get; }
        
        /// <summary>
        /// 読込処理対象のGISファイルのパス。
        /// ※GISファイルは本プログラムの中心処理となるため、出力ファイル名の素材にも使われる
        /// </summary>
        public string GisFilePath { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputMode">出力モード</param>
        /// <param name="outputDirPath">出力モード</param>
        /// <param name="gisFilePath">出力モード</param>
        public OutputRunner(
            string outputMode,
            string outputDirPath,
            string gisFilePath)
        {
            OutputMode = outputMode;
            OutputDirPath = string.IsNullOrEmpty(outputDirPath) ? string.Empty : System.IO.Path.GetFullPath(outputDirPath);
            GisFilePath = gisFilePath;
        }

        #region 出力処理

        /// <summary>
        /// 出力処理。
        /// </summary>
        /// <param name="storage"></param>
        public void Output(
            RegionStorage storage)
        {
            switch (OutputMode)
            {
                case General.Constant.OutputMode.Csv:
                    ExportCsv(storage);
                    break;

                case General.Constant.OutputMode.Svg:
                    ExportSvg(storage);
                    ExportInformationJson(storage);
                    break;

                case General.Constant.OutputMode.SvgPath:
                    ExportSvgPath(storage);
                    ExportInformationJson(storage);
                    break;

                case General.Constant.OutputMode.Information:
                    ExportInformationJson(storage);
                    break;
            }
        }

        /// <summary>
        /// CSVファイルの出力処理。
        /// </summary>
        /// <param name="storage">出力用のデータ一式</param>
        private void ExportCsv(
            RegionStorage storage)
        {
            var csvRowList = CsvRow.CreateFrom(storage.RegionList);
            var writer = new CsvWriter(csvRowList);

            writer.ExportCsv(CreateFilePath("csv"));
        }

        /// <summary>
        /// SVGファイルの出力処理。
        /// </summary>
        /// <param name="storage">出力用のデータ一式</param>
        private void ExportSvg(
            RegionStorage storage)
        {
            var dirPath = PrepareDirPath("svg");
            var svgXmlList = SvgOutputContainer.CreateFrom(storage.RegionList);

            foreach (var svgXml in svgXmlList)
            {
                var writer = new MaterialSvgWriter(svgXml);
                writer.Export($"{dirPath}{System.IO.Path.DirectorySeparatorChar}{svgXml.FileName}");
            }
        }

        /// <summary>
        /// SVGパスファイルの出力処理。
        /// </summary>
        /// <param name="storage">出力用のデータ一式</param>
        private void ExportSvgPath(
            RegionStorage storage)
        {
            var dirPath = PrepareDirPath("svgPath");
            var svgPathList = SvgPathOutputContainer.CreateFrom(storage.RegionList);

            foreach (var svgPath in svgPathList)
            {
                var writer = new MaterialSvgPathWriter(svgPath);
                writer.Export($"{dirPath}{System.IO.Path.DirectorySeparatorChar}{svgPath.FileName}");
            }
        }

        /// <summary>
        /// 基礎情報ファイルの出力処理。
        /// </summary>
        /// <param name="storage">出力用のデータ一式</param>
        private void ExportInformationJson(
            RegionStorage storage)
        {
            var dirPath = PrepareDirPath(string.Empty);
            var outputContainer = new InformationJsonOutputContainer(storage);

            var writer = new InformationJsonWriter(outputContainer);
            writer.Export($"{dirPath}{System.IO.Path.DirectorySeparatorChar}info.json");
        }

        #endregion 出力処理
        #region 出力準備処理

        /// <summary>
        /// 出力先のファイルパスを生成する。
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private string CreateFilePath(
            string extension)
        {
            return OutputPathFactory.CreateFilePathOnImportPath(OutputDirPath, GisFilePath, extension);
        }

        /// <summary>
        /// 出力先のディレクトリパスを生成し、ディレクトリ自体も生成する。
        /// </summary>
        /// <param name="suffix">ディレクトリパスの末尾</param>
        /// <returns></returns>
        private string PrepareDirPath(
            string suffix
            )
        {
            var dirPath = $"{OutputPathFactory.CreateDirPathOnImportPath(OutputDirPath, GisFilePath)}{System.IO.Path.DirectorySeparatorChar}{suffix}";
            OutputFilePreparation.TryCreateDir(dirPath);

            return dirPath;
        }

        #endregion 出力準備処理
    }
}