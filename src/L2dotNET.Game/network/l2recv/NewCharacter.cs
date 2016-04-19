using System.Collections.Generic;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.staticf;

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
            Client.sendPacket(new CharTemplates(ClassIdContainer.basics));
        }
    }
}
