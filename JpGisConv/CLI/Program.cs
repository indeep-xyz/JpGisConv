using JpGisConv.Source.Gis.Factory;
using JpGisConv.Source.LandArea.Factory;
using JpGisConv.Work.File.Output;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace JpGisConv.CLI
{
    public class Program
    {
        #region 起動時オプション

        /// <summary>
        /// 変換対象「GIS」ファイルのパス。
        /// </summary>
        [Required]
        [FileExists]
        [Option(
            Template = "--gis <FILE>",
            Description = "必須。変換対象となる「GIS」ファイルのパス。",
            ShortName = "g"
        )]
        public string GisFilePath { get; } = string.Empty;

        /// <summary>
        /// 変換対象「面積情報」ファイルのパス。
        /// </summary>
        [Required]
        [FileExists]
        [Option(
            Template = "--landarea <FILE>",
            Description = "変換対象となる「面積情報」ファイルのパス。",
            ShortName = "l"
        )]
        public string LandAreaFilePath { get; } = string.Empty;

        /// <summary>
        /// 変換結果の形式。
        /// </summary>
        [AllowedValues(General.Constant.OutputMode.Csv, General.Constant.OutputMode.Svg, General.Constant.OutputMode.SvgPath, General.Constant.OutputMode.Information)]
        [Option(
            Template = $"--mode <EXPORT_TYPE>",
            Description = "変換結果の形式。",
            ShortName = "m"
        )]
        public string OutputMode { get; } = General.Constant.OutputMode.Csv;

        /// <summary>
        /// 変換結果の出力先ディレクトリパス。
        /// </summary>
        [Option(
            Template = "--output <FILE>",
            Description = "変換結果の出力先パス。省略時は変換対象ファイルと同フォルダに展開。",
            ShortName = "o"
        )]
        public string OutputDirPath { get; } = string.Empty;

        /// <summary>
        /// 変換結果のピクセル最大幅。
        /// </summary>
        [Option(
            Template = "--max-width <PIXEL_SIZE>",
            Description = "変換結果のピクセル最大幅。",
            ShortName = "w"
        )]
        public int PixelMaxWidth { get; } = 2048;

        #endregion 起動時オプション
        #region メイン処理

        public static int Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        /// <summary>
        /// McMaster.Extensions.CommandLineUtils 利用時のメイン関数。
        /// </summary>
        /// <see cref="McMaster.Extensions.CommandLineUtils"/>
        /// <exception cref="ApplicationException"></exception>
        [SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "CommandLineApplication から読み出す実質のメイン関数")]
        private void OnExecute()
        {
            // 文字コードの登録
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // GISデータの取得・処理用の値の作成
            var gisStorage = GisSourceFactory.Read(GisFilePath);
            gisStorage.InitializePixelRectangularCoordinateCoordinate(PixelMaxWidth);

            // 面積データの取得・反映
            LandAreaSourceFactory.ReadAndAttach(LandAreaFilePath, gisStorage);

            // 出力処理
            var outputRunner = new OutputRunner(OutputMode, OutputDirPath, GisFilePath);
            outputRunner.Output(gisStorage);
        }

        #endregion メイン処理
    }
}