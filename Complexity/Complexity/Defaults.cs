using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Complexity
{
    class Defaults
    {
        public static void setDefaults(ref Machine machine)
        {
            if (machine.type == 0)
            {
                machine.inputFace = -1;
                machine.outputFace = 1;
            }
        }
    }
}
