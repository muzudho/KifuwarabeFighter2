using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface AstateRecordable
{
    string BreadCrumb { get; }
    string Name { get; }
    bool HasFlag(int enumration);
    /// <summary>
    /// ほんとは列挙型にしたい☆
    /// </summary>
    int AttributeEnum { get; }
}

public abstract class AbstractAstateRecord : AstateRecordable
{
    public string BreadCrumb { get; set; }
    public string Name { get; set; }
    public abstract bool HasFlag(int enumration);
    /// <summary>
    /// ほんとは列挙型にしたい☆
    /// </summary>
    public int AttributeEnum { get; set; }
}

public abstract class AbstractAstateDatabase
{
    /// <summary>
    /// Animator の state の名前と、AnimationClipの種類の対応☆　手作業で入力しておく（２度手間）
    /// </summary>
    public Dictionary<int, AstateRecordable> index_to_record;//AstateIndex
    /// <summary>
    /// Animator の state の hash を、state番号に変換☆
    /// </summary>
    public Dictionary<int, int> hash_to_index;//<hash,AstateIndex>


    /// <summary>
    /// シーンの Start( )メソッドで呼び出してください。
    /// </summary>
    public void InsertAllStates()
    {
        hash_to_index = new Dictionary<int, int>(); // <hash,AstateIndex>

        for (int iAstate = 0; iAstate < index_to_record.Count; iAstate++)
        {
            AstateRecordable astate = index_to_record[iAstate]; // [AstateIndex]
            hash_to_index.Add(Animator.StringToHash(astate.BreadCrumb + astate.Name), iAstate); // (～,AstateIndex)
        }
    }

    /// <summary>
    /// 現在のアニメーター・ステートに対応したデータを取得。
    /// </summary>
    /// <returns></returns>
    public AstateRecordable GetCurrentAstateRecord(Animator animator)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (!this.hash_to_index.ContainsKey(state.fullPathHash))
        {
            throw new UnityException("もしかして列挙型に登録していないステートがあるんじゃないか☆（＾～＾）？　ハッシュは[" + state.fullPathHash + "]だぜ☆");
        }

        int index = this.hash_to_index[state.fullPathHash];//AstateIndex

        if (this.index_to_record.ContainsKey((int)index))
        {
            return this.index_to_record[(int)index];
        }

        throw new UnityException("[" + index + "]のデータが無いぜ☆　なんでだろな☆？（＾～＾）");
    }

    /// <summary>
    /// 属性検索
    /// </summary>
    public List<AstateRecordable> Where(int enumration_attr)
    {
        List<AstateRecordable> recordset = new List<AstateRecordable>();

        foreach (AstateRecordable record in index_to_record.Values)
        {
            if (record.HasFlag(enumration_attr)) // if(attribute.HasFlag(record.attribute))
            {
                recordset.Add(record);
            }
        }

        return recordset;
    }

}
