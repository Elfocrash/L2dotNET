using L2dotNET.GameService.Model.Items.Cursed;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{

        class RequestCursedWeaponLocation : PacketBase
        {
            private readonly GameClient _client;

            public RequestCursedWeaponLocation(Packet packet, GameClient client)
            {
                packet.MoveOffset(2);
                _client = client;
            }

            public override void RunImpl()
            {
              //Not implemented yet
            }
        }
}