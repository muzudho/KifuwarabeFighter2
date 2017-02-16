using System.Collections.Generic;
using System.Text;

namespace StellaQL
{
    public abstract class AbstractUserDefinedDatabase
    {
        public Dictionary<string, AControllable> AnimationControllerFilePath_to_table { get; protected set; }

        /// <summary>
        /// For error.
        /// </summary>
        public void Dump_Presentable(StringBuilder info_message)
        {
            info_message.AppendLine("Please add the path of your animator controller.");
            info_message.Append(AnimationControllerFilePath_to_table.Count); info_message.AppendLine(" mappings of animator controller and generated C # script are registered.");
            int i = 0;
            foreach (string path in AnimationControllerFilePath_to_table.Keys)
            {
                info_message.Append("["); info_message.Append(i); info_message.Append("]"); info_message.AppendLine(path);
                i++;
            }
        }
    }
}
