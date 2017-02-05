/// <summary>
/// Step 1 to 8 here.
/// Step 9 Assets/Scripts/StellaQLEngine/UserDefinedDatabase.cs
/// 
/// 参考：命名規則: https://docs.unity3d.com/ja/current/Manual/Splittinganimations.html
/// 参考：命名規則：http://tsubakit1.hateblo.jp/entry/2015/02/03/232316
/// </summary>
namespace StellaQL.Acons.Demo_Zoo
{
    /// <summary>
    /// (Step 3.) Click [Generate fullpath constant C#] button. and "using StellaQL.FullpathConst;". ([Generate fullpath constant C#]ボタンをクリックしてください)
    /// 
    /// (Step 4.) Please, create table definition of statemachines or states. (ステートマシン、ステートのテーブル定義を作成してください)
    /// Extend generated class. ([Generate fullpath constant C#]ボタンで作ったクラスを継承してください)
    /// </summary>
    public class AControl : Demo_Zoo_AbstractAControl
    {
        /// <summary>
        /// (Step 8.) Please, make singleton. (シングルトンにしてください)
        /// Use by StellaQLEngine/UserDefinedDatabase.cs file. (StellaQLEngine/UserDefinedDatabase.cs ファイルで使います)
        /// </summary>
        static AControl() { Instance = new AControl(); }
        public static AControl Instance { get; private set; }

        #region (Step 5.) Unfortunaly, Please, list user defined tags for StellaQL.  (残念ですが、StellaQL用のユーザー定義タグを定数にしてください)
        public const string TAG_ZERO = "Zero";
        public const string TAG_ALPHA = "Alpha";
        public const string TAG_BETA = "Beta";
        public const string TAG_CEE = "Cee";
        public const string TAG_DEE = "Dee";
        public const string TAG_EEE = "Eee";
        public const string TAG_HORN = "Horn";
        #endregion

        AControl()
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
            SetTag(BASELAYER_           , new[] { TAG_ZERO });

            // 別のケースの例。
            // もし独自のプロパティーがあって初期化したい場合は、レコードごと上書きしてください。
            Set(new DefaultAcState(BASELAYER_FOO, new[] { TAG_ZERO }));

            SetTag(BASELAYER_ANYSTATE   , new[] { TAG_ZERO });// 青緑色の[Any State]とは違って、灰色の[Any State]
            SetTag(BASELAYER_ENTRY      , new[] { TAG_ZERO });// 緑色の[Entry]とは違って、灰色の[Entry]
            SetTag(BASELAYER_EXIT       , new[] { TAG_ZERO });// 赤色の[Exit]とは違って、灰色の[Exit]
            SetTag(BASELAYER_ALPACA     , new[] { TAG_ALPHA, TAG_CEE });
            SetTag(BASELAYER_BEAR       , new[] { TAG_ALPHA, TAG_BETA, TAG_EEE });
            SetTag(BASELAYER_CAT        , new[] { TAG_ALPHA, TAG_CEE });
            SetTag(BASELAYER_DOG        , new[] { TAG_DEE });
            SetTag(BASELAYER_ELEPHANT   , new[] { TAG_ALPHA, TAG_EEE });
            SetTag(BASELAYER_FOX        , new[] { TAG_ZERO });
            SetTag(BASELAYER_GIRAFFE    , new[] { TAG_ALPHA, TAG_EEE });
            SetTag(BASELAYER_HORSE      , new[] { TAG_EEE });
            SetTag(BASELAYER_IGUANA     , new[] { TAG_ALPHA });
            SetTag(BASELAYER_JELLYFISH  , new[] { TAG_EEE });
            SetTag(BASELAYER_KANGAROO   , new[] { TAG_ALPHA });
            SetTag(BASELAYER_LION       , new[] { TAG_ZERO });
            SetTag(BASELAYER_MONKEY     , new[] { TAG_EEE });
            SetTag(BASELAYER_NUTRIA     , new[] { TAG_ALPHA });
            SetTag(BASELAYER_OX         , new[] { TAG_HORN });
            SetTag(BASELAYER_PIG        , new[] { TAG_ZERO });
            SetTag(BASELAYER_QUETZAL    , new[] { TAG_ALPHA, TAG_EEE });
            SetTag(BASELAYER_RABBIT     , new[] { TAG_ALPHA, TAG_BETA });
            SetTag(BASELAYER_SHEEP      , new[] { TAG_EEE });
            SetTag(BASELAYER_TIGER      , new[] { TAG_EEE });
            SetTag(BASELAYER_UNICORN    , new[] { TAG_CEE, TAG_HORN });
            SetTag(BASELAYER_VIXEN      , new[] { TAG_EEE });
            SetTag(BASELAYER_WOLF       , new[] { TAG_ZERO });
            SetTag(BASELAYER_XENOPUS    , new[] { TAG_EEE });
            SetTag(BASELAYER_YAK        , new[] { TAG_ALPHA, TAG_HORN });
            SetTag(BASELAYER_ZEBRA      , new[] { TAG_ALPHA, TAG_BETA, TAG_EEE });
            #endregion
        }
    }
}
