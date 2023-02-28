using JpGisConv.Work.Model;
using System.Runtime.Serialization;

namespace JpGisConv.Work.File.Output.InformationJson.Model
{
    /// <summary>
    /// SVG補足用のJSONファイルの構造。（行政区域単位のデータまわり）
    /// </summary>
    [DataContract]
    public class Distinct
    {
        /// <summary>
        /// 行政区域コード。
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// 面積。
        /// </summary>
        [DataMember]
        public decimal? Area { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="ab"></param>
        public Distinct(
            RegionArea abc)
        {
            Code = abc.Code;
            Area = abc.Area;
        }
    }
}