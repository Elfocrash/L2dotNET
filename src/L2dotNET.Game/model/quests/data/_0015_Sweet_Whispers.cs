using L2dotNET.Game.model.npcs;

namespace L2dotNET.Game.model.quests.data
{
    class _0015_Sweet_Whispers : QuestOrigin
    {
        const int trader_vladimir = 31302;
        const int dark_necromancer = 31518;
        const int dark_presbyter = 31517;

        public _0015_Sweet_Whispers()
        {
            questId = 15;
            questName = "Sweet Whispers";
            startNpc = trader_vladimir;
            talkNpcs = new int[] { startNpc, dark_necromancer, dark_presbyter };
        }

        public override void tryAccept(L2Player player, L2Citizen npc)
        {
            if (player.Level >= 60)
                player.ShowHtm("trader_vladimir_q0015_0101.htm", npc);
            else
            {
                player.ShowHtm("trader_vladimir_q0015_0103.htm", npc);
            }
        }

        public override void onAccept(L2Player player, L2Citizen npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("trader_vladimir_q0015_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Citizen npc, int reply)
        {
            //todo
        }

        public override void onTalkToNpc(L2Player player, L2Citizen npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (npcId == trader_vladimir)
            {
                switch (cond)
                {
                    case 1:
                        htmltext = "trader_vladimir_q0015_0105.htm";
                        break;
                }
            }
            else if (npcId == dark_necromancer)
            {
                switch (cond)
                {
                    case 1:
                        htmltext = "dark_necromancer_q0015_0101.htm";
                        break;
                    case 2:
                        htmltext = "dark_necromancer_q0015_0202.htm";
                        break;
                }
            }
            else if (npcId == dark_presbyter)
            {
                switch (cond)
                {
                    case 2:
                        htmltext = "dark_presbyter_q0015_0201.htm";
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

