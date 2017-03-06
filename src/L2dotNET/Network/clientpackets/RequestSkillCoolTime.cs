namespace L2dotNET.Network.clientpackets
{
    //TODO: Code
    class RequestSkillCoolTime : PacketBase
    {
        private readonly GameClient _client;

        public RequestSkillCoolTime(Packet packet, GameClient client)
        {
            _client = client;
        }

        public override void RunImpl()
        {

        }
    }
}