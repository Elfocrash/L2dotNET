using L2dotNET.GameService.model.npcs;

namespace L2dotNET.GameService.model.quests.data
{
    class _0014_Where_abouts_of_the_Archaeologist : QuestOrigin
    {
        private const int trader_liesel = 31263;
        private const int explorer_ghost_a = 31538;

        private const int q_letter_to_explorer = 7253;

        public _0014_Where_abouts_of_the_Archaeologist()
        {
            questId = 14;
            questName = "Where abouts of the Archaeologist";
            startNpc = trader_liesel;
            talkNpcs = new int[] { startNpc, explorer_ghost_a };
            actItems = new int[] { q_letter_to_explorer };
        }

        public override void tryAccept(L2Player player, L2Citizen npc)
        {
            if (player.Level >= 74)
                player.ShowHtm("trader_liesel_q0014_0101.htm", npc);
            else
            {
                player.ShowHtm("trader_liesel_q0014_0103.htm", npc);
            }
        }

        public override void onAccept(L2Player player, L2Citizen npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("trader_liesel_q0014_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Citizen npc, int reply)
        {
            //todo
        }

        public override void onTalkToNpc(L2Player player, L2Citizen npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (npcId == trader_liesel)
            {
                switch (cond)
                {
                    case 1:
                        htmltext = "trader_liesel_q0014_0104.htm";
                        break;
                }
            }
            else if (npcId == explorer_ghost_a)
            {
                switch (cond)
                {
                    case 1:
                        if (player.hasItem(q_letter_to_explorer))
                            htmltext = "explorer_ghost_a_q0014_0101.htm";
                        break;
                }
            }

            player.ShowHtm(htmltext, npc);
        }

        public override void onEarnItem(L2Player player, int cond, int id)
        {
            //todo
        }
    }
}