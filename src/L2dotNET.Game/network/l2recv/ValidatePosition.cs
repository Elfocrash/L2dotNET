using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.world;
using L2dotNET.Game.model.zones;

namespace L2dotNET.Game.network.l2recv
{
    class ValidatePosition : GameServerNetworkRequest
    {
        public ValidatePosition(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _x;
        private int _y;
        private int _z;
        private int _heading;
        private int _data;

        public override void read()
        {
            _x = readD();
            _y = readD();
            _z = readD();
            _heading = readD();
            _data = readD();
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;
         //   Console.WriteLine("VP: data " + _data);
            string prevReg = player.CurrentRegion;

            player.clientPosX = _x;
            player.clientPosY = _y;
            player.clientPosZ = _z;
            player.clientHeading = _heading;

            //todo checks

            double dx = _x - player.X;
            double dy = _y - player.Y;
            double diffSq = (dx * dx + dy * dy);
            byte flymode = 0;
            if (player.Transform != null)
                flymode = player.Transform.Template.MoveMode;

            if(flymode == 2)
            {
                if (diffSq > 90000) // validate packet, may also cause z bounce if close to land
                    player.sendPacket(new ValidateLocation(player.ObjID, _x, _y, _z, _heading));
                return;
            }

            player.X = _x;
            player.Y = _y;
            player.Z = _z;
            player.Heading = _heading;

            player.validateVisibleObjects(_x, _y, true);

            if (diffSq > 250000)
            {
                player.sendPacket(new ValidateLocation(player.ObjID, _x, _y, _z, _heading));
            }            
        }
    }
}
