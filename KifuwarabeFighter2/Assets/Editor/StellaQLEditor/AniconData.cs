//
// Animation Controller Tables
//
using System.Collections.Generic;
using System.Text;
using UnityEditor.Animations;
using UnityEngine;

namespace StellaQL
{
    /// <summary>
    /// 列定義レコード
    /// </summary>
    public class RecordDefinition
    {
        public enum KeyType
        {
            /// <summary>
            /// StellaQLで使う、一時的なナンバリング
            /// </summary>
            TemporaryNumbering,
            /// <summary>
            /// Unityで識別子に使えそうなもの
            /// </summary>
            Identifiable,
            None,
        }

        public enum FieldType
        {
            Int,
            Float,
            Bool,
            String,
            Other,//対応外
        }

        public RecordDefinition(string name, FieldType type, KeyType keyField, bool input)
        {
            this.Name = name;
            this.Type = type;
            this.KeyField = keyField;
            this.Input = input;
            this.m_getterBool = null;
            this.m_setterBool = null;
            this.m_getterFloat = null;
            this.m_setterFloat = null;
            this.m_getterInt = null;
            this.m_setterInt = null;
            this.m_getterString = null;
            this.m_setterString = null;
        }
        public RecordDefinition(string name, FieldType type, KeyType keyField, GettterBool getter, SetterBool setter)       : this(name, type, keyField, true)
        {
            this.m_getterBool = getter;     this.m_setterBool = setter;
        }
        public RecordDefinition(string name, FieldType type, KeyType keyField, GettterFloat getter, SetterFloat setter)     : this(name,type,keyField, true)
        {
            this.m_getterFloat = getter;    this.m_setterFloat = setter;
        }
        public RecordDefinition(string name, FieldType type, KeyType keyField, GettterInt getter, SetterInt setter)         : this(name, type, keyField, true)
        {
            this.m_getterInt = getter;      this.m_setterInt = setter;
        }
        public RecordDefinition(string name, FieldType type, KeyType keyField, GettterString getter, SetterString setter)   : this(name, type, keyField, true)
        {
            this.m_getterString = getter;   this.m_setterString = setter;
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 型
        /// </summary>
        public FieldType Type { get; private set; }

        /// <summary>
        /// キーとして利用できるフィールドか
        /// </summary>
        public KeyType KeyField { get; private set; }

        /// <summary>
        /// スプレッド・シートから入力可能か
        /// </summary>
        public bool Input { get; private set; }

        /// <summary>
        /// 列の記入漏れを防ぐためのものだぜ☆（＾～＾）
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="c">contents (コンテンツ)</param>
        /// <param name="n">output column name (列名出力)</param>
        /// <param name="d">output definition (列定義出力)</param>
        public void AppendCsv(Dictionary<string, object> fields, StringBuilder contents, bool outputColumnName, bool outputDefinition)
        {
            if (outputDefinition) // 列定義一列分を出力するなら
            {
                if (outputColumnName) // 列名を出力するなら
                {
                }
                else
                {
                    contents.Append(Name); contents.Append(",");
                    contents.Append(Type.ToString().Substring(0, 1).ToLower()); // 型名の先頭を小文字にする
                    contents.Append(Type.ToString().Substring(1));
                    contents.Append(",");
                    contents.Append(KeyField); contents.Append(",");
                    contents.Append(Input); contents.Append(",");
                    contents.AppendLine();
                }
            }
            else // 1フィールド分を出力するなら
            {
                if (outputColumnName) // 列名を出力するなら
                {
                    switch (this.Type)
                    {
                        case FieldType.Int://thru
                        case FieldType.Float:
                        case FieldType.Bool: contents.Append(this.Name); contents.Append(","); break;
                        case FieldType.String://thru
                        case FieldType.Other:
                        default: contents.Append(this.Name); contents.Append(","); break;
                    }
                }
                else
                {
                    switch (this.Type)
                    {
                        case FieldType.Int://thru
                        case FieldType.Float:
                        case FieldType.Bool: contents.Append(fields[Name]); contents.Append(","); break;
                        case FieldType.String://thru
                        case FieldType.Other:
                        default: contents.Append(CsvParser.EscapeCell((string)fields[Name])); contents.Append(","); break;
                    }
                }
            }
        }

        public static void AppendDefinitionHeader(StringBuilder contents)
        {
            contents.AppendLine("Name,Type,KeyField,Input,[EOL],"); // 列定義ヘッダー出力
        }

        public delegate bool   GettterBool  (object instance);                  GettterBool m_getterBool;
        public delegate void   SetterBool   (object instance, bool value);      SetterBool m_setterBool;
        public delegate float  GettterFloat (object instance);                  GettterFloat m_getterFloat;
        public delegate void   SetterFloat  (object instance, float value);     SetterFloat m_setterFloat;
        public delegate int    GettterInt   (object instance);                  GettterInt m_getterInt;
        public delegate void   SetterInt    (object instance, int value);       SetterInt m_setterInt;
        public delegate string GettterString(object instance);                  GettterString m_getterString;
        public delegate void   SetterString (object instance, string value);    SetterString m_setterString;

        public void Update(object instance, UpateReqeustRecord record, StringBuilder message)
        {
            if (null == instance) { throw new UnityException("instanceがヌルだったぜ☆（／＿＼）"); }

            string propertyName = Name;
            switch (Type)
            {
                case FieldType.Bool:
                    {
                        if (null == m_getterBool) { throw new UnityException("m_getterBoolがヌルだったぜ☆（／＿＼）"); }
                        bool actual = m_getterBool(instance);
                        if (actual == record.OldBool) { m_setterBool(instance, record.NewBool); }
                        else { throw new UnityException("Old値が異なる actual=[" + actual + "] old=[" + record.OldBool + "]"); }
                    }
                    break;
                case FieldType.Float:
                    {
                        if (null == m_getterFloat) { throw new UnityException("m_getterFloatがヌルだったぜ☆（／＿＼）"); }
                        float actual = m_getterFloat(instance);
                        if (actual == record.OldFloat) { m_setterFloat(instance, record.NewFloat); }
                        else { throw new UnityException("Old値が異なる actual=[" + actual + "] old=[" + record.OldFloat + "]"); }
                    }
                    break;
                case FieldType.Int:
                    {
                        if (null == m_getterInt) { throw new UnityException("m_getterIntがヌルだったぜ☆（／＿＼）"); }
                        int actual = m_getterInt(instance);
                        if (actual == record.OldInt) { m_setterInt(instance, record.NewInt); }
                        else { throw new UnityException("Old値が異なる actual=[" + actual + "] old=[" + record.OldInt + "]"); }
                    }
                    break;
                case FieldType.Other:
                    // 未対応は、この型にしてあるんだぜ☆（＾▽＾）
                    break;
                case FieldType.String:
                    {
                        if (null == m_getterString) { throw new UnityException("m_getterStringがヌルだったぜ☆（／＿＼）"); }
                        string actual = m_getterString(instance);
                        if (actual == record.Old) { m_setterString(instance, record.New); }
                        else { throw new UnityException("Old値が異なる actual=[" + actual + "] old=[" + record.Old + "]"); }
                    }
                    break;
                default: throw new UnityException("未定義の型だぜ☆（＾～＾） FieldType=["+Type.ToString()+"]");
            }
        }
    }

