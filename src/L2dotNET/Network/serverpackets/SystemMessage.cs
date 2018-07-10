using System.Collections.Generic;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models;
using L2dotNET.Models.Items;
using L2dotNET.Models.Npcs;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    public class SystemMessage : GameserverPacket
    {
        private readonly List<object[]> _data = new List<object[]>();
        public int MessgeId;

        public SystemMessage(SystemMessageId msgId)
        {
            MessgeId = (int)msgId;
        }

        public SystemMessage AddString(string val)
        {
            _data.Add(new object[] { 0, val });
            return this;
        }

        public SystemMessage AddNumber(int val)
        {
            _data.Add(new object[] { 1, val });
            return this;
        }

        public SystemMessage AddNumber(double val)
        {
            _data.Add(new object[] { 1, (int)val });
            return this;
        }

        public SystemMessage AddNpcName(int val)
        {
            _data.Add(new object[] { 2, 1000000 + val });
            return this;
        }

        public SystemMessage AddItemName(int val)
        {
            _data.Add(new object[] { 3, val });
            return this;
        }

        public SystemMessage AddSkillName(int val, int lvl)
        {
            _data.Add(new object[] { 4, val, lvl });
            return this;
        }

        public void AddCastleName(int val)
        {
            _data.Add(new object[] { 5, val });
        }

        public void AddItemCount(int val)
        {
            _data.Add(new object[] { 6, val });
        }

        public void AddZoneName(int val, int y, int z)
        {
            _data.Add(new object[] { 7, val, y, z });
        }

        public void AddElementName(int val)
        {
            _data.Add(new object[] { 9, val });
        }

        public void AddInstanceName(int val)
        {
            _data.Add(new object[] { 10, val });
        }

        public SystemMessage AddPlayerName(string val)
        {
            _data.Add(new object[] { 12, val });
            return this;
        }

        public SystemMessage AddName(L2Object obj)
        {
            if (obj is L2Player)
                return AddPlayerName(((L2Player)obj).Name);
            if (obj is L2Npc)
                return AddNpcName(((L2Npc)obj).NpcId);
            if (obj is L2Item)
                return AddItemName(((L2Item)obj).Template.ItemId);

            return AddString(obj.AsString());
        }

        public void AddSysStr(int val)
        {
            _data.Add(new object[] { 13, val });
        }

        public override void Write()
        {
            WriteByte(0x64);
            WriteInt(MessgeId);
            WriteInt(_data.Count);

            foreach (object[] d in _data)
            {
                int type = (int)d[0];

                WriteInt(type);

                switch (type)
                {
                    case 0: //text
                    case 12:
                        WriteString((string)d[1]);
                        break;
                    case 1: //number
                    case 2: //npcid
                    case 3: //itemid
                    case 5:
                    case 9:
                    case 10:
                    case 13:
                        WriteInt((int)d[1]);
                        break;
                    case 4: //skillname
                        WriteInt((int)d[1]);
                        WriteInt((int)d[2]);
                        break;
                    case 6:
                        WriteLong((long)d[1]);
                        break;
                    case 7: //zone
                        WriteInt((int)d[1]);
                        WriteInt((int)d[2]);
                        WriteInt((int)d[3]);
                        break;
                }
            }
        }

        
    }
}