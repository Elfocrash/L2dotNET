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
            Makeme(client, data);
        }

        private int _id;
        private int _level;
        private int _skillType;

        public override void Read()
        {
            _id = ReadD();
            _level = ReadD();
            _skillType = ReadD();
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            SortedList<int, AcquireSkill> seq = player.ActiveSkillTree;
            if (!seq.ContainsKey(_id))
            {
                player.SendActionFailed();
                return;
            }

            AcquireSkill skill = seq[_id];
            if (skill.Lv != _level)
            {
                player.SendActionFailed();
                return;
            }

            AcquireSkillInfo asi = new AcquireSkillInfo(_id, _level, skill.LvUpSp, _skillType);
            switch (_skillType)
            {
                case 0:
                case 1:
                {
                    if (skill.ItemId > 0)
                    {
                        asi.Reqs.Add(new[] { 4, skill.ItemId, skill.ItemCount, 0 });
                    }
                }
                    break;
            }

            player.SendPacket(asi);
        }
    }
}