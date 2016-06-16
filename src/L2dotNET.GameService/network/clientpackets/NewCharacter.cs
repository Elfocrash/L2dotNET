using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.Templates;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class NewCharacter : GameServerNetworkRequest
    {
        public NewCharacter(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        public override void read()
        {
            // do nothing
        }

        public override void run()
        {
            Dictionary<int, PcTemplate> dict = CharTemplateTable.Instance.Templates;
            List<PcTemplate> pcTemp = dict.Select((t, i) => dict.SingleOrDefault(x => x.Key == i).Value).ToList();

            Client.sendPacket(new CharTemplates(pcTemp));
        }
    }
}