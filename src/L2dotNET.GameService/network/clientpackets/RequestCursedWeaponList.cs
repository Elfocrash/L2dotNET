using L2dotNET.GameService.Model.Items.Cursed;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestCursedWeaponList : GameServerNetworkRequest
    {
        public RequestCursedWeaponList(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            // nothing
        }

        public override void Run()
        {
            int[] ids = CursedWeapons.GetInstance().GetWeaponIds();

            Client.SendPacket(new ExCursedWeaponList(ids));
        }
    }
}