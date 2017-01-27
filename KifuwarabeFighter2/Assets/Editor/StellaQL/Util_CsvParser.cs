using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 手作りの CSVの１行分のパーサーだぜ☆（＾▽＾）
/// </summary>
public abstract class Util_CsvParser
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
        int length = source.Length;
        List<string> list_Destination = new List<string>();
        char ch;

        // 空か。
        if (source.Length < 1)
        {
            goto gt_EndMethod;
        }


        //ystem.Console.WriteLine("（１）source[" + source + "]");

        //１セル分の文字列
        StringBuilder cell = new StringBuilder();
        int index = 0;
        while (index < length)
        {
            cell.Length = 0;
            ch = source[index];

            //ystem.Console.WriteLine("（２）index[" + index + "] ch[" + ch + "]");

            if (',' == ch)
            {
                // 空を追加して次へ。
                index++;

                //ystem.Console.WriteLine("（３）index[" + index + "] ");
            }
            else if ('"' == ch)
            {
                // 1文字目が「"」なら、2文字目へ。
                index++;

                //ystem.Console.WriteLine("（４）index[" + index + "] ");

                // エスケープしながら、単独「"」が出てくるまでそのまま出力。
                while (index < length)
                {
                    ch = source[index];

                    //ystem.Console.WriteLine("（５）index[" + index + "] ");

                    if ('"' == ch)
                    {
                        // 「"」だった。


                        // ここで文字列終わりなのだが、
                        // しかし次の文字が「"」の場合、まだこの「"」で終わってはいけない。
                        // 

                        //ystem.Console.WriteLine("（６）index[" + index + "] ");


                        if (index + 1 == length)
                        {
                            // 2文字目が無ければ、
                            //「"」を無視して終了。
                            index++;

                            //ystem.Console.WriteLine("（７）index[" + index + "] ");

                            break;
                        }
                        else if ('"' == source[index + 1])
                        {
                            // 2文字目も「"」なら、
                            // 1,2文字目の「""」を「"」に変換して続行。
                            index += 2;
                            cell.Append('"');

                            //ystem.Console.WriteLine("（８）index[" + index + "] ");
                        }
                        else
                        {
                            // 2文字目が「"」でなければ、
                            //「"」を無視して終了。
                            index += 2;//【改変/】2012年10月30日変更。旧： index++;

                            //ystem.Console.WriteLine("（９）index[" + index + "] 　2文字目が「\"」でなければ、「\"」を無視して終了。");

                            break;
                        }
                    }
                    else
                    {
                        // 通常文字なので続行。
                        cell.Append(ch);
                        index++;

                        //ystem.Console.WriteLine("（１１）index[" + index + "] ch[" + ch + "]");
                    }

                    //ystem.Console.WriteLine("（１２）index[" + index + "] ");
                }

                //ystem.Console.WriteLine("（１３）index[" + index + "] ");
            }
            else
            {
                //ystem.Console.WriteLine("（１４a）index[" + index + "] s_Cell[" + s_Cell.ToString() + "] ch[" + ch + "]");

                cell.Append(ch);
                index++;

                //ystem.Console.WriteLine("（１４b）index[" + index + "] s_Cell[" + s_Cell.ToString() + "]");

                // 1文字目が「"」でないなら、「,」が出てくるか、次がなくなるまでそのまま出力。
                // フォーマットチェックは行わない。
                while (index < length)
                {
                    ch = source[index];

                    //ystem.Console.WriteLine("（１５）index[" + index + "] ch[" + ch + "]");


                    if (delimiter != ch)
                    {
                        // 文字を追加して次へ。
                        cell.Append(ch);
                        index++;

                        //ystem.Console.WriteLine("（１６）index[" + index + "] ");

                    }
                    else
                    {
                        // 「,」を見つけたのでこれを無視し、
                        // このセル読取は脱出。
                        index++;

                        //ystem.Console.WriteLine("（１７）index[" + index + "] 「,」を見つけたのでこれを無視し、このセル読取は脱出。");

                        break;
                    }

                    //ystem.Console.WriteLine("（１８）index[" + index + "] ");

                }
                // 次が無くなったか、「,」の次の文字を指している。
            }

            //ystem.Console.WriteLine("（２０）index[" + index + "] s_Cell.ToString()[" + s_Cell.ToString() + "]");

            list_Destination.Add(cell.ToString());
        }

        //ystem.Console.WriteLine("（２１）index[" + index + "] ");


        gt_EndMethod:
        return list_Destination;
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
        int length = source.Length;

        // エスケープが必要なら真。
        bool isEscape = false;
        char ch;

        StringBuilder s = new StringBuilder();

        for (int index = 0; index < length;)
        {
            ch = source[index];
            if (',' == ch)
            {
                // エスケープが必要
                isEscape = true;
                s.Append(ch);
                index++;
            }
            else if ('"' == ch)
            {
                // エスケープが必要
                isEscape = true;
                s.Append("\"\"");
                index++;
            }
            else
            {
                s.Append(ch);
                index++;
            }
        }

        if (isEscape)
        {
            s.Insert(0, '"');
            s.Append('"');
        }

        return s.ToString();
    }
}
