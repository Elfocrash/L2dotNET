using L2dotNET.GameService;
using L2dotNET.GameService.Enums;
using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.model.player.basic;
using L2dotNET.GameService.model.quests;

class _0002_what_women_want : QuestOrigin
{
    const int arujien = 30223;
    const int mint = 30146;
    const int green = 30150;
    const int grain = 30157;

    const int arujiens_letter1 = 1092;
    const int arujiens_letter2 = 1093;
    const int arujiens_letter3 = 1094;
    const int poetry_book = 689;
    const int greenis_letter = 693;

    const int mage_earing = 113;

    public _0002_what_women_want()
    {
        questId = 2;
        questName = "What Women Want";
        startNpc = arujien;
        talkNpcs = new int[] { arujien, mint, green, grain };
        actItems = new int[] { arujiens_letter1, arujiens_letter2, arujiens_letter3, poetry_book, greenis_letter };
    }

    public override void tryAccept(L2Player player, L2Citizen npc)
    {
        if (player.Level < 2)
        {
            player.ShowHtm("arujien_q0002_01.htm", npc);
            return;
        }

        if (player.BaseClass.ClassId.ClassRace != ClassRace.ELF && player.BaseClass.ClassId.ClassRace != ClassRace.HUMAN)
        {
            player.ShowHtm("arujien_q0002_00.htm", npc);
            return;
        }

        player.ShowHtm("arujien_q0002_02.htm", npc);
    }

    public override void onAccept(L2Player player, L2Citizen npc)
    {
        if (!player.hasItem(arujiens_letter1))
            player.addItem(arujiens_letter1);

        player.questAccept(new QuestInfo(this));
        player.ShowHtm("arujien_q0002_04.htm", npc);
    }

    public override void onTalkToNpcQM(L2Player player, L2Citizen npc, int reply)
    {
        if (reply == 1)
        {
            player.ShowHtm("arujien_q0002_08.htm", npc);
        }
        else if (reply == 2)
        {
            player.ShowHtm("arujien_q0002_09.htm", npc);
        }
        //тут вроде бред. игнорим запросы. ставим 4ю кондицию
        player.takeItem(arujiens_letter3, 1);
        player.addItem(poetry_book);
        player.changeQuestStage(questId, 4);
    }

    public override void onTalkToNpc(L2Player player, L2Citizen npc, int cond)
    {
        int npcId = npc.Template.NpcId;
        string htmltext = no_action_required;
        if (npcId == arujien)
        {
            if (cond == 1 && player.hasItem(arujiens_letter1))
                htmltext = "arujien_q0002_05.htm";
            else if (cond == 2 && player.hasItem(arujiens_letter2))
                htmltext = "arujien_q0002_06.htm";
            else if (cond == 3 && player.hasItem(arujiens_letter3))
                htmltext = "arujien_q0002_07.htm";
            else if (cond == 4 && player.hasItem(poetry_book))
                htmltext = "arujien_q0002_11.htm";
            else if (cond == 5 && player.hasItem(greenis_letter))
            {
                htmltext = "arujien_q0002_09.htm";
                player.takeItem(greenis_letter, 1);
                player.addItem(mage_earing);
                player.addAdena(2300, true, true);
                player.addExpSp(4254, 335, true);
                player.finishQuest(questId);
            }
        }
        else if (npcId == mint)
        {
            if (cond == 1 && player.hasItem(arujiens_letter1))
            {
                htmltext = "mint_q0002_01.htm";
                player.takeItem(arujiens_letter1, 1);
                player.addItem(arujiens_letter2, 1);
                player.changeQuestStage(questId, 2);
            }
            else if (cond == 2)
                htmltext = "mint_q0002_02.htm";
        }
        else if (npcId == green)
        {
            if (cond == 2 && player.hasItem(arujiens_letter2))
            {
                htmltext = "green_q0002_01.htm";
                player.takeItem(arujiens_letter2, 1);
                player.addItem(arujiens_letter3, 1);
                player.changeQuestStage(questId, 3);
            }
            else if (cond == 3)
                htmltext = "green_q0002_02.htm";
        }
        else if (npcId == grain)
        {
            if (cond == 4 && player.hasItem(poetry_book))
            {
                htmltext = "grain_q0002_02.htm";
                player.takeItem(poetry_book, 1);
                player.addItem(greenis_letter, 1);
                player.changeQuestStage(questId, 5);
            }
            else if (cond == 5 && player.hasItem(greenis_letter))
                htmltext = "grain_q0002_03.htm";
            else
                htmltext = "grain_q0002_01.htm";
        }

        player.ShowHtm(htmltext, npc);
    }
}

