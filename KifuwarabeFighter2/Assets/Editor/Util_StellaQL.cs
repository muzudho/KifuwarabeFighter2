using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace StellaQL
{
    public abstract class Util_StellaQL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression">例えば "( [ ( Alpha Cee ) ( Beta Dee ) ] { Eee } )" といった式。</param>
        /// <returns></returns>
        public static List<int> Execute(string expression)
        {
            List<int> result = new List<int>();
            List<string> tokens = new List<string>();

            // 字句解析
            {
                Util_StellaQL.String_to_tokens(expression, tokens);
                //StringBuilder sb = new StringBuilder();
                //foreach (string token in tokens)
                //{
                //    sb.Append("token: ");
                //    sb.AppendLine(token);
                //}
                //Debug.Log( sb.ToString() );
            }

            Stack<char> stack = new Stack<char>();

            // スタック積み
            {
                string openParen = ""; // 閉じ括弧に対応する、「開きカッコ」
                int iCursor = 0;
                while ( iCursor < tokens.Count )
                {
                    string token = tokens[iCursor];
                    if (""== openParen)
                    {
                        StringBuilder sb1 = new StringBuilder();
                        sb1.Append("go["); sb1.Append(iCursor); sb1.Append("]: "); sb1.Append(token); sb1.AppendLine();
                        Debug.Log(sb1.ToString());
                        switch (token)
                        {
                            case ")": openParen = "("; tokens[iCursor] = ""; break;
                            case "]": openParen = "["; tokens[iCursor] = ""; break;
                            case "}": openParen = "{"; tokens[iCursor] = ""; break;
                            default: break; // 無視して進む
                        }
                    }
                    else // 後ろに進む☆
                    {
                        StringBuilder sb2 = new StringBuilder();
                        sb2.Append("back["); sb2.Append(iCursor); sb2.Append("]: "); sb2.Append(token); sb2.AppendLine();
                        Debug.Log(sb2.ToString());
                        switch (token)
                        {
                            case "(":
                            case "[":
                            case "{": if (openParen == token) { openParen = ""; } break;
                            default: break;
                        }
                        tokens[iCursor] = ""; // 後ろに進んだ先では、その文字を削除する
                    }

                    if ("" == openParen) { iCursor++; }
                    else { iCursor--; }
                }
            }


            return result;
        }

        private static void String_to_tokens(string expression, List<string> tokens)
        {
            StringBuilder word = new StringBuilder();

            for (int iCaret = 0; iCaret < expression.Length; iCaret++)
            {
                char ch = expression[iCaret];
                switch (ch)
                {
                    case ' ':
                        if (0<word.Length)
                        {
                            tokens.Add(word.ToString());
                            word.Length = 0;
                        }
                        break;
                    case '(':
                    case '[':
                    case '{':
                    case ')':
                    case ']':
                    case '}':
                        tokens.Add(ch.ToString());
                        break;
                    default: word.Append(ch); break;
                }
            }

            if (0 < word.Length)//構文エラー
            {
                tokens.Add(word.ToString());
                word.Length = 0;
            }
        }


    }
}
