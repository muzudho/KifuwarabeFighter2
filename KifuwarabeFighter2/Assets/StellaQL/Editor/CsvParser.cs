using System.Collections.Generic;
using System.Text;
using System;

namespace StellaQL
{
    /// <summary>
    /// 手作りの CSVの１行分のパーサーだぜ☆（＾▽＾）
    /// </summary>
    public abstract class CsvParser
    {
        public static List<string> CsvLine_to_cellList(string source)
        {
            return CsvLine_to_cellList(source, ',');
        }
        /// <summary>
        /// アン・エスケープするぜ☆（＾▽＾）
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static List<string> CsvLine_to_cellList(string source, char delimiter)
        {
            List<string> cells = new List<string>();
            if (source.Length < 1) { return cells; } // 空文字列なら終わり

            //１セル分の文字列
            StringBuilder cell = new StringBuilder();
            int caret = 0;
            while (caret < source.Length) // このループで１行分に対応
            {
                switch (source[caret])
                {
                    case ',': caret++; cells.Add(cell.ToString()); cell.Length = 0; break; // トークンを出力して次へ。
                    case '"':
                        // ここからリテラル文字列処理へ
                        caret++;

                        // エスケープしながら、単独「"」が出てくるまでそのまま出力。
                        while (caret < source.Length)
                        {
                            if ('"'==source[caret])
                            {
                                // これが単独の「"」なら終わり、２連続の「"」ならまだ終わらない。

                                if (caret + 1 == source.Length) { caret++; break; }// 「"」が最後の文字だったのなら、無視してループ抜け。
                                else if ('"' == source[caret + 1]) { caret += 2; cell.Append('"'); } // 2文字目も「"」なら、２つの「""」すっとばして代わりに「"」を入れてループ続行。
                                else { caret = source.IndexOf(',',caret)+1; break; } // 2連続でない「"」なら、次の「,」の次までの空白等をスキップ。//【改変/】2012年10月30日変更。旧： index++;//【改変/】2017年02月01日変更。次のカンマの次まで飛ばした。旧： index+=2;
                            }
                            else { cell.Append(source[caret]); caret++; }// 通常文字なのでループ続行。
                        }
                        cells.Add(cell.ToString().Trim()); cell.Length = 0; break; // 前後の空白はカット
                    default: cell.Append(source[caret]); caret++; break;// ダブルクォートされていない文字列か、ダブルクォートの前のスペースだ。
                }
            }

            return cells;
        }

        public static string CellList_to_csvLine(List<string> fieldList)
        {
            return CellList_to_csvLine(fieldList);
        }
        public static string CellList_to_csvLine(List<string> fieldList, char delimiter)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string field in fieldList)
            {
                sb.Append(EscapeCell(field));
                sb.Append(",");
            }

            return sb.ToString();
        }

        /// <summary>
        /// （１）もし「,」または「"」が含まれていれば、両端に「"」を付加します。
        /// （２）含まれている「"」は、「""」に変換します。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EscapeCell(string source)
        {
            bool isEscape = false;// エスケープが必要なら真。

            StringBuilder s = new StringBuilder();

            for (int caret = 0; caret < source.Length;) {
                if (',' == source[caret] || '\r' == source[caret] || '\n' == source[caret]) { isEscape = true; s.Append(source[caret]); caret++; }// カンマが含まれていたので、エスケープが必要になった。(2017-02-09 追加 '\r'、'\n')
                else if ('"' == source[caret]) { isEscape = true; s.Append("\"\""); caret++; }// ダブルクォーテーションが含まれていたので、エスケープが必要になった
                else { s.Append(source[caret]); caret++; }
            }

            if (isEscape) { s.Insert(0, '"'); s.Append('"'); } // ダブルクォーテーションで挟む

            return s.ToString();
        }
    }
}