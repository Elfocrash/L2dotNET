using System;
using L2dotNET.GameService.Model.npcs;
using L2dotNET.GameService.Model.player;

namespace L2dotNET.GameService.Model.quests.data
{
    class _0246_PossessorOfAPreciousSoul : QuestOrigin
    {
        private const int caradine = 31740;
        private const int ossian = 31741;
        private const int magister_ladd = 30721;

        private const int brilliant_prophet = 21541;
        private const int brilliant_justice = 21544;
        private const int blinding_fire_barakiel = 25325;

        private const int q_caradines_letter1 = 7678;
        private const int q_ring_waterbinder = 7591;
        private const int q_necklace_evergreen = 7592;
        private const int q_staff_rainsong = 7593;
        private const int q_caradines_letter2 = 7679;
        private const int q_red_dust = 7594;

        public _0246_PossessorOfAPreciousSoul()
        {
            questId = 246;
            questName = "Possessor of a Precious Soul  - 3";
            startNpc = caradine;
            talkNpcs = new int[] { caradine, ossian, magister_ladd };
            actItems = new int[] { q_caradines_letter1, q_ring_waterbinder, q_necklace_evergreen, q_staff_rainsong, q_caradines_letter2, q_red_dust };
        }

        public override void tryAccept(L2Player player, L2Npc npc)
        {
            if (!player.subActive() || !player.hasItem(q_caradines_letter1) || player.Level < 65)
            {
                player.ShowHtm("caradine_q0246_0102.htm", npc);
                return;
            }

            player.ShowHtm("caradine_q0246_0101.htm", npc, questId);
        }

        public override void onAccept(L2Player player, L2Npc npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("caradine_q0246_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Npc npc, int reply)
        {
            if (reply == 1)
            {
                int cond = player.getQuestCond(questId);
                switch (cond)
                {
                    case 1:
                        player.ShowHtm("ossian_q0246_0201.htm", npc);
                        player.changeQuestStage(questId, 2);
                        break;
                    case 5:
                        player.ShowHtm("ossian_q0246_0401.htm", npc);
                        player.takeItem(q_ring_waterbinder, 1);
                        player.takeItem(q_necklace_evergreen, 1);
                        player.takeItem(q_staff_rainsong, 1);
                        player.addItemQuest(q_red_dust, 1);
                        player.changeQuestStage(questId, 6);
                        break;
                }
            }
            else if (reply == 3)
            {
                player.ShowHtm("magister_ladd_q0246_0501.htm", npc);
                player.takeItem(q_red_dust, 1);
                player.addItemQuest(q_caradines_letter2, 1);
                player.addExpSp(719843, 0, true);
                player.finishQuest(questId);
            }
        }

        public override void onTalkToNpc(L2Player player, L2Npc npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (npcId == caradine)
            {
                htmltext = "caradine_q0246_0105.htm";
            }
            else if (npcId == ossian)
            {
                switch (cond)
                {
                    case 1:
                        htmltext = "ossian_q0246_0101.htm";
                        break;
                    case 2:
                        if (!player.hasAllOfThisItems(q_ring_waterbinder, q_necklace_evergreen))
                            htmltext = "ossian_q0246_0203.htm";
                        else
                            htmltext = "ossian_q0246_0202.htm";
                        break;
                    case 3:
                        htmltext = "ossian_q0246_0301.htm";
                        player.changeQuestStage(questId, 4);
                        break;
                    case 4:
                        htmltext = "ossian_q0246_0401.htm";
                        break;
                    case 5:
                        if (player.hasAllOfThisItems(q_necklace_evergreen, q_ring_waterbinder, q_staff_rainsong))
                            htmltext = "ossian_q0246_0303.htm";
                        else
                            htmltext = "ossian_q0246_0304.htm";
                        break;
                }
            }
            else if (npcId == magister_ladd)
            {
                if (cond == 5)
                {
                    htmltext = "magister_ladd_q0246_0401.htm";
                }
            }

            player.ShowHtm(htmltext, npc);
        }

        public override void onKill(L2Player player, L2Warrior mob, int stage)
        {
            if (stage == 2)
            {
                Random rn = new Random();
                switch (mob.Template.NpcId)
                {
                    case brilliant_prophet:
                    {
                        if (!player.hasItem(q_ring_waterbinder) && rn.Next(100) <= 15)
                            player.addItemQuest(q_ring_waterbinder, 1);
                    }
                        break;
                    case brilliant_justice:
                    {
                        if (!player.hasItem(q_necklace_evergreen) && rn.Next(100) <= 10)
                            player.addItemQuest(q_necklace_evergreen, 1);
                    }
                        break;
                }
            }
            else if (stage == 4)
            {
                switch (mob.Template.NpcId)
                {
                    case blinding_fire_barakiel:
                    {
                        if (!player.hasItem(q_staff_rainsong))
                            player.addItemQuest(q_staff_rainsong, 1);
                    }
                        break;
                }
            }
        }

        public override void onEarnItem(L2Player player, int cond, int id)
        {
            if (cond == 2)
            {
                if (id == q_ring_waterbinder || id == q_necklace_evergreen)
                {
                    if (player.hasAllOfThisItems(q_ring_waterbinder, q_ring_waterbinder))
                        player.changeQuestStage(questId, 3);
                }
            }
            else if (cond == 4)
            {
                if (id == q_staff_rainsong)
                {
                    player.changeQuestStage(questId, 5);
                }
            }
        }
    }
}