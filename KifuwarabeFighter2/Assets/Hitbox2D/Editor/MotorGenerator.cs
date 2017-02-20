using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor.Animations;
using DojinCircleGrayscale.StellaQL;
using System.IO;

namespace DojinCircleGrayscale.Hitbox2DLorikeet
{
    public abstract class MotorGenerator
    {
        public static void WriteCshapScript(AnimatorController ac, StringBuilder info_message)
        {
            AconStateNameScanner aconScanner = new AconStateNameScanner();
            aconScanner.ScanAnimatorController(ac, info_message);

            StringBuilder contents = new StringBuilder();

            // 変換例:

            // 「Main_Char3」は「Main_Char3」(同じ)

            // 「BattleFloor_char@arm@finger」は「Battolefloor_Chararmfinger」
            string className = FullpathConstantGenerator.String_to36_pascalCase(ac.name, "_", "_");

            string abstractClassName = className + "_AbstractMotor";

            contents.Append(@"using DojinCircleGrayscale.Hitbox2DLorikeet;

namespace DojinCircleGrayscale.StellaQL.Acons."); contents.AppendLine(className);
            contents.Append(@"{
    /// <summary>
    /// This file was automatically generated.
    /// It was created by ["); contents.Append(Hitbox2DLorikeetWindow.BUTTON_LABEL_GENERATE_MOTOR); contents.Append(@"] button.
    /// </summary>
    public abstract class "); contents.Append(abstractClassName); contents.Append(@" : AbstractMotor
    {
        #region Motion tags
");
            HashSet<MotionRecord> motionRecords = new HashSet<MotionRecord>(aconScanner.Motions);
            if(0< motionRecords.Count)
            {
                // 文字列のリストに移し替えます
                List<string> motionAssetPaths = new List<string>();
                foreach (MotionRecord record in motionRecords)
                {
                    motionAssetPaths.Add((string)record.Fields[MotionRecord.ASSET_PATH]);
                }
                motionAssetPaths.Sort(); // FIXME: 文字列順になるわけではない？
                contents.Append("        public const string");
                foreach (string motionAssetPath in motionAssetPaths)
                {
                    string motionFilename = Path.GetFileNameWithoutExtension(motionAssetPath);

                    // 先に改行を持ってくる。最後のセミコロンを付ける処理を簡単にする。
                    contents.AppendLine(); 

                    contents.Append("            MOTION_");
                    contents.Append(FullpathConstantGenerator.String_split_toUppercaseAlphabetFigureOnly_join(motionFilename, "@", "_"));
                    contents.Append(@" = """);
                    contents.Append(motionAssetPath);

                    // 改行は最後ではなく、最初に付けておく。
                    contents.Append(@""",");
                }

                // 最後のコンマを削る。
                contents.Length--;

                // 代わりにセミコロンを追加する。
                contents.AppendLine(@"; // semi colon");

                contents.AppendLine();
            }

            contents.AppendLine(@"        #endregion
    }
}");

            FileUtility_Editor.Write(FileUtility_Editor.Filepath_GenerateMotor(ac), contents, info_message);
        }
    }
}