    /// <summary>
    /// パラメーター
    /// </summary>
    public class ParameterRecord
    {
        static ParameterRecord()
        {
            List<RecordDefinition> temp = new List<RecordDefinition>()
            {
                // FIXME: パラメーターの編集は他とパターンが異なる☆
                new RecordDefinition("num", RecordDefinition.FieldType.Int, RecordDefinition.KeyType.TemporaryNumbering, false),
                new RecordDefinition("name", RecordDefinition.FieldType.String,RecordDefinition.KeyType.Identifiable,false),
                new RecordDefinition("numberBool", RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None,false),
                new RecordDefinition("numberFloat", RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None,false),
                new RecordDefinition("numberInt", RecordDefinition.FieldType.Int,RecordDefinition.KeyType.None,false),
                new RecordDefinition("nameHash", RecordDefinition.FieldType.Int,RecordDefinition.KeyType.None,false),
            };
            Definitions = new Dictionary<string, RecordDefinition>();
            foreach (RecordDefinition def in temp) { Definitions.Add(def.Name, def); }
            Empty = new ParameterRecord(-1, "", false, 0.0f, -1, 0);
        }
        public static Dictionary<string, RecordDefinition> Definitions { get; private set; }
        public static ParameterRecord Empty { get; private set; }

        public ParameterRecord(int num, string name, bool numberBool, float numberFloat, int numberInt, int nameHash)
        {
            this.Fields = new Dictionary<string, object>()
            {
                { "num",num },
                { "name", name},
                { "numberBool", numberBool },
                { "numberFloat", numberFloat},
                { "numberInt", numberInt},
                { "nameHash", nameHash},
            };
        }
        public Dictionary<string, object> Fields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">contents (コンテンツ)</param>
        /// <param name="n">output column name (列名出力)</param>
        /// <param name="d">output definition (列定義出力)</param>
        public void AppendCsvLine(StringBuilder c, bool n, bool d)
        {
            Definitions["num"].AppendCsv(Fields, c, n, d);
            Definitions["name"].AppendCsv(Fields, c, n, d);
            Definitions["numberBool"].AppendCsv(Fields, c, n, d);
            Definitions["numberFloat"].AppendCsv(Fields, c, n, d);
            Definitions["numberInt"].AppendCsv(Fields, c, n, d);
            Definitions["nameHash"].AppendCsv(Fields, c, n, d);
            if (n) { c.Append("[EOL],"); }
            if (!d) { c.AppendLine(); }
        }

    }

    /// <summary>
    /// レイヤー
    /// </summary>
    public class LayerRecord
    {
        static LayerRecord()
        {
            List<RecordDefinition> temp = new List<RecordDefinition>()
            {
                // #で囲んでいるのは、StellaQL用のフィールド。文字列検索しやすいように単語を # で挟んでいる。
                new RecordDefinition("#layerNum#",RecordDefinition.FieldType.Int, RecordDefinition.KeyType.TemporaryNumbering, false),
                new RecordDefinition("name",RecordDefinition.FieldType.String,RecordDefinition.KeyType.Identifiable,false),
                new RecordDefinition("avatarMask",RecordDefinition.FieldType.Other,RecordDefinition.KeyType.None,false),
                new RecordDefinition("blendingMode",RecordDefinition.FieldType.Other,RecordDefinition.KeyType.None,false),
                new RecordDefinition("defaultWeight",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None             ,(object i)=>{ return ((AnimatorControllerLayer)i).defaultWeight; }             ,(object i,float v)=>{ ((AnimatorControllerLayer)i).defaultWeight = v; }),
                new RecordDefinition("iKPass",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None                     ,(object i)=>{ return ((AnimatorControllerLayer)i).iKPass; }                    ,(object i,bool v)=>{ ((AnimatorControllerLayer)i).iKPass = v; }),
                new RecordDefinition("syncedLayerAffectsTiming",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None   ,(object i)=>{ return ((AnimatorControllerLayer)i).syncedLayerAffectsTiming; }  ,(object i,bool v)=>{ ((AnimatorControllerLayer)i).syncedLayerAffectsTiming = v; }),
                new RecordDefinition("syncedLayerIndex",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.None            ,(object i)=>{ return ((AnimatorControllerLayer)i).syncedLayerIndex; }          ,(object i,int v)=>{ ((AnimatorControllerLayer)i).syncedLayerIndex = v; }),
            };
            Definitions = new Dictionary<string, RecordDefinition>();
            foreach (RecordDefinition def in temp) { Definitions.Add(def.Name, def); }
            Empty = new LayerRecord(-1, new AnimatorControllerLayer());
        }
        public static Dictionary<string, RecordDefinition> Definitions { get; private set; }
        public static LayerRecord Empty { get; private set; }

