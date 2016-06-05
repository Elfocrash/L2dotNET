using L2dotNET.GameService;
using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.model.quests;

class _0001_letters_of_love : QuestOrigin
{
    const int darings_letter = 687;
    const int rapunzels_kerchief = 688;
    const int darings_receipt = 1079;
    const int bauls_potion = 1080;

    const int daring = 30048;
    const int rapunzel = 30006;
    const int baul = 30033;

    const int necklace_of_knowledge = 906;

    public _0001_letters_of_love()
    {
        questId = 1;
        questName = "Letters of Love";
        startNpc = daring;
        talkNpcs = new int[] { startNpc, rapunzel, baul };
        actItems = new int[] { darings_letter, rapunzels_kerchief, darings_receipt, bauls_potion };
    }

    public override void tryAccept(L2Player player, L2Citizen npc)
    {
        if (player.Level < 2)
        {
            player.ShowHtm("daring_q0001_01.htm", npc);
            return;
        }

        player.ShowHtm("daring_q0001_02.htm", npc, questId);
    }

    public override void onAccept(L2Player player, L2Citizen npc)
    {
        if (!player.hasItem(darings_letter))
            player.addItem(darings_letter);

        player.questAccept(new QuestInfo(this));
        player.ShowHtm("daring_q0001_06.htm", npc);
    }

    public override void onTalkToNpc(L2Player player, L2Citizen npc, int route)
    {
        switch (route)
        {
            case 1:
                {
                    switch (npc.Template.NpcId)
                    {
                        case daring:
                            player.ShowHtm("daring_q0001_07.htm", npc);
                            break;
                        case rapunzel:
                            player.ShowHtm("rapunzel_q0001_01.htm", npc);
                            player.takeItem(darings_letter, 1);
                            player.addItem(rapunzels_kerchief);
                            player.changeQuestStage(questId, 2);
                            break;

                        default:
                            player.ShowHtmPlain(no_action_required, npc);
                            break;
                    }
                }
                break;
            case 2:
                {
                    switch (npc.Template.NpcId)
                    {
                        case daring:
                            player.ShowHtm("daring_q0001_08.htm", npc);
                            player.takeItem(rapunzels_kerchief, 1);
                            player.addItem(darings_receipt);
                            player.changeQuestStage(questId, 3);
                            break;
                        case rapunzel:
                            player.ShowHtm("rapunzel_q0001_02.htm", npc);
                            break;

                        default:
                            player.ShowHtmPlain(no_action_required, npc);
                            break;
                    }
                }
                break;
            case 3:
                {
                    switch (npc.Template.NpcId)
                    {
                        case daring:
                            player.ShowHtm("daring_q0001_09.htm", npc);
                            break;
                        case baul:
                            player.ShowHtm("baul_q0001_01.htm", npc);
                            player.takeItem(darings_receipt, 1);
                            player.addItem(bauls_potion);
                            player.changeQuestStage(questId, 4);
                            break;

                        default:
                            player.ShowHtmPlain(no_action_required, npc);
                            break;
                    }
                }
                break;
            case 4:
                {
                    switch (npc.Template.NpcId)
                    {
                        case baul:
                            player.ShowHtm("baul_q0001_02.htm", npc);
                            break;
                        case daring:
                            player.ShowHtm("daring_q0001_10.htm", npc);
                            player.takeItem(bauls_potion, 1);
                            player.addItem(necklace_of_knowledge);
                            player.addItem(57, 2466);
                            player.addExpSp(5672, 446, true);
                            player.finishQuest(questId);
                            break;

                        default:
                            player.ShowHtmPlain(no_action_required, npc);
                            break;
                    }
                }
                break;
        }
    }
}

