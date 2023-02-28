using System.Text.RegularExpressions;

namespace JpGisConv.Work.Model
{
    /// <summary>
    /// 直交座標で構成された面情報を管理する。
    /// </summary>
    public class RectangularPolygon
    {
        /// <summary>
        /// 座標を文字列結合する際の結合文字。
        /// </summary>
        const string COORDINATE_DELIMITER = "_";

        /// <summary>
        /// 順に線を引くと面になる直交座標のリスト。
        /// </summary>
        public List<RectangularCoordinate> CoordinateList { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="geoCoordinateList">順に線を引くと面になる経緯度のリスト</param>
        public RectangularPolygon(
            IEnumerable<GeoCoordinate> geoCoordinateList)
        {
            CoordinateList = geoCoordinateList.Select(gc => new RectangularCoordinate(gc))
                                              .ToList();
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="coordinateList">順に線を引くと面になる直交座標のリスト</param>
        public RectangularPolygon(
            IEnumerable<RectangularCoordinate> coordinateList)
        {
            CoordinateList = coordinateList.ToList();
        }

        #region 加工処理

        /// <summary>
        /// 保持している座標群の倍率を変更する。
        /// </summary>
        /// <param name="scale">倍率</param>
        public void ScaleCoordinateList(
            decimal scale)
        {
            var newerCoordinateList = new List<RectangularCoordinate>();

            foreach (var c in CoordinateList)
            {
                var scaledCoordinate = c.ScaleWithInt(scale);

                if (!scaledCoordinate.Equals(newerCoordinateList.LastOrDefault()))
                {
                    newerCoordinateList.Add(scaledCoordinate);
                }
            }

            CoordinateList = newerCoordinateList;
        }

        /// <summary>
        /// 受け取ったインスタンス群から、座標パスが重複する要素を削除する。
        /// </summary>
        /// <param name="polygonList">インスタンス群</param>
        /// <returns></returns>
        public static List<RectangularPolygon> Minimize(
            IEnumerable<RectangularPolygon> polygonList)
        {
            var newerPolygonList = new List<RectangularPolygon>();

            foreach (var p in polygonList)
            {
                // 画像出力に向かない余分な面情報を除去しつつ、
                // 重複しないようにパスを追加する
                if (p.CoordinateList.Count > 0
                    && !p.IsPathContains(newerPolygonList))
                {
                    newerPolygonList.Add(p);
                }
            }

            return newerPolygonList;
        }

        #endregion 加工処理
        #region 変換処理
        /// <summary>
        /// 面情報を規定の結合文字で結合する。
        /// </summary>
        public string JoinAsString()
        {
            return string.Join(
                COORDINATE_DELIMITER,
                CoordinateList.Select(c => c.JoinAsString()));
        }

        #endregion 値の変換処理
        #region 同クラスインスタンスへの変換処理

        /// <summary>
        /// 保持している直交座標を左上端に詰める。
        /// </summary>
        /// <param name="leftTopCoordinate">左上端の直交座標観点の座標</param>
        /// <returns></returns>
        public RectangularPolygon MoveToLeftTop(
            RectangularCoordinate leftTopCoordinate)
        {
            return new RectangularPolygon(
                CoordinateList.Select(c => c.Move(leftTopCoordinate.Negate())));
        }

        /// <summary>
        /// 位置情報を上下ひっくり返す。
        /// </summary>
        /// <param name="containerHeight">座標が使われる領域全体の縦幅</param>
        public RectangularPolygon UpsideDownCoordinate(
            decimal containerHeight)
        {
            return new RectangularPolygon(
                CoordinateList.Select(c => c.UpsideDown(containerHeight)));
        }

        #endregion 同クラスインスタンスへの変換処理
        #region 比較処理

        /// <summary>
        /// 受け取ったインスタンス群に、自らの座標パスが完全に含まれているか確認する。
        /// </summary>
        /// <param name="polygonList">インスタンス群</param>
        /// <returns></returns>
        public bool IsPathContains(
            List<RectangularPolygon> polygonList)
        {
            var regex = new Regex(JoinAsString());

            return polygonList.Any(p => regex.IsMatch(p.JoinAsString()));
        }
        
        /// <summary>
        /// 受け取ったインスタンス群に、自らの座標パスが完全に含まれているか確認する。
        /// </summary>
        /// <param name="polygonList">インスタンス群</param>
        /// <returns></returns>
        public bool IsPathContains(
            RectangularPolygon polygon)
        {
            var regex = new Regex(JoinAsString());

            return regex.IsMatch(polygon.JoinAsString());
        }

        #endregion 比較処理
    }
}