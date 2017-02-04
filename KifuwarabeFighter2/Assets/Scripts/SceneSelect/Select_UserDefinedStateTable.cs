using StellaQL;
using StellaQL.FullpathConst;
using System.Collections.Generic;
using UnityEngine;

namespace SceneSelect
{

    /// <summary>
    /// (Step 1.) Please, create record definition of statemachine or state. (ステートマシン、ステートのユーザー定義データ構造)
    /// Extend AbstractUserDefinedStateRecord class. (AbstractUserDefinedStateRecordクラスを継承してください)
    /// </summary>
    public class UserDefinedStateRecord : AbstractUserDefinedStateRecord
    {
        /// <summary>
        /// (Step 2.) Initialize record. (レコードの初期設定)
        /// Use super class constructor. Required fullpath of statemachine or state.
        /// empty string array is OK for userDefinedTags. new string[]{}; Other parameters is option.
        /// (スーパークラスのコンストラクタを使います。必要なのはステートマシン名またはステート名のフルパスです。
        /// ユーザー定義タグは空セットで構いません。 new string[]{};　その他の引数は任意)
        /// </summary>
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        public UserDefinedStateRecord(string fullpath) :base(fullpath, new string[] { })
        {
        }
    }

    /// <summary>
    /// (Step 3.) Please, create table definition of statemachines or states. (ステートマシン、ステートのテーブル定義を作成してください)
    /// Extend AbstractUserDefinedStateTable class. (AbstractUserDefinedStateTable クラスを継承してください)
    /// </summary>
    public class UserDefinedStateTable : AbstractUserDefinedStateTable
    {
        /// <summary>
        /// (Step 8.) Please, make singleton. (シングルトンにしてください)
        /// Use by StellaQLEngine/UserDefinedDatabase.cs file. (StellaQLEngine/UserDefinedDatabase.cs ファイルで使います)
        /// </summary>
        static UserDefinedStateTable() { Instance = new UserDefinedStateTable(); }
        public static UserDefinedStateTable Instance { get; private set; }

        #region (Step 4.) Click [Generate fullpath constant C#] button. and "using StellaQL.FullpathConst;". ([Generate fullpath constant C#]ボタンをクリックしてください)
        #endregion

        #region (Step 5.) Unfortunaly, Please, list user defined tags for StellaQL.  (残念ですが、StellaQL用のユーザー定義タグを定数にしてください)
        public const string TAG_ZERO = "Zero";
        #endregion

        UserDefinedStateTable()
        {
            #region (Step 6.) Activate user defined tags. (ユーザー定義タグの有効化)
            TagString_to_hash = Code.HashesDic(new []{
                TAG_ZERO
            });
            #endregion

            #region (Step 7.) Register and activate user defined record of statemachines or states.(ステートマシン、ステートのユーザー定義レコードを設定してください)
            Code.Register(StateHash_to_record, new List<UserDefindStateRecordable>()
            {
                new UserDefinedStateRecord( ANICON_SELECT.BASELAYER_STAY),
                new UserDefinedStateRecord( ANICON_SELECT.BASELAYER_MOVE),
                new UserDefinedStateRecord( ANICON_SELECT.BASELAYER_READY),
                new UserDefinedStateRecord( ANICON_SELECT.BASELAYER_TIMEOVER),
            });
            #endregion
        }
    }

}
