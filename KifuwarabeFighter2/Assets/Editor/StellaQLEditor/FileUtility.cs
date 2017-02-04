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
        #region 可変のファイル名
        public static string Filepath_StateConstCs(AnimatorController ac)
        {
            string fullpath = System.IO.Path.GetFullPath(AssetDatabase.GetAssetPath(ac.GetInstanceID()));

            return Path.Combine(
                Directory.GetParent(fullpath).FullName,
                Path.GetFileNameWithoutExtension(fullpath) + "_stateConst.cs"
                );
        }
        public static string Filepath_LogStateSelect(string aniconName)
        {
            return "./_log_(" + aniconName + ")STATE_SELECT.csv";
        }
        public static string Filepath_LogTransitionSelect(string aniconName)
        {
            return "./_log_(" + aniconName + ")TRANSITION_SELECT.csv";
        }
        public static string Filepath_LogParameters(string aniconName, bool outputDefinition)
        {
            if (outputDefinition) { return "./_log_(" + aniconName + ")parameters_def.csv"; }
            else { return "./_log_(" + aniconName + ")parameters.csv"; }
        }
        public static string Filepath_LogLayer(string aniconName, bool outputDefinition) {
            if (outputDefinition) { return "./_log_(" + aniconName + ")layers_def.csv"; }
            else { return "./_log_(" + aniconName + ")layers.csv"; }
        }
        public static string Filepath_LogStatemachine(string aniconName, bool outputDefinition)
        {
            if (outputDefinition) { return "./_log_(" + aniconName + ")stateMachines_def.csv"; }
            else { return "./_log_(" + aniconName + ")stateMachines.csv"; }
        }
        public static string Filepath_LogStates(string aniconName, bool outputDefinition)
        {
            if (outputDefinition) { return "./_log_(" + aniconName + ")states_def.csv"; }
            else { return "./_log_(" + aniconName + ")states.csv"; }
        }
        public static string Filepath_LogTransition(string aniconName, bool outputDefinition)
        {
            if (outputDefinition) { return "./_log_(" + aniconName + ")transitions_def.csv"; }
            else { return "./_log_(" + aniconName + ")transitions.csv"; }
        }
        public static string Filepath_LogConditions(string aniconName, bool outputDefinition)
        {
            if (outputDefinition) { return "./_log_(" + aniconName + ")conditions_def.csv"; }
            else { return "./_log_(" + aniconName + ")conditions.csv"; }
        }
        public static string Filepath_LogPositions(string aniconName, bool outputDefinition)
        {
            if (outputDefinition) { return "./_log_(" + aniconName + ")positions_def.csv"; }
            else { return "./_log_(" + aniconName + ")positions.csv"; }
        }
        #endregion

        public static void Write(string filepath, StringBuilder contents, StringBuilder message)
        {
            File.WriteAllText(filepath, contents.ToString());
            message.AppendLine("Writed☆（＾▽＾） " + Path.GetFullPath(filepath));
        }
    }
}
