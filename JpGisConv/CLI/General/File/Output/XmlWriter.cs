using System.Text;
using System.Xml.Serialization;

namespace JpGisConv.General.File.Export
{
    /// <summary>
    /// CSVデータの出力処理を扱うクラス。
    /// </summary>
    class XmlWriter
    {
        /// <summary>
        /// XMLの名前空間。
        /// </summary>
        public string XmlNamespace { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="xmlNamespace">XMLの名前空間</param>
        public XmlWriter(
            string xmlNamespace)
        {
            XmlNamespace = xmlNamespace;
        }

        /// <summary>
        /// 出力処理。
        /// </summary>
        /// <param name="outputPath"></param>
        public void Export(
            string outputPath,
            object obj,
            Type type)
        {
            var outputStream = new FileStream(outputPath, FileMode.Create);
            var writer = new StreamWriter(outputStream, Encoding.UTF8);

            var outputNameSpace = new XmlSerializerNamespaces();
            outputNameSpace.Add(string.Empty, XmlNamespace);

            var outputSerializer = new XmlSerializer(type);
            outputSerializer.Serialize(writer, obj, outputNameSpace);

            writer.Flush();
            writer.Close();
        }
    }
}

