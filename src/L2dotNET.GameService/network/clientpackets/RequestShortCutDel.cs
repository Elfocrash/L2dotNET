using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Player.General;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestShortCutDel : GameServerNetworkRequest
    {
        public RequestShortCutDel(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _slot;
        private int _page;

        public override void Read()
        {
            int id = ReadD();
            _slot = id % 12;
            _page = id / 12;
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

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