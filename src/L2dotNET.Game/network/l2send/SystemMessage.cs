using System.Collections.Generic;
using L2dotNET.Game.world;
using L2dotNET.Game.model.npcs;
using L2dotNET.Game.model.playable;
using L2dotNET.Game.model.items;

namespace L2dotNET.Game.network.l2send
{
    class SystemMessage : GameServerNetworkPacket
    {
        private List<object[]> data = new List<object[]>();
        public int MessgeID;

        public SystemMessage(int msgId)
        {
            MessgeID = msgId;
        }

        public SystemMessage addString(string val)
        {
            data.Add(new object[] { 0, val });
            return this;
        }

        public SystemMessage addNumber(int val)
        {
            data.Add(new object[] { 1, val });
            return this;
        }

        public SystemMessage addNumber(double val)
        {
            data.Add(new object[] { 1, (int)val });
            return this;
        }

        public SystemMessage addNpcName(int val)
        {
            data.Add(new object[] { 2, (1000000 + val) });
            return this;
        }

        public SystemMessage addItemName(int val)
        {
            data.Add(new object[] { 3, val });
            return this;
        }

        public SystemMessage addSkillName(int val, int lvl)
        {
            data.Add(new object[] { 4, val, lvl });
            return this;
        }

        public void addCastleName(int val)
        {
            data.Add(new object[] { 5, val });
        }

        public void addItemCount(long val)
        {
            data.Add(new object[] { 6, val });
        }

        public void addZoneName(int val, int y, int z)
        {
            data.Add(new object[] { 7, val, y, z });
        }

        public void addElementName(int val)
        {
            data.Add(new object[] { 9, val });
        }

        public void addInstanceName(int val)
        {
            data.Add(new object[] { 10, val });
        }

        public SystemMessage addPlayerName(string val)
        {
            data.Add(new object[] { 12, val });
            return this;
        }

        public SystemMessage addName(L2Object obj)
        {
            if (obj is L2Player)
                return addPlayerName(((L2Player)obj).Name);
            else if (obj is L2Citizen)
                return addNpcName(((L2Citizen)obj).NpcId);
            else if (obj is L2Summon)
                return addNpcName(((L2Summon)obj).NpcId);
            else if (obj is L2Item)
                return addItemName(((L2Item)obj).Template.ItemID);
            else
                return addString(obj.asString());
        }

        public void addSysStr(int val)
        {
            data.Add(new object[] { 13, val });
        }

        protected internal override void write()
        {
            writeC(0x62);
            writeD(MessgeID);
            writeD(data.Count);

            foreach (object[] d in data)
            {
                int type = (int)d[0];

                writeD(type);

                switch (type)
                {
                    case 0: //text
                    case 12:
                        writeS((string)d[1]);
                        break;
                    case 1: //number
                    case 2: //npcid
                    case 3: //itemid
                    case 5:
                    case 9:
                    case 10:
                    case 13:
                        writeD((int)d[1]);
                        break;
                    case 4: //skillname
                        writeD((int)d[1]);
                        writeD((int)d[2]);
                        break;
                    case 6:
                        writeQ((long)d[1]);
                        break;
                    case 7: //zone
                        writeD((int)d[1]);
                        writeD((int)d[2]);
                        writeD((int)d[3]);
                        break;
                }
            }
        }
    }
}
