using StellaQL;
using System.Collections.Generic;
using UnityEngine;

namespace SceneSelect
{

    /// <summary>
    /// ステートマシン、ステートの拡張データ構造
    /// </summary>
    public class UserDefinedStateRecord : AbstractUserDefinedStateRecord
    {
        /// <summary>
        /// データ入力用
        /// </summary>
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        /// <returns></returns>
        public static UserDefinedStateRecord Build(string fullpath)
        {
            return new UserDefinedStateRecord(fullpath, Animator.StringToHash(fullpath));
        }

        /// <summary>
        /// データ
        /// </summary>
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        /// <param name="fullpathHash">ステートマシン名、ステート名のフルパスのハッシュ</param>
        public UserDefinedStateRecord(string fullpath, int fullpathHash) :base(fullpath, fullpathHash, new HashSet<int>() { })
        {
        }
    }

    /// <summary>
    /// ステートマシン、ステートの拡張データ構造の集まり
    /// </summary>
    public class UserDefinedStateTable : AbstractUserDefinedStateTable
    {
        static UserDefinedStateTable() { Instance = new UserDefinedStateTable(); }
        public static UserDefinedStateTable Instance { get; set; }

        #region ステートマシン、ステート　フルパス一覧
        public const string STATE_STAY = "Base Layer.Stay";
        public const string STATE_MOVE = "Base Layer.Move";
        public const string STATE_READY = "Base Layer.Ready";
        public const string STATE_TIMEOVER = "Base Layer.Timeover";
        #endregion

        #region StellaQL用のユーザー定義タグ一覧
        public const string TAG_ZERO = "Zero";
        #endregion

        protected UserDefinedStateTable()
        {
            #region タグの有効化
            TagString_to_hash = Code.HashesDic(new []{
                TAG_ZERO
            });
            #endregion

            #region ステートマシン拡張データ、ステート拡張データの有効化
            List<UserDefindStateRecordable> temp = new List<UserDefindStateRecordable>()
            {
                UserDefinedStateRecord.Build( STATE_STAY),
                UserDefinedStateRecord.Build( STATE_MOVE),
                UserDefinedStateRecord.Build( STATE_READY),
                UserDefinedStateRecord.Build( STATE_TIMEOVER),
            };
            foreach (UserDefindStateRecordable record in temp) { StateHash_to_record.Add(record.FullPathHash, record); }
            #endregion
        }
    }

}
