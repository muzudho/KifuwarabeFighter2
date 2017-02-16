/// <summary>
/// 参考
/// I referred to the following site.
/// 命名規則 Naming convention : https://docs.unity3d.com/ja/current/Manual/Splittinganimations.html
/// 命名規則 Naming convention : http://tsubakit1.hateblo.jp/entry/2015/02/03/232316
/// アルファベット名 Alphabet name : SlideShare「1º week」(P17 SPELLING) http://www.slideshare.net/elsa_magdalena/1-week-14841043
/// </summary>
namespace StellaQL.Acons.Demo_Zoo
{
    /// <summary>
    /// アニメーター・コントローラー１つに対応するレコード。
    /// This class corresponds to one animator controller.
    /// 
    /// 自動生成した抽象クラスを継承してください。
    /// Please inherit the automatically generated abstract class.
    /// </summary>
    public class AControl : Demo_Zoo_AbstractAControl
    {
        /// <summary>
        /// シングルトン・デザインパターンとして作っています。
        /// I am making this class as a singleton design pattern.
        /// </summary>
        static AControl() { Instance = new AControl(); }
        public static AControl Instance { get; private set; }

        #region Tags for query
        /// <summary>
        /// StellaQLのコマンドライン用タグを作ることができます。
        /// You can define tags for StellaQL query.
        /// </summary>
        public const string
            TAG_ZERO    = "Zero",
            TAG_EI      = "Ei",     // A(ei)
            TAG_BI      = "Bi",     // B(bi)
            TAG_SI      = "Si",     // C(si)
            TAG_DI      = "Di",     // D(di)
            TAG_I       = "I",      // E(i)
            TAG_EF      = "Ef",     // F(ef)
            TAG_GI      = "Gi",     // G(gi)
            TAG_EICH    = "Eich",   // H(Eich)
            TAG_AI      = "Ai",     // I(ai)
            TAG_JEI     = "Jei",    // J(Jei)
            TAG_KEI     = "Kei",    // K(kei)
            TAG_EL      = "El",     // L(el)
            TAG_EM      = "Em",     // M(em)
            TAG_EN      = "En",     // N(en)
            TAG_OU      = "Ou",     // O(Ou)
            TAG_PI      = "Pi",     // P(Pi)
            TAG_KIU     = "Kiu",    // Q(Kiu)
            TAG_AR      = "Ar",     // R(ar)
            TAG_ES      = "Es",     // S(es)
            TAG_TI      = "Ti",     // T(ti)
            TAG_IU      = "Iu",     // U(iu)
            TAG_VI      = "Vi",     // V(vi)
            TAG_DABLIU  = "Dabliu", // W(dabliu)
            TAG_EKS     = "Eks",    // X(eks)
            TAG_UAI     = "Uai",    // Y(uai)
            TAG_ZI      = "Zi",     // Z(zi)
            TAG_HORN    = "Horn",

            // カンマで終わるリストを作るために最後に置いています。使わないでください。
            // Don't use. Sentinel value for a list that ends with a comma.
            TAG_ = "";
        #endregion

