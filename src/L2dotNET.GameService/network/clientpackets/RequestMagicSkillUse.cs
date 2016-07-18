using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestMagicSkillUse : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _magicId;
        private readonly bool _ctrlPressed;
        private readonly bool _shiftPressed;

        public RequestMagicSkillUse(Packet packet, GameClient client)
        {
            _client = client;
            _magicId = packet.ReadInt(); // Identifier of the used skill
            _ctrlPressed = packet.ReadInt() != 0; // True if it's a ForceAttack : Ctrl pressed
            _shiftPressed = packet.ReadByte() != 0; // True if Shift pressed
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            Skill skill = player.GetSkill(_magicId);

            if (skill == null)
            {
                player.SendMessage("no skill found");
                player.SendActionFailed();
                return;
            }

            bool muted = false;
            switch (skill.IsMagic)
            {
                case 0:
                    muted = player.PBlockSkill == 1;
                    break;
                case 1:
                    muted = player.PBlockSpell == 1;
                    break;
            }

            if (muted)
            {
                player.SendActionFailed();
                return;
            }

            player.CastSkill(skill, _ctrlPressed, _shiftPressed);
        }
    }
}