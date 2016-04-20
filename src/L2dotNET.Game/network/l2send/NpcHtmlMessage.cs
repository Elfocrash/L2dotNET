using L2dotNET.Game.tables;

namespace L2dotNET.Game.network.l2send
{
    class NpcHtmlMessage : GameServerNetworkPacket
    {
        public string _htm; int _objId, _itemId;
        public NpcHtmlMessage(L2Player player, string file, int objId)
        {
            _htm = HtmCache.getInstance().getHtm(player._locale, file);
            _objId = objId;
            _itemId = 0;
        }

        public NpcHtmlMessage(L2Player player, string file, int objId, int itemId)
        {
            _htm = HtmCache.getInstance().getHtm(player._locale, file);
            _objId = objId;
            _itemId = itemId;
        }

        public NpcHtmlMessage(L2Player player, string plain, int objId, bool isPlain)
        {
            _htm = "<html><body>"+plain+"</body></html>";
            _objId = objId;
            _itemId = 0;
        }

        protected internal override void write()
        {
            writeC(0x0f);
            writeD(_objId);
            writeS(_htm);
            writeD(_itemId);
        }

        public void replace(string p, object t)
        {
            _htm = _htm.Replace(p, t.ToString());
        }
    }
}
