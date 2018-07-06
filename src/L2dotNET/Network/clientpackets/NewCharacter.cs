using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.Network.serverpackets;
using L2dotNET.Tables;
using L2dotNET.Templates;

namespace L2dotNET.Network.clientpackets
{
    class NewCharacter : PacketBase
    {
        private readonly GameClient _client;

        public NewCharacter(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
        }

        public override async Task RunImpl()
        {
            await _client.SendPacketAsync(new CharTemplates(CharTemplateTable.GetTemplates()));
        }
    }
}