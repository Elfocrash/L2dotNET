using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Quests.Data
{
    class _0003_will_the_seal_be_broken : QuestOrigin
    {
        private const int redry = 30141;

        private const int onyx_beast = 20031;
        private const int tainted_zombie = 20041;
        private const int stink_zombie = 20046;
        private const int least_succubus = 20048;
        private const int least_succubus_turen = 20052;
        private const int least_succubus_tilfo = 20057;

        private const int onyx_beast_eye = 1081;
        private const int taint_stone = 1082;
        private const int succubus_blood = 1083;
        private const int scrl_of_ench_am_d = 956;

        public _0003_will_the_seal_be_broken()
        {
            questId = 3;
            questName = "Will the Seal be Broken";
            startNpc = redry;
            talkNpcs = new[] { startNpc };
            actItems = new[] { onyx_beast_eye, taint_stone, succubus_blood };
        }

        public override void tryAccept(L2Player player, L2Npc npc)
        {
            if (player.BaseClass.ClassId.ClassRace != ClassRace.DARK_ELF)
                player.ShowHtm("redry_q0003_00.htm", npc);
            else if (player.Level >= 16)
                player.ShowHtm("redry_q0003_02.htm", npc);
            else
                player.ShowHtm("redry_q0003_01.htm", npc);
        }

        public override void onAccept(L2Player player, L2Npc npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("redry_q0003_03.htm", npc);
        }

        public override void onTalkToNpc(L2Player player, L2Npc npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if (npcId == redry)
                if (cond == 2)
                {
                    htmltext = "redry_q0003_06.htm";
                    player.takeItem(onyx_beast_eye, 1);
                    player.takeItem(taint_stone, 1);
                    player.takeItem(succubus_blood, 1);
                    player.addItem(scrl_of_ench_am_d, 1);
                    player.finishQuest(questId);
                }
                else
                    htmltext = "redry_q0003_04.htm";

            player.ShowHtm(htmltext, npc);
        }

        public override void onKill(L2Player player, L2Warrior mob, int stage)
        {
            if (stage == 1)
            {
                switch (mob.Template.NpcId)
                {
                    case onyx_beast:
                    {
                        if (!player.hasItem(onyx_beast_eye))
                            player.addItemQuest(onyx_beast_eye, 1);
                    }
                        break;
                    case tainted_zombie:
                    case stink_zombie:
                    {
                        if (!player.hasItem(taint_stone))
                            player.addItemQuest(taint_stone, 1);
                    }
                        break;
                    case least_succubus:
                    case least_succubus_turen:
                    case least_succubus_tilfo:
                    {
                        if (!player.hasItem(succubus_blood))
                            player.addItemQuest(succubus_blood, 1);
                    }
                        break;
                }

                if (player.hasAllOfThisItems(onyx_beast_eye, taint_stone, succubus_blood))
                    player.changeQuestStage(questId, 2);
            }
        }
    }
}