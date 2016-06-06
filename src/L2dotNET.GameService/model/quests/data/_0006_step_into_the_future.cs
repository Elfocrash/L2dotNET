using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Quests.Data
{
    class _0006_step_into_the_future : QuestOrigin
    {
        private const int rapunzel = 30006;
        private const int baul = 30033;
        private const int sir_collin_windawood = 30311;

        private const int q_letter_paulo = 7571;
        private const int escape_scroll_giran = 7126;
        private const int q_symbol_of_traveler = 7570;

        public _0006_step_into_the_future()
        {
            questId = 6;
            questName = "Step into the Future";
            startNpc = rapunzel;
            talkNpcs = new int[] { startNpc, baul, sir_collin_windawood };
            actItems = new int[] { q_letter_paulo };
        }

        public override void tryAccept(L2Player player, L2Npc npc)
        {
            if (player.BaseClass.ClassId.ClassRace == ClassRace.HUMAN && player.Level >= 3)
                player.ShowHtm("rapunzel_q0006_0101.htm", npc, questId);
            else
            {
                player.ShowHtm("rapunzel_q0006_0102.htm", npc);
            }
        }

        public override void onAccept(L2Player player, L2Npc npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("rapunzel_q0006_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Npc npc, int reply)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (reply == 1 && npcId == baul)
            {
                player.addItem(q_letter_paulo, 1);
                htmltext = "baul_q0006_0201.htm";
                player.changeQuestStage(questId, 2);
            }
            else if (reply == 1 && npcId == sir_collin_windawood)
            {
                if (player.hasItem(q_letter_paulo))
                {
                    htmltext = "sir_collin_windawood_q0006_0301.htm";
                    player.takeItem(q_letter_paulo, 1);
                    player.changeQuestStage(questId, 3);
                }
                else
                    htmltext = "sir_collin_windawood_q0006_0302.htm";
            }
            else if (reply == 3 && npcId == rapunzel)
            {
                htmltext = "rapunzel_q0006_0401.htm";
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
            switch (npcId) {
                case rapunzel:
                    if (cond == 1)
                        htmltext = "rapunzel_q0006_0105.htm";
                    else if (cond == 3)
                        htmltext = "rapunzel_q0006_0301.htm";
                    break;
                case baul:
                    if (cond == 1)
                        htmltext = "baul_q0006_0101.htm";
                    else if (cond == 2 && player.hasItem(q_letter_paulo))
                        htmltext = "baul_q0006_0202.htm";
                    break;
                case sir_collin_windawood:
                    if (cond == 2 && player.hasItem(q_letter_paulo))
                        htmltext = "sir_collin_windawood_q0006_0201.htm";
                    else if (cond == 2 && !player.hasItem(q_letter_paulo))
                        htmltext = "sir_collin_windawood_q0006_0302.htm";
                    else if (cond == 3)
                        htmltext = "sir_collin_windawood_q0006_0303.htm";
                    break;
            }

            player.ShowHtm(htmltext, npc);
        }
    }
}