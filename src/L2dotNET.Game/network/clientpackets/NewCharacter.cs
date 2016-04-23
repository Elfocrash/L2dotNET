using System.Collections.Generic;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;
using L2dotNET.Game.templates;
using System.Linq;

namespace L2dotNET.Game.network.l2recv
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
