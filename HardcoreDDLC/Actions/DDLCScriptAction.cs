namespace HardcoreDDLC.Actions
{
    public class DDLCScriptAction : DDLCAction
    {
        public string Script { get; set; }
        public DDLCScriptAction(string script)
        {
            Script = script;
        }
    }
}