using DotSpatial.Projections;

namespace JpGisConv.Work.Model
{
    /// <summary>
    /// 直交座標での座標を管理する。
    /// </summary>
    public class RectangularCoordinate
    {
        #region 定数

        /// <summary>
        /// 縦横座標を文字列結合する際の結合文字。
        /// </summary>
        const string COORDINATE_DELIMITER = "-";

        #endregion 定数
        #region フィールド、プロパティ

        /// <summary>
        /// 横座標。
        /// </summary>
        public decimal X { get; }

        /// <summary>
        /// 縦座標。
        /// </summary>
        public decimal Y { get; }

        #endregion フィールド、プロパティ
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="x">横座標</param>
        /// <param name="y">縦座標</param>
        public RectangularCoordinate(
            decimal x,
            decimal y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Webメルカトル座標系のESPGコードにて作成された、直角座標系の投影法。
        /// ウェブ上の多くのマップサービスで使用されている。
        /// </summary>
        /// <see cref="ProjectionInfo"/>
        /// <see cref="Reproject.ReprojectPoints"/>
        private static ProjectionInfo GeoCoordinateEpsgCode = ProjectionInfo.FromEpsgCode(6668);

        /// <summary>
        /// Webメルカトル座標系のESPGコードにて作成された、直角座標系の投影法。
        /// ウェブ上の多くのマップサービスで使用されている。
        /// </summary>
        /// <see cref="ProjectionInfo"/>
        /// <see cref="Reproject.ReprojectPoints"/>
        private static ProjectionInfo WebMercatorEpsgCode = ProjectionInfo.FromEpsgCode(3857);

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="geoCoordinate">経緯度を半角空白で区切った文字列</param>
        public RectangularCoordinate(
            GeoCoordinate geoCoordinate)
        {
            var rectCoordinate = geoCoordinate.ToDoubleArrayCoordinate();

            // 変換
            Reproject.ReprojectPoints(rectCoordinate, null, GeoCoordinateEpsgCode, WebMercatorEpsgCode, 0, 1);

            // 設定（水平／垂直の方向を逆にする）
            X = new decimal(rectCoordinate[0]);
            Y = new decimal(rectCoordinate[1]);
        }

        #endregion コンストラクタ
        #region 値の変換処理

        /// <summary>
        /// 係数をかけて整数型の値に変換した座標を返却する。
        /// </summary>
        /// <param name="scale">経緯度にかける倍率</param>
        private static decimal ScaleWithInt(
            decimal d,
            decimal scale)
        {
            d = decimal.Multiply(d, scale);
            d = decimal.Floor(d);

            return d;
        }

        /// <summary>
        /// 縦横座標を規定の結合文字で結合する。
        /// </summary>
        public string JoinAsString()
        {
            return $"{X}{COORDINATE_DELIMITER}{Y}";
        }

        #endregion 値の変換処理
        #region 同クラスインスタンスへの変換処理

        /// <summary>
        /// 係数をかけて整数型の値に変換した座標を返却する。
        /// </summary>
        /// <param name="scale">倍率</param>
        public RectangularCoordinate ScaleWithInt(
            decimal scale)
        {
            return new RectangularCoordinate(ScaleWithInt(X, scale),
                                             ScaleWithInt(Y, scale));
        }

        /// <summary>
        /// 座標を移動する。
        /// </summary>
        /// <param name="distance">移動距離</param>
        /// <returns></returns>
        public RectangularCoordinate Move(
            RectangularCoordinate distance)
        {
            return new RectangularCoordinate(X + distance.X,
                                             Y + distance.Y);
        }

        /// <summary>
        /// 座標の正負を反転する。
        /// </summary>
        /// <returns></returns>
        public RectangularCoordinate Negate()
        {
            return new RectangularCoordinate(decimal.Negate(X),
                                             decimal.Negate(Y));
        }

        /// <summary>
        /// 与えられた座標群から最も左上の座標を抽出する。
        /// </summary>
        /// <param name="polygonList"></param>
        /// <returns></returns>
        /// <exception cref="FileLoadException"></exception>
        public static RectangularRect CreateEdgeRect(
            IEnumerable<RectangularPolygon> polygonList)
        {
            var srcCoordinates = polygonList.SelectMany(p => p.CoordinateList);

            if (srcCoordinates.Any())
            {
                return new RectangularRect(
                            srcCoordinates.Min(c => c.X),
                            srcCoordinates.Max(c => c.X),
                            srcCoordinates.Min(c => c.Y),
                            srcCoordinates.Max(c => c.Y)
                        );
            }

            throw new FileLoadException("取込ファイルの形式に誤りがあります");
        }

        /// <summary>
        /// 位置情報を上下ひっくり返す。
        /// </summary>
        /// <param name="containerHeight">座標が使われる領域全体の縦幅</param>
        /// <returns></returns>
        public RectangularCoordinate UpsideDown(
            decimal containerHeight)
        {
            return new RectangularCoordinate(X, containerHeight - Y);
        }

        #endregion 同クラスインスタンスへの変換処理
        #region 比較処理

        /// <summary>
        /// 等価比較処理。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not RectangularCoordinate)
            {
                return false;
            }

            var another = (RectangularCoordinate)obj;

            return X == another.X
                && Y == another.Y;
        }

        /// <summary>
        /// ハッシュ値の生成。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Tuple.Create(X, Y).GetHashCode();
        }

        #endregion 比較処理
    }
}