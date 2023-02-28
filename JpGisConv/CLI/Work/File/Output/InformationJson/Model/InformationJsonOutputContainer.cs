using JpGisConv.Work.Model;
using System.Runtime.Serialization;

namespace JpGisConv.Work.File.Output.InformationJson.Model
{
    /// <summary>
    /// 基本情報用のJSONファイルの構造。（全体）
    /// </summary>
    [DataContract]
    public class InformationJsonOutputContainer
    {
        /// <summary>
        /// 面積。
        /// </summary>
        [DataMember]
        public decimal WholeArea { get; set; }
        
        /// <summary>
        /// 行政区域別のデータ。
        /// </summary>
        [DataMember]
        public List<Region> AdministrativeBoundaryList { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="ab"></param>
        public InformationJsonOutputContainer(
            RegionStorage storage)
        {
            WholeArea = storage.WholeArea!.Value;
            AdministrativeBoundaryList = storage.RegionList
                                                .Select(ab => new Region(ab))
                                                .ToList();
        }
    }
}