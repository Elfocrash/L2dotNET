using L2dotNET.model.player;
using L2dotNET.model.player.General;

namespace L2dotNET.Network.clientpackets
{
    class RequestShortCutReg : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _type;
        private readonly int _id;
        private readonly int _slot;
        private readonly int _page;
        private readonly int _lvl;
        private readonly int _characterType; // 1 - player, 2 - pet

        public RequestShortCutReg(Packet packet, GameClient client)
        {
            _client = client;
            _type = packet.ReadInt();
            int slot = packet.ReadInt();
            _id = packet.ReadInt();
            _lvl = packet.ReadInt();
            _characterType = packet.ReadInt();

            _slot = slot % 12;
            _page = slot / 12;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

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