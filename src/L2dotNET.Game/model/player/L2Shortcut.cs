
namespace L2dotNET.Game.model.player
{
    public class L2Shortcut
    {
        public int _slot;
        public int _page;
        public int _type;
        public int _id;
        public int _level;
        public int _characterType;

        public const int TYPE_ITEM = 1;
        public const int TYPE_SKILL = 2;
        public const int TYPE_ACTION = 3;
        public const int TYPE_MACRO = 4;
        public const int TYPE_RECIPE = 5;
        public const int TYPE_TELMARK = 6;
    }
}
