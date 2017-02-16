using System.Collections.Generic;
using System.Text;

namespace StellaQL
{
    /// <summary>
    /// Parser for one line of homemade CSV.
    /// </summary>
    public abstract class CsvParser
    {
        public static List<string> CsvLine_to_cellList(string source)
        {
            return CsvLine_to_cellList(source, ',');
        }
        /// <summary>
        /// Unescape.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static List<string> CsvLine_to_cellList(string source, char delimiter)
        {
            List<string> cells = new List<string>();
            if (source.Length < 1) { return cells; } // End if empty string

            // Character string for one cell
            StringBuilder cell = new StringBuilder();
            int caret = 0;
            while (caret < source.Length) // This loop corresponds to one line
            {
                switch (source[caret])
                {
                    case ',': caret++; cells.Add(cell.ToString()); cell.Length = 0; break; // Output the token and continue.
                    case '"':
                        // From here to literal string processing
                        caret++;

                        // While escaping, output it as it is until alone (") comes out.
                        while (caret < source.Length)
                        {
                            if ('"'==source[caret])
                            {
                                // If this is a single ("), it ends.
                                // If it is two consecutive (") it will not end.

                                if (caret + 1 == source.Length) { caret++; break; }// If (") was the last character, ignore it and omit the loop.
                                else if ('"' == source[caret + 1]) { caret += 2; cell.Append('"'); } // If the second letter is also ("), continue with the loop by inserting two (") suddenly and inserting (") instead.
                                else { caret = source.IndexOf(',',caret)+1; break; } // If it is not two consecutive ("), skip whitespace etc. until the next (,). (Change)2012-10-30, 2017-02-01.
                            }
                            else { cell.Append(source[caret]); caret++; }// Since it is a normal character, I continue with a loop.
                        }
                        cells.Add(cell.ToString().Trim()); cell.Length = 0; break; // Cut the front and rear spaces
                    default: cell.Append(source[caret]); caret++; break;// It is either a string that is not double quoted or a space before double quotes.
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
        /// (1) If "," or (") is included, add (") to both ends.
        /// (2) Contains (") converted to ("").
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string EscapeCell(string source)
        {
            bool isEscape = false; // True if you need escape.

            StringBuilder s = new StringBuilder();

            for (int caret = 0; caret < source.Length;) {
                if (',' == source[caret] || '\r' == source[caret] || '\n' == source[caret]) { isEscape = true; s.Append(source[caret]); caret++; }// Since commas were included, escaping became necessary. (2017-02-09 Add '\r', '\n')
                else if ('"' == source[caret]) { isEscape = true; s.Append("\"\""); caret++; } // Since double quotes were included, escape was required.
                else { s.Append(source[caret]); caret++; }
            }

            if (isEscape) { s.Insert(0, '"'); s.Append('"'); } // Put in double quotes.

            return s.ToString();
        }
    }
}