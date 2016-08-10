using System.Linq;
using L2dotNET.model.player;
using L2dotNET.model.player.General;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestShortCutDel : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _slot;
        private readonly int _page;

        public RequestShortCutDel(Packet packet, GameClient client)
        {
            _client = client;
            int id = packet.ReadInt();
            _slot = id % 12;
            _page = id / 12;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            L2Shortcut scx = player.Shortcuts.FirstOrDefault(sc => (sc.Slot == _slot) && (sc.Page == _page));

            if (scx == null)
            {
                player.SendActionFailed();
                return;
            }

            lock (player.Shortcuts)
                player.Shortcuts.Remove(scx);

            player.SendPacket(new ShortCutInit(player));
        }
    }
}