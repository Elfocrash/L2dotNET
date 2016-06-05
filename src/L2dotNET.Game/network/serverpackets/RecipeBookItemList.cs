using System.Collections.Generic;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.network.l2send
{
    class RecipeBookItemList : GameServerNetworkPacket
    {
        private readonly int _type;
        private readonly int _mp;
        private readonly List<L2Recipe> _book;
        public RecipeBookItemList(L2Player player, int type)
        {
            _type = type;
            _mp = (int)player.CharacterStat.getStat(model.skills2.TEffectType.b_max_mp);
            _book = new List<L2Recipe>();

            if (player._recipeBook != null)
            {
                foreach (L2Recipe rec in player._recipeBook)
                {
                    if (rec._iscommonrecipe == type)
                        _book.Add(rec);
                }
            }
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
                writeD(x); x++; //?
            }
        }
    }
}
