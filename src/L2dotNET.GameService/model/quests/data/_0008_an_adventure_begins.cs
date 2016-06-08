using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Quests.Data
{
    class _0008_an_adventure_begins : QuestOrigin
    {
        private const int jasmine = 30134;
        private const int sentry_roseline = 30355;
        private const int harne = 30144;

        private const int q_roseline_paper = 7573;
        private const int escape_scroll_giran = 7126;
        private const int q_symbol_of_traveler = 7570;

        public _0008_an_adventure_begins()
        {
            questId = 8;
            questName = "An Adventure Begins";
            startNpc = jasmine;
            talkNpcs = new[] { startNpc, sentry_roseline, harne };
            actItems = new[] { q_roseline_paper };
        }

        public override void tryAccept(L2Player player, L2Npc npc)
        {
            if ((player.BaseClass.ClassId.ClassRace == ClassRace.DARK_ELF) && (player.Level >= 3))
                player.ShowHtm("jasmine_q0008_0101.htm", npc, questId);
            else
                player.ShowHtm("jasmine_q0008_0102.htm", npc);
        }

        public override void onAccept(L2Player player, L2Npc npc)
        {
            player.questAccept(new QuestInfo(this));
            player.ShowHtm("jasmine_q0008_0104.htm", npc);
        }

        public override void onTalkToNpcQM(L2Player player, L2Npc npc, int reply)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            if ((reply == 1) && (npcId == sentry_roseline))
            {
                player.addItem(q_roseline_paper, 1);
                player.changeQuestStage(questId, 2);
                htmltext = "sentry_roseline_q0008_0201.htm";
            }
            else if ((reply == 1) && (npcId == harne))
            {
                player.takeItem(q_roseline_paper, 1);
                player.changeQuestStage(questId, 3);
                htmltext = "harne_q0008_0301.htm";
            }
            else if ((reply == 3) && (npcId == jasmine))
            {
                htmltext = "jasmine_q0008_0401.htm";
                player.addItem(escape_scroll_giran, 1);
                player.addItem(q_symbol_of_traveler, 1);
                player.finishQuest(questId);
            }

            player.ShowHtm(htmltext, npc);
        }

        public override void onTalkToNpc(L2Player player, L2Npc npc, int cond)
        {
            int npcId = npc.Template.NpcId;
            string htmltext = no_action_required;
            switch (npcId)
            {
                case jasmine:
                    switch (cond)
                    {
                        case 1:
                            htmltext = "jasmine_q0008_0105.htm";
                            break;
                        case 3:
                            htmltext = "jasmine_q0008_0301.htm";
                            break;
                    }

                    break;
                case sentry_roseline:
                    if (!player.hasItem(q_roseline_paper))
                        htmltext = "sentry_roseline_q0008_0101.htm";
                    else
                        htmltext = "sentry_roseline_q0008_0202.htm";
                    break;
                case harne:
                    switch (cond)
                    {
                        case 2:
                            if (player.hasItem(q_roseline_paper))
                                htmltext = "harne_q0008_0201.htm";
                            else if (!player.hasItem(q_roseline_paper))
                                htmltext = "harne_q0008_0302.htm";
                            break;
                        case 3:
                            htmltext = "harne_q0008_0303.htm";
                            break;
                    }

                    break;
            }

            player.ShowHtm(htmltext, npc);
        }
    }
}