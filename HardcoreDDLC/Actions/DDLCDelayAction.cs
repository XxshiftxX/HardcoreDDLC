using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardcoreDDLC.Actions
{
    internal class DDLCDelayAction : DDLCAction
    {
        public int Time;

        public DDLCDelayAction(int time) => Time = time;
    }
}
