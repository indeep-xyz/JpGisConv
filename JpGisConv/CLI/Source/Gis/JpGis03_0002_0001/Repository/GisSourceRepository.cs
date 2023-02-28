using JpGisConv.Source.Gis.Interface;
using JpGisConv.Source.Gis.JpGis03_0002_0001.Model;
using System.Xml;

namespace JpGisConv.Source.Gis.JpGis03_0002_0001.Repository
{
    /// <summary>
    /// XML中の値を読み込む。
    /// 
    /// JPGIS2014準拠のGML形式 (OpenGIS 3.2.1, 座標系JGD2011) が対象
    /// https://nlftp.mlit.go.jp/ksj/gml/datalist/KsjTmplt-N03-v3_1.html
    /// </summary>
    public class GisSourceRepository : IGisSourceRepository
    {
        /// <summary>
        /// 読み込み可否判定に用いる最初の要素タグ名。（KSJ = 国土数値情報）
        /// </summary>
        const string FirstElementName = "ksj:Dataset";

        /// <summary>
        /// 読み込み可否判定に用いる最初の要素タグに含まれる属性名。（GML = Geography Markup Language）
        /// </summary>
        const string FirstElementAttributeName = "xmlns:gml";

        /// <summary>
        /// 読み込み可否判定に用いる最初の要素タグに含まれる属性値。
        /// </summary>
        const string XmlnsGml = "http://schemas.opengis.net/gml/3.2.1/gml.xsd";

        /// <summary>
        /// 読み込み対象のファイルURI。
        /// </summary>
        private string Uri { get; }

        /// <summary>
        /// 渡されたファイルを読み込み可能かを判定する。
        /// </summary>
        /// <param name="_">ファイルのURI</param>
        /// <returns>読み込み可能な場合、TRUE</returns>
        public static bool IsReadable(string uri)
        {
            var reader = XmlReader.Create(uri);
            var count = 0;

            while (reader.Read()
               && count < 50)
            {
                if (reader.Name == FirstElementName
                && reader.GetAttribute(FirstElementAttributeName) == XmlnsGml)
                {
                    return true;
                }
                count++;
            }


            return false;
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="uri"></param>
        public GisSourceRepository(string uri)
        {
            Uri = uri;
        }

        /// <summary>
        /// ファイルからデータを読み込み内容を保持する。
        /// </summary>
        /// <param name="withPolygon">TRUE時、図形パスも読み込む</param>
        public IGisSourceStorage Read()
        {
            GisSourceStorage? storage = null;
            var reader = XmlReader.Create(Uri);

            while (reader.Read())
            {
                // XML中の要素以外は処理対象外
                // ※後続の処理対象種別に含まれるものは、個別に処理を行う
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                // XMLの要素種別ごとに塊として処理を行う
                switch (reader.Name)
                {
                    // 要素「データ提供範囲」の読込処理（最初に読み込まれる要素）
                    case BoundedBySource.TargetElementName:
                        storage = new GisSourceStorage(new BoundedBySource(reader.ReadOuterXml()));
                        break;

                    // 要素「図形」の読込処理
                    case CurveSource.TargetElementName:
                        var curveSource = new CurveSource(reader.ReadOuterXml());
                        storage!.Add(curveSource.Id, curveSource);
                        break;

                    // 要素「図形の紐付け情報」の読込処理
                    case SurfaceSource.TargetElementName:
                        var surfaceSource = new SurfaceSource(reader.ReadOuterXml());
                        storage!.Add(surfaceSource.Id, surfaceSource);
                        break;

                    // 要素「領域の基本情報」の読込処理
                    case AdministrativeBoundarySource.TargetElementName:
                        storage!.Add(new AdministrativeBoundarySource(reader.ReadOuterXml()));
                        break;
                }
            }

            return storage!;
        }
    }
}