        public LayerRecord(int num, AnimatorControllerLayer layer)
        {
            this.Fields = new Dictionary<string, object>()
            {
                { "#layerNum#",num },//レイヤー行番号
                { "name", layer.name},//レイヤー名
                { "avatarMask",layer.avatarMask == null ? "" : layer.avatarMask.ToString() },
                { "blendingMode", layer.blendingMode.ToString()},
                { "defaultWeight", layer.defaultWeight},
                { "iKPass", layer.iKPass},
                { "syncedLayerAffectsTiming", layer.syncedLayerAffectsTiming},
                { "syncedLayerIndex", layer.syncedLayerIndex},
            };
        }
        public Dictionary<string,object> Fields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">contents (コンテンツ)</param>
        /// <param name="n">output column name (列名出力)</param>
        /// <param name="d">output definition (列定義出力)</param>
        public void AppendCsvLine(StringBuilder c, bool n, bool d)
        {
            Definitions["#layerNum#"].AppendCsv(Fields, c, n, d); // レイヤー行番号
            Definitions["name"].AppendCsv(Fields, c, n, d); // レイヤー名
            Definitions["avatarMask"].AppendCsv(Fields, c, n, d);
            Definitions["blendingMode"].AppendCsv(Fields, c, n, d);
            Definitions["defaultWeight"].AppendCsv(Fields, c, n, d);
            Definitions["iKPass"].AppendCsv(Fields, c, n, d);
            Definitions["syncedLayerAffectsTiming"].AppendCsv(Fields, c, n, d);
            Definitions["syncedLayerIndex"].AppendCsv(Fields, c, n, d);
            if (n) { c.Append("[EOL],"); }
            if (!d) { c.AppendLine(); }
        }
    }

    /// <summary>
    /// ステートマシン
    /// </summary>
    public class StatemachineRecord
    {
        static StatemachineRecord()
        {
            List<RecordDefinition> temp = new List<RecordDefinition>()
            {
                // #で囲んでいるのは、StellaQL用のフィールド。文字列検索しやすいように単語を # で挟んでいる。
                new RecordDefinition("#layerNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#machineStateNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#fullnameEndsWithDot#",RecordDefinition.FieldType.String,RecordDefinition.KeyType.Identifiable,false), // 表示用フィールド（フルネーム）はこれで十分。後ろにドットが付いているが……。
                new RecordDefinition("name",RecordDefinition.FieldType.String,RecordDefinition.KeyType.None,false),// 「#fullnameEndsWithDot#」フィールドで十分なので、これは表示用フィールドにしない方が一貫性がある。
                new RecordDefinition("anyStateTransitions",RecordDefinition.FieldType.Other,RecordDefinition.KeyType.None,false),
                new RecordDefinition("behaviours",RecordDefinition.FieldType.Other,RecordDefinition.KeyType.None,false),
                new RecordDefinition("defaultState",RecordDefinition.FieldType.Other,RecordDefinition.KeyType.None,false),
                new RecordDefinition("entryTransitions",RecordDefinition.FieldType.Other,RecordDefinition.KeyType.None,false),
                new RecordDefinition("hideFlags",RecordDefinition.FieldType.Other,RecordDefinition.KeyType.None,false),
            };
            Definitions = new Dictionary<string, RecordDefinition>();
            foreach (RecordDefinition def in temp) { Definitions.Add(def.Name, def); }

            Empty = new StatemachineRecord(-1,-1,"",new AnimatorStateMachine(),new List<PositionRecord>());
        }
        public static Dictionary<string, RecordDefinition> Definitions { get; private set; }
        public static StatemachineRecord Empty { get; private set; }

        public StatemachineRecord(int layerNum, int machineStateNum, string fullnameEndsWithDot, AnimatorStateMachine stateMachine, List<PositionRecord> positionsTable)
        {
            // fullnameEndsWithDot には「Base Layer.」のような文字が入っているが、レイヤーの持つステートマシンの名前も「Base Layer」なので、つなげると「Base Layer.Base Layer」になってしまう。
            // state から見れば親パスだが。

            this.Fields = new Dictionary<string, object>()
            {
                { "#layerNum#",layerNum },//レイヤー行番号
                { "#machineStateNum#", machineStateNum},
                { "#fullnameEndsWithDot#",fullnameEndsWithDot },// FIXME: これはフルパスになるのでは？
                { "name", stateMachine.name},// これは葉の名前
                { "anyStateTransitions", stateMachine.anyStateTransitions == null ? "" : stateMachine.anyStateTransitions.ToString()},
                { "behaviours", stateMachine.behaviours == null ? "" : stateMachine.behaviours.ToString()},
                { "defaultState", stateMachine.defaultState == null ? "" : stateMachine.defaultState.ToString()},
                { "entryTransitions", stateMachine.entryTransitions == null ? "" : stateMachine.entryTransitions.ToString()},
                { "hideFlags", stateMachine.hideFlags.ToString()},
            };

            if (stateMachine.anyStatePosition != null) { positionsTable.Add(new PositionRecord(layerNum, machineStateNum, -1, -1, -1, "anyStatePosition", stateMachine.anyStatePosition)); }
            if (stateMachine.entryPosition != null) { positionsTable.Add(new PositionRecord(layerNum, machineStateNum, -1, -1, -1, "entryPosition", stateMachine.entryPosition)); }
            if (stateMachine.exitPosition != null) { positionsTable.Add(new PositionRecord(layerNum, machineStateNum, -1, -1, -1, "exitPosition", stateMachine.exitPosition)); }
            if (stateMachine.parentStateMachinePosition != null) { positionsTable.Add(new PositionRecord(layerNum, machineStateNum, -1, -1, -1, "parentStateMachinePosition", stateMachine.parentStateMachinePosition)); }
        }
        public Dictionary<string, object> Fields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">contents (コンテンツ)</param>
        /// <param name="n">output column name (列名出力)</param>
        /// <param name="d">output definition (列定義出力)</param>
        public void AppendCsvLine(StringBuilder c, bool n, bool d)
        {
            Definitions["#layerNum#"].AppendCsv(Fields, c, n, d); // レイヤー行番号
            Definitions["#machineStateNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#fullnameEndsWithDot#"].AppendCsv(Fields, c, n, d);
            Definitions["name"].AppendCsv(Fields, c, n, d);
            Definitions["anyStateTransitions"].AppendCsv(Fields, c, n, d);
            Definitions["behaviours"].AppendCsv(Fields, c, n, d);
            Definitions["defaultState"].AppendCsv(Fields, c, n, d);
            Definitions["entryTransitions"].AppendCsv(Fields, c, n, d);
            Definitions["hideFlags"].AppendCsv(Fields, c, n, d);
            if (n) { c.Append("[EOL],"); }
            if (!d) { c.AppendLine(); }
        }
    }

