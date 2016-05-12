using L2dotNET.GameService.model.items.cursed;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
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
