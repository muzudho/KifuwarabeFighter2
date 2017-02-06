using SceneMain;

/// <summary>
/// Main シーン
/// </summary>
namespace StellaQL.Acons.Main_Char3
{
    /// <summary>
    /// (Step 1.) Please, create record definition of statemachine or state. (ステートマシン、ステートのユーザー定義データ構造)
    /// Extend AbstractUserDefinedStateRecord class. (AbstractUserDefinedStateRecordクラスを継承してください)
    /// </summary>
    public class AcState : AbstractAcState
    {
        /// <summary>
        /// (Step 2.) Initialize record. (レコードの初期設定)
        /// Use super class constructor. Required fullpath of statemachine or state.
        /// empty string array is OK for userDefinedTags. new string[]{}; Other parameters is option.
        /// (スーパークラスのコンストラクタを使います。必要なのはステートマシン名またはステート名のフルパスです。
        /// ユーザー定義タグは空セットで構いません。 new string[]{};　その他の引数は任意)
        /// </summary>
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        /// <param name="cliptype">アニメーションの種類</param>
        /// <param name="userDefinedTags_hash">StellaQL用のユーザー定義タグのハッシュ</param>
        public AcState(string fullpath, CliptypeIndex cliptype, string[] userDefinedTags)
            :base(fullpath, userDefinedTags)
        {
            this.Cliptype = (int)cliptype;
        }
    }

    /// <summary>
    /// (Step 3.) Click [Generate fullpath constant C#] button. and "using StellaQL.FullpathConst;". ([Generate fullpath constant C#]ボタンをクリックしてください)
    /// 
    /// (Step 4.) Please, create table definition of statemachines or states. (ステートマシン、ステートのテーブル定義を作成してください)
    /// Extend generated class. ([Generate fullpath constant C#]ボタンで作ったクラスを継承してください)
    /// </summary>
    public class AControl : Main_Char3_AbstractAControl
    {
        /// <summary>
        /// (Step 8.) Please, make singleton. (シングルトンにしてください)
        /// Use by Assets/StellaQL/UserDefinedDatabase.cs file. (Assets/StellaQL/UserDefinedDatabase.cs ファイルで使います)
        /// </summary>
        static AControl() { Instance = new AControl(); }
        public static AControl Instance { get; private set; }

        #region (Step 5.) Unfortunaly, Please, list user defined tags for StellaQL.  (残念ですが、StellaQL用のユーザー定義タグを定数にしてください)
        public const string TAG_NONE = "None";
        public const string TAG_BUSYX = "BusyX";
        public const string TAG_BUSYY = "BusyY";
        public const string TAG_STAND = "Stand";
        public const string TAG_JUMP = "Jump";
        public const string TAG_DASH = "Dash";
        public const string TAG_CROUCH = "Crouch";
        public const string TAG_OTHER = "Other";
        public const string TAG_LIGHT = "Light";
        public const string TAG_MEDIUM = "Medium";
        public const string TAG_HARD = "Hard";
        public const string TAG_PUNCH = "Punch";
        public const string TAG_KICK = "Kick";
        public const string TAG_BLOCK = "Block";
        public const string TAG_DAMAGE = "Damage";
        #endregion

