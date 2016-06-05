using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Quests.Data
{
    class _0013_Parcel_Deliverys : QuestOrigin
    {
        private const int mineral_trader_fundin = 31274;
        private const int warsmith_vulcan = 31539;

        private const int q_package_to_vulcan = 7263;

        public _0013_Parcel_Deliverys()
        {
            questId = 13;
            questName = "Parcel Deliverys";
            startNpc = mineral_trader_fundin;
            talkNpcs = new int[] { startNpc, warsmith_vulcan };
            actItems = new int[] { q_package_to_vulcan };
        }

        public override void tryAccept(L2Player player, L2Npc npc)
        {
            if (player.Level >= 74)
                player.ShowHtm("mineral_trader_fundin_q0013_0101.htm", npc);
            else
            {
                player.ShowHtm("mineral_trader_fundin_q0013_0103.htm", npc);
            }
        }

        public override void onAccept(L2Player player, L2Npc npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("mineral_trader_fundin_q0013_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Npc npc, int reply)
        {
            //todo
        }

        public override void onTalkToNpc(L2Player player, L2Npc npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (npcId == mineral_trader_fundin)
            {
                switch (cond)
                {
                    case 1:
                        htmltext = "mineral_trader_fundin_q0013_0105.htm";
                        break;
                }
            }
            else if (npcId == warsmith_vulcan)
            {
                switch (cond)
                {
                    case 1:
                        if (player.hasItem(q_package_to_vulcan))
                            htmltext = "warsmith_vulcan_q0013_0101.htm";
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