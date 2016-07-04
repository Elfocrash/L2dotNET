using System;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class ValidatePosition : GameServerNetworkRequest
    {
        public ValidatePosition(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        private const int SYNCTYPE = 1;

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
            //string prevReg = player.CurrentRegion;

            int realX = player.X;
            int realY = player.Y;
            //int realZ = player.Z;

            int dx = _x - realX;
            int dy = _y - realY;
            //int dz = _z - realZ;
            double diffSq = (dx * dx + dy * dy);

            if (diffSq < 360000)
            {
                if (!player.IsMoving())
                    if (diffSq < 2500)
                    {
                        player.X = realX;
                        player.Y = realY;
                        player.Z = _z;
                    }
                    else
                    {
                        player.X = _x;
                        player.Y = _y;
                        player.Z = _z;
                    }
                Console.WriteLine($"Current position: X:{player.X}, Y:{player.Y}, Z:{player.Z}"); //debug
                player.BroadcastUserInfo();
                return;
            }

            player.X = _x;
            player.Y = _y;
            player.Z = _z;
            player.Heading = _heading;
            Console.WriteLine($"Current position: X:{player.ClientPosX}, Y:{player.ClientPosY}, Z:{player.ClientPosZ}"); //debug
            player.BroadcastUserInfo();
            //player.validateVisibleObjects(_x, _y, true);
        }
    }
}