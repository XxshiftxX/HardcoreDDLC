using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardcoreDDLC.Actions
{
    class DDLCKeyinputAction : DDLCAction
    {
        public string Input { get; set; }

        public DDLCKeyinputAction(string input)
        {
            Input = input;
        }
    }
}
