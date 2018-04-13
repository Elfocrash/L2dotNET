using L2dotNET.Models.Items.cursed;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestCursedWeaponList : PacketBase
    {
        private readonly GameClient _client;

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