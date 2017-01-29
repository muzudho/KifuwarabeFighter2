//
// State Extend
//
using System.Collections.Generic;
using UnityEngine;
using System;

namespace StellaQL
{
    public interface StateExRecordable
    {
        /// <summary>
        /// 末尾にドットを含む
        /// </summary>
        string GetBreadCrumb();
        string Name { get; }
        string Fullpath { get; }
        int FullPathHash { get; }
        bool HasFlag_attr(HashSet<int> attributeBitfield);
        /// <summary>
        /// ほんとは列挙型にしたい☆
        /// </summary>
        HashSet<int> Tags { get; }
        /// <summary>
        /// オプション。使わなくても可。
        /// </summary>
        int Cliptype { get; set; }//[CliptypeIndex]
    }

    public abstract class AbstractStateExRecord : StateExRecordable
    {
        public AbstractStateExRecord(string fullpath, int fullpathHash, HashSet<int> attributeBitfield)
        {
            Fullpath = fullpath;
            Name = Fullpath.Substring(Fullpath.LastIndexOf('.') + 1); // ドットを含まない
            FullPathHash = fullpathHash;
            this.Tags = attributeBitfield;
        }

        public string GetBreadCrumb() {
            return Fullpath.Substring(0, Fullpath.LastIndexOf('.') + 1);// 末尾にドットを含む
        }
        public string Fullpath { get; set; }
        public string Name { get; set; }
        public int FullPathHash { get; set; }
        public abstract bool HasFlag_attr(HashSet<int> attributeBitfield);
        /// <summary>
        /// ほんとは列挙型にしたい☆
        /// </summary>
        public HashSet<int> Tags { get; set; }
        /// <summary>
        /// オプション。使わなくても可。
        /// </summary>
        public int Cliptype { get; set; }//[CliptypeIndex]
    }

    public interface StateExTableable
    {
        Dictionary<int, StateExRecordable> Hash_to_exRecord { get; }
        Dictionary<string, int> String_to_tagHash { get; }
    }


    public abstract class AbstractStateExTable : StateExTableable
    {
        public AbstractStateExTable()
        {
            Hash_to_exRecord = new Dictionary<int, StateExRecordable>();
            String_to_tagHash = new Dictionary<string, int>();
        }

        /// <summary>
        /// アニメーター・コントローラーのステートのフルパスを Animator.StringToHash( ) でハッシュの数字を算出してこれをキーにし、
        /// レコードを対応させる。
        /// クラスの継承で拡張するので、レコードはあとで追加できるようにすること。
        /// </summary>
        public Dictionary<int, StateExRecordable> Hash_to_exRecord { get; set; }
        public Dictionary<string, int> String_to_tagHash { get; set; }

        /// <summary>
        /// 現在のアニメーター・ステートに対応したデータを取得。
        /// </summary>
        /// <returns></returns>
        public StateExRecordable GetCurrentStateExRecord(Animator animator)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            if (this.Hash_to_exRecord.ContainsKey(state.fullPathHash))
            {
                return this.Hash_to_exRecord[state.fullPathHash];
            }

            throw new UnityException("[" + state.fullPathHash + "]のデータが無いぜ☆　なんでだろな☆？（＾～＾）");
        }

        /// <summary>
        /// 現在のアニメーション・クリップに対応したデータを取得。
        /// </summary>
        /// <returns></returns>
        public CliptypeExRecordable GetCurrentCliptypeExRecord(Animator animator, CliptypeExTableable cliptypeExTable)
        {
            AnimatorStateInfo animeStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            int cliptype = (Hash_to_exRecord[animeStateInfo.fullPathHash]).Cliptype;

            if (cliptypeExTable.Cliptype_to_exRecord.ContainsKey(cliptype))
            {
                return cliptypeExTable.Cliptype_to_exRecord[cliptype];
            }

            throw new UnityException("cliptype = [" + cliptype + "]に対応するアニメーション・クリップのレコードが無いぜ☆");
        }

        /// <summary>
        /// 属性検索
        /// </summary>
        public List<StateExRecordable> Where(HashSet<int> enumration_attr)
        {
            List<StateExRecordable> recordset = new List<StateExRecordable>();

            foreach (StateExRecordable record in Hash_to_exRecord.Values)
            {
                if (record.HasFlag_attr(enumration_attr)) // if(attribute.HasFlag(record.attribute))
                {
                    recordset.Add(record);
                }
            }

            return recordset;
        }

    }
}
