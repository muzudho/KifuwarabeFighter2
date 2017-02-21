using System.Collections.Generic;
using DojinCircleGrayscale.Hitbox2DLorikeet;

namespace DojinCircleGrayscale.StellaQL.Acons.Main_Char3
{
    /// <summary>
    /// モーションするものクラス
    /// </summary>
    public class Motor : Main_Char3_AbstractMotor
    {
        #region Singleton
        static Motor()
        {
            Instance = new Motor();
        }
        public static Motor Instance { get; set; }
        #endregion

        Motor()
        {
            AddMappings_MotionAssetPath_to_Instance( new Dictionary<string, FramedMotionable>()
            {
                { MOTION_CHAR3_SWAIT, new FramedMotion( new int[] { 0,1,2,3 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SMOVE, new FramedMotion( new int[] { 4,5,6,7 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SBLOCKL, new FramedMotion( new int[] { 8,9 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SBLOCKM, new FramedMotion( new int[] { 8,9,10 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SBLOCKH, new FramedMotion( new int[] { 8,9,10,11,9 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SDAMAGEL, new FramedMotion( new int[] { 12,13 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SDAMAGEM, new FramedMotion( new int[] { 12,13,14 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SDAMAGEH, new FramedMotion( new int[] { 12, 13, 14,15,13 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SATKLP, new FramedMotion( new int[] { 16,17 },TilesetfileType.Stand)},
                { MOTION_CHAR3_SATKMP, new FramedMotion( new int[] { 16, 17,18 },TilesetfileType.Stand)},
                { MOTION_CHAR3_SATKHP, new FramedMotion( new int[] { 16, 17, 18,19,17 },TilesetfileType.Stand)},
                { MOTION_CHAR3_SATKLK, new FramedMotion( new int[] { 20,21 },TilesetfileType.Stand)},
                { MOTION_CHAR3_SATKMK, new FramedMotion( new int[] { 20, 21, 22 },TilesetfileType.Stand)},
                { MOTION_CHAR3_SATKHK, new FramedMotion( new int[] { 20, 21, 22, 23,21 },TilesetfileType.Stand)},

                { MOTION_CHAR3_JMOVE0, new FramedMotion( new int[] { 0,1 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JMOVE1, new FramedMotion( new int[] { 2,3 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JMOVE2, new FramedMotion( new int[] { 4 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JMOVE3, new FramedMotion( new int[] { 5,6 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JMOVE4, new FramedMotion( new int[] { 7 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JBLOCKL, new FramedMotion( new int[] { 8,9 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JBLOCKM, new FramedMotion( new int[] { 8,9,10 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JBLOCKH, new FramedMotion( new int[] { 8,9,10,11,9 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JDAMAGEL, new FramedMotion( new int[] { 12,13 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JDAMAGEM, new FramedMotion( new int[] { 12,13,14 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JDAMAGEH, new FramedMotion( new int[] { 12, 13, 14,15,13 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JATKLP, new FramedMotion( new int[] { 16,17 },TilesetfileType.Jump)},
                { MOTION_CHAR3_JATKMP, new FramedMotion( new int[] { 16, 17,18 },TilesetfileType.Jump)},
                { MOTION_CHAR3_JATKHP, new FramedMotion( new int[] { 16, 17, 18,19,17 },TilesetfileType.Jump)},
                { MOTION_CHAR3_JATKLK, new FramedMotion( new int[] { 20,21 },TilesetfileType.Jump)},
                { MOTION_CHAR3_JATKMK, new FramedMotion( new int[] { 20, 21, 22 },TilesetfileType.Jump)},
                { MOTION_CHAR3_JATKHK, new FramedMotion( new int[] { 20, 21, 22, 23,21 },TilesetfileType.Jump)},

                { MOTION_CHAR3_DMOVE, new FramedMotion( new int[] { 0,1,2,3,4,5,6,7 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DBLOCKL, new FramedMotion( new int[] { 8,9 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DBLOCKM, new FramedMotion( new int[] { 8,9,10 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DBLOCKH, new FramedMotion( new int[] { 8,9,10,11,9 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DDAMAGEL, new FramedMotion( new int[] { 12,13 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DDAMAGEM, new FramedMotion( new int[] { 12,13,14 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DDAMAGEH, new FramedMotion( new int[] { 12, 13, 14,15,13 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DATKLP, new FramedMotion( new int[] { 16,17 },TilesetfileType.Dash)},
                { MOTION_CHAR3_DATKMP, new FramedMotion( new int[] { 16, 17,18 },TilesetfileType.Dash)},
                { MOTION_CHAR3_DATKHP, new FramedMotion( new int[] { 16, 17, 18,19,17 },TilesetfileType.Dash)},
                { MOTION_CHAR3_DATKLK, new FramedMotion( new int[] { 20,21 },TilesetfileType.Dash)},
                { MOTION_CHAR3_DATKMK, new FramedMotion( new int[] { 20, 21, 22 },TilesetfileType.Dash)},
                { MOTION_CHAR3_DATKHK, new FramedMotion( new int[] { 20, 21, 22, 23,21 },TilesetfileType.Dash)},

                { MOTION_CHAR3_CWAIT, new FramedMotion( new int[] { 0,1,2,3 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CMOVE, new FramedMotion( new int[] { 4,5,6,7 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CBLOCKL, new FramedMotion( new int[] { 8,9 },TilesetfileType.Crouch) },
                { MOTION_CHAR3_CBLOCKM, new FramedMotion( new int[] { 8,9,10 },TilesetfileType.Crouch) },
                { MOTION_CHAR3_CBLOCKH, new FramedMotion( new int[] { 8,9,10,11,9 },TilesetfileType.Crouch) },
                { MOTION_CHAR3_CDAMAGEL, new FramedMotion( new int[] { 12,13 },TilesetfileType.Crouch) },
                { MOTION_CHAR3_CDAMAGEM, new FramedMotion( new int[] { 12,13,14 },TilesetfileType.Crouch) },
                { MOTION_CHAR3_CDAMAGEH, new FramedMotion( new int[] { 12, 13, 14,15,13 },TilesetfileType.Crouch) },
                { MOTION_CHAR3_CATKLP, new FramedMotion( new int[] { 16,17 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CATKMP, new FramedMotion( new int[] { 16, 17,18 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CATKHP, new FramedMotion( new int[] { 16, 17, 18,19,17 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CATKLK, new FramedMotion( new int[] { 20,21 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CATKMK, new FramedMotion( new int[] { 20, 21, 22 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CATKHK, new FramedMotion( new int[] { 20, 21, 22, 23,21 },TilesetfileType.Crouch)},

                { MOTION_CHAR3_OBACKSTEP, new FramedMotion( new int[] { 0,1,2,3,4,5,6,7 },TilesetfileType.Other)},
                { MOTION_CHAR3_ODOWN, new FramedMotion( new int[] { 8,9 },TilesetfileType.Other)},
                { MOTION_CHAR3_OSTANDUP, new FramedMotion( new int[] { 10,11 },TilesetfileType.Other)},
                { MOTION_CHAR3_OGIVEUP, new FramedMotion( new int[] { 12,13,14,15 },TilesetfileType.Other)},
            });
        }
    }
}
