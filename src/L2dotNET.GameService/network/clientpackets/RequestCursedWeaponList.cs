using L2dotNET.GameService.Config;
using L2dotNET.GameService.Model.Items.Cursed;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestCursedWeaponList : PacketBase
    {
        private GameClient _client;
        public RequestCursedWeaponList(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
        }

        public override void RunImpl()
        {
            int[] ids = CursedWeapons.GetInstance().GetWeaponIds();

            _client.SendPacket(new ExCursedWeaponList(ids));
        }
    }
}