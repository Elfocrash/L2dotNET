using System;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.network.l2recv
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

            player.clientPosX = _x;
            player.clientPosY = _y;
            player.clientPosZ = _z;
            player.clientHeading = _heading;

            player.validateVisibleObjects(_x, _y, true);
        }
    }
}