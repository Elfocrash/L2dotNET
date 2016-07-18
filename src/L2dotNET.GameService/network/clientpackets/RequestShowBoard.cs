using L2dotNET.GameService.Config;
using L2dotNET.GameService.Managers.BBS;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestShowBoard : PacketBase
    {
        private int _type;
        private readonly GameClient _client;

        public RequestShowBoard(Packet packet, GameClient client)
        {
            _client = client;
            _type = packet.ReadInt();
        }

        public override void RunImpl()
        {
            BbsManager.Instance.RequestShow(_client.CurrentPlayer, _type);
        }
    }
}