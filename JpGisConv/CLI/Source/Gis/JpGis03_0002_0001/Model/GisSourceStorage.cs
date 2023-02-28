using JpGisConv.Source.Gis.Interface;
using JpGisConv.Work.Model;

namespace JpGisConv.Source.Gis.JpGis03_0002_0001.Model
{
    /// <summary>
    /// GIS情報の取込データを保持・変換する。
    /// </summary>
    public class GisSourceStorage : IGisSourceStorage
    {
        #region フィールド、プロパティ

        /// <summary>
        /// データ提供範囲のデータ
        /// </summary>
        public BoundedBySource BoundedBySource { get; }

        /// <summary>
        /// 図形のデータ
        /// </summary>
        public Dictionary<string, CurveSource> CurveSourceMap { get; } = new();

        /// <summary>
        /// 図形の紐付け情報のデータ
        /// </summary>
        public Dictionary<string, SurfaceSource> SurfaceSourceMap { get; } = new();

        /// <summary>
        /// 領域の基本情報のデータ
        /// </summary>
        public List<AdministrativeBoundarySource> AdministrativeBoundarySourceList { get; } = new();

        #endregion フィールド、プロパティ
        #region コンストラクタ、初期化処理

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="boundedBySource"></param>
        public GisSourceStorage(
            BoundedBySource boundedBySource)
        {
            BoundedBySource = boundedBySource;
        }

        /// <summary>
        /// 図形のデータの追加
        /// </summary>
        /// <param name="curveId"></param>
        /// <param name="curveSource"></param>
        public void Add(
            string curveId,
            CurveSource curveSource)
        {
            CurveSourceMap.Add(curveId, curveSource);
        }

        /// <summary>
        /// 図形の紐付け情報のデータの追加
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <param name="surfaceSource"></param>
        public void Add(
            string surfaceId,
            SurfaceSource surfaceSource)
        {
            SurfaceSourceMap.Add(surfaceId, surfaceSource);
        }

        /// <summary>
        /// 領域の基本情報のデータの追加
        /// </summary>
        /// <param name="administrativeBoundarySource"></param>
        public void Add(
            AdministrativeBoundarySource administrativeBoundarySource)
        {
            AdministrativeBoundarySourceList.Add(administrativeBoundarySource);
        }

        #endregion コンストラクタ、初期化処理
        #region 処理用の共通モデルへの変換処理

        /// <summary>
        /// 共通モデル生成処理。
        /// </summary>
        /// <returns></returns>
        public RegionStorage ToWorkMaterial()
        {
            return new RegionStorage(
                            BoundedBySource!.LowerCorner,
                            BoundedBySource!.UpperCorner,
                            CreateWorkAdministrativeBoundaryList());
        }

        /// <summary>
        /// 加工・出力処理で使うための区域データを作成する。
        /// </summary>
        private List<Region> CreateWorkAdministrativeBoundaryList()
        {
            var abList = new List<Region>();

            // 都道府県～市区町村名でグルーピング
            foreach (var abSourceGroup in AdministrativeBoundarySourceList.GroupBy(abs => abs.GroupKey))
            {
                // 生成
                var ab = new Region(
                                abSourceGroup.First().Name,
                                AdministrativeBoundarySource.ToWorkInstance(abSourceGroup.ToList()),
                                abSourceGroup.SelectMany(abSource => CreateWorkGeoPolygonList(abSource.SurfaceId)),
                                AdministrativeBoundarySource.ToOutputFileName(abSourceGroup.ToList()));

                // 結果値への追加
                abList.Add(ab);
            }

            return abList;
        }

        /// <summary>
        /// 加工・出力処理で使うための経緯度の面情報データの値を作成する。
        /// </summary>
        /// <param name="surfaceId"></param>
        private List<GeoPolygon> CreateWorkGeoPolygonList(
            string surfaceId)
        {
            // 図形パスのリスト (Surface) を取得
            if (!SurfaceSourceMap.TryGetValue(surfaceId, out var surfaceSource))
            {
                throw new FileLoadException();
            }

            var polygonList = new List<GeoPolygon>();

            // 図形パス (Curve) を追加していく
            foreach (var curveId in surfaceSource.CurveIdList)
            {
                if (CurveSourceMap.TryGetValue(curveId, out var curveSource))
                {
                    polygonList.Add(new GeoPolygon(curveSource.CoordinateList));
                }
            }

            return polygonList;
        }

        #endregion 処理用の共通モデルへの変換処理
    }
}
