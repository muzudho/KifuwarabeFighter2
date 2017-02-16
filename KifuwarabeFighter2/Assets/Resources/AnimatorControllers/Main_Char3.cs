using SceneMain;

namespace StellaQL.Acons.Main_Char3
{
    /// <summary>
    /// 独自拡張
    /// </summary>
    public class AcState : AbstractAcState
    {
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
    /// アニメーター・コントローラー１つに対応するレコード。
    /// This class corresponds to one animator controller.
    /// 
    /// 自動生成した抽象クラスを継承してください。
    /// Please inherit the automatically generated abstract class.
    /// </summary>
    public class AControl : Main_Char3_AbstractAControl
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
            TAG_NONE = "None",
            TAG_BUSYX = "BusyX",
            TAG_BUSYY = "BusyY",
            TAG_STAND = "Stand",
            TAG_JUMP = "Jump",
            TAG_DASH = "Dash",
            TAG_CROUCH = "Crouch",
            TAG_OTHER = "Other",
            TAG_LIGHT = "Light",
            TAG_MEDIUM = "Medium",
            TAG_HARD = "Hard",
            TAG_PUNCH = "Punch",
            TAG_KICK = "Kick",
            TAG_BLOCK = "Block",
            TAG_DAMAGE = "Damage",

            // カンマで終わるリストを作るために最後に置いています。使わないでください。
            // Don't use. Sentinel value for a list that ends with a comma.
            TAG_ = "";
        #endregion

        AControl()
        {
            #region Tags
            // あなたの定義したタグをステートに関連付けることができます
            // You can set your defined tags to state.
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

