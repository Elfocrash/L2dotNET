using L2dotNET.GameService.model.npcs;

namespace L2dotNET.GameService.model.quests.data
{
    class _0018_Meeting_with_the_Golden_Ram : QuestOrigin
    {
        const int warehouse_chief_donal = 31314;
        const int freighter_daisy = 31315;
        const int supplier_abercrombie = 31555;

        const int q_mercs_supplies = 7245;

        public _0018_Meeting_with_the_Golden_Ram()
        {
            questId = 18;
            questName = "Meeting with the Golden Ram";
            startNpc = warehouse_chief_donal;
            talkNpcs = new int[] { startNpc, freighter_daisy, supplier_abercrombie };
            actItems = new int[] { q_mercs_supplies };
        }

        public override void tryAccept(L2Player player, L2Npc npc)
        {
            if (player.Level >= 66)
                player.ShowHtm("warehouse_chief_donal_q0018_0101.htm", npc);
            else
            {
                player.ShowHtm("warehouse_chief_donal_q0018_0103.htm", npc);
            }
        }

        public override void onAccept(L2Player player, L2Npc npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("warehouse_chief_donal_q0018_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Npc npc, int reply)
        {
            //todo
        }

        public override void onTalkToNpc(L2Player player, L2Npc npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (npcId == warehouse_chief_donal)
            {
                switch (cond)
                {
                    case 1:
                        htmltext = "warehouse_chief_donal_q0018_0105.htm";
                        break;
                }
            }
            else if (npcId == freighter_daisy)
            {
                switch (cond)
                {
                    case 1:
                        htmltext = "freighter_daisy_q0018_0101.htm";
                        break;
                    case 2:
                        htmltext = "freighter_daisy_q0018_0202.htm";
                        break;
                }
            }
            else if (npcId == supplier_abercrombie)
            {
                switch (cond)
                {
                    case 2:
                        if (player.hasItem(q_mercs_supplies))
                            htmltext = "supplier_abercrombie_q0018_0201.htm";
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

