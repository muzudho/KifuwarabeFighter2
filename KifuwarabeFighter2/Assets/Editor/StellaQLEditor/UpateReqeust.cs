using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StellaQL
{
    /// <summary>
    /// 更新を要求するレコード
    /// </summary>
    public class UpateReqeustRecord
    {
        public UpateReqeustRecord(string category, string foreignkeycategory, string fullpath, string fullpathTransition, string fullpathCondition, string fullpathPropertyname, string name, string oldValue, string newValue, string delete)
        {
            this.Category = category;
            this.Foreignkeycategory = foreignkeycategory;
            this.Fullpath = fullpath;
            this.FullpathTransition = fullpathTransition;
            this.FullpathCondition = fullpathCondition;
            this.FullpathPropertyname = fullpathPropertyname;
            this.Name = name;
            this.Old = oldValue;
            this.New = newValue;
            this.Delete = delete;
        }

        public string Category { get; private set; }
        public string Foreignkeycategory { get; private set; }
        public string Fullpath { get; private set; }
        public string FullpathTransition { get; private set; }
        public string FullpathCondition { get; private set; }
        public string FullpathPropertyname { get; private set; }
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
            contents.Append(CsvParser.EscapeCell(FullpathTransition));
            contents.Append(",");
            contents.Append(CsvParser.EscapeCell(FullpathCondition));
            contents.Append(",");
            contents.Append(CsvParser.EscapeCell(FullpathPropertyname));
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
