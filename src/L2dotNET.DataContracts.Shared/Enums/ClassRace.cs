using System.ComponentModel;

namespace L2dotNET.DataContracts.Shared.Enums
{
    public enum ClassRace
    {
        [Description("Human")]
        Human,
        [Description("Elf")]
        Elf,
        [Description("Dark Elf")]
        DarkElf,
        [Description("Orc")]
        Orc,
        [Description("Dwarf")]
        Dwarf
    }
}