using StellaQL;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main シーン
/// </summary>
namespace SceneMain
{
    /// <summary>
    /// ステートマシン、ステートの拡張データ構造
    /// </summary>
    public class UserDefinedStateRecord : AbstractUserDefinedStateRecord
    {
        /// <summary>
        /// データ入力用
        /// </summary>
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        /// <param name="cliptype">アニメーションの種類</param>
        /// <param name="userDefinedTags">StellaQL用のユーザー定義タグ</param>
        /// <returns></returns>
        public static UserDefinedStateRecord Build(string fullpath, CliptypeIndex cliptype, string[] userDefinedTags)
        {
            return new UserDefinedStateRecord(fullpath, Animator.StringToHash(fullpath), cliptype, Code.Hashes(userDefinedTags) );
        }

        /// <summary>
        /// データ
        /// </summary>
        /// <param name="fullpath">ステートマシン名、ステート名のフルパス</param>
        /// <param name="fullpathHash">ステートマシン名、ステート名のフルパスのハッシュ</param>
        /// <param name="cliptype">アニメーションの種類</param>
        /// <param name="userDefinedTags_hash">StellaQL用のユーザー定義タグのハッシュ</param>
        public UserDefinedStateRecord(string fullpath, int fullpathHash, CliptypeIndex cliptype, HashSet<int> userDefinedTags_hash)
            :base(fullpath, fullpathHash, userDefinedTags_hash)
        {
            this.Cliptype = (int)cliptype;
        }
    }

    /// <summary>
    /// ステートマシン、ステートの拡張データ構造の集まり
    /// </summary>
    public class UserDefinedStateTable : AbstractUserDefinedStateTable
    {
        static UserDefinedStateTable() { Instance = new UserDefinedStateTable(); }
        public static UserDefinedStateTable Instance { get; set; }

        #region ステートマシン、ステート　フルパス一覧
        public const string STATEMACHINE_BASELAYER = "Base Layer";
        public const string STATE_SWAIT = "Base Layer.SWait";
        public const string STATE_SMOVE = "Base Layer.SMove";
        public const string STATE_SATKLP = "Base Layer.SAtkLP";
        public const string STATE_SATKMP = "Base Layer.SAtkMP";
        public const string STATE_SATKHP = "Base Layer.SAtkHP";
        public const string STATE_SATKLK = "Base Layer.SAtkLK";
        public const string STATE_SATKMK = "Base Layer.SAtkMK";
        public const string STATE_SATKHK = "Base Layer.SAtkHK";
        public const string STATE_SBLOCKL = "Base Layer.SBlockL";
        public const string STATE_SBLOCKM = "Base Layer.SBlockM";
        public const string STATE_SBLOCKH = "Base Layer.SBlockH";
        public const string STATE_SDAMAGEL = "Base Layer.SDamageL";
        public const string STATE_SDAMAGEM = "Base Layer.SDamageM";
        public const string STATE_SDAMAGEH = "Base Layer.SDamageH";

        public const string STATEMACHINE_JMOVE = "Base Layer.JMove";
        public const string STATE_JMOVE0 = "Base Layer.JMove.JMove0";
        public const string STATE_JMOVE1 = "Base Layer.JMove.JMove1";
        public const string STATE_JMOVE2 = "Base Layer.JMove.JMove2";
        public const string STATE_JMOVE3 = "Base Layer.JMove.JMove3";
        public const string STATE_JMOVE4 = "Base Layer.JMove.JMove4";
        public const string STATE_JATKLP = "Base Layer.JAtkLP";
        public const string STATE_JATKMP = "Base Layer.JAtkMP";
        public const string STATE_JATKHP = "Base Layer.JAtkHP";
        public const string STATE_JATKLK = "Base Layer.JAtkLK";
        public const string STATE_JATKMK = "Base Layer.JAtkMK";
        public const string STATE_JATKHK = "Base Layer.JAtkHK";
        public const string STATE_JBLOCKL = "Base Layer.JBlockL";
        public const string STATE_JBLOCKM = "Base Layer.JBlockM";
        public const string STATE_JBLOCKH = "Base Layer.JBlockH";
        public const string STATE_JDAMAGEL = "Base Layer.JDamageL";
        public const string STATE_JDAMAGEM = "Base Layer.JDamageM";
        public const string STATE_JDAMAGEH = "Base Layer.JDamageH";

