using System.Collections.Generic;
using System.Linq;
using L2dotNET.model.player;
using L2dotNET.tables;

namespace L2dotNET.Network.serverpackets
{
    class RecipeBookItemList : GameserverPacket
    {
        private readonly int _type;
        private readonly int _mp;
        private readonly List<L2Recipe> _book;

        public RecipeBookItemList(L2Player player, int type)
        {
            _type = type;
            _mp = 100;// (int)player.CharacterStat.GetStat(EffectType.BMaxMp);
            _book = new List<L2Recipe>();

            if (player.RecipeBook == null)
                return;

            foreach (L2Recipe rec in player.RecipeBook.Where(rec => rec.Iscommonrecipe == type))
                _book.Add(rec);
        }

        public override void Write()
        {
            WriteByte(0xdc);
            WriteInt(_type);
            WriteInt(_mp);

            WriteInt(_book.Count);

            int x = 0;
            foreach (L2Recipe rec in _book)
            {
                WriteInt(rec.RecipeId);
                WriteInt(x);
                x++; //?
            }
        }
    }
}