    /// <summary>
    /// ステート
    /// </summary>
    public class StateRecord
    {
        static StateRecord()
        {
            List<RecordDefinition> temp = new List<RecordDefinition>()
            {
                new RecordDefinition("#layerNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#machineStateNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#stateNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#parentPath#",RecordDefinition.FieldType.String,RecordDefinition.KeyType.Identifiable,false),
                new RecordDefinition("name",RecordDefinition.FieldType.String,RecordDefinition.KeyType.Identifiable,false),
                new RecordDefinition("cycleOffset",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None               ,(object i)=>{ return ((AnimatorState)i).cycleOffset; }         ,(object i,float v)=>{ ((AnimatorState)i).cycleOffset = v; }),
                new RecordDefinition("cycleOffsetParameter",RecordDefinition.FieldType.String,RecordDefinition.KeyType.None     ,(object i)=>{ return ((AnimatorState)i).cycleOffsetParameter; },(object i,string v)=>{ ((AnimatorState)i).cycleOffsetParameter = v; }),
                new RecordDefinition("hideFlags",RecordDefinition.FieldType.Other,RecordDefinition.KeyType.None,false),
                new RecordDefinition("iKOnFeet",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None                   ,(object i)=>{ return ((AnimatorState)i).iKOnFeet; }            ,(object i,bool v)=>{ ((AnimatorState)i).iKOnFeet = v; }),
                new RecordDefinition("mirror",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None                     ,(object i)=>{ return ((AnimatorState)i).mirror; }              ,(object i,bool v)=>{ ((AnimatorState)i).mirror = v; }),
                new RecordDefinition("mirrorParameter",RecordDefinition.FieldType.String,RecordDefinition.KeyType.None          ,(object i)=>{ return ((AnimatorState)i).mirrorParameter; }     ,(object i,string v)=>{ ((AnimatorState)i).mirrorParameter = v; }),
                new RecordDefinition("mirrorParameterActive",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None      ,(object i)=>{ return ((AnimatorState)i).mirrorParameterActive;},(object i,bool v)=>{ ((AnimatorState)i).mirrorParameterActive = v; }),
                new RecordDefinition("motion_name",RecordDefinition.FieldType.String,RecordDefinition.KeyType.None,false),
                new RecordDefinition("nameHash",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.None,false),
                new RecordDefinition("speed",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None                     ,(object i)=>{ return ((AnimatorState)i).speed; }               ,(object i,float v)=>{ ((AnimatorState)i).speed = v; }),
                new RecordDefinition("speedParameter",RecordDefinition.FieldType.String,RecordDefinition.KeyType.None           ,(object i)=>{ return ((AnimatorState)i).speedParameter; }      ,(object i,string v)=>{ ((AnimatorState)i).speedParameter = v; }),
                new RecordDefinition("speedParameterActive",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None       ,(object i)=>{ return ((AnimatorState)i).speedParameterActive; },(object i,bool v)=>{ ((AnimatorState)i).speedParameterActive = v; }),
                new RecordDefinition("tag",RecordDefinition.FieldType.String,RecordDefinition.KeyType.None                      ,(object i)=>{ return ((AnimatorState)i).tag; }                 ,(object i,string v)=>{ ((AnimatorState)i).tag = v; }),
                new RecordDefinition("writeDefaultValues",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None         ,(object i)=>{ return ((AnimatorState)i).writeDefaultValues; }  ,(object i,bool v)=>{ ((AnimatorState)i).writeDefaultValues = v; }),
            };
            Definitions = new Dictionary<string, RecordDefinition>();
            foreach (RecordDefinition def in temp) { Definitions.Add(def.Name, def); }

            Empty = new StateRecord(-1,-1,-1,"",new AnimatorState());
        }
        public static Dictionary<string, RecordDefinition> Definitions { get; private set; }
        public static StateRecord Empty { get; private set; }

        public static StateRecord CreateInstance(int layerNum, int machineStateNum, int stateNum, string parentPath, ChildAnimatorState caState, List<PositionRecord> positionsTable)
        {
            positionsTable.Add(new PositionRecord(layerNum, machineStateNum, stateNum, -1, -1, "position", caState.position));
            return new StateRecord(layerNum, machineStateNum, stateNum, parentPath, caState.state);
        }
        public StateRecord(int layerNum, int machineStateNum, int stateNum, string parentPath, AnimatorState state)
        {
            this.Fields = new Dictionary<string, object>()
            {
                { "#layerNum#",layerNum },//レイヤー行番号
                { "#machineStateNum#", machineStateNum},
                { "#stateNum#",stateNum },
                { "#parentPath#", parentPath},
                { "name", state.name},
                { "cycleOffset", state.cycleOffset},
                { "cycleOffsetParameter", state.cycleOffsetParameter},
                { "hideFlags", state.hideFlags.ToString()},
                { "iKOnFeet", state.iKOnFeet},
                { "mirror", state.mirror},
                { "mirrorParameter",state.mirrorParameter },
                { "mirrorParameterActive",state.mirrorParameterActive },
                { "motion_name", state.motion == null ? "" : state.motion.name},// とりあえず名前だけ☆
                { "nameHash", state.nameHash},// このハッシュは有効なのだろうか？
                { "speed", state.speed},
                { "speedParameter", state.speedParameter},
                { "speedParameterActive", state.speedParameterActive},
                { "tag", state.tag},
                { "writeDefaultValues", state.writeDefaultValues},
            };
        }
        public Dictionary<string, object> Fields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">contents (コンテンツ)</param>
        /// <param name="n">output column name (列名出力)</param>
        /// <param name="d">output definition (列定義出力)</param>
        public void AppendCsvLine(StringBuilder c, bool n, bool d)
        {
            Definitions["#layerNum#"].AppendCsv(Fields, c, n, d); // レイヤー行番号
            Definitions["#machineStateNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#stateNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#parentPath#"].AppendCsv(Fields, c, n, d); // パス（名前抜き）
            Definitions["name"].AppendCsv(Fields, c, n, d);
            Definitions["cycleOffset"].AppendCsv(Fields, c, n, d);
            Definitions["cycleOffsetParameter"].AppendCsv(Fields, c, n, d);
            Definitions["hideFlags"].AppendCsv(Fields, c, n, d);
            Definitions["iKOnFeet"].AppendCsv(Fields, c, n, d);
            Definitions["mirror"].AppendCsv(Fields, c, n, d);
            Definitions["mirrorParameter"].AppendCsv(Fields, c, n, d);
            Definitions["mirrorParameterActive"].AppendCsv(Fields, c, n, d);
            Definitions["motion_name"].AppendCsv(Fields, c, n, d);
            Definitions["nameHash"].AppendCsv(Fields, c, n, d);
            Definitions["speed"].AppendCsv(Fields, c, n, d);
            Definitions["speedParameter"].AppendCsv(Fields, c, n, d);
            Definitions["speedParameterActive"].AppendCsv(Fields, c, n, d);
            Definitions["tag"].AppendCsv(Fields, c, n, d);
            Definitions["writeDefaultValues"].AppendCsv(Fields, c, n, d);
            if (n) { c.Append("[EOL],"); }
            if (!d) { c.AppendLine(); }
        }
    }

