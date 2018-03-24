namespace L2dotNET.Models.player.General
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
}