using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestMagicSkillUse : GameServerNetworkRequest
    {
        public RequestMagicSkillUse(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        private int _magicId;
        private bool _ctrlPressed;
        private bool _shiftPressed;

        public override void read()
        {
            _magicId = readD(); // Identifier of the used skill
            _ctrlPressed = readD() != 0; // True if it's a ForceAttack : Ctrl pressed
            _shiftPressed = readC() != 0; // True if Shift pressed
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            TSkill skill = player.getSkill(_magicId);

            if (skill == null)
            {
                player.SendMessage("no skill found");
                player.SendActionFailed();
                return;
            }

            bool muted = false;
            switch (skill.is_magic)
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

            player.castSkill(skill, _ctrlPressed, _shiftPressed);
        }
    }
}