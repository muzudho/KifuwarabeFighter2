using SceneMain;

/// <summary>
/// Main シーン
/// </summary>
namespace StellaQL.Acons.AconChar3
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
    public class AControll : AbstractAconChar3
    {
        /// <summary>
        /// (Step 8.) Please, make singleton. (シングルトンにしてください)
        /// Use by StellaQLEngine/UserDefinedDatabase.cs file. (StellaQLEngine/UserDefinedDatabase.cs ファイルで使います)
        /// </summary>
        static AControll() { Instance = new AControll(); }
        public static AControll Instance { get; private set; }

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

        AControll()
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
            Set(new AcState(AbstractAconChar3.BASELAYER_, (CliptypeIndex)0, new[] { TAG_NONE }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SWAIT, CliptypeIndex.SWait, new[] { TAG_STAND }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SMOVE, CliptypeIndex.SMove, new[] { TAG_STAND }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SATKLP, CliptypeIndex.SAtkLP, new[] { TAG_BUSYX, TAG_STAND, TAG_LIGHT, TAG_PUNCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SATKMP, CliptypeIndex.SAtkMP, new[] { TAG_BUSYX, TAG_STAND, TAG_MEDIUM, TAG_PUNCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SATKHP, CliptypeIndex.SAtkHP, new[] { TAG_BUSYX, TAG_STAND, TAG_HARD, TAG_PUNCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SATKLK, CliptypeIndex.SAtkLK, new[] { TAG_BUSYX, TAG_STAND, TAG_LIGHT, TAG_KICK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SATKMK, CliptypeIndex.SAtkMK, new[] { TAG_BUSYX, TAG_STAND, TAG_MEDIUM, TAG_KICK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SATKHK, CliptypeIndex.SAtkHK, new[] { TAG_BUSYX, TAG_STAND, TAG_HARD, TAG_KICK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SBLOCKL, CliptypeIndex.SBlockL, new[] { TAG_BUSYX, TAG_STAND, TAG_LIGHT, TAG_BLOCK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SBLOCKM, CliptypeIndex.SBlockM, new[] { TAG_BUSYX, TAG_STAND, TAG_MEDIUM, TAG_BLOCK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SBLOCKH, CliptypeIndex.SBlockH, new[] { TAG_BUSYX, TAG_STAND, TAG_HARD, TAG_BLOCK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SDAMAGEL, CliptypeIndex.SDamageL, new[] { TAG_NONE, TAG_STAND, TAG_LIGHT, TAG_DAMAGE }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SDAMAGEM, CliptypeIndex.SDamageM, new[] { TAG_NONE, TAG_STAND, TAG_MEDIUM, TAG_DAMAGE }));
            Set(new AcState(AbstractAconChar3.BASELAYER_SDAMAGEH, CliptypeIndex.SDamageH, new[] { TAG_NONE, TAG_STAND, TAG_HARD, TAG_DAMAGE }));

            Set(new AcState(AbstractAconChar3.BASELAYER_JMOVE_, (CliptypeIndex)0, new[] { TAG_JUMP }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JMOVE_JMOVE0, CliptypeIndex.JMove0, new[] { TAG_BUSYX, TAG_BUSYY, TAG_JUMP }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JMOVE_JMOVE1, CliptypeIndex.JMove1, new[] { TAG_JUMP }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JMOVE_JMOVE2, CliptypeIndex.JMove2, new[] { TAG_JUMP }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JMOVE_JMOVE3, CliptypeIndex.JMove3, new[] { TAG_JUMP }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JMOVE_JMOVE4, CliptypeIndex.JMove4, new[] { TAG_BUSYX, TAG_JUMP }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JATKLP, CliptypeIndex.JAtkLP, new[] { TAG_JUMP, TAG_LIGHT, TAG_PUNCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JATKMP, CliptypeIndex.JAtkMP, new[] { TAG_JUMP, TAG_MEDIUM, TAG_PUNCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JATKHP, CliptypeIndex.JAtkHP, new[] { TAG_JUMP, TAG_HARD, TAG_PUNCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JATKLK, CliptypeIndex.JAtkLK, new[] { TAG_JUMP, TAG_LIGHT, TAG_KICK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JATKMK, CliptypeIndex.JAtkMK, new[] { TAG_JUMP, TAG_MEDIUM, TAG_KICK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JATKHK, CliptypeIndex.JAtkHK, new[] { TAG_JUMP, TAG_HARD, TAG_KICK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JBLOCKL, CliptypeIndex.JBlockL, new[] { TAG_BUSYX, TAG_JUMP, TAG_LIGHT, TAG_BLOCK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JBLOCKM, CliptypeIndex.JBlockM, new[] { TAG_BUSYX, TAG_JUMP, TAG_MEDIUM, TAG_BLOCK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JBLOCKH, CliptypeIndex.JBlockH, new[] { TAG_BUSYX, TAG_JUMP, TAG_HARD, TAG_BLOCK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JDAMAGEL, CliptypeIndex.JDamageL, new[] { TAG_JUMP, TAG_LIGHT, TAG_DAMAGE }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JDAMAGEM, CliptypeIndex.JDamageM, new[] { TAG_JUMP, TAG_MEDIUM, TAG_DAMAGE }));
            Set(new AcState(AbstractAconChar3.BASELAYER_JDAMAGEH, CliptypeIndex.JDamageH, new[] { TAG_JUMP, TAG_HARD, TAG_DAMAGE }));

            Set(new AcState(AbstractAconChar3.BASELAYER_DMOVE, CliptypeIndex.DMove, new[] { TAG_DASH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_DATKLP, CliptypeIndex.DAtkLP, new[] { TAG_DASH, TAG_LIGHT, TAG_PUNCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_DATKMP, CliptypeIndex.DAtkMP, new[] { TAG_DASH, TAG_MEDIUM, TAG_PUNCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_DATKHP, CliptypeIndex.DAtkHP, new[] { TAG_DASH, TAG_HARD, TAG_PUNCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_DATKLK, CliptypeIndex.DAtkLK, new[] { TAG_DASH, TAG_LIGHT, TAG_KICK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_DATKMK, CliptypeIndex.DAtkMK, new[] { TAG_DASH, TAG_MEDIUM, TAG_KICK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_DATKHK, CliptypeIndex.DAtkHK, new[] { TAG_DASH, TAG_HARD, TAG_KICK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_DBLOCKL, CliptypeIndex.DBlockL, new[] { TAG_BUSYX, TAG_DASH, TAG_LIGHT, TAG_BLOCK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_DBLOCKM, CliptypeIndex.DBlockM, new[] { TAG_BUSYX, TAG_DASH, TAG_MEDIUM, TAG_BLOCK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_DBLOCKH, CliptypeIndex.DBlockH, new[] { TAG_BUSYX, TAG_DASH, TAG_HARD, TAG_BLOCK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_DDAMAGEL, CliptypeIndex.DDamageL, new[] { TAG_DASH, TAG_LIGHT, TAG_DAMAGE }));
            Set(new AcState(AbstractAconChar3.BASELAYER_DDAMAGEM, CliptypeIndex.DDamageM, new[] { TAG_DASH, TAG_MEDIUM, TAG_DAMAGE }));
            Set(new AcState(AbstractAconChar3.BASELAYER_DDAMAGEH, CliptypeIndex.DDamageH, new[] { TAG_DASH, TAG_HARD, TAG_DAMAGE }));

            Set(new AcState(AbstractAconChar3.BASELAYER_CWAIT, CliptypeIndex.CWait, new[] { TAG_CROUCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CMOVE, CliptypeIndex.CMove, new[] { TAG_CROUCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CATKLP, CliptypeIndex.CAtkLP, new[] { TAG_BUSYX, TAG_CROUCH, TAG_LIGHT, TAG_PUNCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CATKMP, CliptypeIndex.CAtkMP, new[] { TAG_BUSYX, TAG_CROUCH, TAG_MEDIUM, TAG_PUNCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CATKHP, CliptypeIndex.CAtkHP, new[] { TAG_BUSYX, TAG_CROUCH, TAG_HARD, TAG_PUNCH }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CATKLK, CliptypeIndex.CAtkLK, new[] { TAG_BUSYX, TAG_CROUCH, TAG_LIGHT, TAG_KICK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CATKMK, CliptypeIndex.CAtkMK, new[] { TAG_BUSYX, TAG_CROUCH, TAG_MEDIUM, TAG_KICK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CATKHK, CliptypeIndex.CAtkHK, new[] { TAG_BUSYX, TAG_CROUCH, TAG_HARD, TAG_KICK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CBLOCKL, CliptypeIndex.CBlockL, new[] { TAG_BUSYX, TAG_CROUCH, TAG_LIGHT, TAG_BLOCK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CBLOCKM, CliptypeIndex.CBlockM, new[] { TAG_BUSYX, TAG_CROUCH, TAG_MEDIUM, TAG_BLOCK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CBLOCKH, CliptypeIndex.CBlockH, new[] { TAG_BUSYX, TAG_CROUCH, TAG_HARD, TAG_BLOCK }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CDAMAGEL, CliptypeIndex.CDamageL, new[] { TAG_CROUCH, TAG_LIGHT, TAG_DAMAGE }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CDAMAGEM, CliptypeIndex.CDamageM, new[] { TAG_CROUCH, TAG_MEDIUM, TAG_DAMAGE }));
            Set(new AcState(AbstractAconChar3.BASELAYER_CDAMAGEH, CliptypeIndex.CDamageH, new[] { TAG_CROUCH, TAG_HARD, TAG_DAMAGE }));

            Set(new AcState(AbstractAconChar3.BASELAYER_OBACKSTEP, CliptypeIndex.OBackstep, new[] { TAG_OTHER }));
            Set(new AcState(AbstractAconChar3.BASELAYER_OGIVEUP, CliptypeIndex.OGiveup, new[] { TAG_OTHER }));
            Set(new AcState(AbstractAconChar3.BASELAYER_ODOWNSDAMAGEH, CliptypeIndex.SDamageH, new[] { TAG_BUSYX, TAG_OTHER, TAG_HARD }));
            Set(new AcState(AbstractAconChar3.BASELAYER_ODOWN, CliptypeIndex.ODown, new[] { TAG_BUSYX, TAG_OTHER }));
            Set(new AcState(AbstractAconChar3.BASELAYER_OSTANDUP, CliptypeIndex.OStandup, new[] { TAG_OTHER }));
            #endregion
        }
    }
}

