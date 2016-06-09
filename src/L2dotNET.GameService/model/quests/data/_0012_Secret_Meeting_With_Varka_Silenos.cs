using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Quests.Data
{
    class _0012_Secret_Meeting_With_Varka_Silenos : QuestOrigin
    {
        private const int guard_cadmon = 31296;
        private const int trader_helmut = 31258;
        private const int herald_naran = 31378;

        private const int q_cargo_for_barka = 7232;

        public _0012_Secret_Meeting_With_Varka_Silenos()
        {
            questId = 12;
            questName = "Secret Meeting With Varka Silenos";
            startNpc = guard_cadmon;
            talkNpcs = new[] { startNpc, trader_helmut, herald_naran };
            actItems = new[] { q_cargo_for_barka };
        }

        public override void tryAccept(L2Player player, L2Npc npc)
        {
            if (player.Level >= 74)
                player.ShowHtm("guard_cadmon_q0012_0101.htm", npc);
            else
                player.ShowHtm("guard_cadmon_q0012_0103.htm", npc);
        }

        public override void onAccept(L2Player player, L2Npc npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("guard_cadmon_q0012_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Npc npc, int reply)
        {
            //todo
        }

        public override void onTalkToNpc(L2Player player, L2Npc npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            switch (npcId)
            {
                case guard_cadmon:
                    switch (cond)
                    {
                        case 1:
                            htmltext = "guard_cadmon_q0012_0105.htm";
                            break;
                    }

                    break;
                case trader_helmut:
                    switch (cond)
                    {
                        case 1:
                            htmltext = "trader_helmut_q0012_0101.htm";
                            break;
                        case 2:
                            htmltext = "trader_helmut_q0012_0202.htm";
                            break;
                    }

                    break;
                case herald_naran:
                    switch (cond)
                    {
                        case 2:
                            if (player.hasItem(q_cargo_for_barka))
                                htmltext = "herald_naran_q0012_0201.htm";
                            break;
                    }

                    break;
            }

            player.ShowHtm(htmltext, npc);
        }

        public override void onEarnItem(L2Player player, int cond, int id)
        {
            //todo
        }
    }
}