using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DojinCircleGrayscale.StellaQL
{
    public class AbstractUserSettings
    {
        protected AbstractUserSettings()
        {
            AnimationControllerFilepath_to_userDefinedInstance = new Dictionary<string, AControllable>();
        }

        public Dictionary<string, AControllable> AnimationControllerFilepath_to_userDefinedInstance { get; protected set; }

        public void AddMappings_AnimatorControllerFilepath_And_UserDefinedInstance(Dictionary<string, AControllable> mappings)
        {
            foreach (KeyValuePair<string, AControllable> pair in mappings)
            {
                if (AnimationControllerFilepath_to_userDefinedInstance.ContainsKey(pair.Key))
                {
                    throw new UnityException("It is already added key. Animator controller filepath = [" + pair.Key + "]");
                }
                AnimationControllerFilepath_to_userDefinedInstance.Add(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// エラー時の表示のために。
        /// </summary>
        public void Dump_Presentable(StringBuilder info_message)
        {
            info_message.AppendLine("Please add the path of your animator controller.");
            info_message.Append(AnimationControllerFilepath_to_userDefinedInstance.Count); info_message.AppendLine(" mappings of animator controller and generated C # script are registered.");
            int i = 0;
            foreach (string path in AnimationControllerFilepath_to_userDefinedInstance.Keys)
            {
                info_message.Append("["); info_message.Append(i); info_message.Append("]"); info_message.AppendLine(path);
                i++;
            }
        }
    }
}
