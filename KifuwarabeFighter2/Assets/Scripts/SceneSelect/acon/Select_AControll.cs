using StellaQL;
using StellaQL.Acons;
using System.Collections.Generic;
using UnityEngine;

namespace SceneSelect
{
    /// <summary>
    /// (Step 3.) Please, create table definition of statemachines or states. (ステートマシン、ステートのテーブル定義を作成してください)
    /// Extend AbstractUserDefinedStateTable class. (AbstractUserDefinedStateTable クラスを継承してください)
    /// 
    /// (Step 4.) Click [Generate fullpath constant C#] button. and "using StellaQL.FullpathConst;". ([Generate fullpath constant C#]ボタンをクリックしてください)
    /// </summary>
    public class AControll : AbstractAControll
    {
        /// <summary>
        /// (Step 8.) Please, make singleton. (シングルトンにしてください)
        /// Use by StellaQLEngine/UserDefinedDatabase.cs file. (StellaQLEngine/UserDefinedDatabase.cs ファイルで使います)
        /// </summary>
        static AControll() { Instance = new AControll(); }
        public static AControll Instance { get; private set; }

        #region (Step 5.) Unfortunaly, Please, list user defined tags for StellaQL.  (残念ですが、StellaQL用のユーザー定義タグを定数にしてください)
        public const string TAG_ZERO = "Zero";
        #endregion

        AControll()
        {
            #region (Step 6.) Activate user defined tags. (ユーザー定義タグの有効化)
            TagString_to_hash = Code.HashesDic(new []{
                TAG_ZERO
            });
            #endregion

            #region (Step 7.) Register and activate user defined record of statemachines or states.(ステートマシン、ステートのユーザー定義レコードを設定してください)
            Code.Register(StateHash_to_record, new List<AcStateRecordable>()
            {
                new DefaultAcState( AconSelect.BASELAYER_STAY),
                new DefaultAcState( AconSelect.BASELAYER_MOVE),
                new DefaultAcState( AconSelect.BASELAYER_READY),
                new DefaultAcState( AconSelect.BASELAYER_TIMEOVER),
            });
            #endregion
        }
    }

}