        AControl()
        {
            #region (Step 6.) Activate user defined tags. (ユーザー定義タグの有効化)
            TagString_to_hash = Code.HashesDic(new []{
                TAG_NONE,
                TAG_BUSYX,
                TAG_BUSYY,
                TAG_STAND,
                TAG_JUMP,
                TAG_DASH,
                TAG_CROUCH,
                TAG_OTHER,
                TAG_LIGHT,
                TAG_MEDIUM,
                TAG_HARD,
                TAG_PUNCH,
                TAG_KICK,
                TAG_BLOCK,
                TAG_DAMAGE,
            });
            #endregion

            #region (Step 7.) You can set user defined tags. (ユーザー定義タグを設定することができます)
            Set(new AcState(BASELAYER_, (CliptypeIndex)0, new[] { TAG_NONE }));
            Set(new AcState(BASELAYER_SWAIT, CliptypeIndex.SWait, new[] { TAG_STAND }));
            Set(new AcState(BASELAYER_SMOVE, CliptypeIndex.SMove, new[] { TAG_STAND }));
            Set(new AcState(BASELAYER_SATKLP, CliptypeIndex.SAtkLP, new[] { TAG_BUSYX, TAG_STAND, TAG_LIGHT, TAG_PUNCH }));
            Set(new AcState(BASELAYER_SATKMP, CliptypeIndex.SAtkMP, new[] { TAG_BUSYX, TAG_STAND, TAG_MEDIUM, TAG_PUNCH }));
            Set(new AcState(BASELAYER_SATKHP, CliptypeIndex.SAtkHP, new[] { TAG_BUSYX, TAG_STAND, TAG_HARD, TAG_PUNCH }));
            Set(new AcState(BASELAYER_SATKLK, CliptypeIndex.SAtkLK, new[] { TAG_BUSYX, TAG_STAND, TAG_LIGHT, TAG_KICK }));
            Set(new AcState(BASELAYER_SATKMK, CliptypeIndex.SAtkMK, new[] { TAG_BUSYX, TAG_STAND, TAG_MEDIUM, TAG_KICK }));
            Set(new AcState(BASELAYER_SATKHK, CliptypeIndex.SAtkHK, new[] { TAG_BUSYX, TAG_STAND, TAG_HARD, TAG_KICK }));
            Set(new AcState(BASELAYER_SBLOCKL, CliptypeIndex.SBlockL, new[] { TAG_BUSYX, TAG_STAND, TAG_LIGHT, TAG_BLOCK }));
            Set(new AcState(BASELAYER_SBLOCKM, CliptypeIndex.SBlockM, new[] { TAG_BUSYX, TAG_STAND, TAG_MEDIUM, TAG_BLOCK }));
            Set(new AcState(BASELAYER_SBLOCKH, CliptypeIndex.SBlockH, new[] { TAG_BUSYX, TAG_STAND, TAG_HARD, TAG_BLOCK }));
            Set(new AcState(BASELAYER_SDAMAGEL, CliptypeIndex.SDamageL, new[] { TAG_NONE, TAG_STAND, TAG_LIGHT, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_SDAMAGEM, CliptypeIndex.SDamageM, new[] { TAG_NONE, TAG_STAND, TAG_MEDIUM, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_SDAMAGEH, CliptypeIndex.SDamageH, new[] { TAG_NONE, TAG_STAND, TAG_HARD, TAG_DAMAGE }));

            Set(new AcState(BASELAYER_JMOVE_, (CliptypeIndex)0, new[] { TAG_JUMP }));
            Set(new AcState(BASELAYER_JMOVE_JMOVE0, CliptypeIndex.JMove0, new[] { TAG_BUSYX, TAG_BUSYY, TAG_JUMP }));
            Set(new AcState(BASELAYER_JMOVE_JMOVE1, CliptypeIndex.JMove1, new[] { TAG_JUMP }));
            Set(new AcState(BASELAYER_JMOVE_JMOVE2, CliptypeIndex.JMove2, new[] { TAG_JUMP }));
            Set(new AcState(BASELAYER_JMOVE_JMOVE3, CliptypeIndex.JMove3, new[] { TAG_JUMP }));
            Set(new AcState(BASELAYER_JMOVE_JMOVE4, CliptypeIndex.JMove4, new[] { TAG_BUSYX, TAG_JUMP }));
            Set(new AcState(BASELAYER_JATKLP, CliptypeIndex.JAtkLP, new[] { TAG_JUMP, TAG_LIGHT, TAG_PUNCH }));
            Set(new AcState(BASELAYER_JATKMP, CliptypeIndex.JAtkMP, new[] { TAG_JUMP, TAG_MEDIUM, TAG_PUNCH }));
            Set(new AcState(BASELAYER_JATKHP, CliptypeIndex.JAtkHP, new[] { TAG_JUMP, TAG_HARD, TAG_PUNCH }));
            Set(new AcState(BASELAYER_JATKLK, CliptypeIndex.JAtkLK, new[] { TAG_JUMP, TAG_LIGHT, TAG_KICK }));
            Set(new AcState(BASELAYER_JATKMK, CliptypeIndex.JAtkMK, new[] { TAG_JUMP, TAG_MEDIUM, TAG_KICK }));
            Set(new AcState(BASELAYER_JATKHK, CliptypeIndex.JAtkHK, new[] { TAG_JUMP, TAG_HARD, TAG_KICK }));
            Set(new AcState(BASELAYER_JBLOCKL, CliptypeIndex.JBlockL, new[] { TAG_BUSYX, TAG_JUMP, TAG_LIGHT, TAG_BLOCK }));
            Set(new AcState(BASELAYER_JBLOCKM, CliptypeIndex.JBlockM, new[] { TAG_BUSYX, TAG_JUMP, TAG_MEDIUM, TAG_BLOCK }));
            Set(new AcState(BASELAYER_JBLOCKH, CliptypeIndex.JBlockH, new[] { TAG_BUSYX, TAG_JUMP, TAG_HARD, TAG_BLOCK }));
            Set(new AcState(BASELAYER_JDAMAGEL, CliptypeIndex.JDamageL, new[] { TAG_JUMP, TAG_LIGHT, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_JDAMAGEM, CliptypeIndex.JDamageM, new[] { TAG_JUMP, TAG_MEDIUM, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_JDAMAGEH, CliptypeIndex.JDamageH, new[] { TAG_JUMP, TAG_HARD, TAG_DAMAGE }));

            Set(new AcState(BASELAYER_DMOVE, CliptypeIndex.DMove, new[] { TAG_DASH }));
            Set(new AcState(BASELAYER_DATKLP, CliptypeIndex.DAtkLP, new[] { TAG_DASH, TAG_LIGHT, TAG_PUNCH }));
            Set(new AcState(BASELAYER_DATKMP, CliptypeIndex.DAtkMP, new[] { TAG_DASH, TAG_MEDIUM, TAG_PUNCH }));
            Set(new AcState(BASELAYER_DATKHP, CliptypeIndex.DAtkHP, new[] { TAG_DASH, TAG_HARD, TAG_PUNCH }));
            Set(new AcState(BASELAYER_DATKLK, CliptypeIndex.DAtkLK, new[] { TAG_DASH, TAG_LIGHT, TAG_KICK }));
            Set(new AcState(BASELAYER_DATKMK, CliptypeIndex.DAtkMK, new[] { TAG_DASH, TAG_MEDIUM, TAG_KICK }));
            Set(new AcState(BASELAYER_DATKHK, CliptypeIndex.DAtkHK, new[] { TAG_DASH, TAG_HARD, TAG_KICK }));
            Set(new AcState(BASELAYER_DBLOCKL, CliptypeIndex.DBlockL, new[] { TAG_BUSYX, TAG_DASH, TAG_LIGHT, TAG_BLOCK }));
            Set(new AcState(BASELAYER_DBLOCKM, CliptypeIndex.DBlockM, new[] { TAG_BUSYX, TAG_DASH, TAG_MEDIUM, TAG_BLOCK }));
            Set(new AcState(BASELAYER_DBLOCKH, CliptypeIndex.DBlockH, new[] { TAG_BUSYX, TAG_DASH, TAG_HARD, TAG_BLOCK }));
            Set(new AcState(BASELAYER_DDAMAGEL, CliptypeIndex.DDamageL, new[] { TAG_DASH, TAG_LIGHT, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_DDAMAGEM, CliptypeIndex.DDamageM, new[] { TAG_DASH, TAG_MEDIUM, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_DDAMAGEH, CliptypeIndex.DDamageH, new[] { TAG_DASH, TAG_HARD, TAG_DAMAGE }));

            Set(new AcState(BASELAYER_CWAIT, CliptypeIndex.CWait, new[] { TAG_CROUCH }));
            Set(new AcState(BASELAYER_CMOVE, CliptypeIndex.CMove, new[] { TAG_CROUCH }));
            Set(new AcState(BASELAYER_CATKLP, CliptypeIndex.CAtkLP, new[] { TAG_BUSYX, TAG_CROUCH, TAG_LIGHT, TAG_PUNCH }));
            Set(new AcState(BASELAYER_CATKMP, CliptypeIndex.CAtkMP, new[] { TAG_BUSYX, TAG_CROUCH, TAG_MEDIUM, TAG_PUNCH }));
            Set(new AcState(BASELAYER_CATKHP, CliptypeIndex.CAtkHP, new[] { TAG_BUSYX, TAG_CROUCH, TAG_HARD, TAG_PUNCH }));
            Set(new AcState(BASELAYER_CATKLK, CliptypeIndex.CAtkLK, new[] { TAG_BUSYX, TAG_CROUCH, TAG_LIGHT, TAG_KICK }));
            Set(new AcState(BASELAYER_CATKMK, CliptypeIndex.CAtkMK, new[] { TAG_BUSYX, TAG_CROUCH, TAG_MEDIUM, TAG_KICK }));
            Set(new AcState(BASELAYER_CATKHK, CliptypeIndex.CAtkHK, new[] { TAG_BUSYX, TAG_CROUCH, TAG_HARD, TAG_KICK }));
            Set(new AcState(BASELAYER_CBLOCKL, CliptypeIndex.CBlockL, new[] { TAG_BUSYX, TAG_CROUCH, TAG_LIGHT, TAG_BLOCK }));
            Set(new AcState(BASELAYER_CBLOCKM, CliptypeIndex.CBlockM, new[] { TAG_BUSYX, TAG_CROUCH, TAG_MEDIUM, TAG_BLOCK }));
            Set(new AcState(BASELAYER_CBLOCKH, CliptypeIndex.CBlockH, new[] { TAG_BUSYX, TAG_CROUCH, TAG_HARD, TAG_BLOCK }));
            Set(new AcState(BASELAYER_CDAMAGEL, CliptypeIndex.CDamageL, new[] { TAG_CROUCH, TAG_LIGHT, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_CDAMAGEM, CliptypeIndex.CDamageM, new[] { TAG_CROUCH, TAG_MEDIUM, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_CDAMAGEH, CliptypeIndex.CDamageH, new[] { TAG_CROUCH, TAG_HARD, TAG_DAMAGE }));

            Set(new AcState(BASELAYER_OBACKSTEP, CliptypeIndex.OBackstep, new[] { TAG_OTHER }));
            Set(new AcState(BASELAYER_OGIVEUP, CliptypeIndex.OGiveup, new[] { TAG_OTHER }));
            Set(new AcState(BASELAYER_ODOWNSDAMAGEH, CliptypeIndex.SDamageH, new[] { TAG_BUSYX, TAG_OTHER, TAG_HARD }));
            Set(new AcState(BASELAYER_ODOWN, CliptypeIndex.ODown, new[] { TAG_BUSYX, TAG_OTHER }));
            Set(new AcState(BASELAYER_OSTANDUP, CliptypeIndex.OStandup, new[] { TAG_OTHER }));
            #endregion
        }
    }
}

