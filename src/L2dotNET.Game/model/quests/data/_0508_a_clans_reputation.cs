using L2dotNET.GameService.model.communities;
using L2dotNET.GameService.model.npcs;

namespace L2dotNET.GameService.model.quests.data
{
    class _0508_a_clans_reputation : QuestOrigin
    {
        const int sir_eric_rodemai = 30868;

        const int flame_stone_golem = 25524;
        const int palibati_queen_themis = 25252;
        const int hekaton_prime = 25140;
        const int gargoyle_lord_tiphon = 25255;
        const int last_lesser_glaki = 25245;
        const int rahha = 25051;

        const int nucleus_of_flamestone_giant1 = 8494;
        const int q_themis_scale = 8277;
        const int q_nucleus_of_hekaton_prime = 8279;
        const int q_tiphon_shard = 8280;
        const int q_glakis_necleus = 8281;
        const int q_rahhas_fang = 8282;

        public _0508_a_clans_reputation()
        {
            questId = 508;
            questName = "A Clan's Reputation";
            startNpc = sir_eric_rodemai;
            talkNpcs = new int[] { startNpc };
            actItems = new int[] { };
        }

        public override void tryAccept(L2Player player, L2Npc npc)
        {
            bool fail = true;
            if (player.ClanId > 0)
            {
                L2Clan clan = player.Clan;
                if (clan.LeaderID == player.ObjID && clan.Level >= 5)
                    fail = false;
            }

            if (fail)
                player.ShowHtm("sir_eric_rodemai_q0508_02.htm", npc);
            else
            {
                player.ShowHtm("sir_eric_rodemai_q0508_01a.htm", npc, questId);
            }
        }

        public override void onAccept(L2Player player, L2Npc npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("sir_eric_rodemai_q0508_01.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Npc npc, int reply)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if(npcId == sir_eric_rodemai)
            {
                switch (reply)
                {
                    case 2:
                        if (player.hasItem(q_themis_scale))
                        {
                            player.Clan.UpdatePledgeNameValue(1000);
                            player.takeItem(q_themis_scale, 1);
                            htmltext = "sir_eric_rodemai_q0508_19.htm";
                        }
                        else
                            htmltext = "sir_eric_rodemai_q0508_10.htm";
                        break;
                    case 4:
                        htmltext = "sir_eric_rodemai_q0508_12.htm";
                        break;
                    case 5:
                        htmltext = "sir_eric_rodemai_q0508_13.htm";
                        break;
                    case 6:
                        htmltext = "sir_eric_rodemai_q0508_14.htm";
                        break;
                    case 7:
                        htmltext = "sir_eric_rodemai_q0508_15.htm";
                        break;
                    case 8:
                        htmltext = "sir_eric_rodemai_q0508_15a.htm";
                        break;
                    case 100:
                    case 110:
                        htmltext = "sir_eric_rodemai_q0508_04.htm";
                        break;
                    case 101:
                        htmltext = "sir_eric_rodemai_q0508_07.htm";
                        break;
                }
            }

            player.ShowHtm(htmltext, npc);
        }

        public override void onTalkToNpc(L2Player player, L2Npc npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (npcId == sir_eric_rodemai)
            {
                htmltext = "sir_eric_rodemai_q0508_01.htm";
            }

            player.ShowHtm(htmltext, npc);
        }

        // вызывается клановым игроком при убийстве босса. [player._clan.notifyBossDeath(boss)]
        public override void onKill(L2Player player, L2Warrior mob, int stage)
        {
            switch (mob.Template.NpcId)
            {
                case palibati_queen_themis:
                    player.addItemQuest(q_themis_scale, 1);
                    break;
            }
        }
    }
}

