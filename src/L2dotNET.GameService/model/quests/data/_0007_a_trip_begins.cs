using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Quests.Data
{
    class _0007_a_trip_begins : QuestOrigin
    {
        private const int mint = 30146;
        private const int ariel = 30148;
        private const int ozzy = 30154;

        private const int q_recommendation_elf = 7572;
        private const int escape_scroll_giran = 7126;
        private const int q_symbol_of_traveler = 7570;

        public _0007_a_trip_begins()
        {
            questId = 7;
            questName = "A Trip Begins";
            startNpc = mint;
            talkNpcs = new[] { startNpc, ariel, ozzy };
            actItems = new[] { q_recommendation_elf };
        }

        public override void tryAccept(L2Player player, L2Npc npc)
        {
            if ((player.BaseClass.ClassId.ClassRace == ClassRace.ELF) && (player.Level >= 3))
                player.ShowHtm("mint_q0007_0101.htm", npc, questId);
            else
                player.ShowHtm("mint_q0007_0102.htm", npc);
        }

        public override void onAccept(L2Player player, L2Npc npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("mint_q0007_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Npc npc, int reply)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if ((reply == 1) && (npcId == ariel))
            {
                player.addItem(q_recommendation_elf, 1);
                player.changeQuestStage(questId, 2);
                htmltext = "ariel_q0007_0201.htm";
            }
            else if ((reply == 1) && (npcId == ozzy))
            {
                if (player.hasItem(q_recommendation_elf))
                {
                    htmltext = "ozzy_q0007_0301.htm";
                    player.takeItem(q_recommendation_elf, 1);
                    player.changeQuestStage(questId, 3);
                }
            }
            else if ((reply == 3) && (npcId == mint))
            {
                htmltext = "mint_q0007_0401.htm";
                player.addItem(escape_scroll_giran, 1);
                player.addItem(q_symbol_of_traveler, 1);
                player.finishQuest(questId);
            }

            player.ShowHtm(htmltext, npc);
        }

        public override void onTalkToNpc(L2Player player, L2Npc npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            switch (npcId)
            {
                case mint:
                    switch (cond)
                    {
                        case 1:
                            htmltext = "mint_q0007_0105.htm";
                            break;
                        case 3:
                            htmltext = "mint_q0007_0301.htm";
                            break;
                    }

                    break;
                case ariel:
                    switch (cond)
                    {
                        case 1:
                            if (!player.hasItem(q_recommendation_elf))
                                htmltext = "ariel_q0007_0101.htm";
                            break;
                        case 2:
                            htmltext = "ariel_q0007_0202.htm";
                            break;
                    }

                    break;
                case ozzy:
                    switch (cond)
                    {
                        case 2:
                            if (player.hasItem(q_recommendation_elf))
                                htmltext = "ozzy_q0007_0201.htm";
                            else
                                htmltext = "ozzy_q0007_0302.htm";
                            break;
                        case 3:
                            htmltext = "ozzy_q0007_0303.htm";
                            break;
                    }

                    break;
            }

            player.ShowHtm(htmltext, npc);
        }
    }
}