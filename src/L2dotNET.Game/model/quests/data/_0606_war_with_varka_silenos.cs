using L2dotNET.Game.model.npcs;
using System;

namespace L2dotNET.Game.model.quests.data
{
    class _0606_war_with_varka_silenos : QuestOrigin
    {
        const int elder_kadun_zu_ketra = 31370;

        const int varka_silenos_grunt = 21350;
        const int varka_silenos_scout = 21353;
        const int varka_silenos_hunter = 21354;
        const int varka_silenos_shaman = 21355;
        const int varka_silenos_priest = 21357;
        const int varka_silenos_warrior = 21358;
        const int varka_silenos_medium = 21360;
        const int varka_silenos_sergeant = 21362;
        const int varka_silenos_seer = 21364;
        const int varka_silenos_archmage = 21365;
        const int varka_silenos_officer = 21366;
        const int varka_silenos_overseer = 21368;
        const int varka_silenos_general = 21369;
        const int varka_high_magus = 21371;
        const int varka_soothsayer = 21373;

        const int q_barka_mane = 7233;
        const int q_buffalo_horn = 7186;

        Random rn;
        public _0606_war_with_varka_silenos()
        {
            questId = 606;
            questName = "War with Varka Silenos";
            startNpc = elder_kadun_zu_ketra;
            talkNpcs = new int[] { startNpc };
            actItems = new int[] { };
            rn = new Random();
        }

        public override void tryAccept(L2Player player, L2Citizen npc)
        {
            if (player.Level < 74)
            {
                player.ShowHtm("elder_kadun_zu_ketra_q0606_0103.htm", npc);
                return;
            }

            player.ShowHtm("elder_kadun_zu_ketra_q0606_0101.htm", npc, questId);
        }

        public override void onAccept(L2Player player, L2Citizen npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("elder_kadun_zu_ketra_q0606_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Citizen npc, int reply)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if(npcId == elder_kadun_zu_ketra)
            {
                if (reply == 1)
                {
                    htmltext = "elder_kadun_zu_ketra_q0606_0106.htm";
                }
                else if (reply == 3)
                {
                    if (player.hasItem(q_barka_mane, 100))
                    {
                        player.takeItem(q_barka_mane, 100);
                        player.addItemQuest(q_buffalo_horn, 100);
                        htmltext = "elder_kadun_zu_ketra_q0606_0202.htm";
                    }
                    else
                        htmltext = "elder_kadun_zu_ketra_q0606_0203.htm";
                }
                else if (reply == 4)
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

                    htmltext = "elder_kadun_zu_ketra_q0606_0204.htm";
                }
            }

            player.ShowHtm(htmltext, npc);
        }

        public override void onTalkToNpc(L2Player player, L2Citizen npc, int cond)
        {
            int id = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (id == elder_kadun_zu_ketra)
            {
                if (cond == 1)
                {
                    htmltext = "elder_kadun_zu_ketra_q0606_0105.htm";
                }
            }

            player.ShowHtm(htmltext, npc);
        }

        public override void onKill(L2Player player, L2Warrior mob, int cond)
        {
            switch (mob.Template.NpcId)
            {
                case varka_silenos_grunt:
                case varka_silenos_scout:
                case varka_silenos_hunter:
                case varka_silenos_shaman:
                case varka_silenos_priest:
                case varka_silenos_warrior:
                case varka_silenos_medium:
                case varka_silenos_sergeant:
                case varka_silenos_seer:
                case varka_silenos_archmage:
                case varka_silenos_officer:
                case varka_silenos_overseer:
                case varka_silenos_general:
                case varka_high_magus:
                case varka_soothsayer:
                    {
                        player.addItemQuest(q_barka_mane, 1);
                    }
                    break;
            }
        }
    }
}