    /// <summary>
    /// トランジション
    /// ※コンディションは別テーブル
    /// </summary>
    public class TransitionRecord
    {
        static TransitionRecord()
        {
            List<RecordDefinition> temp = new List<RecordDefinition>()
            {
                new RecordDefinition("#layerNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#machineStateNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#stateNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#transitionNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("name",RecordDefinition.FieldType.String,RecordDefinition.KeyType.Identifiable,false),
                new RecordDefinition("#stellaQLComment#",RecordDefinition.FieldType.String,RecordDefinition.KeyType.None,false),
                new RecordDefinition("canTransitionToSelf",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None                    ,(object i)=>{ return ((AnimatorStateTransition)i).canTransitionToSelf; }   ,(object i,bool v)=>{ ((AnimatorStateTransition)i).canTransitionToSelf = v; }),
                new RecordDefinition("#destinationState_name#",RecordDefinition.FieldType.String,RecordDefinition.KeyType.None,false),
                new RecordDefinition("#destinationState_nameHash#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.None,false),
                new RecordDefinition("#destinationStateMachine_name#",RecordDefinition.FieldType.String,RecordDefinition.KeyType.None,false),
                new RecordDefinition("duration",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None                              ,(object i)=>{ return ((AnimatorStateTransition)i).duration; }              ,(object i,float v)=>{ ((AnimatorStateTransition)i).duration = v; }),
                new RecordDefinition("exitTime",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None                              ,(object i)=>{ return ((AnimatorStateTransition)i).exitTime; }              ,(object i,float v)=>{ ((AnimatorStateTransition)i).exitTime = v; }),
                new RecordDefinition("hasExitTime",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None                            ,(object i)=>{ return ((AnimatorStateTransition)i).hasExitTime; }           ,(object i,bool v)=>{ ((AnimatorStateTransition)i).hasExitTime = v; }),
                new RecordDefinition("hasFixedDuration",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None                       ,(object i)=>{ return ((AnimatorStateTransition)i).hasFixedDuration; }      ,(object i,bool v)=>{ ((AnimatorStateTransition)i).hasFixedDuration = v; }),
                new RecordDefinition("hideFlags",RecordDefinition.FieldType.Other,RecordDefinition.KeyType.None,false),
                new RecordDefinition("interruptionSource",RecordDefinition.FieldType.Other,RecordDefinition.KeyType.None,false),
                new RecordDefinition("isExit",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None                                 ,(object i)=>{ return ((AnimatorStateTransition)i).isExit; }                ,(object i,bool v)=>{ ((AnimatorStateTransition)i).isExit = v; }),
                new RecordDefinition("mute",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None                                   ,(object i)=>{ return ((AnimatorStateTransition)i).mute; }                  ,(object i,bool v)=>{ ((AnimatorStateTransition)i).mute = v; }),
                new RecordDefinition("offset",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None                                ,(object i)=>{ return ((AnimatorStateTransition)i).offset; }                ,(object i,float v)=>{ ((AnimatorStateTransition)i).offset = v; }),
                new RecordDefinition("orderedInterruption",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None                    ,(object i)=>{ return ((AnimatorStateTransition)i).orderedInterruption; }   ,(object i,bool v)=>{ ((AnimatorStateTransition)i).orderedInterruption = v; }),
                new RecordDefinition("solo",RecordDefinition.FieldType.Bool,RecordDefinition.KeyType.None                                   ,(object i)=>{ return ((AnimatorStateTransition)i).solo; }                  ,(object i,bool v)=>{ ((AnimatorStateTransition)i).solo = v; }),
            };
            Definitions = new Dictionary<string, RecordDefinition>();
            foreach (RecordDefinition def in temp) { Definitions.Add(def.Name, def); }

            Empty = new TransitionRecord(-1,-1,-1,-1,new AnimatorStateTransition(),"");
        }
        public static Dictionary<string, RecordDefinition> Definitions { get; private set; }
        public static TransitionRecord Empty { get; private set; }

        public TransitionRecord(int layerNum, int machineStateNum, int stateNum, int transitionNum, AnimatorStateTransition transition, string stellaQLComment)
        {
            this.Fields = new Dictionary<string, object>()
            {
                { "#layerNum#", layerNum},
                { "#machineStateNum#", machineStateNum},
                { "#stateNum#", stateNum},
                { "#transitionNum#", transitionNum},
                { "name", transition.name},
                { "#stellaQLComment#", stellaQLComment}, // どこからどこへ繋いでるのか、動作確認するために見る開発時用の欄を追加。
                { "canTransitionToSelf", transition.canTransitionToSelf},
                { "#destinationState_name#", transition.destinationState == null ? "" : transition.destinationState.name},// 名前のみ取得
                { "#destinationState_nameHash#", transition.destinationState == null ? 0 : transition.destinationState.nameHash},
                { "#destinationStateMachine_name#", transition.destinationStateMachine == null ? "" : transition.destinationStateMachine.name},// 名前のみ取得
                { "duration", transition.duration},
                { "exitTime", transition.exitTime},
                { "hasExitTime", transition.hasExitTime},
                { "hasFixedDuration", transition.hasFixedDuration},
                { "hideFlags", transition.hideFlags.ToString()},
                { "interruptionSource", transition.interruptionSource.ToString()},
                { "isExit", transition.isExit},
                { "mute", transition.mute},
                { "offset", transition.offset},
                { "orderedInterruption", transition.orderedInterruption},
                { "solo", transition.solo},
            };
        }
        public Dictionary<string, object> Fields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">contents (コンテンツ)</param>
        /// <param name="n">output column name (列名出力)</param>
        /// <param name="d">output definition (列定義出力)</param>
        public void AppendCsvLine(StringBuilder c, bool n, bool d)
        {
            Definitions["#layerNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#machineStateNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#stateNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#transitionNum#"].AppendCsv(Fields, c, n, d);
            Definitions["name"].AppendCsv(Fields, c, n, d);
            Definitions["#stellaQLComment#"].AppendCsv(Fields, c, n, d);
            Definitions["canTransitionToSelf"].AppendCsv(Fields, c, n, d);
            Definitions["#destinationState_name#"].AppendCsv(Fields, c, n, d);
            Definitions["#destinationState_nameHash#"].AppendCsv(Fields, c, n, d);
            Definitions["#destinationStateMachine_name#"].AppendCsv(Fields, c, n, d);
            Definitions["duration"].AppendCsv(Fields, c, n, d);
            Definitions["exitTime"].AppendCsv(Fields, c, n, d);
            Definitions["hasExitTime"].AppendCsv(Fields, c, n, d);
            Definitions["hasFixedDuration"].AppendCsv(Fields, c, n, d);
            Definitions["hideFlags"].AppendCsv(Fields, c, n, d);
            Definitions["interruptionSource"].AppendCsv(Fields, c, n, d);
            Definitions["isExit"].AppendCsv(Fields, c, n, d);
            Definitions["mute"].AppendCsv(Fields, c, n, d);
            Definitions["offset"].AppendCsv(Fields, c, n, d);
            Definitions["orderedInterruption"].AppendCsv(Fields, c, n, d);
            Definitions["solo"].AppendCsv(Fields, c, n, d);
            if (n) { c.Append("[EOL],"); }
            if (!d) { c.AppendLine(); }
        }
    }

    /// <summary>
    /// コンディション
    /// </summary>
    public class ConditionRecord
    {
        /// <summary>
        /// struct を object に渡したいときに使うラッパーだぜ☆（＾～＾）
        /// </summary>
        public class AnimatorConditionWrapper
        {
            public AnimatorConditionWrapper(AnimatorCondition source)
            {
                this.m_source = source;
            }

            public AnimatorCondition m_source;
        }

        static ConditionRecord()
        {
            List<RecordDefinition> temp = new List<RecordDefinition>()
            {
                new RecordDefinition("#layerNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#machineStateNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#stateNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#transitionNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#conditionNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("mode",RecordDefinition.FieldType.Other,RecordDefinition.KeyType.None,false),
                new RecordDefinition("parameter",RecordDefinition.FieldType.String,RecordDefinition.KeyType.None                ,(object i)=>{ return ((AnimatorConditionWrapper)i).m_source.parameter; } ,(object i,string v)=>{ ((AnimatorConditionWrapper)i).m_source.parameter = v; }),
                new RecordDefinition("threshold",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None                 ,(object i)=>{ return ((AnimatorConditionWrapper)i).m_source.threshold; } ,(object i,float v)=>{ ((AnimatorConditionWrapper)i).m_source.threshold = v; }),
            };
            Definitions = new Dictionary<string, RecordDefinition>();
            foreach (RecordDefinition def in temp) { Definitions.Add(def.Name, def); }

            Empty = new ConditionRecord(-1, -1, -1, -1, -1, new AnimatorCondition());
        }
        public static Dictionary<string, RecordDefinition> Definitions { get; private set; }
        public static ConditionRecord Empty { get; set; }

        public ConditionRecord(int layerNum, int machineStateNum, int stateNum, int transitionNum, int conditionNum, AnimatorCondition condition)
        {
            this.Fields = new Dictionary<string, object>()
            {
                { "#layerNum#", layerNum},
                { "#machineStateNum#", machineStateNum},
                { "#stateNum#", stateNum},
                { "#transitionNum#", transitionNum},
                { "#conditionNum#", conditionNum},
                { "mode", condition.mode.ToString()},
                { "parameter", condition.parameter},
                { "threshold", condition.threshold},
            };
        }
        public Dictionary<string, object> Fields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">contents (コンテンツ)</param>
        /// <param name="n">output column name (列名出力)</param>
        /// <param name="d">output definition (列定義出力)</param>
        public void AppendCsvLine(StringBuilder c, bool n, bool d)
        {
            Definitions["#layerNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#machineStateNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#stateNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#transitionNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#conditionNum#"].AppendCsv(Fields, c, n, d);
            Definitions["mode"].AppendCsv(Fields, c, n, d);
            Definitions["parameter"].AppendCsv(Fields, c, n, d);
            Definitions["threshold"].AppendCsv(Fields, c, n, d);
            if (n) { c.Append("[EOL],"); }
            if (!d) { c.AppendLine(); }
        }
    }

    /// <summary>
    /// ポジション
    /// </summary>
    public class PositionRecord
    {
        /// <summary>
        /// struct を object に渡したいときに使うラッパーだぜ☆（＾～＾）
        /// </summary>
        public class PositionWrapper
        {
            public PositionWrapper(Vector3 source)
            {
                this.m_source = source;
            }

            public Vector3 m_source;
        }

        static PositionRecord()
        {
            List<RecordDefinition> temp = new List<RecordDefinition>()
            {
                new RecordDefinition("#layerNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#machineStateNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#stateNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#transitionNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#conditionNum#",RecordDefinition.FieldType.Int,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("#proertyName#",RecordDefinition.FieldType.String,RecordDefinition.KeyType.TemporaryNumbering,false),
                new RecordDefinition("magnitude",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None, false),// リード・オンリー型
                new RecordDefinition("#normalized#",RecordDefinition.FieldType.Other,RecordDefinition.KeyType.None,false),
                new RecordDefinition("#normalizedX#",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None,false),
                new RecordDefinition("#normalizedY#",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None,false),
                new RecordDefinition("#normalizedZ#",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None,false),
                new RecordDefinition("sqrMagnitude",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None,false), // リード・オンリー型
                new RecordDefinition("x",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None,(object i)=>{ return ((PositionWrapper)i).m_source.x; }             ,(object i,float v)=>{ ((PositionWrapper)i).m_source.x = v; }),
                new RecordDefinition("y",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None,(object i)=>{ return ((PositionWrapper)i).m_source.y; }             ,(object i,float v)=>{ ((PositionWrapper)i).m_source.y = v; }),
                new RecordDefinition("z",RecordDefinition.FieldType.Float,RecordDefinition.KeyType.None,(object i)=>{ return ((PositionWrapper)i).m_source.z; }             ,(object i,float v)=>{ ((PositionWrapper)i).m_source.z = v; }),
            };
            Definitions = new Dictionary<string, RecordDefinition>();
            foreach (RecordDefinition def in temp) { Definitions.Add(def.Name, def); }

            Empty = new PositionRecord(-1,-1,-1,-1,-1,"",new Vector3());
        }
        public static Dictionary<string, RecordDefinition> Definitions { get; private set; }
        public static PositionRecord Empty { get; private set; }

        public PositionRecord(int layerNum, int machineStateNum, int stateNum, int transitionNum, int conditionNum, string proertyName, Vector3 position)
        {
            this.Fields = new Dictionary<string, object>()
            {
                { "#layerNum#", layerNum},
                { "#machineStateNum#", machineStateNum},
                { "#stateNum#", stateNum},
                { "#transitionNum#", transitionNum},
                { "#conditionNum#", conditionNum},
                { "#proertyName#", proertyName},
                { "magnitude", position.magnitude},
                { "#normalized#", position.normalized == null ? "" : position.normalized.ToString()},
                { "#normalizedX#", position.normalized == null ? "" : position.normalized.x.ToString()},
                { "#normalizedY#", position.normalized == null ? "" : position.normalized.y.ToString()},
                { "#normalizedZ#", position.normalized == null ? "" : position.normalized.z.ToString()},
                { "sqrMagnitude", position.sqrMagnitude},
                { "x", position.x},
                { "y", position.y},
                { "z", position.z},
            };
        }
        public Dictionary<string, object> Fields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">contents (コンテンツ)</param>
        /// <param name="n">output column name (列名出力)</param>
        /// <param name="d">output definition (列定義出力)</param>
        public void AppendCsvLine(StringBuilder c, bool n, bool d)
        {
            Definitions["#layerNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#machineStateNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#stateNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#transitionNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#conditionNum#"].AppendCsv(Fields, c, n, d);
            Definitions["#proertyName#"].AppendCsv(Fields, c, n, d);
            Definitions["magnitude"].AppendCsv(Fields, c, n, d);
            Definitions["#normalized#"].AppendCsv(Fields, c, n, d);
            Definitions["#normalizedX#"].AppendCsv(Fields, c, n, d);
            Definitions["#normalizedY#"].AppendCsv(Fields, c, n, d);
            Definitions["#normalizedZ#"].AppendCsv(Fields, c, n, d);
            Definitions["sqrMagnitude"].AppendCsv(Fields, c, n, d);
            Definitions["x"].AppendCsv(Fields, c, n, d);
            Definitions["y"].AppendCsv(Fields, c, n, d);
            Definitions["z"].AppendCsv(Fields, c, n, d);
            if (n) { c.Append("[EOL],"); }
            if (!d) { c.AppendLine(); }
        }
    }

    public class AniconData
    {
        public AniconData()
        {
            table_parameter = new HashSet<ParameterRecord>();
            table_layer = new List<LayerRecord>();
            table_statemachine = new List<StatemachineRecord>();
            table_state = new HashSet<StateRecord>();
            table_transition = new HashSet<TransitionRecord>();
            table_condition = new List<ConditionRecord>();
            table_position = new List<PositionRecord>();
        }

        public HashSet<ParameterRecord> table_parameter { get; set; }
        public List<LayerRecord> table_layer { get; set; }
        public List<StatemachineRecord> table_statemachine { get; set; }
        public HashSet<StateRecord> table_state { get; set; }
        public HashSet<TransitionRecord> table_transition { get; set; }
        public List<ConditionRecord> table_condition { get; set; }
        public List<PositionRecord> table_position { get; set; }
    }


    public abstract class AniconDataUtility
    {

        public static void WriteCsv_Parameters(AniconData aniconData, string aniconName, bool outputDefinition, StringBuilder message)
        {
            StringBuilder contents = new StringBuilder();
            if (outputDefinition)
            {
                RecordDefinition.AppendDefinitionHeader(contents); // 列定義ヘッダー出力
                ParameterRecord.Empty.AppendCsvLine(contents, false, outputDefinition); // 列定義出力
            }
            else
            {
                ParameterRecord.Empty.AppendCsvLine(contents, true, outputDefinition); // 列名出力
                foreach (ParameterRecord record in aniconData.table_parameter) { record.AppendCsvLine(contents, false, outputDefinition); }
            }

            contents.AppendLine("[EOF],");
            StellaQLWriter.Write(StellaQLWriter.Filepath_LogParameters(aniconName, outputDefinition), contents, message);
        }

        public static void WriteCsv_Layers(AniconData aniconData, string aniconName, bool outputDefinition, StringBuilder message)
        {
            StringBuilder contents = new StringBuilder();

            if (outputDefinition) {
                RecordDefinition.AppendDefinitionHeader(contents); // 列定義ヘッダー出力
                LayerRecord.Empty.AppendCsvLine(contents, false, outputDefinition); // 列定義出力
            }
            else
            {
                LayerRecord.Empty.AppendCsvLine(contents, true, outputDefinition); // 列名出力
                foreach (LayerRecord record in aniconData.table_layer) { record.AppendCsvLine(contents, false, outputDefinition); }
            }

            contents.AppendLine("[EOF],");
            StellaQLWriter.Write(StellaQLWriter.Filepath_LogLayer(aniconName, outputDefinition), contents, message);
        }

        public static void WriteCsv_Statemachines(AniconData aniconData, string aniconName, bool outputDefinition, StringBuilder message)
        {
            StringBuilder contents = new StringBuilder();

            if (outputDefinition)
            {
                RecordDefinition.AppendDefinitionHeader(contents); // 列定義ヘッダー出力
                StatemachineRecord.Empty.AppendCsvLine(contents, false, outputDefinition); // 列定義出力
            }
            else {
                StatemachineRecord.Empty.AppendCsvLine(contents, true, outputDefinition); // 列名出力
                foreach (StatemachineRecord record in aniconData.table_statemachine) { record.AppendCsvLine(contents, false, outputDefinition); }
            }

            contents.AppendLine("[EOF],");
            StellaQLWriter.Write(StellaQLWriter.Filepath_LogStatemachine(aniconName, outputDefinition), contents, message);
        }

        public static void CreateCsvTable_State( HashSet<StateRecord> table, bool outputDefinition, StringBuilder contents)
        {
            if (outputDefinition)
            {
                RecordDefinition.AppendDefinitionHeader(contents); // 列定義ヘッダー出力
                StateRecord.Empty.AppendCsvLine(contents, false, outputDefinition); // 列定義出力
            }
            else {
                StateRecord.Empty.AppendCsvLine(contents, true, outputDefinition); // 列名出力
                foreach (StateRecord stateRecord in table) { stateRecord.AppendCsvLine(contents, false, outputDefinition); }
            }
        }
        public static void WriteCsv_States( AniconData aniconData, string aniconName, bool outputDefinition, StringBuilder message)
        {
            StringBuilder contents = new StringBuilder();
            CreateCsvTable_State( aniconData.table_state, outputDefinition, contents);

            contents.AppendLine("[EOF],");
            StellaQLWriter.Write(StellaQLWriter.Filepath_LogStates(aniconName, outputDefinition), contents, message);
        }

        public static void CreateCsvTable_Transition(HashSet<TransitionRecord> table, bool outputDefinition, StringBuilder contents)
        {
            if (outputDefinition)
            {
                RecordDefinition.AppendDefinitionHeader(contents); // 列定義ヘッダー出力
                TransitionRecord.Empty.AppendCsvLine(contents, false, outputDefinition); // 列定義出力
            }
            else {
                TransitionRecord.Empty.AppendCsvLine(contents, true, outputDefinition); // 列名出力
                foreach (TransitionRecord record in table) { record.AppendCsvLine(contents, false, outputDefinition); }
            }
        }
        public static void WriteCsv_Transitions(AniconData aniconData, string aniconName, bool outputDefinition, StringBuilder message)
        {
            StringBuilder contents = new StringBuilder();
            CreateCsvTable_Transition(aniconData.table_transition, outputDefinition, contents);

            contents.AppendLine("[EOF],");
            StellaQLWriter.Write(StellaQLWriter.Filepath_LogTransition(aniconName, outputDefinition), contents, message);
        }

        public static void WriteCsv_Conditions(AniconData aniconData, string aniconName, bool outputDefinition, StringBuilder message)
        {
            StringBuilder contents = new StringBuilder();

            if (outputDefinition)
            {
                RecordDefinition.AppendDefinitionHeader(contents); // 列定義ヘッダー出力
                ConditionRecord.Empty.AppendCsvLine(contents, false, outputDefinition); // 列定義出力
            }
            else {
                ConditionRecord.Empty.AppendCsvLine(contents, true, outputDefinition); // 列名出力
                foreach (ConditionRecord record in aniconData.table_condition) { record.AppendCsvLine(contents, false, outputDefinition); }
            }

            contents.AppendLine("[EOF],");
            StellaQLWriter.Write(StellaQLWriter.Filepath_LogConditions(aniconName, outputDefinition), contents, message);
        }

        public static void WriteCsv_Positions(AniconData aniconData, string aniconName, bool outputDefinition, StringBuilder message)
        {
            StringBuilder contents = new StringBuilder();

            if (outputDefinition)
            {
                RecordDefinition.AppendDefinitionHeader(contents); // 列定義ヘッダー出力
                PositionRecord.Empty.AppendCsvLine(contents, false, outputDefinition); // 列定義出力
            }
            else {
                PositionRecord.Empty.AppendCsvLine(contents, true, outputDefinition); // 列名出力
                foreach (PositionRecord record in aniconData.table_position) { record.AppendCsvLine(contents, false, outputDefinition); }
            }

            contents.AppendLine("[EOF],");
            StellaQLWriter.Write(StellaQLWriter.Filepath_LogPositions(aniconName, outputDefinition), contents, message);
        }

        private static void ScanRecursive(string path, AnimatorStateMachine stateMachine, Dictionary<string,AnimatorStateMachine> statemachineList_flat)
        {
            path += stateMachine.name + ".";
            statemachineList_flat.Add(path, stateMachine);

            foreach (ChildAnimatorStateMachine caStateMachine in stateMachine.stateMachines)
            {
                ScanRecursive(path, caStateMachine.stateMachine, statemachineList_flat);
            }
        }

        public static void ScanAnimatorController(AnimatorController ac, out AniconData aniconData, StringBuilder message)
        {
            message.AppendLine("Animator controller Scanning...☆（＾～＾）");
            aniconData = new AniconData();

            // パラメーター
            {
                AnimatorControllerParameter[] acpArray = ac.parameters;
                int num = 0;
                foreach (AnimatorControllerParameter acp in acpArray)
                {
                    ParameterRecord record = new ParameterRecord(num, acp.name, acp.defaultBool, acp.defaultFloat, acp.defaultInt, acp.nameHash);
                    aniconData.table_parameter.Add(record);
                    num++;
                }
            }

            foreach (AnimatorControllerLayer layer in ac.layers)//レイヤー
            {
                LayerRecord layerRecord = new LayerRecord(aniconData.table_layer.Count, layer);
                aniconData.table_layer.Add(layerRecord);
                
                Dictionary<string,AnimatorStateMachine> statemachineList_flat = new Dictionary<string,AnimatorStateMachine>(); // フルパス, ステートマシン
                ScanRecursive("", layer.stateMachine, statemachineList_flat);// 再帰をスキャンして、フラットにする。
                foreach (KeyValuePair<string,AnimatorStateMachine> statemachine_pair in statemachineList_flat) { // ステート・マシン
                    StatemachineRecord stateMachineRecord = new StatemachineRecord(
                        aniconData.table_layer.Count, aniconData.table_statemachine.Count, statemachine_pair.Key, statemachine_pair.Value, aniconData.table_position);
                    aniconData.table_statemachine.Add(stateMachineRecord);

                    foreach (ChildAnimatorState caState in statemachine_pair.Value.states) { //ステート（ラッパー）
                        StateRecord stateRecord = StateRecord.CreateInstance(aniconData.table_layer.Count, aniconData.table_statemachine.Count, aniconData.table_state.Count, statemachine_pair.Key, caState, aniconData.table_position);
                        aniconData.table_state.Add(stateRecord);

                        foreach (AnimatorStateTransition transition in caState.state.transitions) { // トランジション
                            TransitionRecord transitionRecord = new TransitionRecord(aniconData.table_layer.Count, aniconData.table_statemachine.Count, aniconData.table_state.Count, aniconData.table_transition.Count, transition, "");
                            aniconData.table_transition.Add(transitionRecord);

                            foreach (AnimatorCondition aniCondition in transition.conditions) { // コンディション
                                ConditionRecord conditionRecord = new ConditionRecord(aniconData.table_layer.Count, aniconData.table_statemachine.Count, aniconData.table_state.Count, aniconData.table_transition.Count, aniconData.table_condition.Count, aniCondition);
                                aniconData.table_condition.Add(conditionRecord);
                            } // コンディション
                        }//トランジション
                    }//ステート（ラッパー）
                }//ステートマシン
            }//レイヤー

            message.AppendLine( "Scanned☆（＾▽＾）");
        }

    }
}
