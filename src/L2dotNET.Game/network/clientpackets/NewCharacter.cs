using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tables;
using L2dotNET.GameService.templates;

namespace L2dotNET.GameService.network.l2recv
{
    class NewCharacter : GameServerNetworkRequest
    {
        public NewCharacter(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            // do nothing
        }

        public override void run()
        {
            List<PcTemplate> pcTemp = new List<PcTemplate>();
            Dictionary < int, PcTemplate > dict = CharTemplateTable.Instance.Templates;
            for (int i = 0; i < dict.Count; i++)
                pcTemp.Add(dict.SingleOrDefault(x => x.Key == i).Value);

            Client.sendPacket(new CharTemplates(pcTemp));
        }
    }
}
