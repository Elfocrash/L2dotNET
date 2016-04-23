using L2dotNET.Game.model.items.cursed;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.network.l2recv
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
