using L2dotNET.model.player;

namespace L2dotNET.AI.npcai
{
    class HennaManager : Citizen
    {
        public override void TalkedReply(L2Player talker, int ask, int reply)
        {
            if (ask != -16)
                return;

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