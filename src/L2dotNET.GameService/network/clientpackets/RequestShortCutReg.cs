using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Player.General;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestShortCutReg : PacketBase
    {
        private int _type;
        private int _id;
        private int _slot;
        private int _page;
        private int _lvl;
        private int _characterType; // 1 - player, 2 - pet
        private readonly GameClient _client;

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