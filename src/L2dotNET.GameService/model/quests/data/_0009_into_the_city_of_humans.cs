using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Quests.Data
{
    class _0009_into_the_city_of_humans : QuestOrigin
    {
        private const int centurion_petukai = 30583;
        private const int seer_tanapi = 30571;
        private const int gatekeeper_tamil = 30576;

        private const int escape_scroll_giran = 7126;
        private const int q_symbol_of_traveler = 7570;

        public _0009_into_the_city_of_humans()
        {
            questId = 9;
            questName = "Into The City of Humans";
            startNpc = centurion_petukai;
            talkNpcs = new int[] { startNpc, seer_tanapi, gatekeeper_tamil };
            actItems = new int[] { };
        }

        public override void tryAccept(L2Player player, L2Npc npc)
        {
            if (player.BaseClass.ClassId.ClassRace == ClassRace.ORC && player.Level >= 3)
                player.ShowHtm("centurion_petukai_q0009_0101.htm", npc, questId);
            else
            {
                player.ShowHtm("centurion_petukai_q0009_0102.htm", npc);
            }
        }

        public override void onAccept(L2Player player, L2Npc npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("centurion_petukai_q0009_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Npc npc, int reply)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (reply == 1 && npcId == seer_tanapi)
            {
                player.changeQuestStage(questId, 2);
                htmltext = "seer_tanapi_q0009_0201.htm";
            }
            else if (reply == 3 && npcId == gatekeeper_tamil)
            {
                htmltext = "gatekeeper_tamil_q0009_0301.htm";
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
                case centurion_petukai:
                    switch (cond)
                    {
                        case 1:
                            htmltext = "centurion_petukai_q0009_0105.htm";
                            break;
                    }
                    break;
                case seer_tanapi:
                    switch (cond)
                    {
                        case 1:
                            htmltext = "seer_tanapi_q0009_0101.htm";
                            break;
                        case 2:
                            htmltext = "seer_tanapi_q0009_0202.htm";
                            break;
                    }
                    break;
                case gatekeeper_tamil:
                    switch (cond)
                    {
                        case 2:
                            htmltext = "gatekeeper_tamil_q0009_0201.htm";
                            break;
                    }
                    break;
            }

            player.ShowHtm(htmltext, npc);
        }
    }
}