using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class AcquireSkillList : GameServerNetworkPacket
    {
        private readonly int _type;
        public static int EsttNormal = 0;
        public static int EsttFishing = 1;
        public static int EsttClan = 2;
        public static int EsttSubClan = 3;
        public static int EsttTransform = 4;
        public static int EsttSubjob = 5; // CT1.5
        public static int EsttCollect = 6; // CT2 Final
        public static int EsttBishopSharing = 7; // CT2.5 Skill Sharing
        public static int EsttElderSharing = 8; // CT2.5 Skill Sharing
        public static int EsttSilenElderSharing = 9; // CT2.5 Skill Sharing
        private readonly SortedList<int, AcquireSkill> _list;

        public AcquireSkillList(int type, L2Player player)
        {
            _list = player.ActiveSkillTree;
            _type = type;
        }

        protected internal override void Write()
        {
            WriteC(0x8a);
            WriteD(_type);
            WriteD(_list.Count);

            foreach (AcquireSkill sk in _list.Values)
            {
                WriteD(sk.Id);
                WriteD(sk.Lv);
                WriteD(sk.Lv);
                WriteD(sk.LvUpSp);
                WriteD(0); //reqs
            }
        }
    }
}