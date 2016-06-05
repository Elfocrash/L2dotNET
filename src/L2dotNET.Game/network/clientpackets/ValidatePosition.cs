using System;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets
{
    class ValidatePosition : GameServerNetworkRequest
    {
        public ValidatePosition(GameClient client, byte[] data)
        {
            base.makeme(client, data);
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
            string prevReg = player.CurrentRegion;

            int realX = player.X;
            int realY = player.Y;
            int realZ = player.Z;

            int dx,
                dy,
                dz;
            double diffSq;

            dx = _x - realX;
            dy = _y - realY;
            dz = _z - realZ;
            diffSq = (dx * dx + dy * dy);

            if (diffSq < 360000)
            {
                if (SYNCTYPE == 1)
                {
                    if (!player.isMoving())
                    {
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
                    }
                    Console.WriteLine($"Current position: X:{player.X}, Y:{player.Y}, Z:{player.Z}"); //debug
                    player.BroadcastUserInfo();
                    return;
                }
                else if (diffSq > 250000 || Math.Abs(dz) > 200)
                {
                    if (Math.Abs(dz) > 200 && Math.Abs(dz) < 1500 && Math.Abs(_z - player.clientHeading) < 800)
                    {
                        player.X = realX;
                        player.Y = realY;
                        player.Z = _z;
                    }
                    else
                    {
                        player.sendPacket(new ValidateLocation(player));
                    }
                }
            }

            player.X = _x;
            player.Y = _y;
            player.Z = _z;
            player.Heading = _heading;
            Console.WriteLine($"Current position: X:{player.clientPosX}, Y:{player.clientPosY}, Z:{player.clientPosZ}"); //debug
            player.BroadcastUserInfo();
            //player.validateVisibleObjects(_x, _y, true);         
        }
    }
}