using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace StellaQL
{
    public abstract class StellaQLWriter
    {
        #region 可変のファイル名
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
            //Debug.Log(message2.ToString());
        }
    }
}
