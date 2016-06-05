using L2dotNET.GameService.model.player;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestShortCutReg : GameServerNetworkRequest
    {
        public RequestShortCutReg(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _type;
        private int _id;
        private int _slot;
        private int _page;
        private int _lvl;
        private int _characterType; // 1 - player, 2 - pet

        public override void read()
        {
            _type = readD();
            int slot = readD();
            _id = readD();
            _lvl = readD();
            _characterType = readD();

            _slot = slot % 12;
            _page = slot / 12;
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (_page > 10 || _page < 0)
            {
                player.sendActionFailed();
                return;
            }

            switch (_type)
            {
                case L2Shortcut.TYPE_ITEM:
                case L2Shortcut.TYPE_SKILL:
                case L2Shortcut.TYPE_ACTION:
                case L2Shortcut.TYPE_MACRO:
                case L2Shortcut.TYPE_RECIPE:
                    player.registerShortcut(_slot, _page, _type, _id, _lvl, _characterType);
                    break;
            }
        }
    }
}