using System.Text;

namespace StellaQL
{
    /// <summary>
    /// データ操作を要求するレコード。ここでは　項目挿入、プロパティー更新、項目削除。項目選択を除く。
    /// </summary>
    public class DataManipulationRecord
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="foreignkeycategory"></param>
        /// <param name="fullpath">レイヤー、ステートマシン、ステートのいずれかのフルパス。</param>
        /// <param name="fullpathTransition"></param>
        /// <param name="fullpathCondition"></param>
        /// <param name="fullpathPropertyname"></param>
        /// <param name="name"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="delete"></param>
        public DataManipulationRecord(string category, string foreignkeycategory, string fullpath, string fullpathTransition, string fullpathCondition, string fullpathPropertyname, string name, string oldValue, string newValue, string delete)
        {
            this.Category = category;
            this.Foreignkeycategory = foreignkeycategory;
            this.Fullpath = fullpath;
            this.TransitionNum_ofFullpath = fullpathTransition;
            this.ConditionNum_ofFullpath = fullpathCondition;
            this.Propertyname_ofFullpath = fullpathPropertyname;
            this.Name = name;
            this.Old = oldValue;
            this.New = newValue;
            this.Delete = delete;
        }

        public string Category { get; private set; }
        public string Foreignkeycategory { get; private set; }
        public string Fullpath { get; private set; }
        public string TransitionNum_ofFullpath { get; private set; }
        public string ConditionNum_ofFullpath { get; private set; }
        public string Propertyname_ofFullpath { get; private set; } // position で使う
        public string Name { get; private set; }

        public string Old { get; private set; }
        public bool OldBool { get { return bool.Parse(Old); } }
        public float OldFloat { get { return float.Parse(Old); } }
        public int OldInt { get { return int.Parse(Old); } }

        public string New { get; private set; }
        public bool NewBool { get { return bool.Parse(New); } }
        public float NewFloat { get { return float.Parse(New); } }
        public int NewInt { get { return int.Parse(New); } }

        public string Delete { get; private set; }
        public bool IsDelete { get { bool val; if (!bool.TryParse(Delete, out val)) { val = false; } return val; } }

        /// <summary>
        /// 中身を確認するのに使う（＾～＾）
        /// </summary>
        /// <returns></returns>
        public void ToCsvLine(StringBuilder contents)
        {
            contents.Append(CsvParser.EscapeCell(Category));
            contents.Append(",");
            contents.Append(CsvParser.EscapeCell(Fullpath));
            contents.Append(",");
            contents.Append(CsvParser.EscapeCell(TransitionNum_ofFullpath));
            contents.Append(",");
            contents.Append(CsvParser.EscapeCell(ConditionNum_ofFullpath));
            contents.Append(",");
            contents.Append(CsvParser.EscapeCell(Propertyname_ofFullpath));
            contents.Append(",");
            contents.Append(CsvParser.EscapeCell(Name));
            contents.Append(",");
            contents.Append(CsvParser.EscapeCell(Old));
            contents.Append(",");
            contents.Append(CsvParser.EscapeCell(New));
            contents.Append(",");
            contents.Append(CsvParser.EscapeCell(Delete));
            contents.Append(",");
            contents.AppendLine();
        }
    }
}
