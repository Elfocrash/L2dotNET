using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Models.Player.General;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestShortCutDel : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _slot;
        private readonly int _page;

        public RequestShortCutDel(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            int id = packet.ReadInt();
            _slot = id % 12;
            _page = id / 12;
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                L2Shortcut scx = player.Shortcuts.FirstOrDefault(sc => (sc.Slot == _slot) && (sc.Page == _page));

                if (scx == null)
                {
                    player.SendActionFailedAsync();
                    return;
                }

                lock (player.Shortcuts)
                    player.Shortcuts.Remove(scx);

                player.SendPacketAsync(new ShortCutInit(player));
            });
        }
    }
}