
namespace L2dotNET.Game.ai.npcai
{
    class henna_manager : citizen
    {
        public override void TalkedReply(L2Player talker, int ask, int reply)
        {
            if (ask == -16)
            {
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
        }

        private void OpenHennaListForUnquip(L2Player talker)
        {
            talker.sendMessage("henna_manager.OpenHennaListForUnquip");
        }

        private void OpenHennaItemListForEquip(L2Player talker)
        {
            talker.sendMessage("henna_manager.OpenHennaItemListForEquip");
        }
    }
}
