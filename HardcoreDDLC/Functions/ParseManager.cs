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
                    var args = currentScript.Split();
                    switch (args[0].ToLower())
                    {
                        case "#move":
                            resList.Add(new DDLCMoveAction(
                                MainWindow.MonikaStatic,
                                new System.Windows.Point(double.Parse(args[3]), double.Parse(args[4])),
                                double.Parse(args[2]))
                            { isSkiped = args[1] == "T" }
                            );
                            break;
                        case "#key":
                            resList.Add(new DDLCKeyinputAction(
                                string.Join(" ", args.Skip(2))
                            )
                            { isSkiped = args[1] == "T" });
                            break;
                        case "#process":
                            string e = null;
                            switch (args[2])
                            {
                                case "chrome":
                                    e = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                                    break;
                            }
                            if (e != null)
                                resList.Add(new DDLCProcessAction(e) { isSkiped = args[1] == "T" });
                            break;
                        case "#delay":
                            resList.Add(new DDLCDelayAction(int.Parse(args[2])) { isSkiped = args[1] == "T" });
                            break;
                    }
                }
                else
                {
                    // Text Mode
                    var strExtractor = new StringBuilder();
                    currentScript = currentScript.Replace("@@", "@").Replace("##", "#") + ' ';
                    string command = null;
                    for (var i = 0; i < currentScript.Length; i++)
                    {
                        var c = currentScript[i];

                        if (command == null)
                        {
                            if (c == '#')
                            {
                                command = string.Empty;
                            }
                            else
                                strExtractor.Append(c);
                        }
                        else
                        {
                            if (c == ' ')
                            {
                                switch (command)
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
