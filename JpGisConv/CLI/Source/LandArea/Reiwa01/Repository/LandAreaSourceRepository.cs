using JpGisConv.Source.LandArea.Interface;
using JpGisConv.Source.LandArea.Reiwa01.Model;
using System.Text;

namespace JpGisConv.Source.LandArea.Reiwa01.Repository
{
    /// <summary>
    /// XML中の値を保持・変換する。
    /// 
    /// 国土地理院が公開している令和１年以降の面積調データ用
    /// https://www.gsi.go.jp/KOKUJYOHO/OLD-MENCHO-title.htm
    /// </summary>
    public class LandAreaSourceRepository : ILandAreaSourceRepository
    {
        /// <summary>
        /// 読み込み可否判定に用いる先頭行・先頭セルの内容
        /// ※ファイルの題名的なもの
        /// </summary>
        const string FirstLineFirstCell = "全国都道府県市区町村別面積調";

        /// <summary>
        /// 全国面積を表す特殊な標準地域コード。
        /// </summary>
        const string WholeCountryCode = "全国面積";

        /// <summary>
        /// データが存在する最初の行番号
        /// </summary>
        const int DataLineFirstNo = 6;

        /// <summary>
        /// 読み込み対象のファイルのエンコーディング。
        /// </summary>
        private static Encoding FileEncoding { get; } = Encoding.GetEncoding("shift_jis");

        /// <summary>
        /// 読み込み対象のファイルURI。
        /// </summary>
        private string Uri { get; }

        /// <summary>
        /// 渡されたファイルを読み込み可能かを判定する。
        /// </summary>
        /// <param name = "uri" > ファイルのURI </ param >
        /// <returns>読み込み可能な場合、TRUE</returns>
        public static bool IsReadable(
            string uri)
        {
            using (var sr = new StreamReader(uri, FileEncoding))
            {
                var line = string.Empty;
                var lineCount = 0;

                while ((line = sr.ReadLine()) != null
                        && ++lineCount < DataLineFirstNo + 1)
                {
                    // 指定行内の先頭セルにファイルの目的を表す文字が入っていなければ対象外
                    if (lineCount == 1
                        && line.Split(',')[0] != FirstLineFirstCell)
                    {
                        return false;
                    }

                    // 指定行内の先頭セルに全国面積を表す文字が入っていなければ対象外
                    if (lineCount == DataLineFirstNo
                        && line.Split(',')[0] != WholeCountryCode)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="uri"></param>
        public LandAreaSourceRepository(
            string uri)
        {
            Uri = uri;
        }

        /// <summary>
        /// ファイルからデータを読み込み内容を保持する。
        /// </summary>
        public ILandAreaSourceStorage Read()
        {
            LandAreaSourceStorage? storage = null;

            using (var sr = new StreamReader(Uri, FileEncoding))
            {
                var line = string.Empty;
                var lineCount = 0;

                // データ行より前の処理（無視）
                while ((sr.ReadLine()) != null
                        && ++lineCount < DataLineFirstNo - 1) ;

                // データ行以降の処理
                while ((line = sr.ReadLine()) != null)
                {
                    var row = new LandAreaSource(line.Split(','));

                    // 全国面積
                    if (row.Code == WholeCountryCode)
                    {
                        storage = new LandAreaSourceStorage(row.Area);
                    }

                    // 地域別の面積
                    else if (!string.IsNullOrEmpty(row.Code))
                    {
                        storage!.Add(row);
                    }
                }
            }

            return storage!;
        }
    }
}
