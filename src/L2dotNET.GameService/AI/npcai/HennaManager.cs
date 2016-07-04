using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.AI.NpcAI
{
    internal class HennaManager : Citizen
    {
        public override void TalkedReply(L2Player talker, int ask, int reply)
        {
            if (ask == -16)
                switch (reply)
                {
                    case 1:
                        OpenHennaItemListForEquip(talker);
                        break;
                    case 2:
                        OpenHennaListForUnquip(talker);
                        break;
                }
        }

        private void OpenHennaListForUnquip(L2Player talker)
        {
            talker.SendMessage("henna_manager.OpenHennaListForUnquip");
        }

        private void OpenHennaItemListForEquip(L2Player talker)
        {
            talker.SendMessage("henna_manager.OpenHennaItemListForEquip");
        }
    }
}