        public const string STATE_DMOVE = "Base Layer.DMove";
        public const string STATE_DATKLP = "Base Layer.DAtkLP";
        public const string STATE_DATKMP = "Base Layer.DAtkMP";
        public const string STATE_DATKHP = "Base Layer.DAtkHP";
        public const string STATE_DATKLK = "Base Layer.DAtkLK";
        public const string STATE_DATKMK = "Base Layer.DAtkMK";
        public const string STATE_DATKHK = "Base Layer.DAtkHK";
        public const string STATE_DBLOCKL = "Base Layer.DBlockL";
        public const string STATE_DBLOCKM = "Base Layer.DBlockM";
        public const string STATE_DBLOCKH = "Base Layer.DBlockH";
        public const string STATE_DDAMAGEL = "Base Layer.DDamageL";
        public const string STATE_DDAMAGEM = "Base Layer.DDamageM";
        public const string STATE_DDAMAGEH = "Base Layer.DDamageH";

        public const string STATE_CWAIT = "Base Layer.CWait";
        public const string STATE_CMOVE = "Base Layer.CMove";
        public const string STATE_CATKLP = "Base Layer.CAtkLP";
        public const string STATE_CATKMP = "Base Layer.CAtkMP";
        public const string STATE_CATKHP = "Base Layer.CAtkHP";
        public const string STATE_CATKLK = "Base Layer.CAtkLK";
        public const string STATE_CATKMK = "Base Layer.CAtkMK";
        public const string STATE_CATKHK = "Base Layer.CAtkHK";
        public const string STATE_CBLOCKL = "Base Layer.CBlockL";
        public const string STATE_CBLOCKM = "Base Layer.CBlockM";
        public const string STATE_CBLOCKH = "Base Layer.CBlockH";
        public const string STATE_CDAMAGEL = "Base Layer.CDamageL";
        public const string STATE_CDAMAGEM = "Base Layer.CDamageM";
        public const string STATE_CDAMAGEH = "Base Layer.CDamageH";

        public const string STATE_OBACKSTEP = "Base Layer.OBackstep";
        public const string STATE_OGIVEUP = "Base Layer.OGiveup";
        public const string STATE_ODOWN_SDAMAGEH = "Base Layer.ODown_SDamageH";
        public const string STATE_ODOWN = "Base Layer.ODown";
        public const string STATE_OSTANDUP = "Base Layer.OStandup";
        #endregion

        #region StellaQL用のユーザー定義タグ一覧
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

