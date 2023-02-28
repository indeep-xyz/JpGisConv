using JpGisConv.Work.File.Output.SvgPath.Model;

namespace JpGisConv.Work.File.Output.SvgPath
{
    /// <summary>
    /// CSVデータの出力処理を扱うクラス。
    /// </summary>
    class MaterialSvgPathWriter
    {
        /// <summary>
        /// 出力対象データ。
        /// </summary>
        public SvgPathOutputContainer SvgPath { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="rowList">出力対象データ</param>
        public MaterialSvgPathWriter(
            SvgPathOutputContainer svgXml)
        {
            SvgPath = svgXml;
        }

        /// <summary>
        /// 出力処理。
        /// </summary>
        /// <param name="outputPath"></param>
        public void Export(
            string outputPath)
        {
            using (var writer = new StreamWriter(outputPath))
            {
                writer.WriteLine(SvgPath.Draw);
            }
        }
    }
}

