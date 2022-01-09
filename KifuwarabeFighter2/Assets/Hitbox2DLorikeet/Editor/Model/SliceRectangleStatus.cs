namespace DojinCircleGrayscale.Hitbox2DLorikeetMaker
{
    using UnityEngine;
    using System.Text;

    /// <summary>
    /// どのスライス画像（１枚の画像を格子状に分割した１コマ）の中の、更に どのあたりの矩形か。
    /// </summary>
    public class SliceRectangleStatus
    {
        public SliceRectangleStatus()
        {
            this.Collider = Rect.zero;
            this.Slice = Rect.zero;
        }
        public SliceRectangleStatus(int sliceX, int sliceY, int sliceWidth, int sliceHeight, int colliderX, int colliderY, int colliderWidth, int colliderHeight)
        {
            this.Collider = new Rect(colliderX, colliderY, colliderWidth, colliderHeight);
            this.Slice = new Rect(sliceX, sliceY, sliceWidth, sliceHeight);
        }

        public int SliceNumber { get; set; }
        /// <summary>
        /// 当たり判定の矩形
        /// </summary>
        public Rect Collider { get; set; }
        /// <summary>
        /// スライス領域の矩形
        /// </summary>
        public Rect Slice { get; set; }

        /// <summary>
        /// 中心座標に変換するぜ☆（＾▽＾）
        /// 解説 : 「中心座標の求め方の考え方」 http://qiita.com/muzudho1/items/9298c8181450e92d564e
        /// </summary>
        /// <returns></returns>
        public float GetOffsetX(StringBuilder info_message)
        {
            //float x = (Collider.width / 2.0f + Collider.x) - (Slice.width / 2.0f + Slice.x);
            float x = Collider.center.x - Slice.center.x;
            x /= DataClassFile.UNITY_GRID_UNIT;
            if (info_message.Length < 10000)
            {
                info_message.AppendLine("GetOffsetX: x=[" + x + "] ((Collider.center.x=[" + Collider.center.x + "]) - (Slice.center.x=[" + Slice.center.x + "]) / DataGenerator.UNITY_GRID_UNIT=[" + DataClassFile.UNITY_GRID_UNIT + "]");
            }
            return x;
        }
        public float GetOffsetY(StringBuilder info_message)
        {
            // X,Yは矩形の左上で、中心Yは下方向にある。
            float y = (Collider.y - Collider.height / 2.0f) - (Slice.y - Slice.height / 2.0f);
            // X,Yは矩形の左上で、centerで中心を求めると矩形の上方向に飛び出てしまう。
            //float y = Collider.center.y - Slice.center.y;
            y /= DataClassFile.UNITY_GRID_UNIT;//-1 * 
            if (info_message.Length < 10000)
            {
                info_message.AppendLine("GetOffsetY: y=[" + y + "] (( Collider.center.y=[" + Collider.center.y + "]) - (Slice.center.y=[" + Slice.center.y + "])) / DataGenerator.UNITY_GRID_UNIT=[" + DataClassFile.UNITY_GRID_UNIT + "]");
            }
            return y;
        }
        /// <summary>
        /// 当たり判定画像も、１スライスのサイズと同じとするぜ☆（＾▽＾）
        /// </summary>
        /// <returns></returns>
        public float GetScaleX()
        {
            return this.Collider.width / (float)DataClassFile.SLICE_WIDTH;
        }
        public float GetScaleY()
        {
            return this.Collider.height / (float)DataClassFile.SLICE_HEIGHT;
        }
    }
}
