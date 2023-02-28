namespace JpGisConv.General.Extension
{
    /// <summary>
    /// 出力処理前のファイル関連処理群。
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 空文字の場合、null値に変換して返却する。
        /// </summary>
        /// <param name="value"></param>
        public static string? TryConvertEmptyToNull(this string value)
        {
            return string.IsNullOrEmpty(value)
                ? null
                : value;
        }
    }
}

