using DojinCircleGrayscale.Hitbox2DLorikeet;

namespace DojinCircleGrayscale.StellaQL.Acons.Main_Char3
{
    public abstract class Main_Char3_AbstractMotor3 : AbstractMotor
    {
        #region Motion tags
        /// <summary>
        /// AnimationClip の種類に一対一対応☆
        /// 
        /// TODO: これはステート名と一致しないのか？→ほぼ一致する。
        /// TODO: 一意になることが確実な、アセットパスに変えてみるか。
        /// 
        /// ２つのステートに使いまわされるモーションもある。
        /// 
        /// アニメーター・コントローラーの全ステートを走査し、
        /// 自動生成できないか？
        /// 
        /// TODO: 定数名は 頭に MOTION_ を付け、拡張子を除いたファイル名を大文字にして付ける。
        /// 英数字以外はアンダースコアに置き換えてはどうか。
        /// </summary>
        public const string
            MOTION_CHAR3_SWAIT = "Assets/Resources/AnimationClips/Char3@SWait.anim",
            MOTION_CHAR3_SATKLP = "Assets/Resources/AnimationClips/Char3@SAtkLP.anim",
            MOTION_CHAR3_SATKMP = "Assets/Resources/AnimationClips/Char3@SAtkMP.anim",
            MOTION_CHAR3_SATKHP = "Assets/Resources/AnimationClips/Char3@SAtkHP.anim",
            MOTION_CHAR3_SATKLK = "Assets/Resources/AnimationClips/Char3@SAtkLK.anim",
            MOTION_CHAR3_SATKMK = "Assets/Resources/AnimationClips/Char3@SAtkMK.anim",
            MOTION_CHAR3_SATKHK = "Assets/Resources/AnimationClips/Char3@SAtkHK.anim",
            MOTION_CHAR3_DMOVE = "Assets/Resources/AnimationClips/Char3@DMove.anim",
            MOTION_CHAR3_OBACKSTEP = "Assets/Resources/AnimationClips/Char3@OBackstep.anim",
            MOTION_CHAR3_SDAMAGEM = "Assets/Resources/AnimationClips/Char3@SDamageM.anim",
            MOTION_CHAR3_SDAMAGEL = "Assets/Resources/AnimationClips/Char3@SDamageL.anim",
            MOTION_CHAR3_SDAMAGEH = "Assets/Resources/AnimationClips/Char3@SDamageH.anim",
            MOTION_CHAR3_ODOWN = "Assets/Resources/AnimationClips/Char3@ODown.anim",
            MOTION_CHAR3_OSTANDUP = "Assets/Resources/AnimationClips/Char3@OStandup.anim",
            MOTION_CHAR3_OGIVEUP = "Assets/Resources/AnimationClips/Char3@OGiveup.anim",
            MOTION_CHAR3_SMOVE = "Assets/Resources/AnimationClips/Char3@SMove.anim",
            MOTION_CHAR3_SBLOCKL = "Assets/Resources/AnimationClips/Char3@SBlockL.anim",
            MOTION_CHAR3_SBLOCKM = "Assets/Resources/AnimationClips/Char3@SBlockM.anim",
            MOTION_CHAR3_SBLOCKH = "Assets/Resources/AnimationClips/Char3@SBlockH.anim",
            MOTION_CHAR3_JATKLP = "Assets/Resources/AnimationClips/Char3@JAtkLP.anim",
            MOTION_CHAR3_JATKMP = "Assets/Resources/AnimationClips/Char3@JAtkMP.anim",
            MOTION_CHAR3_JATKHP = "Assets/Resources/AnimationClips/Char3@JAtkHP.anim",
            MOTION_CHAR3_JATKLK = "Assets/Resources/AnimationClips/Char3@JAtkLK.anim",
            MOTION_CHAR3_JATKMK = "Assets/Resources/AnimationClips/Char3@JAtkMK.anim",
            MOTION_CHAR3_JATKHK = "Assets/Resources/AnimationClips/Char3@JAtkHK.anim",
            MOTION_CHAR3_JBLOCKL = "Assets/Resources/AnimationClips/Char3@JBlockL.anim",
            MOTION_CHAR3_JBLOCKM = "Assets/Resources/AnimationClips/Char3@JBlockM.anim",
            MOTION_CHAR3_JBLOCKH = "Assets/Resources/AnimationClips/Char3@JBlockH.anim",
            MOTION_CHAR3_JDAMAGEL = "Assets/Resources/AnimationClips/Char3@JDamageL.anim",
            MOTION_CHAR3_JDAMAGEM = "Assets/Resources/AnimationClips/Char3@JDamageM.anim",
            MOTION_CHAR3_JDAMAGEH = "Assets/Resources/AnimationClips/Char3@JDamageH.anim",
            MOTION_CHAR3_DATKLP = "Assets/Resources/AnimationClips/Char3@DAtkLP.anim",
            MOTION_CHAR3_DATKMP = "Assets/Resources/AnimationClips/Char3@DAtkMP.anim",
            MOTION_CHAR3_DATKHP = "Assets/Resources/AnimationClips/Char3@DAtkHP.anim",
            MOTION_CHAR3_DATKLK = "Assets/Resources/AnimationClips/Char3@DAtkLK.anim",
            MOTION_CHAR3_DATKMK = "Assets/Resources/AnimationClips/Char3@DAtkMK.anim",
            MOTION_CHAR3_DATKHK = "Assets/Resources/AnimationClips/Char3@DAtkHK.anim",
            MOTION_CHAR3_DBLOCKL = "Assets/Resources/AnimationClips/Char3@DBlockL.anim",
            MOTION_CHAR3_DBLOCKM = "Assets/Resources/AnimationClips/Char3@DBlockM.anim",
            MOTION_CHAR3_DBLOCKH = "Assets/Resources/AnimationClips/Char3@DBlockH.anim",
            MOTION_CHAR3_DDAMAGEL = "Assets/Resources/AnimationClips/Char3@DDamageL.anim",
            MOTION_CHAR3_DDAMAGEM = "Assets/Resources/AnimationClips/Char3@DDamageM.anim",
            MOTION_CHAR3_DDAMAGEH = "Assets/Resources/AnimationClips/Char3@DDamageH.anim",
            MOTION_CHAR3_CWAIT = "Assets/Resources/AnimationClips/Char3@CWait.anim",
            MOTION_CHAR3_CMOVE = "Assets/Resources/AnimationClips/Char3@CMove.anim",
            MOTION_CHAR3_CATKLP = "Assets/Resources/AnimationClips/Char3@CAtkLP.anim",
            MOTION_CHAR3_CATKMP = "Assets/Resources/AnimationClips/Char3@CAtkMP.anim",
            MOTION_CHAR3_CATKHP = "Assets/Resources/AnimationClips/Char3@CAtkHP.anim",
            MOTION_CHAR3_CATKLK = "Assets/Resources/AnimationClips/Char3@CAtkLK.anim",
            MOTION_CHAR3_CATKMK = "Assets/Resources/AnimationClips/Char3@CAtkMK.anim",
            MOTION_CHAR3_CATKHK = "Assets/Resources/AnimationClips/Char3@CAtkHK.anim",
            MOTION_CHAR3_CBLOCKL = "Assets/Resources/AnimationClips/Char3@CBlockL.anim",
            MOTION_CHAR3_CBLOCKM = "Assets/Resources/AnimationClips/Char3@CBlockM.anim",
            MOTION_CHAR3_CBLOCKH = "Assets/Resources/AnimationClips/Char3@CBlockH.anim",
            MOTION_CHAR3_CDAMAGEL = "Assets/Resources/AnimationClips/Char3@CDamageL.anim",
            MOTION_CHAR3_CDAMAGEM = "Assets/Resources/AnimationClips/Char3@CDamageM.anim",
            MOTION_CHAR3_CDAMAGEH = "Assets/Resources/AnimationClips/Char3@CDamageH.anim",
            MOTION_CHAR3_JMOVE0 = "Assets/Resources/AnimationClips/Char3@JMove0.anim",
            MOTION_CHAR3_JMOVE1 = "Assets/Resources/AnimationClips/Char3@JMove1.anim",
            MOTION_CHAR3_JMOVE2 = "Assets/Resources/AnimationClips/Char3@JMove2.anim",
            MOTION_CHAR3_JMOVE3 = "Assets/Resources/AnimationClips/Char3@JMove3.anim",
            MOTION_CHAR3_JMOVE4 = "Assets/Resources/AnimationClips/Char3@JMove4.anim",

            // カンマで終わるリストを作るために最後に置いています。使わないでください。
            // Don't use. Sentinel value for a list that ends with a comma.
            MOTION_ = "";
        #endregion
    }
}
