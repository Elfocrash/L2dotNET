namespace L2dotNET.GameService.Model.Player
{
    public class L2Shortcut
    {
        public const int TYPE_ITEM = 1;
        public const int TYPE_SKILL = 2;
        public const int TYPE_ACTION = 3;
        public const int TYPE_MACRO = 4;
        public const int TYPE_RECIPE = 5;

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