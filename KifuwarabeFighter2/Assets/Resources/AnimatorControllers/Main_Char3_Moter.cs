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
            AddMappings_MotionAssetPath_to_Instance( new Dictionary<string, Motionable>()
            {
                { MOTION_CHAR3_SWAIT, new Motion( new int[] { 0,1,2,3 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SMOVE, new Motion( new int[] { 4,5,6,7 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SBLOCKL, new Motion( new int[] { 8,9 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SBLOCKM, new Motion( new int[] { 8,9,10 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SBLOCKH, new Motion( new int[] { 8,9,10,11,9 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SDAMAGEL, new Motion( new int[] { 12,13 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SDAMAGEM, new Motion( new int[] { 12,13,14 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SDAMAGEH, new Motion( new int[] { 12, 13, 14,15,13 } ,TilesetfileType.Stand)},
                { MOTION_CHAR3_SATKLP, new Motion( new int[] { 16,17 },TilesetfileType.Stand)},
                { MOTION_CHAR3_SATKMP, new Motion( new int[] { 16, 17,18 },TilesetfileType.Stand)},
                { MOTION_CHAR3_SATKHP, new Motion( new int[] { 16, 17, 18,19,17 },TilesetfileType.Stand)},
                { MOTION_CHAR3_SATKLK, new Motion( new int[] { 20,21 },TilesetfileType.Stand)},
                { MOTION_CHAR3_SATKMK, new Motion( new int[] { 20, 21, 22 },TilesetfileType.Stand)},
                { MOTION_CHAR3_SATKHK, new Motion( new int[] { 20, 21, 22, 23,21 },TilesetfileType.Stand)},

                { MOTION_CHAR3_JMOVE0, new Motion( new int[] { 0,1 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JMOVE1, new Motion( new int[] { 2,3 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JMOVE2, new Motion( new int[] { 4 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JMOVE3, new Motion( new int[] { 5,6 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JMOVE4, new Motion( new int[] { 7 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JBLOCKL, new Motion( new int[] { 8,9 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JBLOCKM, new Motion( new int[] { 8,9,10 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JBLOCKH, new Motion( new int[] { 8,9,10,11,9 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JDAMAGEL, new Motion( new int[] { 12,13 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JDAMAGEM, new Motion( new int[] { 12,13,14 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JDAMAGEH, new Motion( new int[] { 12, 13, 14,15,13 },TilesetfileType.Jump) },
                { MOTION_CHAR3_JATKLP, new Motion( new int[] { 16,17 },TilesetfileType.Jump)},
                { MOTION_CHAR3_JATKMP, new Motion( new int[] { 16, 17,18 },TilesetfileType.Jump)},
                { MOTION_CHAR3_JATKHP, new Motion( new int[] { 16, 17, 18,19,17 },TilesetfileType.Jump)},
                { MOTION_CHAR3_JATKLK, new Motion( new int[] { 20,21 },TilesetfileType.Jump)},
                { MOTION_CHAR3_JATKMK, new Motion( new int[] { 20, 21, 22 },TilesetfileType.Jump)},
                { MOTION_CHAR3_JATKHK, new Motion( new int[] { 20, 21, 22, 23,21 },TilesetfileType.Jump)},

                { MOTION_CHAR3_DMOVE, new Motion( new int[] { 0,1,2,3,4,5,6,7 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DBLOCKL, new Motion( new int[] { 8,9 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DBLOCKM, new Motion( new int[] { 8,9,10 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DBLOCKH, new Motion( new int[] { 8,9,10,11,9 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DDAMAGEL, new Motion( new int[] { 12,13 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DDAMAGEM, new Motion( new int[] { 12,13,14 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DDAMAGEH, new Motion( new int[] { 12, 13, 14,15,13 },TilesetfileType.Dash) },
                { MOTION_CHAR3_DATKLP, new Motion( new int[] { 16,17 },TilesetfileType.Dash)},
                { MOTION_CHAR3_DATKMP, new Motion( new int[] { 16, 17,18 },TilesetfileType.Dash)},
                { MOTION_CHAR3_DATKHP, new Motion( new int[] { 16, 17, 18,19,17 },TilesetfileType.Dash)},
                { MOTION_CHAR3_DATKLK, new Motion( new int[] { 20,21 },TilesetfileType.Dash)},
                { MOTION_CHAR3_DATKMK, new Motion( new int[] { 20, 21, 22 },TilesetfileType.Dash)},
                { MOTION_CHAR3_DATKHK, new Motion( new int[] { 20, 21, 22, 23,21 },TilesetfileType.Dash)},

                { MOTION_CHAR3_CWAIT, new Motion( new int[] { 0,1,2,3 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CMOVE, new Motion( new int[] { 4,5,6,7 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CBLOCKL, new Motion( new int[] { 8,9 },TilesetfileType.Crouch) },
                { MOTION_CHAR3_CBLOCKM, new Motion( new int[] { 8,9,10 },TilesetfileType.Crouch) },
                { MOTION_CHAR3_CBLOCKH, new Motion( new int[] { 8,9,10,11,9 },TilesetfileType.Crouch) },
                { MOTION_CHAR3_CDAMAGEL, new Motion( new int[] { 12,13 },TilesetfileType.Crouch) },
                { MOTION_CHAR3_CDAMAGEM, new Motion( new int[] { 12,13,14 },TilesetfileType.Crouch) },
                { MOTION_CHAR3_CDAMAGEH, new Motion( new int[] { 12, 13, 14,15,13 },TilesetfileType.Crouch) },
                { MOTION_CHAR3_CATKLP, new Motion( new int[] { 16,17 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CATKMP, new Motion( new int[] { 16, 17,18 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CATKHP, new Motion( new int[] { 16, 17, 18,19,17 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CATKLK, new Motion( new int[] { 20,21 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CATKMK, new Motion( new int[] { 20, 21, 22 },TilesetfileType.Crouch)},
                { MOTION_CHAR3_CATKHK, new Motion( new int[] { 20, 21, 22, 23,21 },TilesetfileType.Crouch)},

                { MOTION_CHAR3_OBACKSTEP, new Motion( new int[] { 0,1,2,3,4,5,6,7 },TilesetfileType.Other)},
                { MOTION_CHAR3_ODOWN, new Motion( new int[] { 8,9 },TilesetfileType.Other)},
                { MOTION_CHAR3_OSTANDUP, new Motion( new int[] { 10,11 },TilesetfileType.Other)},
                { MOTION_CHAR3_OGIVEUP, new Motion( new int[] { 12,13,14,15 },TilesetfileType.Other)},
            });
        }
    }
}
