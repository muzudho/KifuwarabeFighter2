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

            {
                StringBuilder sb = new StringBuilder();
                Util_StellaQL.String_to_tokens(expression, tokens);
                foreach (string token in tokens)
                {
                    sb.Append("token: ");
                    sb.AppendLine(token);
                }
                Debug.Log( sb.ToString() );
            }

            Stack<char> stack = new Stack<char>();

            int caret = 0;
            switch (expression[caret])
            {
                case ' ':
                    caret++;
                    break;
                case '(':
                    stack.Push('(');
                    break;
                case '[':
                    break;
                case '{':
                    break;
                case ')':
                    break;
                case ']':
                    break;
                case '}':
                    break;
                default:
                    break;
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
