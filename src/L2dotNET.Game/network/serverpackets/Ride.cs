
namespace L2dotNET.GameService.network.l2send
{
    class Ride : GameServerNetworkPacket
    {
        private int id;
        private int bRide;
        private int rideType;
        private int npcID;
        private int x;
        private int y;
        private int z;
        public Ride(L2Player player, bool mount, int npc = 0)
        {
            id = player.ObjID;
            bRide = mount ? 1 : 0;
            npcID = npc + 1000000;
            x = player.X;
            y = player.Y;
            z = player.Z;

            switch (npc)
            {
                case 0: // dismount
                    rideType = 0;
                    break;
                case 12526: // Wind
                case 12527: // Star
                case 12528: // Twilight
                case 16038: // red strider of wind
                case 16039: // red strider of star
                case 16040: // red strider of dusk
                    rideType = 1;
                    break;
                case 12621: // Wyvern
                    rideType = 2;
                    break;
                case 16037: // Great Snow Wolf
                case 16041: // Fenrir Wolf
                case 16042: // White Fenrir Wolf
                    rideType = 3;
                    break;
                case 13130: // Light Purple Maned Horse
                case 13146: // Tawny-Maned Lion
                case 13147: // Steam Sledge
                    rideType = 4;
                    break;
            }

            player.MountType = rideType;
        }

        protected internal override void write()
        {
            writeC(0x86);
            writeD(id);
            writeD(bRide);
            writeD(rideType);
            writeD(npcID);
            //writeD(x);
            //writeD(y);
            //writeD(z);
        }
    }
}
