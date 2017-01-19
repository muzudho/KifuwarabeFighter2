using System.Drawing;

namespace Hitbox2DMaker
{
    /// <summary>
    /// どのスライス画像の、どのあたりの矩形か。
    /// </summary>
    public class RectangleOnSlice
    {
        public RectangleOnSlice()
        {
            this.Collider = Rectangle.Empty;
            this.Slice = Rectangle.Empty;
        }
        public RectangleOnSlice(int sliceX, int sliceY, int sliceWidth, int sliceHeight, int x, int y, int width, int height)
        {
            this.Collider = new Rectangle(x, y, width, height);
            this.Slice = new Rectangle(sliceX, sliceY, sliceWidth, sliceHeight);
        }

        public int SliceNumber { get; set; }
        /// <summary>
        /// 当たり判定の矩形
        /// </summary>
        public Rectangle Collider { get; set; }
        /// <summary>
        /// スライス領域の矩形
        /// </summary>
        public Rectangle Slice { get; set; }

        /// <summary>
        /// 中心座標に変換するぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public float GetOffsetX()
        {
            return ((this.Collider.X - this.Slice.X) - this.Slice.Width / 2.0f + this.Collider.Width / 2.0f) / 100.0f;
        }
        public float GetOffsetY()
        {
            return -1 * ((this.Collider.Y - this.Slice.Y) - this.Slice.Height / 2.0f + this.Collider.Height / 2.0f) / 100.0f;
        }
        /// <summary>
        /// 当たり判定画像も、１スライスのサイズと同じとするぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public float GetScaleX()
        {
            return this.Collider.Width / (float)Program.SLICE_WIDTH;
        }
        public float GetScaleY()
        {
            return this.Collider.Height / (float)Program.SLICE_HEIGHT;
        }
    }
}
