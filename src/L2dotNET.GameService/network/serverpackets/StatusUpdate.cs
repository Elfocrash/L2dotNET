using System.Collections.Generic;

namespace L2dotNET.GameService.Network.Serverpackets
{
    public class StatusUpdate : GameServerNetworkPacket
    {
        public static int Level = 0x01;
        public static int Exp = 0x02;
        public static int Str = 0x03;
        public static int Dex = 0x04;
        public static int Con = 0x05;
        public static int Int = 0x06;
        public static int Wit = 0x07;
        public static int Men = 0x08;

        public static int CurHp = 0x09;
        public static int MaxHp = 0x0a;
        public static int CurMp = 0x0b;
        public static int MaxMp = 0x0c;

        public static int Sp = 0x0d;
        public static int CurLoad = 0x0e;
        public static int MaxLoad = 0x0f;

        public static int PAtk = 0x11;
        public static int AtkSpd = 0x12;
        public static int PDef = 0x13;
        public static int Evasion = 0x14;
        public static int Accuracy = 0x15;
        public static int Critical = 0x16;
        public static int MAtk = 0x17;
        public static int CastSpd = 0x18;
        public static int MDef = 0x19;
        public static int PvpFlag = 0x1a;
        public static int Karma = 0x1b;

        public static int CurCp = 0x21;
        public static int MaxCp = 0x22;

        public List<object[]> Attrs = new List<object[]>();
        private readonly int _id;

        public void Add(int type, object val)
        {
            Attrs.Add(new[] { type, val });
        }

        public StatusUpdate(int id)
        {
            _id = id;
        }

        protected internal override void Write()
        {
            WriteC(0x0e);
            WriteD(_id);
            WriteD(Attrs.Count);

            foreach (object[] d in Attrs)
            {
                int type = (int)d[0];
                WriteD(type);
                //if(type == EXP)
                //    writeQ((long)d[1]);
                //else
                WriteD((int)d[1]);
            }
        }
    }
}