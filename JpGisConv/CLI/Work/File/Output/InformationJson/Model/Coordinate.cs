using JpGisConv.Work.Model;
using System.Runtime.Serialization;

namespace JpGisConv.Work.File.Output.InformationJson.Model
{
    /// <summary>
    /// SVG補足用のJSONファイルの構造。（座標）
    /// </summary>
    [DataContract]
    public class Coordinate
    {
        /// <summary>
        /// 直交座標の左端。
        /// </summary>
        [DataMember]
        public int Left { get; set; }

        /// <summary>
        /// 直交座標の上端。
        /// </summary>
        [DataMember]
        public int Top { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="ab"></param>
        public Coordinate(
            RectangularCoordinate coordinate)
        {
            // 端の座標情報
            Left = decimal.ToInt32(coordinate.X);
            Top = decimal.ToInt32(coordinate.Y);
        }
    }
}
