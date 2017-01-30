using StellaQL;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main シーン
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

        #region (Step 4.) Unfortunaly, Please, list fullpath of statemachines of states.  (残念ですが、ステートマシン、ステートのフルパスを定数にしてください)
        public const string STATEMACHINE_BASELAYER = "Base Layer";
        public const string STATE_FOO = "Base Layer.Foo";
        public const string STATE_ALPACA = "Base Layer.Alpaca";
        public const string STATE_BEAR = "Base Layer.Bear";
        public const string STATE_CAT = "Base Layer.Cat";
        public const string STATE_DOG = "Base Layer.Dog";
        public const string STATE_ELEPHANT = "Base Layer.Elephant";
        public const string STATE_FOX = "Base Layer.Fox";
        public const string STATE_GIRAFFE = "Base Layer.Giraffe";
        public const string STATE_HORSE = "Base Layer.Horse";
        public const string STATE_IGUANA = "Base Layer.Iguana";
        public const string STATE_JELLYFISH = "Base Layer.Jellyfish";
        public const string STATE_KANGAROO = "Base Layer.Kangaroo";
        public const string STATE_LION = "Base Layer.Lion";
        public const string STATE_MONKEY = "Base Layer.Monkey";
        public const string STATE_NUTRIA = "Base Layer.Nutria";
        public const string STATE_OX = "Base Layer.Ox";
        public const string STATE_PIG = "Base Layer.Pig";
        public const string STATE_QUETZAL = "Base Layer.Quetzal";
        public const string STATE_RABBIT = "Base Layer.Rabbit";
        public const string STATE_SHEEP = "Base Layer.Sheep";
        public const string STATE_TIGER = "Base Layer.Tiger";
        public const string STATE_UNICORN = "Base Layer.Unicorn";
        public const string STATE_VIXEN = "Base Layer.Vixen";
        public const string STATE_WOLF = "Base Layer.Wolf";
        public const string STATE_XENOPUS = "Base Layer.Xenopus";
        public const string STATE_YAK = "Base Layer.Yak";
        public const string STATE_ZEBRA = "Base Layer.Zebra";
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
                new UserDefinedStateRecord(  STATEMACHINE_BASELAYER,         new []{TAG_ZERO}),
                new UserDefinedStateRecord(  STATE_FOO,         new []{TAG_ZERO}),
                new UserDefinedStateRecord(  STATE_ALPACA,      new []{TAG_ALPHA, TAG_CEE}),
                new UserDefinedStateRecord(  STATE_BEAR,        new []{TAG_ALPHA, TAG_BETA, TAG_EEE}),
                new UserDefinedStateRecord(  STATE_CAT,         new []{TAG_ALPHA, TAG_CEE }),
                new UserDefinedStateRecord(  STATE_DOG,         new []{TAG_DEE }),
                new UserDefinedStateRecord(  STATE_ELEPHANT,    new []{TAG_ALPHA, TAG_EEE }),
                new UserDefinedStateRecord(  STATE_FOX,         new []{TAG_ZERO }),
                new UserDefinedStateRecord(  STATE_GIRAFFE,     new []{TAG_ALPHA, TAG_EEE}),
                new UserDefinedStateRecord(  STATE_HORSE,       new []{TAG_EEE }),
                new UserDefinedStateRecord(  STATE_IGUANA,      new []{TAG_ALPHA }),
                new UserDefinedStateRecord(  STATE_JELLYFISH,   new []{TAG_EEE }),
                new UserDefinedStateRecord(  STATE_KANGAROO,    new []{TAG_ALPHA }),
                new UserDefinedStateRecord(  STATE_LION,        new []{TAG_ZERO }),
                new UserDefinedStateRecord(  STATE_MONKEY,      new []{TAG_EEE }),
                new UserDefinedStateRecord(  STATE_NUTRIA,      new []{TAG_ALPHA }),
                new UserDefinedStateRecord(  STATE_OX,          new []{TAG_HORN }),
                new UserDefinedStateRecord(  STATE_PIG,         new []{TAG_ZERO }),
                new UserDefinedStateRecord(  STATE_QUETZAL,     new []{TAG_ALPHA, TAG_EEE }),
                new UserDefinedStateRecord(  STATE_RABBIT,      new []{TAG_ALPHA, TAG_BETA }),
                new UserDefinedStateRecord(  STATE_SHEEP,       new []{TAG_EEE }),
                new UserDefinedStateRecord(  STATE_TIGER,       new []{TAG_EEE }),
                new UserDefinedStateRecord(  STATE_UNICORN,     new []{TAG_CEE, TAG_HORN}),
                new UserDefinedStateRecord(  STATE_VIXEN,       new []{TAG_EEE }),
                new UserDefinedStateRecord(  STATE_WOLF,        new []{TAG_ZERO }),
                new UserDefinedStateRecord(  STATE_XENOPUS,     new []{TAG_EEE }),
                new UserDefinedStateRecord(  STATE_YAK,         new []{TAG_ALPHA, TAG_HORN }),
                new UserDefinedStateRecord(  STATE_ZEBRA,       new []{TAG_ALPHA, TAG_BETA, TAG_EEE }),
            });
            #endregion
        }
    }
}
