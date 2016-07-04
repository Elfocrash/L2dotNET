using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class NpcHtmlMessage : GameServerNetworkPacket
    {
        public string Htm;
        private readonly int _objId;
        private readonly int _itemId;

        public NpcHtmlMessage(L2Player player, string file, int objId)
        {
            Htm = HtmCache.Instance.GetHtmByFilename(file);
            _objId = objId;
            _itemId = 0;
        }

        public NpcHtmlMessage(L2Player player, string file, int objId, int itemId)
        {
            Htm = HtmCache.Instance.GetHtmByFilename(file);
            _objId = objId;
            _itemId = itemId;
        }

        public NpcHtmlMessage(L2Player player, string plain, int objId, bool isPlain)
        {
            Htm = "<html><body>" + plain + "</body></html>";
            _objId = objId;
            _itemId = 0;
        }

        protected internal override void Write()
        {
            WriteC(0x0f);
            WriteD(_objId);
            WriteS(Htm);
            WriteD(_itemId);
        }

        public void Replace(string p, object t)
        {
            Htm = Htm.Replace(p, t.ToString());
        }
    }
}