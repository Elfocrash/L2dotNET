namespace L2dotNET.Models.Player.General
{
    public class L2Shortcut
    {
        public const int TypeItem = 1;
        public const int TypeSkill = 2;
        public const int TypeAction = 3;
        public const int TypeMacro = 4;
        public const int TypeRecipe = 5;

        public int Slot { get; set; }
        public int Page { get; set; }
        public int Type { get; set; }
        public int Id { get; set; }
        public int Level { get; set; }
        public int CharacterType { get; set; }

        public L2Shortcut(int slotId, int pageId, int shortcutType, int shortcutId, int shortcutLevel, int characterType)
        {
            Slot = slotId;
            Page = pageId;
            Type = shortcutType;
            Id = shortcutId;
            Level = shortcutLevel;
            CharacterType = characterType;
        }
    }
}