using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor.Animations;

namespace StellaQL
{
    public abstract class FullpathConstantGenerator
    {
        public static void WriteCshapScript(AnimatorController ac, StringBuilder message)
        {
            AconStateNameScanner aconScanner = new AconStateNameScanner();
            aconScanner.ScanAnimatorController(ac, message);

            StringBuilder contents = new StringBuilder();

            contents.AppendLine("using System.Collections.Generic;");
            contents.AppendLine();
            contents.AppendLine("namespace StellaQL.Acons");
            contents.AppendLine("{");
            contents.AppendLine("    /// <summary>");
            contents.AppendLine("    /// This file was automatically generated.");
            contents.AppendLine("    /// It was created by [Generate fullpath constant C #] button.");
            contents.AppendLine("    /// </summary>");
            string className = FullpathConstantGenerator.String_to36_pascalCase(ac.name, "@");
            contents.Append("    public abstract class "); contents.Append(className);
            contents.AppendLine(" : AbstractAControll");
            contents.AppendLine("    {");
            List<string> fullpaths = new List<string>(aconScanner.FullpathSet);
            fullpaths.Sort();
            foreach (string fullpath in fullpaths)
            {
                contents.Append("        public const string ");
                contents.Append(FullpathConstantGenerator.String_split_toUppercaseAlphabetFigureOnly_join(fullpath, ".", "_"));
                contents.Append(@" = """);
                contents.Append(fullpath);
                contents.AppendLine(@""";");
            }

            contents.Append("        public "); contents.Append(className); contents.AppendLine("()");
            contents.AppendLine("        {");
            contents.AppendLine("            Code.Register(StateHash_to_record, new List<AcStateRecordable>()");
            contents.AppendLine("            {");

            foreach (string fullpath in fullpaths)
            {
                contents.Append("                new DefaultAcState( ");
                contents.Append(FullpathConstantGenerator.String_split_toUppercaseAlphabetFigureOnly_join(fullpath, ".", "_"));
                contents.AppendLine("),");
            }

            contents.AppendLine("            });");
            contents.AppendLine("        }");
            contents.AppendLine("    }");
            contents.AppendLine("}");

            StellaQLWriter.Write(StellaQLWriter.Filepath_GenerateFullpathConstCs(ac), contents, message);
        }

        /// <summary>
        /// アルファベット２６文字と、数字１０文字だけに詰めます。英字・数字に変換できない文字は無視します。
        /// パスカルケースにします。（white.alpaca -> WhiteAlpaca）
        /// 通称 To36
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string String_to36_pascalCase(string source1, string splitSeparator)
        {
            StringBuilder fullnameSb = new StringBuilder();
            string[] tokens = source1.Split(new string[] { splitSeparator }, StringSplitOptions.None);
            for (int iToken = 0; iToken < tokens.Length; iToken++)
            {
                StringBuilder tokenSb = new StringBuilder();
                string token = tokens[iToken];
                for (int caret = 0; caret < token.Length; caret++)
                {
                    if (tokenSb.Length == 0)//先頭の文字
                    {
                        if (Char.IsUpper(token[caret]) || Char.IsDigit(token[caret])) { tokenSb.Append(token[caret]); } // 大文字と数字はそのまま追加
                        else if (Char.IsLower(token[caret])) { tokenSb.Append(Char.ToUpper(token[caret])); } // 小文字は大文字にして追加
                                                                                                        // その他の文字は無視
                    }
                    else//先頭以降の文字
                    {
                        if (Char.IsLower(token[caret]) || Char.IsDigit(token[caret])) { tokenSb.Append(token[caret]); } // 小文字と数字はそのまま追加
                        else if (Char.IsUpper(token[caret])) { tokenSb.Append(Char.ToLower(token[caret])); } // 大文字は小文字にして追加
                                                                                                        // その他の文字は無視
                    }
                }
                fullnameSb.Append(tokenSb.ToString());
            }
            return fullnameSb.ToString();
        }

        /// <summary>
        /// 大文字アルファベット２６文字と、数字１０文字だけに詰めます。英字・数字に変換できない文字は無視します。
        /// 通称 To36
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string String_split_toUppercaseAlphabetFigureOnly_join(string source1, string splitSeparator, string joinSeparator)
        {
            string[] tokens = source1.Split(new string[] { splitSeparator }, StringSplitOptions.None);
            for (int iToken = 0; iToken < tokens.Length; iToken++)
            {
                string token = tokens[iToken];
                StringBuilder sb = new StringBuilder();
                for (int caret = 0; caret < token.Length; caret++)
                {
                    if (Char.IsUpper(token[caret]) || Char.IsDigit(token[caret])) { sb.Append(token[caret]); } // 大文字と数字はそのまま追加
                    else if (Char.IsLower(token[caret])) { sb.Append(Char.ToUpper(token[caret])); } // 小文字は大文字にして追加
                                                                                                    // その他の文字は無視
                }
                tokens[iToken] = sb.ToString();
            }
            return string.Join(joinSeparator, tokens);
        }

        /// <summary>
        /// 大文字アルファベット２６文字と、数字１０文字だけに詰めます。英字・数字に変換できない文字は無視します。
        /// 通称 To36
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string String_to_UppercaseAlphabetFigureOnly(string source)
        {
            StringBuilder sb = new StringBuilder();
            for (int caret = 0; caret < source.Length; caret++)
            {
                if (Char.IsUpper(source[caret]) || Char.IsDigit(source[caret])) { sb.Append(source[caret]); } // 大文字と数字はそのまま追加
                else if (Char.IsLower(source[caret])) { sb.Append(Char.ToUpper(source[caret])); } // 小文字は大文字にして追加
                // その他の文字は無視
            }
            return sb.ToString();
        }

    }
}
