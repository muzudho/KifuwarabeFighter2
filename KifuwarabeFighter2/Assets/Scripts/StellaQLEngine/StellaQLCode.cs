using System.Collections.Generic;
using UnityEngine;

namespace StellaQL
{
    /// <summary>
    /// コーディング量を減らしたいときに使うクラス。
    /// </summary>
    public abstract class Code
    {
        public static Dictionary<string, int> HashsDic(string[] strings)
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
        public static HashSet<int> Hashs(string[] strings)
        {
            HashSet<int> hashSet = new HashSet<int>();
            foreach (string str in strings)
            {
                hashSet.Add(Animator.StringToHash(str));
            }
            return hashSet;
        }
    }
}
