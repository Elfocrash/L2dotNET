using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.network.serverpackets;
using L2dotNET.GameService.tables;
using L2dotNET.GameService.Templates;

namespace L2dotNET.GameService.network.clientpackets
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
            Dictionary<int, PcTemplate> dict = CharTemplateTable.Instance.Templates;
            for (int i = 0; i < dict.Count; i++)
                pcTemp.Add(dict.SingleOrDefault(x => x.Key == i).Value);

            Client.sendPacket(new CharTemplates(pcTemp));
        }
    }
}