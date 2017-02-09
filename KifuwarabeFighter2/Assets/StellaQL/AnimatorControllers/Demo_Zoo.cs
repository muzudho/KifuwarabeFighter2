/// <summary>
/// Step 1 to 8 here.
/// Step 9 Assets/StellaQL/UserDefinedDatabase.cs
/// 
/// 参考：命名規則: https://docs.unity3d.com/ja/current/Manual/Splittinganimations.html
/// 参考：命名規則：http://tsubakit1.hateblo.jp/entry/2015/02/03/232316
/// </summary>
namespace StellaQL.Acons.Demo_Zoo
{
    /// <summary>
    /// (Step 3.) Click [Generate C# (Fullpath of all states)] button. ([Generate C# (Fullpath of all states)]ボタンをクリックしてください)
    /// (Step 4.) Please, create AControl class extends generated XXXX_YYYY_AbstractAControl class. ([Generate fullpath constant C#]ボタンで作ったクラスを継承してください)
    /// </summary>
    public class AControl : Demo_Zoo_AbstractAControl
    {
        /// <summary>
        /// (Step 8.) Please, make singleton. Use by Assets/StellaQL/UserDefinedDatabase.cs file. (シングルトンにしてください。Assets/StellaQL/UserDefinedDatabase.cs ファイルで使います)
        /// </summary>
        static AControl() { Instance = new AControl(); }
        public static AControl Instance { get; private set; }

        #region (Step 5.) You can defined tags for StellaQL command line.  (StellaQLのコマンドライン用タグを作ることができます)
        public const string
            TAG_ZERO = "Zero",
            TAG_ALPHA = "Alpha",
            TAG_BETA = "Beta",
            TAG_CEE = "Cee",
            TAG_DEE = "Dee",
            TAG_EEE = "Eee",
            TAG_HORN = "Horn",
            TAG_ = ""; // Don't use. Sentinel value for a list that ends with a comma. カンマで終わるリストを作るために置いてあるぜ☆（＾～＾）使うなだぜ☆（＾～＾）
        #endregion

        AControl() {
            #region (Step 6.) Activate user defined tags. (ユーザー定義タグの有効化)
            TagString_to_hash = Code.HashesDic(new []{  TAG_ZERO,   TAG_ALPHA,  TAG_BETA,   TAG_CEE,    TAG_DEE,    TAG_EEE,    TAG_HORN,   });
            #endregion

            #region (Step 7.) You can set your defined tags to state. (あなたの定義したタグをステートに関連付けることができます)
            SetTag(BASELAYER_           , new[] { TAG_ZERO });

            // 別のやり方の例。(Another way) もし独自のプロパティーがあって初期化したい場合は、レコードごと上書きしてください。
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
