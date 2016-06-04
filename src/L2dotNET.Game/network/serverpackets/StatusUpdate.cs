using System.Collections.Generic;

namespace L2dotNET.GameService.network.l2send
{
    public class StatusUpdate : GameServerNetworkPacket
    {
        public static int LEVEL = 0x01;
        public static int EXP = 0x02;
        public static int STR = 0x03;
        public static int DEX = 0x04;
        public static int CON = 0x05;
        public static int INT = 0x06;
        public static int WIT = 0x07;
        public static int MEN = 0x08;

        public static int CUR_HP = 0x09;
        public static int MAX_HP = 0x0a;
        public static int CUR_MP = 0x0b;
        public static int MAX_MP = 0x0c;

        public static int SP = 0x0d;
        public static int CUR_LOAD = 0x0e;
        public static int MAX_LOAD = 0x0f;

        public static int P_ATK = 0x11;
        public static int ATK_SPD = 0x12;
        public static int P_DEF = 0x13;
        public static int EVASION = 0x14;
        public static int ACCURACY = 0x15;
        public static int CRITICAL = 0x16;
        public static int M_ATK = 0x17;
        public static int CAST_SPD = 0x18;
        public static int M_DEF = 0x19;
        public static int PVP_FLAG = 0x1a;
        public static int KARMA = 0x1b;

        public static int CUR_CP = 0x21;
        public static int MAX_CP = 0x22;

        public List<object[]> attrs = new List<object[]>();
        private int _id;

        public void add(int type, object val)
        {
            attrs.Add(new object[] { type, val });
        }

        public StatusUpdate(int id)
        {
            _id = id;
        }

        protected internal override void write()
        {
            writeC(0x0e);
            writeD(_id);
            writeD(attrs.Count);

            foreach (object[] d in attrs)
            {
                int type = (int)d[0];
                writeD(type);
                //if(type == EXP)
                //    writeQ((long)d[1]);
                //else
                writeD((int)d[1]);
            }
        }
    }
}
