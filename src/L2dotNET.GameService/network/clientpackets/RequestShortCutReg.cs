using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestShortCutReg : GameServerNetworkRequest
    {
        public RequestShortCutReg(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _type;
        private int _id;
        private int _slot;
        private int _page;
        private int _lvl;
        private int _characterType; // 1 - player, 2 - pet

        public override void Read()
        {
            _type = ReadD();
            int slot = ReadD();
            _id = ReadD();
            _lvl = ReadD();
            _characterType = ReadD();

            _slot = slot % 12;
            _page = slot / 12;
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            if ((_page > 10) || (_page < 0))
            {
                player.SendActionFailed();
                return;
            }

            switch (_type)
            {
                case L2Shortcut.TypeItem:
                case L2Shortcut.TypeSkill:
                case L2Shortcut.TypeAction:
                case L2Shortcut.TypeMacro:
                case L2Shortcut.TypeRecipe:
                    player.RegisterShortcut(_slot, _page, _type, _id, _lvl, _characterType);
                    break;
            }
        }
    }
}