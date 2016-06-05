using L2dotNET.GameService.Model.items.cursed;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets
{
    class RequestCursedWeaponList : GameServerNetworkRequest
    {
        public RequestCursedWeaponList(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            // nothing
        }

        public override void run()
        {
            int[] ids = CursedWeapons.getInstance().getWeaponIds();

            Client.sendPacket(new ExCursedWeaponList(ids));
        }
    }
}