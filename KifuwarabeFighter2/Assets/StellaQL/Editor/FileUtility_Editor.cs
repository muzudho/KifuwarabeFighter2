using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace StellaQL
{
    /// <summary>
    /// Collect read related file paths related to unity editor.
    /// </summary>
    public abstract class StellaQLReader
    {
        #region Fixed file name
        public static string Filepath_UpdateRequestCsv() { return "./UpdateRequest.csv"; }
        #endregion

        public static void ReadUpdateRequestCsv(out HashSet<DataManipulationRecord> updateRequestRecords, StringBuilder message)
        {
            string filepath = Filepath_UpdateRequestCsv();
            updateRequestRecords = new HashSet<DataManipulationRecord>();

            string[] lines = File.ReadAllLines(filepath);
            int row = 0;
            foreach (string line in lines)
            {
                Debug.Log("lines["+row+"]="+line);
                if (row == 0) { row++; continue; } // Because the [0] line is a header line, skip it
                List<string> cells = CsvParser.CsvLine_to_cellList(line);
                if ("[EOF]" == cells[0]) { break; } // Finding [EOF] is over.
                updateRequestRecords.Add(new DataManipulationRecord(cells[0], cells[1], cells[2], cells[3], cells[4], cells[5], cells[6], cells[7], cells[8], cells[9]));
                row++;
            }

            message.AppendLine("Read. " + lines.Length + " rows to " + updateRequestRecords.Count + " records. " + Path.GetFullPath(filepath));
        }

        public static void DeleteUpdateRequestCsv( StringBuilder message)
        {
            string filepath = Filepath_UpdateRequestCsv();
            File.Delete(Path.GetFullPath(filepath));
            message.AppendLine("Deleted file. " + Path.GetFullPath(filepath));
        }
    }

    /// <summary>
    /// Write related file paths related to the unity editor.
    /// </summary>
    public abstract class StellaQLWriter
    {
        #region Fixed file name
        public static string Filepath_StellaQLMacroApplicationOds() { return Path.GetFullPath( "./StellaQL_MacroApplication.ods"); }
        #endregion

        #region Variable filename
        public static string Filepath_GenerateFullpathConstCs(AnimatorController ac)
        {
            string fullpath = System.IO.Path.GetFullPath(AssetDatabase.GetAssetPath(ac.GetInstanceID()));

            // ファイル名はネーム・スペースに合わせたいので同じ処理をします。
            // Since we want to match the file name to the namespace we will do the same thing.
            string filename = FullpathConstantGenerator.String_to36_pascalCase(Path.GetFileNameWithoutExtension(fullpath), "_", "_");

            return Path.Combine(
                Directory.GetParent(fullpath).FullName,
                filename + "_Abstract.cs"
                );
        }
        public static string Filepath_LogStateSelect(string aconName, string theName)
        {
            StringBuilder sb = new StringBuilder();
            if ("" == theName)
            {
                sb.Append("./_log_("); sb.Append(aconName); sb.Append(")STATE_SELECT.csv");
                return sb.ToString();
            }
            else
            {
                sb.Append("./_log_("); sb.Append(aconName); sb.Append(")("); sb.Append(theName); sb.Append(")STATE_SELECT.csv");
                return sb.ToString();
            }
        }
        public static string Filepath_LogTransitionSelect(string aconName, string theName)
        {
            StringBuilder sb = new StringBuilder();
            if ("" == theName)
            {
                sb.Append("./_log_("); sb.Append(aconName); sb.Append(")TRANSITION_SELECT.csv");
                return sb.ToString();
            }
            else
            {
                sb.Append("./_log_("); sb.Append(aconName); sb.Append(")("); sb.Append(theName); sb.Append(")TRANSITION_SELECT.csv");
                return sb.ToString();
            }
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
            message.AppendLine("Writed. " + Path.GetFullPath(filepath));
        }
    }
}
