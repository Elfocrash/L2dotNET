using System.Collections.Generic;
using L2dotNET.model.player;
using L2dotNET.model.skills2;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets
{
    class RequestAcquireSkillInfo : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _id;
        private readonly int _level;
        private readonly int _skillType;

        public RequestAcquireSkillInfo(Packet packet, GameClient client)
        {
            _client = client;
            _id = packet.ReadInt();
            _level = packet.ReadInt();
            _skillType = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

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
                        asi.Reqs.Add(new[] { 4, skill.ItemId, skill.ItemCount, 0 });
                }
                    break;
            }

            player.SendPacket(asi);
        }
    }
}