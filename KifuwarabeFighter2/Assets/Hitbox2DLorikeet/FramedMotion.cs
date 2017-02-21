namespace DojinCircleGrayscale.Hitbox2DLorikeet
{
    /// <summary>
    /// スライスする画像の元となるファイルのインデックス。
    /// １キャラに付き、立ちモーション、ジャンプモーションなどに画像を分けていると想定。
    /// キャラクターの行動、キャラクター画像とも対応。
    /// Unityのテキストボックスで数字を直打ちしているので、対応する数字も保つこと。
    /// 旧名： ActioningIndex
    /// </summary>
    public enum TilesetfileType
    {
        /// <summary>
        /// [0]
        /// </summary>
        Stand,
        Jump,
        Dash,
        Crouch,
        Other,
        /// <summary>
        /// 列挙型の要素数、または未使用の値として使用。
        /// </summary>
        Num
    }

    public interface FramedMotionable
    {
        /// <summary>
        /// スライスする画像の元となるファイルのインデックス。
        /// Tile set file (type) index.
        /// 
        /// １キャラに付き、立ちモーション、ジャンプモーションなどに画像を分けていると想定。
        /// ex. 0:Stand, 1:Jump, 2:Dash, 3:Crouch, 4:Other...
        /// 
        /// １枚しか使っていないのなら 0 で。
        /// When not in use, set it to zero.
        /// </summary>
        int TilesetfileType { get; set; }

        /// <summary>
        /// 2Dアニメーションの動画の何フレーム目か。
        /// How many frames of animation in 2D animation?
        /// </summary>
        int[] Slices { get; set; }
    }

    public abstract class AbstractFramedMotion : FramedMotionable
    {
        public AbstractFramedMotion(int[] slices, int tilesetfileTypeIndex)
        {
            this.Slices = slices;
            this.TilesetfileType = tilesetfileTypeIndex;
        }

        public int TilesetfileType { get; set; }
        public int[] Slices { get; set; }
    }

    /// <summary>
    /// ステートに設定されるアニメーション・クリップの種類
    /// （キャラクター＠モーション名　となっているとして、モーション名の部分）
    /// </summary>
    public class FramedMotion : AbstractFramedMotion
    {
        public FramedMotion(int[] slices, TilesetfileType tilesetfileType) : base(slices, (int)tilesetfileType)
        {

        }
    }
}
