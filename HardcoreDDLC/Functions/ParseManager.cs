using HardcoreDDLC.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardcoreDDLC.Functions
{
    static class ParseManager
    {
        public static List<DDLCAction> ParseRawScript(string rawscript)
        {
            var resList = new List<DDLCAction>();

            var scriptReader = new StringReader(rawscript);
            while(true)
            {
                var currentScript = scriptReader.ReadLine();
                if (currentScript == null) break;
                if (currentScript.StartsWith("@") && !currentScript.StartsWith("@@")) continue;
                if (currentScript.StartsWith("#") && !currentScript.StartsWith("##"))
                {
                    // Command Mode
                    var args = currentScript.Remove(1).Split();
                    switch(args[0])
                    {
                        case "Move":
                            
                            break;
                    }
                }
                else
                {
                    // Text Mode
                    var strExtractor = new StringBuilder();
                    currentScript = currentScript.Replace("@@", "@").Replace("##", "#") + ' ';
                    string command = null;
                    for(var i = 0; i < currentScript.Length; i++)
                    {
                        var c = currentScript[i];

                        if (command == null)
                        {
                            if(c == '#')
                            {
                                command = string.Empty;
                            }
                            else
                                strExtractor.Append(c);
                        }
                        else
                        {
                            if(c == ' ')
                            {
                                switch(command)
                                {
                                    case "endl":
                                        currentScript = scriptReader.ReadLine().Replace("@@", "@").Replace("##", "#") + ' ';
                                        strExtractor.Append('\n');
                                        i = -1;
                                        break;
                                }
                                command = null;
                            }
                            else
                            {
                                command += c;
                            }
                        }
                    }
                    resList.Add(new DDLCScriptAction(strExtractor.ToString()));
                }
            }

            return resList;
        }
    }
}
