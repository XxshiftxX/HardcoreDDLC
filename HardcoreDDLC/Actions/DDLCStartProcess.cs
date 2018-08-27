using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardcoreDDLC.Actions
{
    class DDLCProcessAction : DDLCAction
    {
        public string Path { get; set; }

        public DDLCProcessAction(string path)
        {
            Path = path;
        }
    }
}
