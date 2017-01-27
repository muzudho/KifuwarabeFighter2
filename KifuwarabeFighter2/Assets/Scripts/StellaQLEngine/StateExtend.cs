//
// Scene Template
//
using System.Collections.Generic;
using UnityEngine;

namespace StellaQL
{
    public interface StateExRecordable
    {
        string BreadCrumb { get; }
        string Name { get; }
        //int FullPathHash { get; }
        bool HasFlag_attr(int enumration);
        /// <summary>
        /// ほんとは列挙型にしたい☆
        /// </summary>
        int AttributeEnum { get; }
    }

    public abstract class AbstractStateExRecord : StateExRecordable
    {
        public string BreadCrumb { get; set; }
        public string Name { get; set; }
        //public int FullPathHash { get; set; }
        public abstract bool HasFlag_attr(int attributeEnumration);
        /// <summary>
        /// ほんとは列挙型にしたい☆
        /// </summary>
        public int AttributeEnum { get; set; }
    }

    public abstract class AbstractStateExTable
    {
        /// <summary>
        /// Animator の state の名前と、AnimationClipの種類の対応☆　手作業で入力しておく（２度手間）
        /// ほんとは キーを StateIndex にしたかった。
        /// </summary>
        public Dictionary<int, StateExRecordable> index_to_exRecord;
        /// <summary>
        /// Animator の state の hash を、state番号に変換☆
        /// </summary>
        public Dictionary<int, int> hash_to_index;//<hash,StateIndex>


        /// <summary>
        /// シーンの Start( )メソッドで呼び出してください。
        /// </summary>
        public void InsertAllStates()
        {
            hash_to_index = new Dictionary<int, int>(); // <hash,StateIndex>

            for (int iState = 0; iState < index_to_exRecord.Count; iState++)
            {
                StateExRecordable astate = index_to_exRecord[iState];
                hash_to_index.Add(Animator.StringToHash(astate.BreadCrumb + astate.Name), iState);
            }
        }

        /// <summary>
        /// 現在のアニメーター・ステートに対応したデータを取得。
        /// </summary>
        /// <returns></returns>
        public StateExRecordable GetCurrentStateExRecord(Animator animator)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (!this.hash_to_index.ContainsKey(state.fullPathHash))
            {
                throw new UnityException("もしかして列挙型に登録していないステートがあるんじゃないか☆（＾～＾）？　ハッシュは[" + state.fullPathHash + "]だぜ☆");
            }

            int stateIndex = this.hash_to_index[state.fullPathHash];

            if (this.index_to_exRecord.ContainsKey(stateIndex))
            {
                StateExRecordable stateExRecord = this.index_to_exRecord[stateIndex];
                //((AbstractStateExRecord)stateExRecord).FullPathHash = state.fullPathHash;
                return stateExRecord;
            }

            throw new UnityException("[" + stateIndex + "]のデータが無いぜ☆　なんでだろな☆？（＾～＾）");
        }

        /// <summary>
        /// 属性検索
        /// </summary>
        public List<StateExRecordable> Where(int enumration_attr)
        {
            List<StateExRecordable> recordset = new List<StateExRecordable>();

            foreach (StateExRecordable record in index_to_exRecord.Values)
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
