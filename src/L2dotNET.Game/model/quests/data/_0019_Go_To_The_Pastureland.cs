using L2dotNET.GameService.model.npcs;

namespace L2dotNET.GameService.model.quests.data
{
    class _0019_Go_To_The_Pastureland : QuestOrigin
    {
        private const int trader_vladimir = 31302;
        private const int beast_herder_tunatun = 31537;

        private const int q_youngmeat_of_beast = 7547;

        public _0019_Go_To_The_Pastureland()
        {
            questId = 19;
            questName = "Go To The Pastureland";
            startNpc = trader_vladimir;
            talkNpcs = new int[] { startNpc, beast_herder_tunatun };
            actItems = new int[] { q_youngmeat_of_beast };
        }

        public override void tryAccept(L2Player player, L2Citizen npc)
        {
            if (player.Level >= 63)
                player.ShowHtm("trader_vladimir_q0019_0101.htm", npc);
            else
            {
                player.ShowHtm("trader_vladimir_q0019_0103.htm", npc);
            }
        }

        public override void onAccept(L2Player player, L2Citizen npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("trader_vladimir_q0019_0104.htm", npc);
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
                if (cond != 0)
                    htmltext = "trader_vladimir_q0019_0105.htm";
            }
            else if (npcId == beast_herder_tunatun)
            {
                if (player.hasItem(q_youngmeat_of_beast))
                    htmltext = "beast_herder_tunatun_q0019_0101.htm";
                else
                {
                    htmltext = "beast_herder_tunatun_q0019_0202.htm";
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