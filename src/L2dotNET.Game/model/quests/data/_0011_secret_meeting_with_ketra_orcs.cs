using L2dotNET.GameService.model.npcs;

namespace L2dotNET.GameService.model.quests.data
{
    class _0011_secret_meeting_with_ketra_orcs : QuestOrigin
    {
        private const int guard_cadmon = 31296;
        private const int trader_leon = 31256;
        private const int herald_wakan = 31371;

        private const int q_cargo_for_ketra = 7231;

        public _0011_secret_meeting_with_ketra_orcs()
        {
            questId = 11;
            questName = "Secret Meeting With Ketra Orcs";
            startNpc = guard_cadmon;
            talkNpcs = new int[] { startNpc, trader_leon, herald_wakan };
            actItems = new int[] { q_cargo_for_ketra };
        }

        public override void tryAccept(L2Player player, L2Citizen npc)
        {
            if (player.Level >= 74)
                player.ShowHtm("guard_cadmon_q0011_0101.htm", npc, questId);
            else
            {
                player.ShowHtm("guard_cadmon_q0011_0103.htm", npc);
            }
        }

        public override void onAccept(L2Player player, L2Citizen npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("guard_cadmon_q0011_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Citizen npc, int reply)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (reply == 1 && npcId == trader_leon)
            {
                htmltext = "trader_leon_q0011_0201.htm";
                player.addItem(q_cargo_for_ketra, 1);
                player.changeQuestStage(questId, 2);
            }
            else if (reply == 3 && npcId == herald_wakan)
            {
                htmltext = "herald_wakan_q0011_0301.htm";
                player.takeItem(q_cargo_for_ketra, 1);
                player.addExpSp(22787, 0, true);
                player.finishQuest(questId);
            }

            player.ShowHtm(htmltext, npc);
        }

        public override void onTalkToNpc(L2Player player, L2Citizen npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (npcId == guard_cadmon)
            {
                switch (cond)
                {
                    case 1:
                        htmltext = "guard_cadmon_q0011_0105.htm";
                        break;
                }
            }
            else if (npcId == trader_leon)
            {
                switch (cond)
                {
                    case 1:
                        htmltext = "trader_leon_q0011_0101.htm";
                        break;
                    case 2:
                        htmltext = "trader_leon_q0011_0202.htm";
                        break;
                }
            }
            else if (npcId == herald_wakan)
            {
                switch (cond)
                {
                    case 2:
                        if (player.hasItem(q_cargo_for_ketra))
                            htmltext = "herald_wakan_q0011_0201.htm";
                        break;
                }
            }

            player.ShowHtm(htmltext, npc);
        }
    }
}

