using StellaQL;
using StellaQL.Acons;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Step 1 to 8 here.
/// Step 9 Assets/Scripts/StellaQLEngine/UserDefinedDatabase.cs
/// </summary>
namespace SceneStellaQLTest
{
    /// <summary>
    /// (Step 3.) Click [Generate fullpath constant C#] button. and "using StellaQL.FullpathConst;". ([Generate fullpath constant C#]ボタンをクリックしてください)
    /// 
    /// (Step 4.) Please, create table definition of statemachines or states. (ステートマシン、ステートのテーブル定義を作成してください)
    /// Extend generated class. ([Generate fullpath constant C#]ボタンで作ったクラスを継承してください)
    /// </summary>
    public class AControll : AconZoo
    {
        /// <summary>
        /// (Step 8.) Please, make singleton. (シングルトンにしてください)
        /// Use by StellaQLEngine/UserDefinedDatabase.cs file. (StellaQLEngine/UserDefinedDatabase.cs ファイルで使います)
        /// </summary>
        static AControll() { Instance = new AControll(); }
        public static AControll Instance { get; private set; }

        #region (Step 5.) Unfortunaly, Please, list user defined tags for StellaQL.  (残念ですが、StellaQL用のユーザー定義タグを定数にしてください)
        public const string TAG_ZERO = "Zero";
        public const string TAG_ALPHA = "Alpha";
        public const string TAG_BETA = "Beta";
        public const string TAG_CEE = "Cee";
        public const string TAG_DEE = "Dee";
        public const string TAG_EEE = "Eee";
        public const string TAG_HORN = "Horn";
        #endregion

        AControll()
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

            #region (Step 7.) You can set user defined tags. (ユーザー定義タグを設定することができます)
            SetTag(AconZoo.BASELAYER_           , new[] { TAG_ZERO });

            // 別のケースの例。
            // もし独自のプロパティーがあって初期化したい場合は、レコードごと上書きしてください。
            Set(new DefaultAcState(AconZoo.BASELAYER_FOO, new[] { TAG_ZERO }));

            SetTag(AconZoo.BASELAYER_ANYSTATE   , new[] { TAG_ZERO });// 青緑色の[Any State]とは違って、灰色の[Any State]
            SetTag(AconZoo.BASELAYER_ENTRY      , new[] { TAG_ZERO });// 緑色の[Entry]とは違って、灰色の[Entry]
            SetTag(AconZoo.BASELAYER_EXIT       , new[] { TAG_ZERO });// 赤色の[Exit]とは違って、灰色の[Exit]
            SetTag(AconZoo.BASELAYER_ALPACA     , new[] { TAG_ALPHA, TAG_CEE });
            SetTag(AconZoo.BASELAYER_BEAR       , new[] { TAG_ALPHA, TAG_BETA, TAG_EEE });
            SetTag(AconZoo.BASELAYER_CAT        , new[] { TAG_ALPHA, TAG_CEE });
            SetTag(AconZoo.BASELAYER_DOG        , new[] { TAG_DEE });
            SetTag(AconZoo.BASELAYER_ELEPHANT   , new[] { TAG_ALPHA, TAG_EEE });
            SetTag(AconZoo.BASELAYER_FOX        , new[] { TAG_ZERO });
            SetTag(AconZoo.BASELAYER_GIRAFFE    , new[] { TAG_ALPHA, TAG_EEE });
            SetTag(AconZoo.BASELAYER_HORSE      , new[] { TAG_EEE });
            SetTag(AconZoo.BASELAYER_IGUANA     , new[] { TAG_ALPHA });
            SetTag(AconZoo.BASELAYER_JELLYFISH  , new[] { TAG_EEE });
            SetTag(AconZoo.BASELAYER_KANGAROO   , new[] { TAG_ALPHA });
            SetTag(AconZoo.BASELAYER_LION       , new[] { TAG_ZERO });
            SetTag(AconZoo.BASELAYER_MONKEY     , new[] { TAG_EEE });
            SetTag(AconZoo.BASELAYER_NUTRIA     , new[] { TAG_ALPHA });
            SetTag(AconZoo.BASELAYER_OX         , new[] { TAG_HORN });
            SetTag(AconZoo.BASELAYER_PIG        , new[] { TAG_ZERO });
            SetTag(AconZoo.BASELAYER_QUETZAL    , new[] { TAG_ALPHA, TAG_EEE });
            SetTag(AconZoo.BASELAYER_RABBIT     , new[] { TAG_ALPHA, TAG_BETA });
            SetTag(AconZoo.BASELAYER_SHEEP      , new[] { TAG_EEE });
            SetTag(AconZoo.BASELAYER_TIGER      , new[] { TAG_EEE });
            SetTag(AconZoo.BASELAYER_UNICORN    , new[] { TAG_CEE, TAG_HORN });
            SetTag(AconZoo.BASELAYER_VIXEN      , new[] { TAG_EEE });
            SetTag(AconZoo.BASELAYER_WOLF       , new[] { TAG_ZERO });
            SetTag(AconZoo.BASELAYER_XENOPUS    , new[] { TAG_EEE });
            SetTag(AconZoo.BASELAYER_YAK        , new[] { TAG_ALPHA, TAG_HORN });
            SetTag(AconZoo.BASELAYER_ZEBRA      , new[] { TAG_ALPHA, TAG_BETA, TAG_EEE });
            #endregion
        }
    }
}
