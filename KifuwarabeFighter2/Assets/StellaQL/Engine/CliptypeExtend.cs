//
// (Option) For 2D fighting game.
//
using System.Collections.Generic;

namespace StellaQL
{
    public interface CliptypeExRecordable
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
        int TilesetfileTypeIndex { get; set; }

        /// <summary>
        /// 2Dアニメーションの動画の何フレーム目か。
        /// How many frames of animation in 2D animation?
        /// </summary>
        int[] Slices { get; set; }
    }

    public abstract class AbstractCliptypeExRecord : CliptypeExRecordable
    {
        public AbstractCliptypeExRecord(int[] slices, int tilesetfileTypeIndex)
        {
            this.Slices = slices;
            this.TilesetfileTypeIndex = tilesetfileTypeIndex;
        }

        public int TilesetfileTypeIndex { get; set; }
        public int[] Slices { get; set; }
    }

    /// <summary>
    /// いい造語がないんで、Tableable にした。（＾～＾）
    /// I could not think of coined words:-) Tableable.
    /// </summary>
    public interface UserDefinedCliptypeTableable
    {
        /// <summary>
        /// [CliptypeIndex]
        /// </summary>
        Dictionary<int, CliptypeExRecordable> Cliptype_to_exRecord { get; set; }
    }

    public abstract class AbstractCliptypeExTable : UserDefinedCliptypeTableable
    {
        /// <summary>
        /// [CliptypeIndex]
        /// </summary>
        public Dictionary<int, CliptypeExRecordable> Cliptype_to_exRecord { get; set; }
    }
}
