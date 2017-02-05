using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace StellaQL
{
    public abstract class StellaQLReader
    {
        #region 固定のファイル名
        public static string Filepath_UpdateRequestCsv() { return "./UpdateRequest.csv"; }
        #endregion

        public static void ReadUpdateRequestCsv(out HashSet<UpateReqeustRecord> updateRequestRecords, StringBuilder message)
        {
            string filepath = Filepath_UpdateRequestCsv();
            updateRequestRecords = new HashSet<UpateReqeustRecord>();

            string[] lines = File.ReadAllLines(filepath);
            int row = 0;
            foreach (string line in lines)
            {
                Debug.Log("lines["+row+"]="+line);
                if (row == 0) { row++; continue; } // [0]行目はヘッダー行なので飛ばす
                List<string> cells = CsvParser.CsvLine_to_cellList(line);
                if ("[EOF]" == cells[0]) { break; } // [EOF]を見つけたら終わり。
                updateRequestRecords.Add(new UpateReqeustRecord(cells[0], cells[1], cells[2], cells[3], cells[4], cells[5], cells[6], cells[7], cells[8], cells[9]));
                row++;
            }

            message.AppendLine("Read☆（＾▽＾） " + lines.Length + " rows to " + updateRequestRecords.Count + " records. " + Path.GetFullPath(filepath));
        }

        public static void DeleteUpdateRequestCsv( StringBuilder message)
        {
            string filepath = Filepath_UpdateRequestCsv();
            File.Delete(Path.GetFullPath(filepath));
            message.AppendLine("Deleted file☆（＾▽＾） " + Path.GetFullPath(filepath));
        }
    }

    public abstract class StellaQLWriter
    {
        #region 固定のファイル名
        public static string Filepath_StellaQLMacroApplicationOds() { return Path.GetFullPath( "./StellaQL_MacroApplication.ods"); }
        #endregion

        #region 可変のファイル名
        public static string Filepath_GenerateFullpathConstCs(AnimatorController ac)
        {
            string fullpath = System.IO.Path.GetFullPath(AssetDatabase.GetAssetPath(ac.GetInstanceID()));

            return Path.Combine(
                Directory.GetParent(fullpath).FullName,
                Path.GetFileNameWithoutExtension(fullpath) + "_Abstract.cs"
                );
        }
        public static string Filepath_LogStateSelect(string aconName)
        {
            return "./_log_(" + aconName + ")STATE_SELECT.csv";
        }
        public static string Filepath_LogTransitionSelect(string aconName)
        {
            return "./_log_(" + aconName + ")TRANSITION_SELECT.csv";
        }
        public static string Filepath_LogParameters(string aconName, bool outputDefinition)
        {
            if (outputDefinition) { return "./_log_(" + aconName + ")parameters_def.csv"; }
            else { return "./_log_(" + aconName + ")parameters.csv"; }
        }
        public static string Filepath_LogLayer(string aconName, bool outputDefinition) {
            if (outputDefinition) { return "./_log_(" + aconName + ")layers_def.csv"; }
            else { return "./_log_(" + aconName + ")layers.csv"; }
        }
        public static string Filepath_LogStatemachine(string aconName, bool outputDefinition)
        {
            if (outputDefinition) { return "./_log_(" + aconName + ")stateMachines_def.csv"; }
            else { return "./_log_(" + aconName + ")stateMachines.csv"; }
        }
        public static string Filepath_LogStates(string aconName, bool outputDefinition)
        {
            if (outputDefinition) { return "./_log_(" + aconName + ")states_def.csv"; }
            else { return "./_log_(" + aconName + ")states.csv"; }
        }
        public static string Filepath_LogTransition(string aconName, bool outputDefinition)
        {
            if (outputDefinition) { return "./_log_(" + aconName + ")transitions_def.csv"; }
            else { return "./_log_(" + aconName + ")transitions.csv"; }
        }
        public static string Filepath_LogConditions(string aconName, bool outputDefinition)
        {
            if (outputDefinition) { return "./_log_(" + aconName + ")conditions_def.csv"; }
            else { return "./_log_(" + aconName + ")conditions.csv"; }
        }
        public static string Filepath_LogPositions(string aconName, bool outputDefinition)
        {
            if (outputDefinition) { return "./_log_(" + aconName + ")positions_def.csv"; }
            else { return "./_log_(" + aconName + ")positions.csv"; }
        }
        #endregion

        public static void Write(string filepath, StringBuilder contents, StringBuilder message)
        {
            File.WriteAllText(filepath, contents.ToString());
            message.AppendLine("Writed☆（＾▽＾） " + Path.GetFullPath(filepath));
        }
    }
}
