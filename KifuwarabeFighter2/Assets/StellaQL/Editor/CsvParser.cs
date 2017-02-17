using System.Collections.Generic;
using System.Text;

namespace StellaQL
{
    /// <summary>
    /// 手作りの CSVの１行分のパーサー
    /// Parser for one line of homemade CSV.
    /// </summary>
    public abstract class CsvParser
    {
        public static List<string> CsvLine_to_cellList(string source)
        {
            return CsvLine_to_cellList(source, ',');
        }
        /// <summary>
        /// アン・エスケープする
        /// Unescape.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static List<string> CsvLine_to_cellList(string source, char delimiter)
        {
            List<string> cells = new List<string>();

            // 空文字列なら終わり
            // End if empty string
            if (source.Length < 1) { return cells; }

            // １セル分の文字列
            // Character string for one cell
            StringBuilder cell = new StringBuilder();
            int caret = 0;

            // このループで１行分に対応
            // This loop corresponds to one line
            while (caret < source.Length)
            {
                switch (source[caret])
                {
                    // トークンを出力して次へ。
                    // Output the token and continue.
                    case ',': caret++; cells.Add(cell.ToString()); cell.Length = 0; break;

                    case '"':
                        // ここからリテラル文字列処理へ
                        // From here to literal string processing
                        caret++;

                        // エスケープしながら、単独「"」が出てくるまでそのまま出力。
                        // While escaping, output it as it is until alone (") comes out.
                        while (caret < source.Length)
                        {
                            if ('"'==source[caret])
                            {
                                // これが単独の「"」なら終わり、
                                // If this is a single ("), it ends.

                                // ２連続の「"」ならまだ終わらない。
                                // If it is two consecutive (") it will not end.

                                // 「"」が最後の文字だったのなら、無視してループ抜け。
                                // If (") was the last character, ignore it and omit the loop.
                                if (caret + 1 == source.Length) { caret++; break; }

                                // 2文字目も「"」なら、２つの「""」すっとばして代わりに「"」を入れてループ続行。
                                // If the second letter is also ("), continue with the loop by inserting two (") suddenly and inserting (") instead.
                                else if ('"' == source[caret + 1]) { caret += 2; cell.Append('"'); }

                                // 2連続でない「"」なら、次の「,」の次までの空白等をスキップ。//【改変/】2012年10月30日変更。旧： index++;//【改変/】2017年02月01日変更。次のカンマの次まで飛ばした。旧： index+=2;
                                // If it is not two consecutive ("), skip whitespace etc. until the next (,). (Change)2012-10-30, 2017-02-01.
                                else { caret = source.IndexOf(',',caret)+1; break; }
                            }
                            // 通常文字なのでループ続行。
                            // Since it is a normal character, I continue with a loop.
                            else { cell.Append(source[caret]); caret++; }
                        }

                        // 前後の空白はカット
                        // Cut the front and rear spaces
                        cells.Add(cell.ToString().Trim()); cell.Length = 0; break;

                    // ダブルクォートされていない文字列か、ダブルクォートの前のスペースだ。
                    // It is either a string that is not double quoted or a space before double quotes.
                    default: cell.Append(source[caret]); caret++; break;
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
        /// (1) もし「,」または「"」が含まれていれば、両端に「"」を付加します。
        /// (1) If "," or (") is included, add (") to both ends.
        /// 
        /// (2) 含まれている「"」は、「""」に変換します。
        /// (2) Contains (") converted to ("").
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EscapeCell(string source)
        {
            // エスケープが必要なら真。
            // True if you need escape.
            bool isEscape = false;

            StringBuilder s = new StringBuilder();

            for (int caret = 0; caret < source.Length;) {
                // カンマが含まれていたので、エスケープが必要になった。(2017-02-09 追加 '\r'、'\n')
                // Since commas were included, escaping became necessary. (2017-02-09 Add '\r', '\n')
                if (',' == source[caret] || '\r' == source[caret] || '\n' == source[caret]) { isEscape = true; s.Append(source[caret]); caret++; }

                // ダブルクォーテーションが含まれていたので、エスケープが必要になった
                // Since double quotes were included, escape was required.
                else if ('"' == source[caret]) { isEscape = true; s.Append("\"\""); caret++; }
                else { s.Append(source[caret]); caret++; }
            }

            // ダブルクォーテーションで挟む
            // Put in double quotes.
            if (isEscape) { s.Insert(0, '"'); s.Append('"'); }

            return s.ToString();
        }
    }
}