using System.Collections.Generic;
using UnityEngine;

namespace DojinCircleGrayscale.StellaQL
{
    /// <summary>
    /// コーディング量を減らしたいときに使うクラス。
    /// </summary>
    public abstract class Code
    {
        /// <summary>
        /// 文字列の配列を、連想配列に変換する。
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static Dictionary<string, int> HashesDic(string[] strings)
        {
            Dictionary<string, int> string_to_tagHash = new Dictionary<string, int>();
            foreach (string str in strings)
            {
                string_to_tagHash.Add(str, Animator.StringToHash(str));
            }
            return string_to_tagHash;
        }

        /// <summary>
        /// 文字列の配列を、ハッシュセットに変換する。
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static HashSet<int> Hashes(string[] strings)
        {
            HashSet<int> hashSet = new HashSet<int>();
            foreach (string str in strings)
            {
                hashSet.Add(Animator.StringToHash(str));
            }
            return hashSet;
        }

        /// <summary>
        /// コレクションを変更します。
        /// </summary>
        public static void Register(Dictionary<int, AcStateRecordable> stateHash_to_record, List<AcStateRecordable> temp)
        {
            foreach (AcStateRecordable record in temp) { stateHash_to_record.Add(record.FullPathHash, record); }
        }
    }
}
