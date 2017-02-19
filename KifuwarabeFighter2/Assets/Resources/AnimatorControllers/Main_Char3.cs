using DojinCircleGrayscale.Hitbox2D;
using UnityEngine;

namespace DojinCircleGrayscale.StellaQL.Acons.Main_Char3
{
    /// <summary>
    /// 独自拡張
    /// </summary>
    public class AcState : AbstractAcState
    {
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        /// <param name="motionAssetPath">演技の種類</param>
        /// <param name="userDefinedTags_hash">StellaQL用のユーザー定義タグのハッシュ</param>
        public AcState(string fullpath, string motionAssetPath, string[] userDefinedTags)
            :base(fullpath, userDefinedTags)
        {
            this.MotionAssetPathHash = Animator.StringToHash(motionAssetPath);
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
            Set(new AcState(BASELAYER_, "", new[] { TAG_NONE }));
            Set(new AcState(BASELAYER_SWAIT, Moviestar.MOTION_CHAR3_SWAIT, new[] { TAG_STAND }));
            Set(new AcState(BASELAYER_SMOVE, Moviestar.MOTION_CHAR3_SMOVE, new[] { TAG_STAND }));
            Set(new AcState(BASELAYER_SATKLP, Moviestar.MOTION_CHAR3_SATKLP, new[] { TAG_BUSYX, TAG_STAND, TAG_LIGHT, TAG_PUNCH }));
            Set(new AcState(BASELAYER_SATKMP, Moviestar.MOTION_CHAR3_SATKMP, new[] { TAG_BUSYX, TAG_STAND, TAG_MEDIUM, TAG_PUNCH }));
            Set(new AcState(BASELAYER_SATKHP, Moviestar.MOTION_CHAR3_SATKHP, new[] { TAG_BUSYX, TAG_STAND, TAG_HARD, TAG_PUNCH }));
            Set(new AcState(BASELAYER_SATKLK, Moviestar.MOTION_CHAR3_SATKLK, new[] { TAG_BUSYX, TAG_STAND, TAG_LIGHT, TAG_KICK }));
            Set(new AcState(BASELAYER_SATKMK, Moviestar.MOTION_CHAR3_SATKMK, new[] { TAG_BUSYX, TAG_STAND, TAG_MEDIUM, TAG_KICK }));
            Set(new AcState(BASELAYER_SATKHK, Moviestar.MOTION_CHAR3_SATKHK, new[] { TAG_BUSYX, TAG_STAND, TAG_HARD, TAG_KICK }));
            Set(new AcState(BASELAYER_SBLOCKL, Moviestar.MOTION_CHAR3_SBLOCKL, new[] { TAG_BUSYX, TAG_STAND, TAG_LIGHT, TAG_BLOCK }));
            Set(new AcState(BASELAYER_SBLOCKM, Moviestar.MOTION_CHAR3_SBLOCKM, new[] { TAG_BUSYX, TAG_STAND, TAG_MEDIUM, TAG_BLOCK }));
            Set(new AcState(BASELAYER_SBLOCKH, Moviestar.MOTION_CHAR3_SBLOCKH, new[] { TAG_BUSYX, TAG_STAND, TAG_HARD, TAG_BLOCK }));
            Set(new AcState(BASELAYER_SDAMAGEL, Moviestar.MOTION_CHAR3_SDAMAGEL, new[] { TAG_NONE, TAG_STAND, TAG_LIGHT, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_SDAMAGEM, Moviestar.MOTION_CHAR3_SDAMAGEM, new[] { TAG_NONE, TAG_STAND, TAG_MEDIUM, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_SDAMAGEH, Moviestar.MOTION_CHAR3_SDAMAGEH, new[] { TAG_NONE, TAG_STAND, TAG_HARD, TAG_DAMAGE }));

            Set(new AcState(BASELAYER_JMOVE_, "", new[] { TAG_JUMP }));
            Set(new AcState(BASELAYER_JMOVE_JMOVE0, Moviestar.MOTION_CHAR3_JMOVE0, new[] { TAG_BUSYX, TAG_BUSYY, TAG_JUMP }));
            Set(new AcState(BASELAYER_JMOVE_JMOVE1, Moviestar.MOTION_CHAR3_JMOVE1, new[] { TAG_JUMP }));
            Set(new AcState(BASELAYER_JMOVE_JMOVE2, Moviestar.MOTION_CHAR3_JMOVE2, new[] { TAG_JUMP }));
            Set(new AcState(BASELAYER_JMOVE_JMOVE3, Moviestar.MOTION_CHAR3_JMOVE3, new[] { TAG_JUMP }));
            Set(new AcState(BASELAYER_JMOVE_JMOVE4, Moviestar.MOTION_CHAR3_JMOVE4, new[] { TAG_BUSYX, TAG_JUMP }));
            Set(new AcState(BASELAYER_JATKLP, Moviestar.MOTION_CHAR3_JATKLP, new[] { TAG_JUMP, TAG_LIGHT, TAG_PUNCH }));
            Set(new AcState(BASELAYER_JATKMP, Moviestar.MOTION_CHAR3_JATKMP, new[] { TAG_JUMP, TAG_MEDIUM, TAG_PUNCH }));
            Set(new AcState(BASELAYER_JATKHP, Moviestar.MOTION_CHAR3_JATKHP, new[] { TAG_JUMP, TAG_HARD, TAG_PUNCH }));
            Set(new AcState(BASELAYER_JATKLK, Moviestar.MOTION_CHAR3_JATKLK, new[] { TAG_JUMP, TAG_LIGHT, TAG_KICK }));
            Set(new AcState(BASELAYER_JATKMK, Moviestar.MOTION_CHAR3_JATKMK, new[] { TAG_JUMP, TAG_MEDIUM, TAG_KICK }));
            Set(new AcState(BASELAYER_JATKHK, Moviestar.MOTION_CHAR3_JATKHK, new[] { TAG_JUMP, TAG_HARD, TAG_KICK }));
            Set(new AcState(BASELAYER_JBLOCKL, Moviestar.MOTION_CHAR3_JBLOCKL, new[] { TAG_BUSYX, TAG_JUMP, TAG_LIGHT, TAG_BLOCK }));
            Set(new AcState(BASELAYER_JBLOCKM, Moviestar.MOTION_CHAR3_JBLOCKM, new[] { TAG_BUSYX, TAG_JUMP, TAG_MEDIUM, TAG_BLOCK }));
            Set(new AcState(BASELAYER_JBLOCKH, Moviestar.MOTION_CHAR3_JBLOCKH, new[] { TAG_BUSYX, TAG_JUMP, TAG_HARD, TAG_BLOCK }));
            Set(new AcState(BASELAYER_JDAMAGEL, Moviestar.MOTION_CHAR3_JDAMAGEL, new[] { TAG_JUMP, TAG_LIGHT, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_JDAMAGEM, Moviestar.MOTION_CHAR3_JDAMAGEM, new[] { TAG_JUMP, TAG_MEDIUM, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_JDAMAGEH, Moviestar.MOTION_CHAR3_JDAMAGEH, new[] { TAG_JUMP, TAG_HARD, TAG_DAMAGE }));

            Set(new AcState(BASELAYER_DMOVE, Moviestar.MOTION_CHAR3_DMOVE, new[] { TAG_DASH }));
            Set(new AcState(BASELAYER_DATKLP, Moviestar.MOTION_CHAR3_DATKLP, new[] { TAG_DASH, TAG_LIGHT, TAG_PUNCH }));
            Set(new AcState(BASELAYER_DATKMP, Moviestar.MOTION_CHAR3_DATKMP, new[] { TAG_DASH, TAG_MEDIUM, TAG_PUNCH }));
            Set(new AcState(BASELAYER_DATKHP, Moviestar.MOTION_CHAR3_DATKHP, new[] { TAG_DASH, TAG_HARD, TAG_PUNCH }));
            Set(new AcState(BASELAYER_DATKLK, Moviestar.MOTION_CHAR3_DATKLK, new[] { TAG_DASH, TAG_LIGHT, TAG_KICK }));
            Set(new AcState(BASELAYER_DATKMK, Moviestar.MOTION_CHAR3_DATKMK, new[] { TAG_DASH, TAG_MEDIUM, TAG_KICK }));
            Set(new AcState(BASELAYER_DATKHK, Moviestar.MOTION_CHAR3_DATKHK, new[] { TAG_DASH, TAG_HARD, TAG_KICK }));
            Set(new AcState(BASELAYER_DBLOCKL, Moviestar.MOTION_CHAR3_DBLOCKL, new[] { TAG_BUSYX, TAG_DASH, TAG_LIGHT, TAG_BLOCK }));
            Set(new AcState(BASELAYER_DBLOCKM, Moviestar.MOTION_CHAR3_DBLOCKM, new[] { TAG_BUSYX, TAG_DASH, TAG_MEDIUM, TAG_BLOCK }));
            Set(new AcState(BASELAYER_DBLOCKH, Moviestar.MOTION_CHAR3_DBLOCKH, new[] { TAG_BUSYX, TAG_DASH, TAG_HARD, TAG_BLOCK }));
            Set(new AcState(BASELAYER_DDAMAGEL, Moviestar.MOTION_CHAR3_DDAMAGEL, new[] { TAG_DASH, TAG_LIGHT, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_DDAMAGEM, Moviestar.MOTION_CHAR3_DDAMAGEM, new[] { TAG_DASH, TAG_MEDIUM, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_DDAMAGEH, Moviestar.MOTION_CHAR3_DDAMAGEH, new[] { TAG_DASH, TAG_HARD, TAG_DAMAGE }));

            Set(new AcState(BASELAYER_CWAIT, Moviestar.MOTION_CHAR3_CWAIT, new[] { TAG_CROUCH }));
            Set(new AcState(BASELAYER_CMOVE, Moviestar.MOTION_CHAR3_CMOVE, new[] { TAG_CROUCH }));
            Set(new AcState(BASELAYER_CATKLP, Moviestar.MOTION_CHAR3_CATKLP, new[] { TAG_BUSYX, TAG_CROUCH, TAG_LIGHT, TAG_PUNCH }));
            Set(new AcState(BASELAYER_CATKMP, Moviestar.MOTION_CHAR3_CATKMP, new[] { TAG_BUSYX, TAG_CROUCH, TAG_MEDIUM, TAG_PUNCH }));
            Set(new AcState(BASELAYER_CATKHP, Moviestar.MOTION_CHAR3_CATKHP, new[] { TAG_BUSYX, TAG_CROUCH, TAG_HARD, TAG_PUNCH }));
            Set(new AcState(BASELAYER_CATKLK, Moviestar.MOTION_CHAR3_CATKLK, new[] { TAG_BUSYX, TAG_CROUCH, TAG_LIGHT, TAG_KICK }));
            Set(new AcState(BASELAYER_CATKMK, Moviestar.MOTION_CHAR3_CATKMK, new[] { TAG_BUSYX, TAG_CROUCH, TAG_MEDIUM, TAG_KICK }));
            Set(new AcState(BASELAYER_CATKHK, Moviestar.MOTION_CHAR3_CATKHK, new[] { TAG_BUSYX, TAG_CROUCH, TAG_HARD, TAG_KICK }));
            Set(new AcState(BASELAYER_CBLOCKL, Moviestar.MOTION_CHAR3_CBLOCKL, new[] { TAG_BUSYX, TAG_CROUCH, TAG_LIGHT, TAG_BLOCK }));
            Set(new AcState(BASELAYER_CBLOCKM, Moviestar.MOTION_CHAR3_CBLOCKM, new[] { TAG_BUSYX, TAG_CROUCH, TAG_MEDIUM, TAG_BLOCK }));
            Set(new AcState(BASELAYER_CBLOCKH, Moviestar.MOTION_CHAR3_CBLOCKH, new[] { TAG_BUSYX, TAG_CROUCH, TAG_HARD, TAG_BLOCK }));
            Set(new AcState(BASELAYER_CDAMAGEL, Moviestar.MOTION_CHAR3_CDAMAGEL, new[] { TAG_CROUCH, TAG_LIGHT, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_CDAMAGEM, Moviestar.MOTION_CHAR3_CDAMAGEM, new[] { TAG_CROUCH, TAG_MEDIUM, TAG_DAMAGE }));
            Set(new AcState(BASELAYER_CDAMAGEH, Moviestar.MOTION_CHAR3_CDAMAGEH, new[] { TAG_CROUCH, TAG_HARD, TAG_DAMAGE }));

            Set(new AcState(BASELAYER_OBACKSTEP, Moviestar.MOTION_CHAR3_OBACKSTEP, new[] { TAG_OTHER }));
            Set(new AcState(BASELAYER_OGIVEUP, Moviestar.MOTION_CHAR3_OGIVEUP, new[] { TAG_OTHER }));
            Set(new AcState(BASELAYER_ODOWNSDAMAGEH, Moviestar.MOTION_CHAR3_SDAMAGEH, new[] { TAG_BUSYX, TAG_OTHER, TAG_HARD }));
            Set(new AcState(BASELAYER_ODOWN, Moviestar.MOTION_CHAR3_ODOWN, new[] { TAG_BUSYX, TAG_OTHER }));
            Set(new AcState(BASELAYER_OSTANDUP, Moviestar.MOTION_CHAR3_OSTANDUP, new[] { TAG_OTHER }));
            #endregion
        }
    }
}

