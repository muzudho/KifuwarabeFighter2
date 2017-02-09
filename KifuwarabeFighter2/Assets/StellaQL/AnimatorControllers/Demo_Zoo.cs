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
        // 出典： SlideShare「1º week」(P17 SPELLING) http://www.slideshare.net/elsa_magdalena/1-week-14841043
        public const string
            TAG_ZERO = "Zero",
            TAG_ALPHA = "Alpha", // A(ei)
            TAG_BETA = "Beta", // B(bi)
            TAG_CEE = "Cee", // C(si)
            TAG_DEE = "Dee", // D(di)
            TAG_EEE = "Eee", // E(i)
            TAG_EF = "Ef", // F(ef)
            TAG_GI = "Gi", // G(gi)
            TAG_EICH = "Eich", // H(Eich)
            TAG_AI = "Ai", // I(ai)
            TAG_JEI = "Jei", // J(Jei)
            TAG_KEI = "Kei", // K(kei)
            TAG_EL = "El", // L(el)
            TAG_EM = "Em", // M(em)
            TAG_EN = "En", // N(en)
            TAG_OU = "Ou", // O(Ou)
            TAG_PI = "Pi", // P(Pi)
            TAG_KIU = "Kiu", // Q(Kiu)
            TAG_AR = "Ar", // R(ar)
            TAG_ES = "Es", // S(es)
            TAG_TI = "Ti", // T(ti)
            TAG_IU = "Iu", // U(iu)
            TAG_VI = "Vi", // V(vi)
            TAG_DABLIU = "Dabliu", // W(dabliu)
            TAG_EKS = "Eks", // X(eks)
            TAG_UAI = "Uai", // Y(uai)
            TAG_ZI = "Zi", // Z(zi)
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
            SetTag(BASELAYER_ALPACA     , new[] { TAG_ALPHA, TAG_CEE, TAG_EL, TAG_PI });
            SetTag(BASELAYER_BEAR       , new[] { TAG_ALPHA, TAG_BETA, TAG_EEE, TAG_AR });
            SetTag(BASELAYER_CAT        , new[] { TAG_ALPHA, TAG_CEE, TAG_TI });
            SetTag(BASELAYER_DOG        , new[] { TAG_DEE, TAG_GI, TAG_OU });
            SetTag(BASELAYER_ELEPHANT   , new[] { TAG_ALPHA, TAG_EEE, TAG_EICH, TAG_EL, TAG_EN, TAG_PI, TAG_TI });
            SetTag(BASELAYER_FOX        , new[] { TAG_EF, TAG_OU, TAG_EKS });
            SetTag(BASELAYER_GIRAFFE    , new[] { TAG_ALPHA, TAG_EEE, TAG_EF, TAG_GI, TAG_AI, TAG_AR });
            SetTag(BASELAYER_HORSE      , new[] { TAG_EEE, TAG_OU, TAG_AR, TAG_ES });
            SetTag(BASELAYER_IGUANA     , new[] { TAG_ALPHA, TAG_GI, TAG_AI, TAG_EN, TAG_IU });
            SetTag(BASELAYER_JELLYFISH  , new[] { TAG_EEE, TAG_EF, TAG_EICH, TAG_AI, TAG_JEI, TAG_EL, TAG_ES, TAG_UAI });
            SetTag(BASELAYER_KANGAROO   , new[] { TAG_ALPHA, TAG_GI, TAG_KEI, TAG_EN, TAG_OU, TAG_AR });
            SetTag(BASELAYER_LION       , new[] { TAG_OU, TAG_AI, TAG_EL, TAG_EN });
            SetTag(BASELAYER_MONKEY     , new[] { TAG_EEE, TAG_KEI, TAG_EM, TAG_EN, TAG_OU, TAG_UAI });
            SetTag(BASELAYER_NUTRIA     , new[] { TAG_ALPHA, TAG_AI, TAG_EN, TAG_AR, TAG_TI, TAG_IU });
            SetTag(BASELAYER_OX         , new[] { TAG_HORN, TAG_OU, TAG_EKS });
            SetTag(BASELAYER_PIG        , new[] { TAG_GI, TAG_AI, TAG_PI });
            SetTag(BASELAYER_QUETZAL    , new[] { TAG_ALPHA, TAG_EEE, TAG_EL, TAG_KIU, TAG_TI, TAG_IU, TAG_ZI });
            SetTag(BASELAYER_RABBIT     , new[] { TAG_ALPHA, TAG_BETA, TAG_AI, TAG_AR, TAG_TI });
            SetTag(BASELAYER_SHEEP      , new[] { TAG_EEE, TAG_EICH, TAG_PI, TAG_ES });
            SetTag(BASELAYER_TIGER      , new[] { TAG_EEE, TAG_GI, TAG_AI, TAG_AR, TAG_TI });
            SetTag(BASELAYER_UNICORN    , new[] { TAG_CEE, TAG_HORN, TAG_AI, TAG_EN, TAG_OU, TAG_AR, TAG_IU });
            SetTag(BASELAYER_VIXEN      , new[] { TAG_EEE, TAG_AI, TAG_EN, TAG_VI, TAG_EKS });
            SetTag(BASELAYER_WOLF       , new[] { TAG_EF, TAG_OU, TAG_EL, TAG_DABLIU });
            SetTag(BASELAYER_XENOPUS    , new[] { TAG_EEE, TAG_EN, TAG_OU, TAG_PI, TAG_ES, TAG_IU, TAG_EKS });
            SetTag(BASELAYER_YAK        , new[] { TAG_ALPHA, TAG_KEI, TAG_HORN, TAG_UAI });
            SetTag(BASELAYER_ZEBRA      , new[] { TAG_ALPHA, TAG_BETA, TAG_EEE, TAG_AR, TAG_ZI });
            #endregion
        }
    }
}
