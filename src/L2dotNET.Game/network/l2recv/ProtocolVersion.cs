using L2dotNET.Game.logger;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.network.l2recv
{
    class ProtocolVersion : GameServerNetworkRequest
    {
        public ProtocolVersion(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _protocol;
        public override void read()
        {
            _protocol = readD();
        }

        public override void run()
        {
            if (_protocol != 216 && _protocol != 251)
            {
                CLogger.error("protocol fail. "+_protocol);
                getClient().sendPacket(new KeyPacket(getClient(), 0));
                getClient().termination();
                return;
            }
            else if (_protocol == -1)
            {
                CLogger.extra_info("ping received " + _protocol);
                getClient().sendPacket(new KeyPacket(getClient(), 0));
                getClient().termination();
                return;
            }

            CLogger.info("accepted "+_protocol+" client");

            getClient().sendPacket(new KeyPacket(getClient(), 1));
            getClient().Protocol = _protocol;

  
        }
    }
}
