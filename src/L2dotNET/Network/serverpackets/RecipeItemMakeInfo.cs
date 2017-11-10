using L2dotNET.model.player;
using L2dotNET.tables;

namespace L2dotNET.Network.serverpackets
{
    class RecipeItemMakeInfo : GameserverPacket
    {
        private readonly int _recipeId;
        private readonly int _type;
        private readonly int _currentMp;
        private readonly int _maxMp;
        private readonly int _makingResult;

        public RecipeItemMakeInfo(L2Player player, L2Recipe rec, int result)
        {
            _recipeId = rec.RecipeId;
            _type = rec.Iscommonrecipe;
            _currentMp = (int)player.CurMp;
            _maxMp = 100;// (int)player.CharacterStat.GetStat(EffectType.BMaxMp);
            _makingResult = result;
        }

        public override void Write()
        {
            WriteByte(0xdd);
            WriteInt(_recipeId);
            WriteInt(_type);
            WriteInt(_currentMp);
            WriteInt(_maxMp);
            WriteInt(_makingResult);
        }
    }
}