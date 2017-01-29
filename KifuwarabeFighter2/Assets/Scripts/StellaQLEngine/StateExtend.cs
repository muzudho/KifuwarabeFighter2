//
// State Extend
//
using System.Collections.Generic;
using UnityEngine;

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
        bool HasFlag_attr(int enumration);
        /// <summary>
        /// ほんとは列挙型にしたい☆
        /// </summary>
        int AttributeEnum { get; }
        /// <summary>
        /// オプション。使わなくても可。
        /// </summary>
        int Cliptype { get; set; }//[CliptypeIndex]
    }

    public abstract class AbstractStateExRecord : StateExRecordable
    {
        public AbstractStateExRecord(string fullpath, int fullpathHash, int attributeEnum)
        {
            Fullpath = fullpath;
            Name = Fullpath.Substring(Fullpath.LastIndexOf('.') + 1); // ドットを含まない
            FullPathHash = fullpathHash;// Animator.StringToHash(Fullpath);
            //Debug.Log("fullpath=["+this.Fullpath+"] name=["+this.Name+"]");
            this.AttributeEnum = attributeEnum;//StateExTable.Attr
        }

        public string GetBreadCrumb() {
            return Fullpath.Substring(0, Fullpath.LastIndexOf('.') + 1);// 末尾にドットを含む
        }
        public string Fullpath { get; set; }
        public string Name { get; set; }
        public int FullPathHash { get; set; }
        public abstract bool HasFlag_attr(int attributeEnumration);
        /// <summary>
        /// ほんとは列挙型にしたい☆
        /// </summary>
        public int AttributeEnum { get; set; }
        /// <summary>
        /// オプション。使わなくても可。
        /// </summary>
        public int Cliptype { get; set; }//[CliptypeIndex]
    }

    public interface StateExTableable
    {
    }


    public abstract class AbstractStateExTable : StateExTableable
    {
        public AbstractStateExTable()
        {
            hash_to_exRecord = new Dictionary<int, StateExRecordable>();
        }

        /// <summary>
        /// アニメーター・コントローラーのステートのフルパスを Animator.StringToHash( ) でハッシュの数字を算出してこれをキーにし、
        /// レコードを対応させる。
        /// クラスの継承で拡張するので、レコードはあとで追加できるようにすること。
        /// </summary>
        public Dictionary<int, StateExRecordable> hash_to_exRecord;

        /// <summary>
        /// 現在のアニメーター・ステートに対応したデータを取得。
        /// </summary>
        /// <returns></returns>
        public StateExRecordable GetCurrentStateExRecord(Animator animator)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            if (this.hash_to_exRecord.ContainsKey(state.fullPathHash))
            {
                return this.hash_to_exRecord[state.fullPathHash];
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

            int cliptype = (hash_to_exRecord[animeStateInfo.fullPathHash]).Cliptype;

            if (cliptypeExTable.Cliptype_to_exRecord.ContainsKey(cliptype))
            {
                return cliptypeExTable.Cliptype_to_exRecord[cliptype];
            }

            throw new UnityException("cliptype = [" + cliptype + "]に対応するアニメーション・クリップのレコードが無いぜ☆");
        }

        /// <summary>
        /// 属性検索
        /// </summary>
        public List<StateExRecordable> Where(int enumration_attr)
        {
            List<StateExRecordable> recordset = new List<StateExRecordable>();

            foreach (StateExRecordable record in hash_to_exRecord.Values)
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
