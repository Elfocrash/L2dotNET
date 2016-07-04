using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestMagicSkillUse : GameServerNetworkRequest
    {
        public RequestMagicSkillUse(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _magicId;
        private bool _ctrlPressed;
        private bool _shiftPressed;

        public override void Read()
        {
            _magicId = ReadD(); // Identifier of the used skill
            _ctrlPressed = ReadD() != 0; // True if it's a ForceAttack : Ctrl pressed
            _shiftPressed = ReadC() != 0; // True if Shift pressed
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

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