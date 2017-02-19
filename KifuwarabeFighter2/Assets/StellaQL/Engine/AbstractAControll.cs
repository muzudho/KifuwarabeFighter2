using System.Collections.Generic;
using UnityEngine;

namespace DojinCircleGrayscale.StellaQL
{
    /// <summary>
    /// アニメーター・ステート１つに対応するレコードです。
    /// </summary>
    public interface AcStateRecordable
    {
        /// <summary>
        /// 末尾にドットを含む
        /// 
        /// （葉を含まない）
        /// 
        /// ex. "Base Layer.Japan."
        /// </summary>
        string GetBreadCrumb();
        /// <summary>
        /// ステートマシン名、ステート名（葉より上のノード含まない）
        /// 
        /// 箱のラベルです。
        /// </summary>
        string Name { get; }
        /// <summary>
        /// ステートマシン名、ステート名のフルパス
        /// </summary>
        string Fullpath { get; }
        /// <summary>
        /// ステートマシン名、ステート名のフルパスのハッシュ
        /// Fullpath hash.
        /// </summary>
        int FullPathHash { get; }
        /// <summary>
        /// ユーザー定義タグのハッシュ。
        /// </summary>
        HashSet<int> Tags { get; set; }
        /// <summary>
        /// （オプション）モーション・ファイル・パスのハッシュ。使わなくても可。
        /// </summary>
        int MotionAssetPathHash { get; set; }
        /// <summary>
        /// ユーザー定義タグのハッシュを全て含むか
        /// </summary>
        bool HasEverythingTags(HashSet<int> requiredAllTags_hash);
    }

    /// <summary>
    /// アニメーター・ステート１つに対応するレコード。
    /// </summary>
    public abstract class AbstractAcState : AcStateRecordable
    {
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        /// <param name="tags">ユーザー定義タグ</param>
        public AbstractAcState(string fullpath, string[] tags)
        {
            Fullpath = fullpath;

            // ドットを含まない
            Name = Fullpath.Substring(Fullpath.LastIndexOf('.') + 1);

            FullPathHash = Animator.StringToHash(fullpath);
            Tags = Code.Hashes(tags);
        }

        public string GetBreadCrumb() {
            // 末尾にドットを含む
            return Fullpath.Substring(0, Fullpath.LastIndexOf('.') + 1);
        }
        public string Fullpath { get; set; }
        public string Name { get; set; }
        public int FullPathHash { get; set; }
        public bool HasEverythingTags(HashSet<int> requiredAllTags)
        {
            foreach (int tag in requiredAllTags)
            {
                // １個でも持ってないタグがあれば偽。
                if (!Tags.Contains(tag)) { return false; }
            }
            return true;
        }
        public HashSet<int> Tags { get; set; }
        public int MotionAssetPathHash { get; set; }
    }

    /// <summary>
    /// ステートマシン、またはステート１つに対応するデータ。
    /// </summary>
    public class DefaultAcState : AbstractAcState
    {
        public DefaultAcState(string fullpath) :base(fullpath, new string[] { })
        {
        }

        /// <param name="fullpath">ステートマシン、またはステートのフルパス</param>
        /// <param name="userDefinedTags_hash">ユーザー定義タグ</param>
        public DefaultAcState(string fullpath, string[] userDefinedTags) : base(fullpath, userDefinedTags)
        {
        }
    }

    /// <summary>
    /// アニメーター・コントローラー１つに対応するデータ。
    /// </summary>
    public interface AControllable
    {
        /// <summary>
        /// ステートマシン、ステート名のハッシュと、ステート１つに対応するデータとの紐付け。
        /// 
        /// 名前のハッシュは Animator.StringToHash( ) メソッドで算出すること。
        /// </summary>
        Dictionary<int, AcStateRecordable> StateHash_to_record { get; }
    }

    /// <summary>
    /// ステートマシン、またはステート１つに対応するデータ。
    /// </summary>
    public abstract class AbstractAControl : AControllable
    {
        public AbstractAControl()
        {
            StateHash_to_record = new Dictionary<int, AcStateRecordable>();
        }

        public Dictionary<int, AcStateRecordable> StateHash_to_record { get; set; }

        /// <summary>
        /// 独自フィールドなどを追加している場合、レコードを丸ごと差し替えてください。
        /// </summary>
        /// <param name="record">拡張クラスなど</param>
        public void Set(AcStateRecordable record)
        {
            StateHash_to_record[Animator.StringToHash(record.Fullpath)] = record;
        }
        /// <summary>
        /// ユーザー定義タグを変更する場合。
        /// </summary>
        public void SetTag(string fullpath, string[] tags)
        {
            StateHash_to_record[Animator.StringToHash(fullpath)].Tags = Code.Hashes(tags);
        }

        /// <summary>
        /// 現在のアニメーター・ステートに対応したユーザー定義レコードを取得。
        /// </summary>
        /// <returns></returns>
        public AcStateRecordable GetCurrentAcStateRecord(Animator animator)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            if (this.StateHash_to_record.ContainsKey(state.fullPathHash))
            {
                return this.StateHash_to_record[state.fullPathHash];
            }

            throw new UnityException("Not found [" + state.fullPathHash + "].");
        }
    }
}