        protected UserDefinedStateTable()
        {
            #region タグの有効化
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

            #region ステートマシン拡張データ、ステート拡張データの有効化
            List<UserDefinedStateRecord> temp = new List<UserDefinedStateRecord>()
            {
                UserDefinedStateRecord.Build(  STATEMACHINE_BASELAYER, (CliptypeIndex)0, new []{ TAG_NONE}),

                UserDefinedStateRecord.Build(  STATE_SWAIT, CliptypeIndex.SWait, new []{ TAG_STAND}),
                UserDefinedStateRecord.Build(  STATE_SMOVE, CliptypeIndex.SMove, new []{ TAG_STAND}),
                UserDefinedStateRecord.Build(  STATE_SATKLP, CliptypeIndex.SAtkLP, new []{ TAG_BUSYX, TAG_STAND, TAG_LIGHT, TAG_PUNCH }),
                UserDefinedStateRecord.Build(  STATE_SATKMP, CliptypeIndex.SAtkMP, new []{ TAG_BUSYX, TAG_STAND, TAG_MEDIUM, TAG_PUNCH }),
                UserDefinedStateRecord.Build(  STATE_SATKHP,  CliptypeIndex.SAtkHP, new []{ TAG_BUSYX, TAG_STAND, TAG_HARD, TAG_PUNCH }),
                UserDefinedStateRecord.Build(  STATE_SATKLK, CliptypeIndex.SAtkLK, new []{ TAG_BUSYX, TAG_STAND, TAG_LIGHT, TAG_KICK }),
                UserDefinedStateRecord.Build(  STATE_SATKMK, CliptypeIndex.SAtkMK, new []{ TAG_BUSYX, TAG_STAND, TAG_MEDIUM, TAG_KICK }),
                UserDefinedStateRecord.Build(  STATE_SATKHK, CliptypeIndex.SAtkHK, new []{ TAG_BUSYX, TAG_STAND, TAG_HARD, TAG_KICK }),
                UserDefinedStateRecord.Build(  STATE_SBLOCKL, CliptypeIndex.SBlockL, new []{ TAG_BUSYX, TAG_STAND, TAG_LIGHT, TAG_BLOCK }),
                UserDefinedStateRecord.Build(  STATE_SBLOCKM, CliptypeIndex.SBlockM, new []{ TAG_BUSYX, TAG_STAND, TAG_MEDIUM, TAG_BLOCK }),
                UserDefinedStateRecord.Build(  STATE_SBLOCKH, CliptypeIndex.SBlockH, new []{ TAG_BUSYX, TAG_STAND, TAG_HARD, TAG_BLOCK }),
                UserDefinedStateRecord.Build(  STATE_SDAMAGEL, CliptypeIndex.SDamageL, new []{ TAG_NONE, TAG_STAND, TAG_LIGHT, TAG_DAMAGE }),
                UserDefinedStateRecord.Build(  STATE_SDAMAGEM, CliptypeIndex.SDamageM, new []{ TAG_NONE, TAG_STAND, TAG_MEDIUM, TAG_DAMAGE }),
                UserDefinedStateRecord.Build(  STATE_SDAMAGEH, CliptypeIndex.SDamageH, new []{ TAG_NONE, TAG_STAND, TAG_HARD, TAG_DAMAGE }),

                UserDefinedStateRecord.Build(  STATEMACHINE_JMOVE, (CliptypeIndex)0, new []{ TAG_JUMP}),
                UserDefinedStateRecord.Build(  STATE_JMOVE0, CliptypeIndex.JMove0,new []{ TAG_BUSYX, TAG_BUSYY, TAG_JUMP }),
                UserDefinedStateRecord.Build(  STATE_JMOVE1, CliptypeIndex.JMove1, new []{ TAG_JUMP }),
                UserDefinedStateRecord.Build( STATE_JMOVE2,  CliptypeIndex.JMove2, new []{ TAG_JUMP }),
                UserDefinedStateRecord.Build(  STATE_JMOVE3, CliptypeIndex.JMove3, new []{ TAG_JUMP }),
                UserDefinedStateRecord.Build( STATE_JMOVE4, CliptypeIndex.JMove4, new []{ TAG_BUSYX, TAG_JUMP }),
                UserDefinedStateRecord.Build(  STATE_JATKLP, CliptypeIndex.JAtkLP, new []{ TAG_JUMP, TAG_LIGHT, TAG_PUNCH }),
                UserDefinedStateRecord.Build(  STATE_JATKMP, CliptypeIndex.JAtkMP, new []{ TAG_JUMP, TAG_MEDIUM, TAG_PUNCH }),
                UserDefinedStateRecord.Build(  STATE_JATKHP, CliptypeIndex.JAtkHP, new []{ TAG_JUMP, TAG_HARD, TAG_PUNCH }),
                UserDefinedStateRecord.Build( STATE_JATKLK, CliptypeIndex.JAtkLK, new []{ TAG_JUMP, TAG_LIGHT, TAG_KICK }),
                UserDefinedStateRecord.Build( STATE_JATKMK,  CliptypeIndex.JAtkMK, new []{ TAG_JUMP, TAG_MEDIUM, TAG_KICK }),
                UserDefinedStateRecord.Build(  STATE_JATKHK, CliptypeIndex.JAtkHK, new []{ TAG_JUMP, TAG_HARD, TAG_KICK }),
                UserDefinedStateRecord.Build(  STATE_JBLOCKL, CliptypeIndex.JBlockL, new []{ TAG_BUSYX, TAG_JUMP, TAG_LIGHT, TAG_BLOCK }),
                UserDefinedStateRecord.Build(  STATE_JBLOCKM, CliptypeIndex.JBlockM, new []{ TAG_BUSYX, TAG_JUMP, TAG_MEDIUM, TAG_BLOCK }),
                UserDefinedStateRecord.Build(  STATE_JBLOCKH, CliptypeIndex.JBlockH, new []{ TAG_BUSYX, TAG_JUMP, TAG_HARD, TAG_BLOCK }),
                UserDefinedStateRecord.Build(  STATE_JDAMAGEL, CliptypeIndex.JDamageL, new []{ TAG_JUMP, TAG_LIGHT, TAG_DAMAGE }),
                UserDefinedStateRecord.Build(  STATE_JDAMAGEM, CliptypeIndex.JDamageM, new []{ TAG_JUMP, TAG_MEDIUM, TAG_DAMAGE }),
                UserDefinedStateRecord.Build(  STATE_JDAMAGEH, CliptypeIndex.JDamageH, new []{ TAG_JUMP, TAG_HARD, TAG_DAMAGE }),

                UserDefinedStateRecord.Build(  STATE_DMOVE, CliptypeIndex.DMove, new []{ TAG_DASH }),
                UserDefinedStateRecord.Build(  STATE_DATKLP, CliptypeIndex.DAtkLP, new []{ TAG_DASH, TAG_LIGHT, TAG_PUNCH }),
                UserDefinedStateRecord.Build(  STATE_DATKMP, CliptypeIndex.DAtkMP, new []{ TAG_DASH, TAG_MEDIUM, TAG_PUNCH }),
                UserDefinedStateRecord.Build(  STATE_DATKHP, CliptypeIndex.DAtkHP, new []{ TAG_DASH, TAG_HARD, TAG_PUNCH }),
                UserDefinedStateRecord.Build(  STATE_DATKLK, CliptypeIndex.DAtkLK, new []{ TAG_DASH, TAG_LIGHT, TAG_KICK }),
                UserDefinedStateRecord.Build(  STATE_DATKMK, CliptypeIndex.DAtkMK, new []{ TAG_DASH, TAG_MEDIUM, TAG_KICK }),
                UserDefinedStateRecord.Build(  STATE_DATKHK, CliptypeIndex.DAtkHK, new []{ TAG_DASH, TAG_HARD, TAG_KICK }),
                UserDefinedStateRecord.Build(  STATE_DBLOCKL, CliptypeIndex.DBlockL, new []{ TAG_BUSYX, TAG_DASH, TAG_LIGHT, TAG_BLOCK }),
                UserDefinedStateRecord.Build(  STATE_DBLOCKM, CliptypeIndex.DBlockM, new []{ TAG_BUSYX, TAG_DASH, TAG_MEDIUM, TAG_BLOCK }),
                UserDefinedStateRecord.Build(  STATE_DBLOCKH, CliptypeIndex.DBlockH, new []{ TAG_BUSYX, TAG_DASH, TAG_HARD, TAG_BLOCK }),
                UserDefinedStateRecord.Build(  STATE_DDAMAGEL, CliptypeIndex.DDamageL, new []{ TAG_DASH, TAG_LIGHT, TAG_DAMAGE }),
                UserDefinedStateRecord.Build(  STATE_DDAMAGEM, CliptypeIndex.DDamageM, new []{ TAG_DASH, TAG_MEDIUM, TAG_DAMAGE }),
                UserDefinedStateRecord.Build(  STATE_DDAMAGEH, CliptypeIndex.DDamageH, new []{ TAG_DASH, TAG_HARD, TAG_DAMAGE }),

                UserDefinedStateRecord.Build(  STATE_CWAIT, CliptypeIndex.CWait, new []{ TAG_CROUCH }),
                UserDefinedStateRecord.Build(  STATE_CMOVE, CliptypeIndex.CMove, new []{ TAG_CROUCH }),
                UserDefinedStateRecord.Build(  STATE_CATKLP, CliptypeIndex.CAtkLP, new []{ TAG_BUSYX, TAG_CROUCH, TAG_LIGHT, TAG_PUNCH }),
                UserDefinedStateRecord.Build(  STATE_CATKMP, CliptypeIndex.CAtkMP, new []{ TAG_BUSYX, TAG_CROUCH, TAG_MEDIUM, TAG_PUNCH }),
                UserDefinedStateRecord.Build(  STATE_CATKHP, CliptypeIndex.CAtkHP, new []{ TAG_BUSYX, TAG_CROUCH, TAG_HARD, TAG_PUNCH }),
                UserDefinedStateRecord.Build( STATE_CATKLK, CliptypeIndex.CAtkLK, new []{ TAG_BUSYX, TAG_CROUCH, TAG_LIGHT, TAG_KICK }),
                UserDefinedStateRecord.Build( STATE_CATKMK, CliptypeIndex.CAtkMK, new []{ TAG_BUSYX, TAG_CROUCH, TAG_MEDIUM, TAG_KICK }),
                UserDefinedStateRecord.Build(  STATE_CATKHK, CliptypeIndex.CAtkHK, new []{ TAG_BUSYX, TAG_CROUCH, TAG_HARD, TAG_KICK }),
                UserDefinedStateRecord.Build(  STATE_CBLOCKL, CliptypeIndex.CBlockL, new []{ TAG_BUSYX, TAG_CROUCH, TAG_LIGHT, TAG_BLOCK }),
                UserDefinedStateRecord.Build(  STATE_CBLOCKM, CliptypeIndex.CBlockM, new []{ TAG_BUSYX, TAG_CROUCH, TAG_MEDIUM, TAG_BLOCK }),
                UserDefinedStateRecord.Build(  STATE_CBLOCKH, CliptypeIndex.CBlockH, new []{ TAG_BUSYX, TAG_CROUCH, TAG_HARD, TAG_BLOCK }),
                UserDefinedStateRecord.Build(  STATE_CDAMAGEL, CliptypeIndex.CDamageL, new []{ TAG_CROUCH, TAG_LIGHT, TAG_DAMAGE }),
                UserDefinedStateRecord.Build(  STATE_CDAMAGEM, CliptypeIndex.CDamageM, new []{ TAG_CROUCH, TAG_MEDIUM, TAG_DAMAGE }),
                UserDefinedStateRecord.Build(  STATE_CDAMAGEH, CliptypeIndex.CDamageH, new []{ TAG_CROUCH, TAG_HARD, TAG_DAMAGE }),

                UserDefinedStateRecord.Build(  STATE_OBACKSTEP, CliptypeIndex.OBackstep, new []{ TAG_OTHER }),
                UserDefinedStateRecord.Build(  STATE_OGIVEUP, CliptypeIndex.OGiveup, new []{ TAG_OTHER }),
                UserDefinedStateRecord.Build(  STATE_ODOWN_SDAMAGEH, CliptypeIndex.SDamageH, new []{ TAG_BUSYX, TAG_OTHER, TAG_HARD }),
                UserDefinedStateRecord.Build(  STATE_ODOWN, CliptypeIndex.ODown,             new []{ TAG_BUSYX, TAG_OTHER }),
                UserDefinedStateRecord.Build(  STATE_OSTANDUP, CliptypeIndex.OStandup, new []{ TAG_OTHER }),
            };
            foreach (UserDefinedStateRecord record in temp) { StateHash_to_record.Add(record.FullPathHash, record); }
            #endregion
        }
    }
}

