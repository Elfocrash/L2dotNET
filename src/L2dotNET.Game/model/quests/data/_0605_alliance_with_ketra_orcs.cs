using System;
using L2dotNET.GameService.model.npcs;

namespace L2dotNET.GameService.model.quests.data
{
    class _0605_alliance_with_ketra_orcs : QuestOrigin
    {
        const int herald_wakan = 31371;

        // уровень1
        const int varka_silenos_grunt = 21350;
        const int varka_silenos_footman = 21351;
        const int varka_silenos_scout = 21353;
        const int varka_silenos_hunter = 21354;
        const int varka_silenos_shaman = 21355;
        // уровень2
        const int varka_silenos_priest = 21357;
        const int varka_silenos_warrior = 21358;
        const int varka_silenos_medium = 21360;
        const int varka_silenos_mage = 21361;
        const int varka_silenos_sergeant = 21362;
        const int varka_silenos_general = 21369;
        const int varka_elite_guard = 21370;
        // уровень3
        const int varka_silenos_seer = 21364;
        const int varka_silenos_archmage = 21365;
        const int varka_silenos_officer = 21366;
        const int varka_silenos_overseer = 21368;
        const int varka_high_magus = 21371;
        const int varka_high_guard = 21372;
        const int varka_soothsayer = 21373;
        const int soothsayers_escort = 21374;
        const int soothsayers_apostle = 21375;

        const int q_barka_badge_grunt = 7216;
        const int q_barka_badge_captn = 7217;
        const int q_barka_badge_officer = 7218;
        const int q_ketra_friendship_1 = 7211;
        const int q_ketra_friendship_2 = 7212;
        const int q_ketra_friendship_3 = 7213;
        const int q_ketra_friendship_4 = 7214;
        const int q_ketra_friendship_5 = 7215;

        const int q_barka_friendship_1 = 7221;
        const int q_barka_friendship_2 = 7222;
        const int q_barka_friendship_3 = 7223;
        const int q_barka_friendship_4 = 7224;
        const int q_barka_friendship_5 = 7225;
        const int q_totem_of_valor = 7219;
        const int q_totem_of_wisdom = 7220;

        Random rn;
        public _0605_alliance_with_ketra_orcs()
        {
            questId = 605;
            questName = "Alliance with Ketra Orcs";
            startNpc = herald_wakan;
            talkNpcs = new int[] { startNpc };
            actItems = new int[] { q_ketra_friendship_1, q_ketra_friendship_2, q_ketra_friendship_3, q_ketra_friendship_4, q_ketra_friendship_5 };
            rn = new Random();
        }

        public override void tryAccept(L2Player player, L2Citizen npc)
        {
            if (player.Level < 74)
            {
                player.ShowHtm("herald_wakan_q0605_03.htm", npc);
                return;
            }

            if (player.hasSomeOfThisItems(new int[] { q_barka_friendship_1, q_barka_friendship_2, q_barka_friendship_3, q_barka_friendship_4, q_barka_friendship_5 }))
            {
                player.ShowHtm("herald_wakan_q0605_02.htm", npc);
                return;
            }

            player.ShowHtm("herald_wakan_q0605_01.htm", npc, questId);
        }

