using System;
using System.Threading.Tasks;

namespace L2dotNET.Network.clientpackets
{
    //TODO: Code
    class RequestSkillCoolTime : PacketBase
    {
        private readonly GameClient _client;

        public RequestSkillCoolTime(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
        }

        public override async Task RunImpl()
        {
            await Task.FromResult(1);
        }
    }
}