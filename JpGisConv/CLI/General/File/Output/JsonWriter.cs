using System.Runtime.Serialization.Json;
using System.Text;

namespace JpGisConv.General.File.Export
{
    /// <summary>
    /// JSONデータの出力処理を扱うクラス。
    /// </summary>
    class JsonWriter
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public JsonWriter() { }

        /// <summary>
        /// 出力処理。
        /// </summary>
        /// <param name="outputPath"></param>
        public void Export(
            string outputPath,
            object obj,
            Type type)
        {
            using (var stream = new MemoryStream())
            using (var fs = new FileStream(outputPath, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                var serializer = new DataContractJsonSerializer(type);

                serializer.WriteObject(stream, obj);
                sw.Write(Encoding.UTF8.GetString(stream.ToArray()));
            }
        }
    }
}