        public override void onAccept(L2Player player, L2Citizen npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("herald_wakan_q0605_04.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Citizen npc, int reply)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if(npcId == herald_wakan)
            {
                if (reply == 1)
                {
                    if (player.hasItem(q_barka_badge_grunt, 100))
                    {
                        player.takeItem(q_barka_badge_grunt, 100);
                        player.addItem(q_ketra_friendship_1, 1);
                        player.changeQuestStage(questId, 2);
                        htmltext = "herald_wakan_q0605_12.htm";
                    }
                    else
                        htmltext = "herald_wakan_q0605_12b.htm";
                }
                else if (reply == 2)
                {
                    if (player.hasItem(q_barka_badge_grunt, 200) && player.hasItem(q_barka_badge_captn, 100) && player.hasItem(q_ketra_friendship_1))
                    {
                        player.takeItem(q_barka_badge_grunt, 200);
                        player.takeItem(q_barka_badge_captn, 100);
                        player.takeItem(q_ketra_friendship_1, 1);
                        player.addItem(q_ketra_friendship_2, 1);
                        player.changeQuestStage(questId, 3);
                        htmltext = "herald_wakan_q0605_15.htm";
                    }
                    else
                        htmltext = "herald_wakan_q0605_12b.htm";
                }
                else if (reply == 3)
                {
                    if (player.hasItem(q_barka_badge_grunt, 300) && player.hasItem(q_barka_badge_captn, 200) && player.hasItem(q_barka_badge_officer, 100) && player.hasItem(q_ketra_friendship_2))
                    {
                        player.takeItem(q_barka_badge_grunt, 200);
                        player.takeItem(q_barka_badge_captn, 100);
                        player.takeItem(q_ketra_friendship_2, 1);
                        player.addItem(q_ketra_friendship_3, 1);
                        player.changeQuestStage(questId, 4);
                        htmltext = "herald_wakan_q0605_18.htm";
                    }
                    else
                        htmltext = "herald_wakan_q0605_12b.htm";
                }
                else if (reply == 4)
                {
                    if (player.hasItem(q_barka_badge_grunt, 300) && player.hasItem(q_barka_badge_captn, 300) && player.hasItem(q_barka_badge_officer, 200) && player.hasItem(q_totem_of_valor) && player.hasItem(q_ketra_friendship_3))
                    {
                        player.takeItem(q_barka_badge_grunt, 300);
                        player.takeItem(q_barka_badge_captn, 300);
                        player.takeItem(q_barka_badge_officer, 200);
                        player.takeItem(q_totem_of_valor, 1);
                        player.takeItem(q_ketra_friendship_3, 1);
                        player.addItem(q_ketra_friendship_4, 1);
                        player.changeQuestStage(questId, 5);
                        htmltext = "herald_wakan_q0605_21.htm";
                    }
                    else
                        htmltext = "herald_wakan_q0605_12b.htm";
                }
                else if (reply == 5)
                {
                    htmltext = "herald_wakan_q0605_25.htm";
                }
                else if (reply == 6)
                {
                    foreach (QuestInfo qi in player._quests)
                    {
                        if (qi.id == questId)
                        {
                            foreach (int id in qi._template.actItems)
                            {
                                player.Inventory.destroyItemAll(id, true, true);
                            }

                            player.stopQuest(qi, true);
                            return;
                        }
                    }

                    htmltext = "herald_wakan_q0605_26.htm";
                }
            }

            player.ShowHtm(htmltext, npc);
        }

        public override void onTalkToNpc(L2Player player, L2Citizen npc, int cond)
        {
            int npcIdh = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (npcIdh == herald_wakan)
            {
                if (cond == 1)
                {
                    if (player.hasItem(q_barka_badge_grunt, 100))
                    {
                        htmltext = "herald_wakan_q0605_11.htm";
                    }
                    else
                        htmltext = "herald_wakan_q0605_10.htm";
                }
                else if (cond == 2)
                {
                    if (player.hasItem(q_barka_badge_grunt, 200) && player.hasItem(q_barka_badge_captn, 100))
                    {
                        htmltext = "herald_wakan_q0605_14.htm";
                    }
                    else
                        htmltext = "herald_wakan_q0605_13.htm";
                }
                else if (cond == 3)
                {
                    if (player.hasItem(q_barka_badge_grunt, 300) &&  player.hasItem(q_barka_badge_captn, 200) && player.hasItem(q_barka_badge_officer, 100))
                    {
                        htmltext = "herald_wakan_q0605_17.htm";
                    }
                    else
                        htmltext = "herald_wakan_q0605_16.htm";
                }
                else if (cond == 4)
                {
                    if (player.hasItem(q_barka_badge_grunt, 300) && player.hasItem(q_barka_badge_captn, 300) && player.hasItem(q_barka_badge_officer, 200) && player.hasItem(q_totem_of_valor))
                    {
                        htmltext = "herald_wakan_q0605_20.htm";
                    }
                    else
                        htmltext = "herald_wakan_q0605_19.htm";
                }
                else if (cond == 5)
                {
                    if (player.hasItem(q_barka_badge_grunt, 400) && player.hasItem(q_barka_badge_captn, 400) && player.hasItem(q_barka_badge_officer, 200) && player.hasItem(q_totem_of_wisdom) && player.hasItem(q_ketra_friendship_4))
                    {
                        player.takeItem(q_barka_badge_grunt, 400);
                        player.takeItem(q_barka_badge_captn, 400);
                        player.takeItem(q_barka_badge_officer, 200);
                        player.takeItem(q_totem_of_wisdom, 1);
                        player.takeItem(q_ketra_friendship_4, 1);
                        player.addItem(q_ketra_friendship_5, 1);
                        player.changeQuestStage(questId, 6);
                        htmltext = "herald_wakan_q0605_23.htm";
                    }
                    else
                        htmltext = "herald_wakan_q0605_22.htm";
                }
                else if (cond == 6)
                    htmltext = "herald_wakan_q0605_09.htm";
            }

            player.ShowHtm(htmltext, npc);
        }

        public override void onKill(L2Player player, L2Warrior mob, int cond)
        {
            if (cond == 6)
                return;

            switch (mob.Template.NpcId)
            {
                case varka_silenos_grunt:
                case varka_silenos_footman:
                case varka_silenos_scout:
                case varka_silenos_hunter:
                case varka_silenos_shaman:
                    {
                        if (cond >= 1)
                        {
                            if (rn.Next(100) <= 80)
                                player.addItemQuest(q_barka_badge_grunt, 1);
                        }
                    }
                    break;
                case varka_silenos_priest:
                case varka_silenos_warrior:
                case varka_silenos_medium:
                case varka_silenos_mage:
                case varka_silenos_sergeant:
                case varka_silenos_general:
                case varka_elite_guard:
                    {
                        if (cond >= 2)
                        {
                            if (rn.Next(100) <= 80)
                                player.addItemQuest(q_barka_badge_captn, 1);
                        }
                    }
                    break;
                case varka_silenos_seer:
                case varka_silenos_archmage:
                case varka_silenos_officer:
                case varka_silenos_overseer:
                case varka_high_magus:
                case varka_high_guard:
                case varka_soothsayer:
                case soothsayers_escort:
                case soothsayers_apostle:
                    {
                        if (cond >= 3)
                        {
                            if (rn.Next(100) <= 80)
                                player.addItemQuest(q_barka_badge_officer, 1);
                        }
                    }
                    break;
            }
        }
    }
}

