using System.Text;

namespace StellaQL
{
    /// <summary>
    /// Record that requests data manipulation.
    /// 
    /// INSERT,
    /// INSERT(Change Destination),
    /// UPDATE,
    /// DELETE,
    /// SELECT
    /// </summary>
    public class DataManipulationRecord
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="foreignkeycategory"></param>
        /// <param name="fullpath">Full path of any of layer, state machine, state.</param>
        /// <param name="fullpathTransition"></param>
        /// <param name="fullpathCondition"></param>
        /// <param name="fullpathPropertyname"></param>
        /// <param name="name"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="flagOf_clear"></param>
        public DataManipulationRecord(string category, string foreignkeycategory, string fullpath, string fullpathTransition, string fullpathCondition, string fullpathPropertyname, string name, string oldValue, string newValue, string flagOf_clear)
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
            this.FlagOf_Clear = flagOf_clear;
        }

        public string Category { get; private set; }
        public string Foreignkeycategory { get; private set; }
        public string Fullpath { get; private set; }
        public string TransitionNum_ofFullpath { get; private set; }
        public string ConditionNum_ofFullpath { get; private set; }
        public string Propertyname_ofFullpath { get; private set; } // Use with position
        public string Name { get; private set; }

        public string Old { get; private set; }
        public bool OldBool { get { return bool.Parse(Old); } }
        public float OldFloat { get { return float.Parse(Old); } }
        public int OldInt { get { return int.Parse(Old); } }

        public string New { get; private set; }
        public bool NewBool { get { return bool.Parse(New); } }
        public float NewFloat { get { return float.Parse(New); } }
        public int NewInt { get { return int.Parse(New); } }

        public string FlagOf_Clear { get; private set; }
        public bool IsClear { get { bool val; if (!bool.TryParse(FlagOf_Clear, out val)) { val = false; } return val; } }

        /// <summary>
        /// Used to check the contents.
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
            contents.Append(CsvParser.EscapeCell(FlagOf_Clear));
            contents.Append(",");
            contents.AppendLine();
        }
    }
}
