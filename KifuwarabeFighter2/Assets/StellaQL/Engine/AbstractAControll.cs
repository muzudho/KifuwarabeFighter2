using System.Collections.Generic;
using UnityEngine;

namespace StellaQL
{
    /// <summary>
    /// アニメーター・ステート１つに対応するレコードです。
    /// This interface is a record corresponding to one animator state.
    /// </summary>
    public interface AcStateRecordable
    {
        /// <summary>
        /// 末尾にドットを含む
        /// It ends with a dot ".".
        /// 
        /// （葉を含まない）
        /// It does not contain leaf node (state name).
        /// 
        /// ex. "Base Layer.Japan."
        /// </summary>
        string GetBreadCrumb();
        /// <summary>
        /// ステートマシン名、ステート名（葉より上のノード含まない）
        /// Statemachine name or state name.
        /// 
        /// 箱のラベルです。
        /// This is a label of box.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// ステートマシン名、ステート名のフルパス
        /// Statemachine full path or state full path.
        /// </summary>
        string Fullpath { get; }
        /// <summary>
        /// ステートマシン名、ステート名のフルパスのハッシュ
        /// Fullpath hash.
        /// </summary>
        int FullPathHash { get; }
        /// <summary>
        /// ユーザー定義タグのハッシュ。
        /// Your defined tags hash.
        /// </summary>
        HashSet<int> Tags { get; set; }
        /// <summary>
        /// （オプション）アニメーションの種類。使わなくても可。
        /// (Option) for 2D fighting game.
        /// </summary>
        int Cliptype { get; set; }
        /// <summary>
        /// ユーザー定義タグのハッシュを全て含むか
        /// True if it contains all tags.
        /// </summary>
        /// <param name="requiredAllTags_hash"></param>
        /// <returns></returns>
        bool HasEverythingTags(HashSet<int> requiredAllTags_hash);
    }

    /// <summary>
    /// アニメーター・ステート１つに対応するレコード。
    /// This abstract class is a record corresponding to one animator state.
    /// </summary>
    public abstract class AbstractAcState : AcStateRecordable
    {
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス
        /// Statemachine or state.</param>
        /// <param name="tags">ユーザー定義タグ
        /// Your defined tags.</param>
        public AbstractAcState(string fullpath, string[] tags)
        {
            Fullpath = fullpath;

            // ドットを含まない
            // Does not include dots.
            Name = Fullpath.Substring(Fullpath.LastIndexOf('.') + 1);

            FullPathHash = Animator.StringToHash(fullpath);
            Tags = Code.Hashes(tags);
        }

        public string GetBreadCrumb() {
            // 末尾にドットを含む
            // It ends with a dot.
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
                // If there is a tag that does not have even one, it is false.
                if (!Tags.Contains(tag)) { return false; }
            }
            return true;
        }
        public HashSet<int> Tags { get; set; }
        public int Cliptype { get; set; }
    }

    /// <summary>
    /// For generators. For you.
    /// </summary>
    public class DefaultAcState : AbstractAcState
    {
        /// <summary>
        /// Correspond to statemachine or state.
        /// </summary>
        public DefaultAcState(string fullpath) :base(fullpath, new string[] { })
        {
        }

        /// <summary>
        /// Base for you.
        /// </summary>
        /// <param name="fullpath">Statemachine or state full path.</param>
        /// <param name="userDefinedTags_hash">Your defined tags.</param>
        public DefaultAcState(string fullpath, string[] userDefinedTags) : base(fullpath, userDefinedTags)
        {
        }
    }

    /// <summary>
    /// States.
    /// </summary>
    public interface AControllable
    {
        /// <summary>
        /// Mapping Statemachine, state name hash to state object.
        /// Calculate the name hash with Animator.StringToHash( ).
        /// </summary>
        Dictionary<int, AcStateRecordable> StateHash_to_record { get; }
    }

    /// <summary>
    /// States.
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
        /// For your defined class.
        /// </summary>
        /// <param name="record">Your defined class.</param>
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
        /// Current.
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
