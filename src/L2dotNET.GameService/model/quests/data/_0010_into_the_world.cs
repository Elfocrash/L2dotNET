using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Quests.Data
{
    class _0010_into_the_world : QuestOrigin
    {
        private const int elder_balanki = 30533;
        private const int warehouse_chief_reed = 30520;
        private const int gerald_priest_of_earth = 30650;

        private const int q_expensive_necklace = 7574;
        private const int escape_scroll_giran = 7126;
        private const int q_symbol_of_traveler = 7570;

        public _0010_into_the_world()
        {
            questId = 10;
            questName = "Into the World";
            startNpc = elder_balanki;
            talkNpcs = new int[] { startNpc, warehouse_chief_reed, gerald_priest_of_earth };
            actItems = new int[] { q_expensive_necklace };
        }

        public override void tryAccept(L2Player player, L2Npc npc)
        {
            if (player.BaseClass.ClassId.ClassRace == ClassRace.DWARF && player.Level >= 3)
                player.ShowHtm("elder_balanki_q0010_0101.htm", npc, questId);
            else
            {
                player.ShowHtm("elder_balanki_q0010_0102.htm", npc);
            }
        }

        public override void onAccept(L2Player player, L2Npc npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("elder_balanki_q0010_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Npc npc, int reply)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (reply == 1 && npcId == warehouse_chief_reed)
            {
                int cond = player.getQuestCond(questId);
                switch (cond)
                {
                    case 1:
                        player.addItem(q_expensive_necklace, 1);
                        player.changeQuestStage(questId, 2);
                        htmltext = "warehouse_chief_reed_q0010_0201.htm";
                        break;
                    case 3:
                        player.changeQuestStage(questId, 4);
                        htmltext = "warehouse_chief_reed_q0010_0401.htm";
                        break;
                }
            }
            else if (reply == 1 && npcId == gerald_priest_of_earth)
            {
                player.takeItem(q_expensive_necklace, 1);
                player.changeQuestStage(questId, 3);
                htmltext = "gerald_priest_of_earth_q0010_0301.htm";
            }
            else if (reply == 3 && npcId == elder_balanki)
            {
                htmltext = "elder_balanki_q0010_0501.htm";
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
                case elder_balanki:
                    switch (cond)
                    {
                        case 1:
                            htmltext = "elder_balanki_q0010_0105.htm";
                            break;
                        case 4:
                            htmltext = "elder_balanki_q0010_0401.htm";
                            break;
                    }
                    break;
                case warehouse_chief_reed:
                    switch (cond)
                    {
                        case 1:
                            htmltext = "warehouse_chief_reed_q0010_0101.htm";
                            break;
                        case 2:
                            htmltext = "warehouse_chief_reed_q0010_0202.htm";
                            break;
                        case 3:
                            htmltext = "warehouse_chief_reed_q0010_0301.htm";
                            break;
                        case 4:
                            htmltext = "warehouse_chief_reed_q0010_0402.htm";
                            break;
                    }
                    break;
                case gerald_priest_of_earth:
                    switch (cond)
                    {
                        case 2:
                            if (player.hasItem(q_expensive_necklace))
                                htmltext = "gerald_priest_of_earth_q0010_0201.htm";
                            else
                                htmltext = "gerald_priest_of_earth_q0010_0303.htm";
                            break;
                        case 3:
                            htmltext = "gerald_priest_of_earth_q0010_0302.htm";
                            break;
                    }
                    break;
            }

            player.ShowHtm(htmltext, npc);
        }
    }
}