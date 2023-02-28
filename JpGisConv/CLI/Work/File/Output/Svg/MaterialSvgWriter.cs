using JpGisConv.General.File.Export;
using JpGisConv.Work.File.Output.Svg.Model;

namespace JpGisConv.Work.File.Output.Svg
{
    /// <summary>
    /// CSVデータの出力処理を扱うクラス。
    /// </summary>
    class MaterialSvgWriter
    {
        /// <summary>
        /// XMLの名前空間。
        /// </summary>
        public const string XmlNamespace = "http://www.w3.org/2000/svg";

        /// <summary>
        /// 出力対象データ。
        /// </summary>
        public SvgOutputContainer SvgXml { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="rowList">出力対象データ</param>
        public MaterialSvgWriter(
            SvgOutputContainer svgXml)
        {
            SvgXml = svgXml;
        }

        /// <summary>
        /// 出力処理。
        /// </summary>
        /// <param name="outputPath"></param>
        public void Export(
            string outputPath)
        {
            var xmlWriter = new XmlWriter(XmlNamespace);
            xmlWriter.Export(outputPath, SvgXml, typeof(SvgOutputContainer));
        }
    }
}

