using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellaQL;

/// <summary>
/// Main シーン
/// </summary>
namespace SceneMain
{
    /// <summary>
    /// アニメーターのステート
    /// </summary>
    public class StateExRecord : AbstractStateExRecord
    {
        public static StateExRecord Build(string fullpath, HashSet<int> requiredAllTags)
        {
            return new StateExRecord(fullpath, Animator.StringToHash(fullpath), requiredAllTags);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="cliptype"></param>
        /// <param name="attributeEnum"></param>
        /// <returns></returns>
        public static StateExRecord Build(string fullpath, CliptypeIndex cliptype, string[] requiredAllTags)
        {
            return new StateExRecord(fullpath, Animator.StringToHash(fullpath), cliptype, Code.Hashs(requiredAllTags) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="fullpathHash"></param>
        /// <param name="attributeEnum">タグのハッシュ</param>
        public StateExRecord(string fullpath, int fullpathHash, HashSet<int> requiredAllTags) :base(fullpath, fullpathHash, requiredAllTags)
        {
            this.Cliptype = -1; // クリップタイプを使わない場合
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="fullpathHash"></param>
        /// <param name="cliptype"></param>
        /// <param name="attributeEnum">タグのハッシュ</param>
        public StateExRecord(string fullpath, int fullpathHash, CliptypeIndex cliptype, HashSet<int> requiredAllTags) :base(fullpath, fullpathHash, requiredAllTags)
        {
            this.Cliptype = (int)cliptype;
        }

        public override bool HasFlag_attr(HashSet<int> requiredAllTags)
        {
            foreach (int tag in requiredAllTags)
            {
                if (!Tags.Contains(tag)) { return false; } // １個でも持ってないタグがあれば偽。
            }
            return true;
        }
    }

    public class StateExTable : AbstractStateExTable
    {
        static StateExTable()
        {
            Instance = new StateExTable();
        }
        public static StateExTable Instance { get; set; }

        public const string TAG_NONE = "None";
        public const string TAG_BUSYX = "BusyX";
        public const string TAG_BUSYY = "BusyY";
        public const string TAG_BLOCK = "Block";
        public const string TAG_STAND = "Stand";

        #region フルパス一覧
        public const string STATE_SWAIT = "Base Layer.SWait";
        public const string STATE_SMOVE = "Base Layer.SMove";
        public const string STATE_SBLOCKL = "Base Layer.SBlockL";
        public const string STATE_SBLOCKM = "Base Layer.SBlockM";
        public const string STATE_SBLOCKH = "Base Layer.SBlockH";
        public const string STATE_SATKLP = "Base Layer.SAtkLP";
        public const string STATE_SATKMP = "Base Layer.SAtkMP";
        public const string STATE_SATKHP = "Base Layer.SAtkHP";
        public const string STATE_SATKLK = "Base Layer.SAtkLK";
        public const string STATE_SATKMK = "Base Layer.SAtkMK";
        public const string STATE_SATKHK = "Base Layer.SAtkHK";

        public const string STATE_OBACKSTEP = "Base Layer.OBackstep";

        public const string STATE_JBLOCKL = "Base Layer.JBlockL";
        public const string STATE_JBLOCKM = "Base Layer.JBlockM";
        public const string STATE_JBLOCKH = "Base Layer.JBlockH";
        public const string STATE_JATKLP = "Base Layer.JAtkLP";
        public const string STATE_JATKMP = "Base Layer.JAtkMP";
        public const string STATE_JATKHP = "Base Layer.JAtkHP";
        public const string STATE_JATKLK = "Base Layer.JAtkLK";
        public const string STATE_JATKMK = "Base Layer.JAtkMK";
        public const string STATE_JATKHK = "Base Layer.JAtkHK";

        public const string STATE_JMOVE0 = "Base Layer.JMove.JMove0";
        public const string STATE_JMOVE1 = "Base Layer.JMove.JMove1";
        public const string STATE_JMOVE2 = "Base Layer.JMove.JMove2";
        public const string STATE_JMOVE3 = "Base Layer.JMove.JMove3";
        public const string STATE_JMOVE4 = "Base Layer.JMove.JMove4";

        public const string STATE_DBLOCKL = "Base Layer.DBlockL";
        public const string STATE_DBLOCKM = "Base Layer.DBlockM";
        public const string STATE_DBLOCKH = "Base Layer.DBlockH";
        public const string STATE_DATKLP = "Base Layer.DAtkLP";
        public const string STATE_DATKMP = "Base Layer.DAtkMP";
        public const string STATE_DATKHP = "Base Layer.DAtkHP";
        public const string STATE_DATKLK = "Base Layer.DAtkLK";
        public const string STATE_DATKMK = "Base Layer.DAtkMK";
        public const string STATE_DATKHK = "Base Layer.DAtkHK";

        public const string STATE_DMOVE = "Base Layer.DMove";

        public const string STATE_CBLOCKL = "Base Layer.CBlockL";
        public const string STATE_CBLOCKM = "Base Layer.CBlockM";
        public const string STATE_CBLOCKH = "Base Layer.CBlockH";
        public const string STATE_CATKLP = "Base Layer.CAtkLP";
        public const string STATE_CATKMP = "Base Layer.CAtkMP";
        public const string STATE_CATKHP = "Base Layer.CAtkHP";
        public const string STATE_CATKLK = "Base Layer.CAtkLK";
        public const string STATE_CATKMK = "Base Layer.CAtkMK";
        public const string STATE_CATKHK = "Base Layer.CAtkHK";

        public const string STATE_CWAIT = "Base Layer.CWait";
        public const string STATE_CMOVE = "Base Layer.CMove";

        public const string STATE_OGIVEUP = "Base Layer.OGiveup";
        public const string STATE_ODOWN_SDAMAGEH = "Base Layer.ODown_SDamageH";
        public const string STATE_ODOWN = "Base Layer.ODown";
        public const string STATE_OSTANDUP = "Base Layer.OStandup";

        public const string STATE_SDAMAGEL = "Base Layer.SDamageL";
        public const string STATE_SDAMAGEM = "Base Layer.SDamageM";
        public const string STATE_SDAMAGEH = "Base Layer.SDamageH";

        public const string STATE_JDAMAGEL = "Base Layer.JDamageL";
        public const string STATE_JDAMAGEM = "Base Layer.JDamageM";
        public const string STATE_JDAMAGEH = "Base Layer.JDamageH";

        public const string STATE_DDAMAGEL = "Base Layer.DDamageL";
        public const string STATE_DDAMAGEM = "Base Layer.DDamageM";
        public const string STATE_DDAMAGEH = "Base Layer.DDamageH";

        public const string STATE_CDAMAGEL = "Base Layer.CDamageL";
        public const string STATE_CDAMAGEM = "Base Layer.CDamageM";
        public const string STATE_CDAMAGEH = "Base Layer.CDamageH";
        #endregion


        protected StateExTable()
        {
            String_to_tagHash = Code.HashsDic(new []{
                TAG_NONE,
                TAG_BUSYX,
                TAG_BUSYY,
                TAG_BLOCK,
                TAG_STAND,
            });

            List<StateExRecord> temp = new List<StateExRecord>()
            {
                StateExRecord.Build(  STATE_SWAIT, CliptypeIndex.SWait, new []{ TAG_NONE}),
                StateExRecord.Build(  STATE_SMOVE, CliptypeIndex.SMove, new []{ TAG_NONE}),
                StateExRecord.Build(  STATE_SBLOCKL, CliptypeIndex.SBlockL, new []{ TAG_BUSYX, TAG_BLOCK }),
                StateExRecord.Build(  STATE_SBLOCKM, CliptypeIndex.SBlockM, new []{ TAG_BUSYX, TAG_BLOCK }),
                StateExRecord.Build(  STATE_SBLOCKH, CliptypeIndex.SBlockH, new []{ TAG_BUSYX, TAG_BLOCK }),
                StateExRecord.Build(  STATE_SATKLP, CliptypeIndex.SAtkLP, new []{ TAG_BUSYX }),
                StateExRecord.Build(  STATE_SATKMP, CliptypeIndex.SAtkMP, new []{ TAG_BUSYX }),
                StateExRecord.Build(  STATE_SATKHP,  CliptypeIndex.SAtkHP, new []{ TAG_BUSYX }),
                StateExRecord.Build(  STATE_SATKLK, CliptypeIndex.SAtkLK, new []{ TAG_BUSYX }),
                StateExRecord.Build(  STATE_SATKMK, CliptypeIndex.SAtkMK, new []{ TAG_BUSYX }),
                StateExRecord.Build(  STATE_SATKHK, CliptypeIndex.SAtkHK, new []{ TAG_BUSYX }),

                StateExRecord.Build(  STATE_OBACKSTEP, CliptypeIndex.OBackstep, new []{ TAG_NONE }),

                StateExRecord.Build(  STATE_JBLOCKL, CliptypeIndex.JBlockL, new []{ TAG_BUSYX, TAG_BLOCK }),
                StateExRecord.Build(  STATE_JBLOCKM, CliptypeIndex.JBlockM, new []{ TAG_BUSYX, TAG_BLOCK }),
                StateExRecord.Build(  STATE_JBLOCKH, CliptypeIndex.JBlockH, new []{ TAG_BUSYX, TAG_BLOCK }),
                StateExRecord.Build(  STATE_JATKLP, CliptypeIndex.JAtkLP, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_JATKMP, CliptypeIndex.JAtkMP, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_JATKHP, CliptypeIndex.JAtkHP, new []{ TAG_NONE }),
                StateExRecord.Build( STATE_JATKLK, CliptypeIndex.JAtkLK, new []{ TAG_NONE }),
                StateExRecord.Build( STATE_JATKMK,  CliptypeIndex.JAtkMK, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_JATKHK, CliptypeIndex.JAtkHK, new []{ TAG_NONE }),

                StateExRecord.Build(  STATE_JMOVE0, CliptypeIndex.JMove0,new []{ TAG_BUSYX, TAG_BUSYY }),
                StateExRecord.Build(  STATE_JMOVE1, CliptypeIndex.JMove1, new []{ TAG_NONE }),
                StateExRecord.Build( STATE_JMOVE2,  CliptypeIndex.JMove2, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_JMOVE3, CliptypeIndex.JMove3, new []{ TAG_NONE }),
                StateExRecord.Build( STATE_JMOVE4, CliptypeIndex.JMove4, new []{ TAG_BUSYX }),

                StateExRecord.Build(  STATE_DBLOCKL, CliptypeIndex.DBlockL, new []{ TAG_BUSYX, TAG_BLOCK }),
                StateExRecord.Build(  STATE_DBLOCKM, CliptypeIndex.DBlockM, new []{ TAG_BUSYX, TAG_BLOCK }),
                StateExRecord.Build(  STATE_DBLOCKH, CliptypeIndex.DBlockH, new []{ TAG_BUSYX, TAG_BLOCK }),
                StateExRecord.Build(  STATE_DATKLP, CliptypeIndex.DAtkLP, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_DATKMP, CliptypeIndex.DAtkMP, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_DATKHP, CliptypeIndex.DAtkHP, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_DATKLK, CliptypeIndex.DAtkLK, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_DATKMK, CliptypeIndex.DAtkMK, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_DATKHK, CliptypeIndex.DAtkHK, new []{ TAG_NONE }),

                StateExRecord.Build(  STATE_DMOVE, CliptypeIndex.DMove, new []{ TAG_NONE }),

                StateExRecord.Build(  STATE_CBLOCKL, CliptypeIndex.CBlockL, new []{ TAG_BUSYX, TAG_BLOCK }),
                StateExRecord.Build(  STATE_CBLOCKM, CliptypeIndex.CBlockM, new []{ TAG_BUSYX, TAG_BLOCK }),
                StateExRecord.Build(  STATE_CBLOCKH, CliptypeIndex.CBlockH, new []{ TAG_BUSYX, TAG_BLOCK }),
                StateExRecord.Build(  STATE_CATKLP, CliptypeIndex.CAtkLP, new []{ TAG_BUSYX }),
                StateExRecord.Build(  STATE_CATKMP, CliptypeIndex.CAtkMP, new []{ TAG_BUSYX }),
                StateExRecord.Build(  STATE_CATKHP, CliptypeIndex.CAtkHP, new []{ TAG_BUSYX }),
                StateExRecord.Build( STATE_CATKLK, CliptypeIndex.CAtkLK, new []{ TAG_BUSYX }),
                StateExRecord.Build( STATE_CATKMK, CliptypeIndex.CAtkMK, new []{ TAG_BUSYX }),
                StateExRecord.Build(  STATE_CATKHK, CliptypeIndex.CAtkHK, new []{ TAG_BUSYX }),

                StateExRecord.Build(  STATE_CWAIT, CliptypeIndex.CWait, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_CMOVE, CliptypeIndex.CMove, new []{ TAG_NONE }),

                StateExRecord.Build(  STATE_OGIVEUP, CliptypeIndex.OGiveup, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_ODOWN_SDAMAGEH, CliptypeIndex.SDamageH, new []{ TAG_BUSYX }),
                StateExRecord.Build(  STATE_ODOWN, CliptypeIndex.ODown,             new []{ TAG_BUSYX }),
                StateExRecord.Build(  STATE_OSTANDUP, CliptypeIndex.OStandup, new []{ TAG_NONE }),

                StateExRecord.Build(  STATE_SDAMAGEL, CliptypeIndex.SDamageL, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_SDAMAGEM, CliptypeIndex.SDamageM, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_SDAMAGEH, CliptypeIndex.SDamageH, new []{ TAG_NONE }),

                StateExRecord.Build(  STATE_JDAMAGEL, CliptypeIndex.JDamageL, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_JDAMAGEM, CliptypeIndex.JDamageM, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_JDAMAGEH, CliptypeIndex.JDamageH, new []{ TAG_NONE }),

                StateExRecord.Build(  STATE_DDAMAGEL, CliptypeIndex.DDamageL, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_DDAMAGEM, CliptypeIndex.DDamageM, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_DDAMAGEH, CliptypeIndex.DDamageH, new []{ TAG_NONE }),

                StateExRecord.Build(  STATE_CDAMAGEL, CliptypeIndex.CDamageL, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_CDAMAGEM, CliptypeIndex.CDamageM, new []{ TAG_NONE }),
                StateExRecord.Build(  STATE_CDAMAGEH, CliptypeIndex.CDamageH, new []{ TAG_NONE }),
            };
            foreach (StateExRecord record in temp) { Hash_to_exRecord.Add(record.FullPathHash, record); }
        }

        /// <summary>
        /// キャラクターと、モーション、現在のフレームを指定することで、通し画像番号とスライス番号を返す。
        /// これにより Hitbox2DScript と連携を取ることができる。
        /// </summary>
        /// <param name="serialTilesetfileIndex"></param>
        /// <param name="slice"></param>
        /// <param name="character"></param>
        /// <param name="motion"></param>
        /// <param name="currentMotionFrame"></param>
        public static void GetSlice(out int serialTilesetfileIndex, out int slice, CharacterIndex character, CliptypeExRecordable cliptypeExRecord, int currentMotionFrame)
        {
            serialTilesetfileIndex = Hitbox2DOperationScript.GetSerialImageIndex(character, (TilesetfileTypeIndex)cliptypeExRecord.TilesetfileTypeIndex);
            slice = cliptypeExRecord.Slices[currentMotionFrame];
        }
    }
}

