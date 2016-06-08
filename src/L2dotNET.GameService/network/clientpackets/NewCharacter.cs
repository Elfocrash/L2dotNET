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
            List<PcTemplate> pcTemp = new List<PcTemplate>();
            Dictionary<int, PcTemplate> dict = CharTemplateTable.Instance.Templates;
            for (int i = 0; i < dict.Count; i++)
                pcTemp.Add(dict.SingleOrDefault(x => x.Key == i).Value);

            Client.sendPacket(new CharTemplates(pcTemp));
        }
    }
}