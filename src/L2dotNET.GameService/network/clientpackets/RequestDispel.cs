using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestDispel : GameServerNetworkRequest
    {
        private int _ownerId;
        private int _skillId;
        private int _skillLv;

        public RequestDispel(GameClient client, byte[] data)
        {
            Makeme(client, data, 2);
        }

        public override void Read()
        {
            _ownerId = ReadD();
            _skillId = ReadD();
            _skillLv = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            if (_ownerId != player.ObjId)
            {
                player.SendActionFailed();
                return;
            }

            AbnormalEffect avestop = null;
            foreach (AbnormalEffect ave in player.Effects)
            {
                if ((ave.Id != _skillId) && (ave.Lvl != _skillLv))
                    continue;

                if ((ave.Skill.Debuff == 1) && (ave.Skill.IsMagic > 1))
                    break;

                avestop = ave;
                break;
            }

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