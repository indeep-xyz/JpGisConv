namespace JpGisConv.Work.Model
{
    /// <summary>
    /// 直交座標の矩形情報を管理する。
    /// </summary>
    public class RectangularRect
    {
        /// <summary>
        /// 左端の辺の座標。
        /// </summary>
        public decimal Left { get; set; }

        /// <summary>
        /// 右端の辺の座標。
        /// </summary>
        public decimal Right { get; set; }

        /// <summary>
        /// 上端の辺の座標。
        /// </summary>
        public decimal Top { get; set; }

        /// <summary>
        /// 下左端の辺の座標。
        /// </summary>
        public decimal Bottom { get; set; }

        /// <summary>
        /// 左上端の辺の座標。
        /// </summary>
        public RectangularCoordinate LeftTop
        {
            get => new(Left, Top);
        }

        /// <summary>
        /// 右下端の辺の座標。
        /// </summary>
        public RectangularCoordinate RightBottom
        {
            get => new(Right, Bottom);
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        public RectangularRect(
            decimal left,
            decimal right,
            decimal top,
            decimal bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}