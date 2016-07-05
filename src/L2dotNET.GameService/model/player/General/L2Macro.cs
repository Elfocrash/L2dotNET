namespace L2dotNET.GameService.Model.Player.General
{
    public class L2Macro
    {
        public static readonly int CmdTypeSkill = 1;
        public static readonly int CmdTypeAction = 3;
        public static readonly int CmdTypeShortcut = 4;

        public int Id;
        public int Icon;
        public string Name;
	    public string Descr;
	    public string Acronym;
	    public L2MacroCmd[] Commands;
    }

    public class L2MacroCmd
    {
        public int Entry;
        public int Type;
        public int D1; // skill_id or page for shortcuts
        public int D2; // shortcut
        public string Cmd;
		
		public L2MacroCmd(int pEntry, int pType, int pD1, int pD2, string pCmd)
        {
            Entry = pEntry;
            Type = pType;
            D1 = pD1;
            D2 = pD2;
            Cmd = pCmd;
        }
    }
}
