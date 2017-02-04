//
// State Extend
//
using System.Collections.Generic;
using UnityEngine;
using System;

namespace StellaQL
{
    /// <summary>
    /// ユーザー定義レコード
    /// </summary>
    public interface AcStateRecordable
    {
        /// <summary>
        /// 末尾にドットを含む（葉を含まない）
        /// </summary>
        string GetBreadCrumb();
        /// <summary>
        /// ステートマシン名、ステート名（葉より上のノード含まない）
        /// </summary>
        string Name { get; }
        /// <summary>
        /// ステートマシン名、ステート名のフルパス
        /// </summary>
        string Fullpath { get; }
        /// <summary>
        /// ステートマシン名、ステート名のフルパスのハッシュ
        /// </summary>
        int FullPathHash { get; }
        /// <summary>
        /// ユーザー定義タグのハッシュを全て含むか
        /// </summary>
        /// <param name="requiredAllTags_hash"></param>
        /// <returns></returns>
        bool HasEverythingTags(HashSet<int> requiredAllTags_hash);
        /// <summary>
        /// ユーザー定義タグのハッシュ。後から変更できる。
        /// </summary>
        HashSet<int> Tags { get; set; }
        /// <summary>
        /// （オプション）アニメーションの種類。使わなくても可。
        /// </summary>
        int Cliptype { get; set; }
    }

    /// <summary>
    /// ユーザー定義レコード
    /// </summary>
    public abstract class AbstractAcState : AcStateRecordable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        /// <param name="tags">ユーザー定義タグ</param>
        public AbstractAcState(string fullpath, string[] tags)
        {
            Fullpath = fullpath;
            Name = Fullpath.Substring(Fullpath.LastIndexOf('.') + 1); // ドットを含まない
            FullPathHash = Animator.StringToHash(fullpath);
            this.Tags = Code.Hashes(tags);
        }

