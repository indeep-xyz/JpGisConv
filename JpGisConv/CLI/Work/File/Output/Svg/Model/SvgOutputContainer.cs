using JpGisConv.Work.Model;
using System.Xml.Serialization;

namespace JpGisConv.Work.File.Output.Svg.Model
{
    /// <summary>
    /// SVGのXML中の全体部分。
    /// </summary>
    [Serializable]
    [XmlRoot("svg", Namespace = "http://www.w3.org/2000/svg")]
    public class SvgOutputContainer
    {
        /// <summary>
        /// SVG全体の横幅。
        /// </summary>
        [XmlAttribute("width")]
        public int Width { get; set; }

        /// <summary>
        /// SVG全体の縦幅。
        /// </summary>
        [XmlAttribute("height")]
        public int Height { get; set; }

        /// <summary>
        /// pathタグの情報。
        /// </summary>
        [XmlElement("path")]
        public Path? Path { get; set; }

        /// <summary>
        /// SVGの出力ファイル名。
        /// </summary>
        [XmlIgnore]
        public string FileName { get; } = string.Empty;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public SvgOutputContainer() { }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="abSource"></param>
        public SvgOutputContainer(
            Region ab)
        {
            Width = decimal.ToInt16(ab.PixelRectangularPolygonStorage!.Width);
            Height = decimal.ToInt16(ab.PixelRectangularPolygonStorage.Height);
            Path = new Path(ab.PixelRectangularPolygonStorage.PolygonList);
            FileName = $"{ab.OutputFileName}.svg";
        }

        public static List<SvgOutputContainer> CreateFrom(
            List<Region> administrativeBoundaryList)
        {
            return administrativeBoundaryList
                       .Select(ab => new SvgOutputContainer(ab))
                       .ToList();
        }
    }
}