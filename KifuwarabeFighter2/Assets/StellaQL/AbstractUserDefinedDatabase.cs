using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StellaQL
{
    public abstract class AbstractUserDefinedDatabase
    {
        public Dictionary<string, AControllable> AnimationControllerFilePath_to_table { get; protected set; }
    }
}