        public string GetBreadCrumb() {
            return Fullpath.Substring(0, Fullpath.LastIndexOf('.') + 1);// 末尾にドットを含む
        }
        public string Fullpath { get; set; }
        public string Name { get; set; }
        public int FullPathHash { get; set; }
        public bool HasEverythingTags(HashSet<int> requiredAllTags)
        {
            foreach (int tag in requiredAllTags)
            {
                if (!Tags.Contains(tag)) { return false; } // １個でも持ってないタグがあれば偽。
            }
            return true;
        }
        public HashSet<int> Tags { get; set; }
        public int Cliptype { get; set; }
    }

    /// <summary>
    /// (Step 1.) Please, create record definition of statemachine or state. (ステートマシン、ステートのユーザー定義データ構造)
    /// Extend AbstractUserDefinedStateRecord class. (AbstractUserDefinedStateRecordクラスを継承してください)
    /// </summary>
    public class DefaultAcState : AbstractAcState
    {
        /// <summary>
        /// (Step 2a.) Initialize record. (レコードの初期設定)
        /// Use super class constructor. Required fullpath of statemachine or state.
        /// empty string array is OK for userDefinedTags. new string[]{}; Other parameters is option.
        /// (スーパークラスのコンストラクタを使います。必要なのはステートマシン名またはステート名のフルパスです。
        /// ユーザー定義タグは空セットで構いません。 new string[]{};　その他の引数は任意)
        /// </summary>
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        public DefaultAcState(string fullpath) :base(fullpath, new string[] { })
        {
        }

        /// <summary>
        /// (Step 2b.) Initialize record. (レコードの初期設定)
        /// Use super class constructor. Required fullpath of statemachine or state.
        /// empty string array is OK for userDefinedTags. new string[]{}; Other parameters is option.
        /// (スーパークラスのコンストラクタを使います。必要なのはステートマシン名またはステート名のフルパスです。
        /// ユーザー定義タグは空セットで構いません。 new string[]{};　その他の引数は任意)
        /// </summary>
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        /// <param name="fullpathHash">ステートマシン名、ステート名のフルパスのハッシュ</param>
        /// <param name="userDefinedTags_hash">StellaQL用のユーザー定義タグのハッシュ</param>
        public DefaultAcState(string fullpath, string[] userDefinedTags) : base(fullpath, userDefinedTags)
        {
        }
    }

    /// <summary>
    /// ユーザー定義レコードの集まり
    /// </summary>
    public interface AControllable
    {
        /// <summary>
        /// ステートマシン名、ステート名のフルパスのハッシュとユーザー定義レコードの対応付け。
        /// ハッシュは Animator.StringToHash( ) で算出する。
        /// </summary>
        Dictionary<int, AcStateRecordable> StateHash_to_record { get; }
        /// <summary>
        /// ユーザー定義タグ文字列とハッシュの対応付け
        /// </summary>
        Dictionary<string, int> TagString_to_hash { get; }
    }

    /// <summary>
    /// ユーザー定義レコードの集まり
    /// </summary>
    public abstract class AbstractAControll : AControllable
    {
        public AbstractAControll()
        {
            StateHash_to_record = new Dictionary<int, AcStateRecordable>();
            TagString_to_hash = new Dictionary<string, int>();
        }

        /// <summary>
        /// </summary>
        public Dictionary<int, AcStateRecordable> StateHash_to_record { get; set; }
        public Dictionary<string, int> TagString_to_hash { get; set; }

        /// <summary>
        /// 独自フィールドなどを追加している場合、レコードを丸ごと差し替えてください。
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="record"></param>
        public void Set(AcStateRecordable record)
        {
            StateHash_to_record[Animator.StringToHash(record.Fullpath)] = record;
        }
        /// <summary>
        /// ユーザー定義タグを変更する場合。
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="tags"></param>
        public void SetTag(string fullpath, string[] tags)
        {
            StateHash_to_record[Animator.StringToHash(fullpath)].Tags = Code.Hashes(tags);
        }

        /// <summary>
        /// 現在のアニメーター・ステートに対応したユーザー定義レコードを取得。
        /// </summary>
        /// <returns></returns>
        public AcStateRecordable GetCurrentUserDefinedStateRecord(Animator animator)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            if (this.StateHash_to_record.ContainsKey(state.fullPathHash))
            {
                return this.StateHash_to_record[state.fullPathHash];
            }

            throw new UnityException("[" + state.fullPathHash + "]のユーザー定義レコードが無いぜ☆　なんでだろな☆？（＾～＾）");
        }

        /// <summary>
        /// 現在のアニメーション・クリップに対応したユーザー定義レコードを取得。
        /// </summary>
        /// <returns></returns>
        public CliptypeExRecordable GetCurrentUserDefinedCliptypeRecord(Animator animator, UserDefinedCliptypeTableable userDefinedCliptypeTable)
        {
            AnimatorStateInfo animeStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            int cliptype = (StateHash_to_record[animeStateInfo.fullPathHash]).Cliptype;

            if (userDefinedCliptypeTable.Cliptype_to_exRecord.ContainsKey(cliptype))
            {
                return userDefinedCliptypeTable.Cliptype_to_exRecord[cliptype];
            }

            throw new UnityException("cliptype = [" + cliptype + "]に対応するアニメーション・クリップのユーザー定義レコードが無いぜ☆");
        }

        /// <summary>
        /// 完全一致タグ検索
        /// </summary>
        public HashSet<AcStateRecordable> FetchByEverythingTags(HashSet<int> requiredAllTags)
        {
            HashSet<AcStateRecordable> hit = new HashSet<AcStateRecordable>();

            foreach (AcStateRecordable record in StateHash_to_record.Values)
            {
                if (record.HasEverythingTags(requiredAllTags))
                {
                    hit.Add(record);
                }
            }

            return hit;
        }

    }
}
