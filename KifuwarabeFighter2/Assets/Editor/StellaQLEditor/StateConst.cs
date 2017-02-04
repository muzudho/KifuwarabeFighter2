using System;
using System.Text;
using UnityEditor.Animations;

namespace StellaQL
{
    public abstract class StateConst
    {
        public static void WriteCshapScript(AnimatorController ac, StringBuilder message)
        {
            AniconStateNameScanner aniconScanner = new AniconStateNameScanner();
            aniconScanner.ScanAnimatorController(ac, message);
            //message.Append(aniconScanner.Dump());
            //message.AppendLine("ac.name = " + ac.name + " To26=["+ StateConst.String_split_toUppercaseAlphabetOnly_join(ac.name,"@","_") +"]");
            //message.AppendLine("ac.path = " + AssetDatabase.GetAssetPath(ac.GetInstanceID()));
            //message.AppendLine("filefullpath = " + System.IO.Path.GetFullPath( AssetDatabase.GetAssetPath(ac.GetInstanceID())));
            //message.AppendLine("new-path = " + StellaQLWriter.Filepath_StateConstCs(ac));

            StringBuilder contents = new StringBuilder();

            string namespaceStr = StateConst.String_split_toUppercaseAlphabetFigureOnly_join(ac.name, "@", "_");
            contents.AppendLine("namespace StellaQL");
            contents.AppendLine("{");
            contents.Append("    public abstract class "); contents.AppendLine(namespaceStr);
            contents.AppendLine("    {");
            foreach (string fullpath in aniconScanner.FullpathSet)
            {
                contents.Append("        public const string ");
                contents.Append(StateConst.String_split_toUppercaseAlphabetFigureOnly_join(fullpath, ".", "_"));
                contents.Append(@" = """);
                contents.Append(fullpath);
                contents.AppendLine(@""";");
            }
            contents.AppendLine("    }");
            contents.AppendLine("}");

            StellaQLWriter.Write(StellaQLWriter.Filepath_StateConstCs(ac), contents, message);
        }

        /// <summary>
        /// 大文字アルファベット２６文字と、数字１０文字だけに詰めます。大文字に変換できない文字は無視します。
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
            return string.Join("_", tokens);
        }

        /// <summary>
        /// 大文字アルファベット２６文字と、数字１０文字だけに詰めます。大文字に変換できない文字は無視します。
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
