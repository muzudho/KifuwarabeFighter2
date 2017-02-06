//
// Cliptype Extend
//
using System.Collections.Generic;
using UnityEngine;

namespace StellaQL
{
    public interface CliptypeExRecordable
    {
        /// <summary>
        /// スライスする画像の元となるファイルのインデックス。
        /// １キャラに付き、立ちモーション、ジャンプモーションなどに画像を分けていると想定。
        /// １枚しか使っていないのなら 0 で。
        /// 
        /// ほんとは列挙型にしたい。
        /// </summary>
        int TilesetfileTypeIndex { get; set; } // TilesetfileTypeIndex

        /// <summary>
        /// 2Dアニメーションの動画の何フレーム目か。
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

        public int TilesetfileTypeIndex { get; set; } // TilesetfileTypeIndex
        public int[] Slices { get; set; }
    }

    /// <summary>
    /// いい造語がないんで、Tableable にするか☆（＾～＾）
    /// </summary>
    public interface UserDefinedCliptypeTableable
    {
        Dictionary<int, CliptypeExRecordable> Cliptype_to_exRecord { get; set; } // [CliptypeIndex]
    }

    public abstract class AbstractCliptypeExTable : UserDefinedCliptypeTableable
    {
        public Dictionary<int, CliptypeExRecordable> Cliptype_to_exRecord { get; set; } // [CliptypeIndex]
    }
}
