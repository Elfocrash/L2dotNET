using L2dotNET.GameService.Model.Items.Cursed;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
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