using JpGisConv.Work.Model;
using System.Xml.Serialization;

namespace JpGisConv.Work.File.Output.Svg.Model
{
    /// <summary>
    /// SVGのXML中のpathタグ部分。
    /// </summary>
    [Serializable]
    public class Path
    {
        /// <summary>
        /// XMLの名前空間。
        /// </summary>
        [XmlAttribute("stroke")]
        public string Stroke { get; set; } = "#000000";

        /// <summary>
        /// XMLの名前空間。
        /// </summary>
        [XmlAttribute("stroke-width")]
        public int StrokeWidth { get; set; } = 1;

        /// <summary>
        /// XMLの名前空間。
        /// </summary>
        [XmlAttribute("fill")]
        public string Fill { get; set; } = "none";

        /// <summary>
        /// SVG内のパス。
        /// </summary>
        [XmlAttribute("d")]
        public string Draw { get; set; } = string.Empty;


        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Path() { }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="abSource"></param>
        public Path(
            IEnumerable<RectangularPolygon> polygonList)
        {
            Draw = string.Join(
                       ' ',
                       polygonList.Select(p => CreateDraw(p)));
        }

        /// <summary>
        /// 座標情報群を繋ぎ、パス用の文字列を生成する。
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        private string CreateDraw(
            RectangularPolygon polygon)
        {
            var draw = new List<string>();

            // 座標Mに移動し、座標Lに線を引いていく
            foreach (var c in polygon.CoordinateList)
            {
                var prefix = draw.Count < 1 ? "M" : "L";
                draw.Add($"{prefix}{c.X},{c.Y}");
            }

            // 文字列結合、
            // および上記ループの最後の座標から、zで開始座標Mに線を引く
            return $"{string.Join(" ", draw)} z";
        }
    }
}