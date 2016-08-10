using System.Linq;
using L2dotNET.model.player;
using L2dotNET.model.skills;

namespace L2dotNET.Network.clientpackets
{
    class RequestDispel : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _ownerId;
        private readonly int _skillId;
        private readonly int _skillLv;

        public RequestDispel(Packet packet, GameClient client)
        {
            packet.MoveOffset(2);
            _client = client;
            _ownerId = packet.ReadInt();
            _skillId = packet.ReadInt();
            _skillLv = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (_ownerId != player.ObjId)
            {
                player.SendActionFailed();
                return;
            }

            AbnormalEffect avestop = player.Effects.Where(ave => (ave.Id == _skillId) || (ave.Lvl == _skillLv)).TakeWhile(ave => (ave.Skill.Debuff != 1) || (ave.Skill.IsMagic <= 1)).FirstOrDefault();

            if (avestop == null)
            {
                player.SendActionFailed();
                return;
            }

            lock (player.Effects)
            {
                avestop.ForcedStop(true, true);
                player.Effects.Remove(avestop);
            }
        }
    }
}