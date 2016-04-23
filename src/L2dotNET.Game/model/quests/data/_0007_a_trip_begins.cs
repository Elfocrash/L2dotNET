using L2dotNET.Game.Enums;
using L2dotNET.Game.model.npcs;
using L2dotNET.Game.model.player.basic;

namespace L2dotNET.Game.model.quests.data
{
    class _0007_a_trip_begins : QuestOrigin
    {
        const int mint = 30146;
        const int ariel = 30148;
        const int ozzy = 30154;

        const int q_recommendation_elf = 7572;
        const int escape_scroll_giran = 7126;
        const int q_symbol_of_traveler = 7570;

        public _0007_a_trip_begins()
        {
            questId = 7;
            questName = "A Trip Begins";
            startNpc = mint;
            talkNpcs = new int[] { startNpc, ariel, ozzy };
            actItems = new int[] { q_recommendation_elf };
        }

        public override void tryAccept(L2Player player, L2Citizen npc)
        {
            if (player.BaseClass.ClassId.ClassRace == ClassRace.ELF && player.Level >= 3)
                player.ShowHtm("mint_q0007_0101.htm", npc, questId);
            else
            {
                player.ShowHtm("mint_q0007_0102.htm", npc);
            }
        }

        public override void onAccept(L2Player player, L2Citizen npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("mint_q0007_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Citizen npc, int reply)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (reply == 1 && npcId == ariel)
            {
                player.addItem(q_recommendation_elf, 1);
                player.changeQuestStage(questId, 2);
                htmltext = "ariel_q0007_0201.htm";
            }
            else if (reply == 1 && npcId == ozzy)
            {
                if (player.hasItem(q_recommendation_elf))
                {
                    htmltext = "ozzy_q0007_0301.htm";
                    player.takeItem(q_recommendation_elf, 1);
                    player.changeQuestStage(questId, 3);
                }
            }
            else if (reply == 3 && npcId == mint)
            {
                htmltext = "mint_q0007_0401.htm";
                player.addItem(escape_scroll_giran, 1);
                player.addItem(q_symbol_of_traveler, 1);
                player.finishQuest(questId);
            }

            player.ShowHtm(htmltext, npc);
        }

        public override void onTalkToNpc(L2Player player, L2Citizen npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (npcId == mint)
            {
                switch (cond)
                {
                    case 1:
                        htmltext = "mint_q0007_0105.htm";
                        break;
                    case 3:
                        htmltext = "mint_q0007_0301.htm";
                        break;
                }
            }
            else if (npcId == ariel)
            {
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
            }
            else if (npcId == ozzy)
            {
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
            }

            player.ShowHtm(htmltext, npc);
        }
    }
}

