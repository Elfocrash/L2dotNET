using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestAcquireSkillInfo : GameServerNetworkRequest
    {
        public RequestAcquireSkillInfo(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        private int _id;
        private int _level;
        private int _skillType;

        public override void read()
        {
            _id = readD();
            _level = readD();
            _skillType = readD();
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            SortedList<int, TAcquireSkill> seq = player.ActiveSkillTree;
            if (!seq.ContainsKey(_id))
            {
                player.sendActionFailed();
                return;
            }

            TAcquireSkill skill = seq[_id];
            if (skill.lv != _level)
            {
                player.sendActionFailed();
                return;
            }

            AcquireSkillInfo asi = new AcquireSkillInfo(_id, _level, skill.lv_up_sp, _skillType);
            switch (_skillType)
            {
                case 0:
                case 1:
                {
                    if (skill.itemid > 0)
                        asi._reqs.Add(new[] { 4, skill.itemid, (int)skill.itemcount, 0 });
                }
                    break;
            }

            player.sendPacket(asi);
        }
    }
}