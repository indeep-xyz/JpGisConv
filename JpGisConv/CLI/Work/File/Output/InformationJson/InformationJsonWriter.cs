using JpGisConv.General.File.Export;
using JpGisConv.Work.File.Output.InformationJson.Model;

namespace JpGisConv.Work.File.Output.InformationJson
{
    /// <summary>
    /// 基礎情報データの出力処理を扱うクラス。
    /// </summary>
    class InformationJsonWriter
    {
        /// <summary>
        /// 出力対象データ。
        /// </summary>
        public InformationJsonOutputContainer OutputConteiner { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="outputContainer">出力対象データ</param>
        public InformationJsonWriter(
            InformationJsonOutputContainer outputContainer)
        {
            OutputConteiner = outputContainer;
        }

        /// <summary>
        /// 出力処理。
        /// </summary>
        /// <param name="outputPath"></param>
        public void Export(
            string outputPath)
        {
            var xmlWriter = new JsonWriter();
            xmlWriter.Export(outputPath, OutputConteiner, typeof(InformationJsonOutputContainer));
        }
    }
}