        AControl() {
            #region Tags
            // あなたの定義したタグをステートに関連付けることができます
            // You can set your defined tags to state.
            SetTag(BASELAYER_           , new[] { TAG_ZERO });

            //  別のやり方の例。(Another way) もし独自のプロパティーがあって初期化したい場合は、レコードごと上書きしてください。
            // Another way. If you have your own properties and want to initialize, overwrite each record.
            Set(new DefaultAcState(BASELAYER_FOO, new[] { TAG_ZERO }));

            // 青緑色の[Any State]とは違って、灰色の[Any State]
            // Unlike the blue-green [Any State], the gray [Any State]
            SetTag(BASELAYER_ANYSTATE   , new[] { TAG_ZERO });

            // 緑色の[Entry]とは違って、灰色の[Entry]
            // Unlike green [Entry], gray [Entry]
            SetTag(BASELAYER_ENTRY      , new[] { TAG_ZERO });

            // 赤色の[Exit]とは違って、灰色の[Exit]
            // Unlike red [Exit], gray [Exit]
            SetTag(BASELAYER_EXIT       , new[] { TAG_ZERO });
            SetTag(BASELAYER_ALPACA     , new[] { TAG_EI ,TAG_SI    ,TAG_EL     , TAG_PI                                        });
            SetTag(BASELAYER_BEAR       , new[] { TAG_EI ,TAG_BI   ,TAG_I    , TAG_AR                                        });
            SetTag(BASELAYER_CAT        , new[] { TAG_EI ,TAG_SI    ,TAG_TI                                                     });
            SetTag(BASELAYER_DOG        , new[] { TAG_DI   ,TAG_GI     ,TAG_OU                                                     });
            SetTag(BASELAYER_ELEPHANT   , new[] { TAG_EI ,TAG_I    ,TAG_EICH   , TAG_EL    ,TAG_EN     ,TAG_PI ,TAG_TI         });
            SetTag(BASELAYER_FOX        , new[] { TAG_EF    ,TAG_OU     ,TAG_EKS                                                    });
            SetTag(BASELAYER_GIRAFFE    , new[] { TAG_EI ,TAG_I    ,TAG_EF     , TAG_GI    ,TAG_AI     ,TAG_AR                 });
            SetTag(BASELAYER_HORSE      , new[] { TAG_I   ,TAG_OU     ,TAG_AR     , TAG_ES                                        });
            SetTag(BASELAYER_IGUANA     , new[] { TAG_EI ,TAG_GI     ,TAG_AI     , TAG_EN    ,TAG_IU                             });
            SetTag(BASELAYER_JELLYFISH  , new[] { TAG_I   ,TAG_EF     ,TAG_EICH   , TAG_AI    ,TAG_JEI    ,TAG_EL ,TAG_ES ,TAG_UAI});
            SetTag(BASELAYER_KANGAROO   , new[] { TAG_EI ,TAG_GI     ,TAG_KEI    , TAG_EN    ,TAG_OU     ,TAG_AR                 });
            SetTag(BASELAYER_LION       , new[] { TAG_OU    ,TAG_AI     ,TAG_EL     , TAG_EN                                        });
            SetTag(BASELAYER_MONKEY     , new[] { TAG_I   ,TAG_KEI    ,TAG_EM     , TAG_EN    ,TAG_OU     ,TAG_UAI                });
            SetTag(BASELAYER_NUTRIA     , new[] { TAG_EI ,TAG_AI     ,TAG_EN     , TAG_AR    ,TAG_TI     ,TAG_IU                 });
            SetTag(BASELAYER_OX         , new[] { TAG_HORN  ,TAG_OU     ,TAG_EKS                                                    });
            SetTag(BASELAYER_PIG        , new[] { TAG_GI    ,TAG_AI     ,TAG_PI                                                     });
            SetTag(BASELAYER_QUETZAL    , new[] { TAG_EI ,TAG_I    ,TAG_EL     , TAG_KIU   ,TAG_TI     ,TAG_IU ,TAG_ZI         });
            SetTag(BASELAYER_RABBIT     , new[] { TAG_EI ,TAG_BI   ,TAG_AI     , TAG_AR    ,TAG_TI                             });
            SetTag(BASELAYER_SHEEP      , new[] { TAG_I   ,TAG_EICH   ,TAG_PI     , TAG_ES                                        });
            SetTag(BASELAYER_TIGER      , new[] { TAG_I   ,TAG_GI     ,TAG_AI     , TAG_AR    ,TAG_TI                             });
            SetTag(BASELAYER_UNICORN    , new[] { TAG_SI   ,TAG_HORN   ,TAG_AI     , TAG_EN    ,TAG_OU     ,TAG_AR ,TAG_IU         });
            SetTag(BASELAYER_VIXEN      , new[] { TAG_I   ,TAG_AI     ,TAG_EN     , TAG_VI    ,TAG_EKS                            });
            SetTag(BASELAYER_WOLF       , new[] { TAG_EF    ,TAG_OU     ,TAG_EL     , TAG_DABLIU                                    });
            SetTag(BASELAYER_XENOPUS    , new[] { TAG_I   ,TAG_EN     ,TAG_OU     , TAG_PI    ,TAG_ES     ,TAG_IU ,TAG_EKS        });
            SetTag(BASELAYER_YAK        , new[] { TAG_EI ,TAG_KEI    ,TAG_HORN   , TAG_UAI                                       });
            SetTag(BASELAYER_ZEBRA      , new[] { TAG_EI ,TAG_BI   ,TAG_I    , TAG_AR    ,TAG_ZI                             });
            // Very tired.
            #endregion
        }
    }
}
