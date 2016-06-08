using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class RecipeBookItemList : GameServerNetworkPacket
    {
        private readonly int _type;
        private readonly int _mp;
        private readonly List<L2Recipe> _book;

        public RecipeBookItemList(L2Player player, int type)
        {
            _type = type;
            _mp = (int)player.CharacterStat.getStat(TEffectType.b_max_mp);
            _book = new List<L2Recipe>();

            if (player._recipeBook != null)
                foreach (L2Recipe rec in player._recipeBook.Where(rec => rec._iscommonrecipe == type))
                    _book.Add(rec);
        }

        protected internal override void write()
        {
            writeC(0xdc);
            writeD(_type);
            writeD(_mp);

            writeD(_book.Count);

            int x = 0;
            foreach (L2Recipe rec in _book)
            {
                writeD(rec.RecipeID);
                writeD(x);
                x++; //?
            }
        }
    }
}