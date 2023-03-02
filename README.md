# JpGisConv

国土交通省（国土数値情報・行政区域データ）の変換処理を中心とした CLI ツール。

行政区域ごとに扱う。

## 基本仕様

### 変換元

#### ポリゴンデータ
- [国土交通省 - 値情報・行政区域データ](https://nlftp.mlit.go.jp/ksj/gml/datalist/KsjTmplt-N03-v2_3.html)

#### 面積データ
- [国土地理院 - 道府県市区町村別面積調](https://www.gsi.go.jp/KOKUJYOHO/MENCHO-title.htm)

### 変換先

#### SVG
- 行政区域ごとに単独で開ける複数のSVGファイル
- SVGファイルからpathタグの情報のみ切り出した複数のテキストファイル

SVG 出力時、 JSON ファイルも自動出力する。

#### JSON
- 行政区域ごとの「都道府県名」「市区町村名」「面積（㎢）」を保持する1ファイル

#### CSV
- 行政区域ごとの「都道府県名」「市区町村名」「座標ポリゴン」を保持する1ファイル

## 実行オプション

### 例

#### SVG パス 出力（最大幅4000px, 面積情報JSON付き）
```
--mode=svgPath
--gis="/path/to//N03-20220101_02_GML\N03-22_02_220101.xml"
--landarea="/path/to/R1_R4_all_mencho.csv"
--max-width=4000
```

#### SVG 出力（最大幅4000px, 面積情報JSON付き）
```
--mode=svg
--gis="/path/to//N03-20220101_02_GML\N03-22_02_220101.xml"
--landarea="/path/to/R1_R4_all_mencho.csv"
--max-width=4000
```

#### CSV 出力（最大幅4000px, 面積情報付き）
```
--mode=csv
--gis="/path/to//N03-20220101_02_GML\N03-22_02_220101.xml"
--landarea="/path/to/R1_R4_all_mencho.csv"
```

#### 基本情報 JSON 出力（最大幅4000px, 面積情報JSON付き）
```
--mode=info
--gis="/path/to//N03-20220101_02_GML\N03-22_02_220101.xml"
--landarea="/path/to/R1_R4_all_mencho.csv"
--max-width=4000
```

## 基本情報 JSON 構成

```js
{
  "WholeArea": 377973.26           // ファイル内全体の面積合計（㎢）
  AdministrativeBoundaryList: [    // 区域単位の情報。区域は「都道府県」「支庁」「群」「市区町村」で一意となる
    {
      "PrefectureName": "青森県",  // 都道府県名
      "SubPrefectureName": null,   // 振興局名
      "CountyName": "三戸郡",      // 郡・政令都市名
      "CityName": "新郷村",        // 市区町村名
      "Width": 475,                // ピクセル基準の座標横幅
      "Height": 235,               // ピクセル基準の座標縦幅
      "CoordinateOnWholeSource": {
        "Left": 2711,              // ピクセル基準でのソース内の左端座標
        "Top": 2566                // ピクセル基準でのソース内の上端座標
      },
      "DistinctList": [            // 行政区域コード単位の情報
        {
          "Area": 150.77,          // 面積（㎢）
          "Code": "02450"          // 行政区域コード
        },
        
        ...
      ]
    },
    
    ...
  ]
}
```