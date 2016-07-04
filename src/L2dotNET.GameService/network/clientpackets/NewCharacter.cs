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
            Makeme(client, data);
        }

        public override void Read()
        {
            // do nothing
        }

        public override void Run()
        {
            Dictionary<int, PcTemplate> dict = CharTemplateTable.Instance.Templates;
            List<PcTemplate> pcTemp = dict.Select((t, i) => dict.SingleOrDefault(x => x.Key == i).Value).ToList();

            Client.SendPacket(new CharTemplates(pcTemp));
        }
    }
}