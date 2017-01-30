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
    public interface UserDefindStateRecordable
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
        /// ユーザー定義タグのハッシュ
        /// </summary>
        HashSet<int> Tags { get; }
        /// <summary>
        /// （オプション）アニメーションの種類。使わなくても可。
        /// </summary>
        int Cliptype { get; set; }
    }

    /// <summary>
    /// ユーザー定義レコード
    /// </summary>
    public abstract class AbstractUserDefinedStateRecord : UserDefindStateRecordable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        /// <param name="tags">ユーザー定義タグ</param>
        public AbstractUserDefinedStateRecord(string fullpath, string[] tags)
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
    /// ユーザー定義レコードの集まり
    /// </summary>
    public interface UserDefinedStateTableable
    {
        /// <summary>
        /// ステートマシン名、ステート名のフルパスのハッシュとユーザー定義レコードの対応付け。
        /// ハッシュは Animator.StringToHash( ) で算出する。
        /// </summary>
        Dictionary<int, UserDefindStateRecordable> StateHash_to_record { get; }
        /// <summary>
        /// ユーザー定義タグ文字列とハッシュの対応付け
        /// </summary>
        Dictionary<string, int> TagString_to_hash { get; }
    }

    /// <summary>
    /// ユーザー定義レコードの集まり
    /// </summary>
    public abstract class AbstractUserDefinedStateTable : UserDefinedStateTableable
    {
        public AbstractUserDefinedStateTable()
        {
            StateHash_to_record = new Dictionary<int, UserDefindStateRecordable>();
            TagString_to_hash = new Dictionary<string, int>();
        }

        /// <summary>
        /// </summary>
        public Dictionary<int, UserDefindStateRecordable> StateHash_to_record { get; set; }
        public Dictionary<string, int> TagString_to_hash { get; set; }

        /// <summary>
        /// 現在のアニメーター・ステートに対応したユーザー定義レコードを取得。
        /// </summary>
        /// <returns></returns>
        public UserDefindStateRecordable GetCurrentUserDefinedStateRecord(Animator animator)
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
        public HashSet<UserDefindStateRecordable> FetchByEverythingTags(HashSet<int> requiredAllTags)
        {
            HashSet<UserDefindStateRecordable> hit = new HashSet<UserDefindStateRecordable>();

            foreach (UserDefindStateRecordable record in StateHash_to_record.Values)
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
