using StellaQL;
using StellaQL.FullpathConst;
using System.Collections.Generic;

/// <summary>
/// Step 1 to 8 here.
/// Step 9 Assets/Scripts/StellaQLEngine/UserDefinedDatabase.cs
/// </summary>
namespace SceneStellaQLTest
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
        /// <param name="fullpathHash">ステートマシン名、ステート名のフルパスのハッシュ</param>
        /// <param name="userDefinedTags_hash">StellaQL用のユーザー定義タグのハッシュ</param>
        public UserDefinedStateRecord(string fullpath, string[] userDefinedTags) : base(fullpath, userDefinedTags)
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
        public const string TAG_ALPHA = "Alpha";
        public const string TAG_BETA = "Beta";
        public const string TAG_CEE = "Cee";
        public const string TAG_DEE = "Dee";
        public const string TAG_EEE = "Eee";
        public const string TAG_HORN = "Horn";
        #endregion

        UserDefinedStateTable()
        {
            #region (Step 6.) Activate user defined tags. (ユーザー定義タグの有効化)
            TagString_to_hash = Code.HashesDic(new []{
                TAG_ZERO,
                TAG_ALPHA,
                TAG_BETA,
                TAG_CEE,
                TAG_DEE,
                TAG_EEE,
                TAG_HORN,
            });
            #endregion

            #region (Step 7.) Register and activate user defined record of statemachines or states.(ステートマシン、ステートのユーザー定義レコードを設定してください)
            Code.Register(StateHash_to_record, new List<UserDefindStateRecordable>()
            {
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_,            new []{TAG_ZERO}),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_FOO,         new []{TAG_ZERO}),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_ANYSTATE,    new []{TAG_ZERO}),// 青緑色の[Any State]とは違って、灰色の[Any State]
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_ENTRY,       new []{TAG_ZERO}),// 緑色の[Entry]とは違って、灰色の[Entry]
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_EXIT,        new []{TAG_ZERO}),// 赤色の[Exit]とは違って、灰色の[Exit]
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_ALPACA,      new []{TAG_ALPHA, TAG_CEE}),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_BEAR,        new []{TAG_ALPHA, TAG_BETA, TAG_EEE}),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_CAT,         new []{TAG_ALPHA, TAG_CEE }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_DOG,         new []{TAG_DEE }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_ELEPHANT,    new []{TAG_ALPHA, TAG_EEE }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_FOX,         new []{TAG_ZERO }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_GIRAFFE,     new []{TAG_ALPHA, TAG_EEE}),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_HORSE,       new []{TAG_EEE }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_IGUANA,      new []{TAG_ALPHA }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_JELLYFISH,   new []{TAG_EEE }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_KANGAROO,    new []{TAG_ALPHA }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_LION,        new []{TAG_ZERO }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_MONKEY,      new []{TAG_EEE }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_NUTRIA,      new []{TAG_ALPHA }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_OX,          new []{TAG_HORN }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_PIG,         new []{TAG_ZERO }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_QUETZAL,     new []{TAG_ALPHA, TAG_EEE }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_RABBIT,      new []{TAG_ALPHA, TAG_BETA }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_SHEEP,       new []{TAG_EEE }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_TIGER,       new []{TAG_EEE }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_UNICORN,     new []{TAG_CEE, TAG_HORN}),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_VIXEN,       new []{TAG_EEE }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_WOLF,        new []{TAG_ZERO }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_XENOPUS,     new []{TAG_EEE }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_YAK,         new []{TAG_ALPHA, TAG_HORN }),
                new UserDefinedStateRecord(  ANICON_STELLAQL.BASELAYER_ZEBRA,       new []{TAG_ALPHA, TAG_BETA, TAG_EEE }),
            });
            #endregion
        }
    }
}
