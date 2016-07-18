using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Config;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.Templates;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class NewCharacter : PacketBase
    {
        private GameClient _client;
        public NewCharacter(Packet packet, GameClient client)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            Dictionary<int, PcTemplate> dict = CharTemplateTable.Instance.Templates;
            List<PcTemplate> pcTemp = dict.Select((t, i) => dict.SingleOrDefault(x => x.Key == i).Value).ToList();

            _client.SendPacket(new CharTemplates(pcTemp));
        }
    }
}