using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneSelect {

    /// <summary>
    /// Animator の State に一対一対応☆
    /// </summary>
    public enum AstateIndex
    {
        Stay,
        Move,
        Ready,
        Timeover,
        Num,
    }

    /// <summary>
    /// アニメーターのステート
    /// </summary>
    public struct AstateRecord
    {
        public string breadCrumb;
        public string name;

        public AstateRecord(string breadCrumb, string name)
        {
            this.breadCrumb = breadCrumb;
            this.name = name;
        }
    }

    public abstract class AstateDatabase
    {
        static AstateDatabase()
        {
            index_to_record = new Dictionary<AstateIndex, AstateRecord>()
            {
                {AstateIndex.Stay, new AstateRecord(  "Base Layer.", "Stay")},
                {AstateIndex.Move, new AstateRecord(  "Base Layer.", "Move")},
                {AstateIndex.Ready, new AstateRecord(  "Base Layer.", "Ready")},
                {AstateIndex.Timeover, new AstateRecord(  "Base Layer.", "Timeover")},
            };
        }
        /// <summary>
        /// Animator の state の hash を、state番号に変換☆
        /// </summary>
        public static Dictionary<int, AstateIndex> hash_to_index;
        /// <summary>
        /// Animator の state の名前と、AnimationClipの種類の対応☆　手作業で入力しておく（２度手間）
        /// </summary>
        public static Dictionary<AstateIndex, AstateRecord> index_to_record;
        /// <summary>
        /// シーンの Start( )メソッドで呼び出してください。
        /// </summary>
        public static void InsertAllStates()
        {
            hash_to_index = new Dictionary<int, AstateIndex>();

            for (int iAstate = 0; iAstate < (int)AstateIndex.Num; iAstate++)
            {
                AstateRecord astate = index_to_record[(AstateIndex)iAstate];
                hash_to_index.Add(Animator.StringToHash(astate.breadCrumb + astate.name), (AstateIndex)iAstate);
            }
        }

        /// <summary>
        /// 現在のアニメーター・ステートに対応したデータを取得。
        /// </summary>
        /// <returns></returns>
        public static AstateRecord GetCurrentAstateRecord(Animator animator)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (!AstateDatabase.hash_to_index.ContainsKey(state.fullPathHash))
            {
                throw new UnityException("もしかして列挙型に登録していないステートがあるんじゃないか☆（＾～＾）？　ハッシュは[" + state.fullPathHash + "]だぜ☆");
            }

            AstateIndex index = AstateDatabase.hash_to_index[state.fullPathHash];

            if (AstateDatabase.index_to_record.ContainsKey(index))
            {
                return AstateDatabase.index_to_record[index];
            }

            throw new UnityException("[" + index + "]のデータが無いぜ☆　なんでだろな☆？（＾～＾）");
        }
    }

}
