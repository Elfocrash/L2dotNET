using System.Collections.Generic;
using L2dotNET.GameService.model.skills2;

namespace L2dotNET.GameService.network.l2send
{
    class AcquireSkillList : GameServerNetworkPacket
    {
        private readonly int _type;
        public static int ESTT_NORMAL = 0;
        public static int ESTT_FISHING = 1;
        public static int ESTT_CLAN = 2;
        public static int ESTT_SUB_CLAN = 3;
        public static int ESTT_TRANSFORM = 4;
        public static int ESTT_SUBJOB = 5;		// CT1.5
        public static int ESTT_COLLECT = 6;		// CT2 Final
        public static int ESTT_BISHOP_SHARING = 7;		// CT2.5 Skill Sharing	
        public static int ESTT_ELDER_SHARING = 8;		// CT2.5 Skill Sharing	
        public static int ESTT_SILEN_ELDER_SHARING = 9;			// CT2.5 Skill Sharing
        private readonly SortedList<int, TAcquireSkill> _list;
        public AcquireSkillList(int type, L2Player player)
        {
            _list = player.ActiveSkillTree;
            _type = type;
        }

        protected internal override void write()
        {
            writeC(0x8a);
            writeD(_type);
            writeD(_list.Count);

            foreach (TAcquireSkill sk in _list.Values)
            {
                writeD(sk.id);
                writeD(sk.lv);
                writeD(sk.lv);
                writeD(sk.lv_up_sp);
                writeD(0); //reqs
            }
        }
    }
}
