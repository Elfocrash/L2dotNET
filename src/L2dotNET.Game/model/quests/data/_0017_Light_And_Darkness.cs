using L2dotNET.GameService.model.npcs;

namespace L2dotNET.GameService.model.quests.data
{
    class _0017_Light_And_Darkness : QuestOrigin
    {
        private const int dark_presbyter = 31517;
        private const int blessed_altar1 = 31508;
        private const int blessed_altar2 = 31509;
        private const int blessed_altar3 = 31510;
        private const int blessed_altar4 = 31511;

        private const int q_blood_of_saint = 7168;

        public _0017_Light_And_Darkness()
        {
            questId = 17;
            questName = "Light And Darkness";
            startNpc = dark_presbyter;
            talkNpcs = new int[] { startNpc, blessed_altar1, blessed_altar2, blessed_altar3, blessed_altar4 };
            actItems = new int[] { q_blood_of_saint };
        }

        public override void tryAccept(L2Player player, L2Citizen npc)
        {
            if (player.Level >= 61)
                player.ShowHtm("dark_presbyter_q0017_01.htm", npc);
            else
            {
                player.ShowHtm("dark_presbyter_q0017_03.htm", npc);
            }
        }

        public override void onAccept(L2Player player, L2Citizen npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("dark_presbyter_q0017_04.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Citizen npc, int reply)
        {
            //todo
        }

        public override void onTalkToNpc(L2Player player, L2Citizen npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (npcId == dark_presbyter)
            {
                if (cond > 0 && cond < 5 && player.hasItem(q_blood_of_saint))
                    htmltext = "dark_presbyter_q0017_05.htm";
                else if (cond > 0 && cond < 5 && !player.hasItem(q_blood_of_saint))
                {
                    htmltext = "dark_presbyter_q0017_06.htm";
                    player.changeQuestStage(questId, 0);
                }

            }
            else if (npcId == blessed_altar1)
            {
                switch (cond)
                {
                    case 1:
                        if (player.hasItem(q_blood_of_saint))
                            htmltext = "blessed_altar1_q0017_01.htm";
                        else
                            htmltext = "blessed_altar1_q0017_03.htm";
                        break;
                    case 2:
                        htmltext = "blessed_altar1_q0017_05.htm";
                        break;
                }
            }
            else if (npcId == blessed_altar2)
            {
                switch (cond)
                {
                    case 2:
                        if (player.hasItem(q_blood_of_saint))
                            htmltext = "blessed_altar2_q0017_01.htm";
                        else
                            htmltext = "blessed_altar2_q0017_03.htm";
                        break;
                    case 3:
                        htmltext = "blessed_altar2_q0017_05.htm";
                        break;
                }
            }
            else if (npcId == blessed_altar3)
            {
                switch (cond)
                {
                    case 3:
                        if (player.hasItem(q_blood_of_saint))
                            htmltext = "blessed_altar3_q0017_01.htm";
                        else
                            htmltext = "blessed_altar3_q0017_03.htm";
                        break;
                    case 4:
                        htmltext = "blessed_altar3_q0017_05.htm";
                        break;
                }
            }
            else if (npcId == blessed_altar4)
            {
                switch (cond)
                {
                    case 4:
                        if (player.hasItem(q_blood_of_saint))
                            htmltext = "blessed_altar4_q0017_01.htm";
                        else
                            htmltext = "blessed_altar4_q0017_03.htm";
                        break;
                    case 5:
                        htmltext = "blessed_altar4_q0017_05.htm";
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

