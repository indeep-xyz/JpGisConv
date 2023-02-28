using JpGisConv.Work.Model;
using System.Xml.Serialization;

namespace JpGisConv.Work.File.Output.SvgPath.Model
{
    /// <summary>
    /// SVGのXML中のpathタグ部。
    /// </summary>
    [Serializable]
    public class SvgPathOutputContainer
    {
        /// <summary>
        /// SVG内のパス。
        /// </summary>
        [XmlAttribute("d")]
        public string Draw { get; set; } = string.Empty;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public SvgPathOutputContainer() { }

        /// <summary>
        /// SVGの出力ファイル名。
        /// </summary>
        [XmlIgnore]
        public string FileName { get; } = string.Empty;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="abSource"></param>
        public SvgPathOutputContainer(
            Region ab)
        {
            Draw = string.Join(
                       ' ',
                       ab.PixelRectangularPolygonStorage!.PolygonList.Select(p => CreateDraw(p)));

            FileName = $"{ab.OutputFileName}.svgPath";
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
        public static List<SvgPathOutputContainer> CreateFrom(
            List<Region> administrativeBoundaryList)
        {
            return administrativeBoundaryList
                       .Select(ab => new SvgPathOutputContainer(ab))
                       .ToList();
        }
    }
}