using System.Collections.Generic;
using L2dotNET.GameService.Model.Communities;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.AI.NpcAI
{
    public class guild_master : Template.AI
    {
        private const int @proof_of_blood = 57;
        private const int @q_proof_of_alliance = 57;
        private const int @q_proof_of_aspiration = 57;
        private const int @vow_of_blood = 9910;
        private const int @oath_of_blood = 9911;
        private const int @PP_MANAGE_GROWTH = 0;
        private const int @royalGuard = 2;
        private const int @knights = 4;
        private const int @EffectSkill1 = 334430209;

        public override void Talked(L2Player talker)
        {
            talker.ShowHtm(GetDialog("fnHi"), myself);
        }

        public override void TalkedReply(L2Player talker, int ask, int reply)
        {
            switch (ask)
            {
                case -3:
                    L2Clan pledge = talker.Clan;
                    byte pledgLv = pledge != null ? pledge.Level : (byte)0;

                    switch (reply)
                    {
                        case 0: //new clan
                            if (talker.Level < 10)
                                talker.ShowHtm("pl002.htm", myself);
                            else if (talker.Clan != null && talker.Clan.LeaderID == talker.ObjID)
                                talker.ShowHtm("pl003.htm", myself);
                            else if (talker.Clan != null && talker.Clan.LeaderID != talker.ObjID)
                                talker.ShowHtm("pl004.htm", myself);
                            else
                                talker.ShowHtm("pl005.htm", myself);
                            break;
                        case 1: //Повысить
                            if (talker.ClanLeader)
                                talker.ShowHtm("pl013.htm", myself);
                            else
                                talker.ShowHtm("pl014.htm", myself);
                            break;
                        case 2: //Распустить
                            if (talker.ClanLeader || talker.HavePledgePower(@PP_MANAGE_GROWTH))
                                talker.ShowHtm("pl007.htm", myself);
                            else
                                talker.ShowHtm("pl008.htm", myself);
                            break;
                        case 3: //Восстановить
                            if (talker.ClanLeader || talker.HavePledgePower(@PP_MANAGE_GROWTH))
                                talker.ShowHtm("pl010.htm", myself);
                            else
                                talker.ShowHtm("pl011.htm", myself);
                            break;
                        case 4: //Умения
                            if (talker.ClanLeader)
                                ShowEtcSkillList(talker, 2);
                            else
                                talker.ShowHtm("pl017.htm", myself);
                            break;
                        case 5: //Управлять Академией
                            talker.ShowHtm("pl_aca_help.htm", myself);
                            break;
                        case 6: //Управлять Подразделениями Стражей
                            talker.ShowHtm("pl_sub_help.htm", myself);
                            break;
                        case 7:
                            if (pledge == null)
                                talker.ShowHtm("pl_no_pledgeman.htm", myself);
                            else if (talker.ClanLeader || talker.HavePledgePower(@PP_MANAGE_GROWTH))
                            {
                                if (pledge.Level > 5)
                                {
                                    if (pledge.HasSubPledge(100))
                                    {
                                        if (pledge.HasSubPledge(200))
                                            talker.ShowHtm("pl_err_more_sub.htm", myself);
                                        else
                                            talker.ShowHtm("pl_create_sub200.htm", myself);
                                    }
                                    else
                                        talker.ShowHtm("pl_create_sub100.htm", myself);
                                }
                                else
                                    talker.ShowHtm("pl_err_plv.htm", myself);
                            }
                            else
                                talker.ShowHtm("pl_err_master.htm", myself);
                            break;
                        case 8:
                            if (pledge == null)
                                talker.ShowHtm("pl_no_pledgeman.htm", myself);
                            else if (talker.ClanLeader || talker.HavePledgePower(@PP_MANAGE_GROWTH))
                            {
                                if (pledge.Level > 5)
                                {
                                    if (pledge.HasSubPledge(100))
                                    {
                                        talker.ShowHtm("pl_submaster.htm", myself);
                                    }
                                    else
                                        talker.ShowHtm("pl_err_more_sm.htm", myself);
                                }
                                else
                                    talker.ShowHtm("pl_err_plv.htm", myself);
                            }
                            else
                                talker.ShowHtm("pl_err_master.htm", myself);
                            break;
                        case 9: //Управлять Подразделениями Рыцарей
                            talker.ShowHtm("pl_sub2_help.htm", myself);
                            break;
                        case 10:
                            if (pledge == null)
                                talker.ShowHtm("pl_no_pledgeman.htm", myself);
                            else if (talker.ClanLeader || talker.HavePledgePower(@PP_MANAGE_GROWTH))
                            {
                                if (pledge.Level > 6)
                                {
                                    if (pledge.HasSubPledge(100))
                                    {
                                        if (pledge.HasSubPledge(10001))
                                        {
                                            if (pledge.HasSubPledge(1002))
                                            {
                                                if (pledge.HasSubPledge(200))
                                                {
                                                    if (pledge.HasSubPledge(2001))
                                                    {
                                                        if (pledge.HasSubPledge(2002))
                                                        {
                                                            talker.ShowHtm("pl_err_more_sub2.htm", myself);
                                                        }
                                                        else
                                                            talker.ShowHtm("pl_create_sub2002.htm", myself);
                                                    }
                                                    else
                                                        talker.ShowHtm("pl_create_sub2001.htm", myself);
                                                }
                                                else
                                                    talker.ShowHtm("pl_need_high_lv_sub.htm", myself);
                                            }
                                            else
                                                talker.ShowHtm("pl_create_sub1002.htm", myself);
                                        }
                                        else
                                            talker.ShowHtm("pl_create_sub1001.htm", myself);
                                    }
                                    else
                                        talker.ShowHtm("pl_need_high_lv_sub.htm", myself);
                                }
                                else
                                    talker.ShowHtm("pl_err_plv.htm", myself);
                            }
                            else
                                talker.ShowHtm("pl_err_master.htm", myself);
                            break;
                        case 11:
                            if (pledge == null)
                                talker.ShowHtm("pl_no_pledgeman.htm", myself);
                            else if (talker.ClanLeader || talker.HavePledgePower(@PP_MANAGE_GROWTH))
                            {
                                if (pledge.Level > 6)
                                {
                                    if (pledge.HasSubPledge(100))
                                    {
                                        if (pledge.HasSubPledge(1001))
                                        {
                                            talker.ShowHtm("pl_submaster2.htm", myself);
                                        }
                                        else
                                            talker.ShowHtm("pl_err_more_sm2.htm", myself);
                                    }
                                    else
                                        talker.ShowHtm("pl_need_high_lv_sub.htm", myself);
                                }
                                else
                                    talker.ShowHtm("pl_err_plv.htm", myself);
                            }
                            else
                                talker.ShowHtm("pl_err_master.htm", myself);
                            break;
                        case 12:
                            if (pledge == null)
                                talker.ShowHtm("pl_no_pledgeman.htm", myself);
                            else if (talker.ClanLeader || talker.HavePledgePower(@PP_MANAGE_GROWTH))
                            {
                                if (pledge.Level > 4)
                                    talker.ShowHtm("pl_create_aca.htm", myself);
                                else
                                    talker.ShowHtm("pl_err_plv.htm", myself);
                            }
                            else
                                talker.ShowHtm("pl_err_master.htm", myself);
                            break;
                        case 13: //Передать Полномочия Лидера Клана
                            talker.ShowHtm("pl_master.htm", myself);
                            break;
                        case 14: //Запрос Передачи Полномочий Лидера Клана
                            if (talker.ClanLeader)
                                talker.ShowHtm("pl_transfer_master.htm", myself);
                            else
                                talker.ShowHtm("pl_err_master.htm", myself);
                            break;
                        case 15: //Отменить Запрос Передачи Полномочий Лидера Клана
                            if (talker.ClanLeader)
                                talker.ShowHtm("pl_cancel_master.htm", myself);
                            else
                                talker.ShowHtm("pl_err_master.htm", myself);
                            break;
                        case 16:
                            if (talker.ClanLeader || talker.HavePledgePower(@PP_MANAGE_GROWTH))
                                talker.ShowHtm("pl_rename.htm", myself);
                            else
                                talker.ShowHtm("pl_err_master.htm", myself);
                            break;
                        case 17:
                            if (talker.ClanLeader || talker.HavePledgePower(@PP_MANAGE_GROWTH))
                                talker.ShowHtm("pl_rename2.htm", myself);
                            else
                                talker.ShowHtm("pl_err_master.htm", myself);
                            break;
                        case 100: //main
                            talker.ShowHtm("pl001.htm", myself);
                            break;
                        case 101:
                            if (talker.ClanLeader)
                                ShowChangePledgeNameUI(talker);
                            else
                                talker.ShowHtm("pl_err_master.htm", myself);
                            break;
                    }
                    break;
                case -111:
                    if (!talker.ClanLeader || !talker.HavePledgePower(@PP_MANAGE_GROWTH))
                    {
                        talker.ShowHtm("pl_err_master.htm", myself);
                        return;
                    }

                    if (reply < 1000)
                    {
                        if (reply < 100)
                            talker.ShowHtm("pl_err_rename_aca.htm", myself);
                        else if (reply / 100 > royalGuard)
                            talker.ShowHtm("pl_err_more_sub.htm", myself);
                        else if (talker.Clan.HasSubPledge(reply))
                        {
                            NpcHtmlMessage htm = new NpcHtmlMessage(talker, "pl_ch_submaster" + reply + ".htm", myself.ObjID);
                            string mstr = talker.Clan.GetSubpledgeMasterName(reply) ?? FString.getInstance().get(1010642);
                            htm.replace("<?" + reply + "submaster?>", mstr);

                            talker.sendPacket(htm);
                        }
                        else
                            talker.ShowHtm("pl_err_more_sm.htm", myself);
                    }
                    else if (reply < 10000)
                    {
                        if (reply <= 1000 && reply % 1000 == 0)
                            talker.ShowHtm("pl_err_more_sm2.htm", myself);
                        else if (reply % 1000 > knights / royalGuard)
                            talker.ShowHtm("pl_err_more_sub2.htm", myself);
                        else if (talker.Clan.HasSubPledge(reply))
                        {
                            NpcHtmlMessage htm = new NpcHtmlMessage(talker, "pl_ch_submaster" + reply + ".htm", myself.ObjID);
                            string mstr = talker.Clan.GetSubpledgeMasterName(reply) ?? FString.getInstance().get(1010642);
                            htm.replace("<?" + reply + "submaster?>", mstr);

                            talker.sendPacket(htm);
                        }
                        else
                            talker.ShowHtm("pl_err_more_sm2.htm", myself);
                    }
                    break;
                case -222:
                    if (!talker.ClanLeader || !talker.HavePledgePower(@PP_MANAGE_GROWTH))
                    {
                        talker.ShowHtm("pl_err_master.htm", myself);
                        return;
                    }

                    if (reply == -1)
                    {
                        if (talker.Clan.HasSubPledge(reply))
                            talker.ShowHtm("pl_ch_rename_aca.htm", myself);
                        else
                            talker.ShowHtm("pl_err_rename_aca.htm", myself);
                    }
                    else if (reply < 1000)
                    {
                        if (reply / 100 > royalGuard)
                            talker.ShowHtm("pl_err_more_sub.htm", myself);
                        else if (talker.Clan.HasSubPledge(reply))
                            talker.ShowHtm("pl_ch_rename" + reply + ".htm", myself);
                        else
                            talker.ShowHtm("pl_err_rename_sub.htm", myself);
                    }
                    else if (reply < 10000)
                    {
                        if (reply % 1000 > knights / royalGuard)
                            talker.ShowHtm("pl_err_more_sub2.htm", myself);
                        else if (talker.Clan.HasSubPledge(reply))
                            talker.ShowHtm("pl_ch_rename" + reply + ".htm", myself);
                        else
                            talker.ShowHtm("pl_err_rename_sub2.htm", myself);
                    }
                    break;
                case -223:
                    byte lvl = talker.ClanLevel;
                    switch (reply)
                    {
                        case 0:
                            switch (lvl)
                            {
                                case 9:
                                    talker.ShowHtm(GetDialog("fnHalfUpgrade"), myself);
                                    break;
                                case 10:
                                case 11:
                                    talker.ShowHtm(GetDialog("fnFullUpgrade"), myself);
                                    break;
                                default:
                                    talker.ShowHtm(GetDialog("fnLowSkillLvForUpgrade"), myself);
                                    break;
                            }
                            break;
                        case 1:
                            if (lvl == 11)
                                talker.ShowHtm(GetDialog("fnFullUpgradeSub"), myself);
                            else
                                talker.ShowHtm(GetDialog("fnLowSkillLvForUpgradeSub"), myself);
                            break;
                    }
                    break;
                case -4:
                    if (reply == 0)
                        talker.ShowHtm("al005.htm", myself);
                    break;
            }
        }

        private void ShowChangePledgeNameUI(L2Player talker)
        {
            talker.sendMessage("ai.ShowChangePledgeNameUI");
        }

        private void ShowEtcSkillList(L2Player talker, int type)
        {
            TAcquireSkillsEntry skills = TSkillTable.Instance.GetPledgeSkills();
            SortedList<int, TAcquireSkill> avail = new SortedList<int, TAcquireSkill>();

            int nextLvl = 800;
            foreach (TAcquireSkill e in skills.skills)
            {
                if (e.get_lv > talker.Clan.Level)
                {
                    if (nextLvl > e.get_lv)
                        nextLvl = e.get_lv;
                    continue;
                }

                if (avail.ContainsKey(e.id))
                    continue;

                if (talker.Clan.MainSkill.ContainsKey(e.id))
                {
                    if (talker.Clan.MainSkill[e.id] >= e.lv)
                        continue;

                    if (!avail.ContainsKey(e.id))
                    {
                        avail.Add(e.id, e);
                        break;
                    }
                }
                else
                    avail.Add(e.id, e);
            }

            talker.ActiveSkillTree = avail;
            talker.sendPacket(new AcquireSkillList(AcquireSkillList.ESTT_CLAN, talker));
        }

        public override void TalkedBypass(L2Player talker, string bypass)
        {
            if (bypass == "pledge_levelup")
            {
                byte level = talker.ClanLevel;

                switch (level)
                {
                    case 0:
                        ValidateLevelUp1(talker, 20000, @adena, 650000, 1);
                        break;
                    case 1:
                        ValidateLevelUp1(talker, 100000, @adena, 2500000, 2);
                        break;
                    case 2:
                        ValidateLevelUp1(talker, 350000, @proof_of_blood, 1, 3);
                        break;
                    case 3:
                        ValidateLevelUp1(talker, 1000000, @q_proof_of_alliance, 1, 4);
                        break;
                    case 4:
                        ValidateLevelUp1(talker, 2500000, @q_proof_of_aspiration, 1, 5);
                        break;
                    case 5:
                        ValidateLevelUp2(talker, 10000, 30, 6);
                        break;
                    case 6:
                        ValidateLevelUp2(talker, 20000, 80, 7);
                        break;
                    case 7:
                        ValidateLevelUp2(talker, 40000, 120, 8);
                        break;
                    case 8:
                        ValidateLevelUp2(talker, 40000, 120, 9, @vow_of_blood, 150);
                        break;
                    case 9:
                        ValidateLevelUp2(talker, 40000, 140, 10, @oath_of_blood, 5);
                        break;
                    case 10:
                        ValidateLevelUp2(talker, 75000, 170, 11, @oath_of_blood, 5, true);
                        break;
                }
            }
            else if (bypass.StartsWith("create_pledge"))
            {
                CreateClan(talker, bypass.Substring("create_pledge?pledge_name= ".Length));
            }
        }

        private void CreateClan(L2Player talker, string name)
        {
            L2Clan clan = new L2Clan();
            clan.LeaderID = talker.ObjID;
            clan.ClanMasterName = talker.Name;
            clan.Name = name;
            clan.Level = 0;
            clan.ClanID = IdFactory.Instance.nextId();

            clan.addMember(talker, 0);

            talker.ClanId = clan.ClanID;
            talker.ClanPrivs = L2Clan.CP_ALL;

            talker.broadcastUserInfo();
            talker.sendPacket(new PledgeShowMemberListAll(clan, 0));

            talker.ShowHtm("pl006.htm", myself);

            //SQL_Block sqb = new SQL_Block("clan_data");
            //sqb.param("id", clan.ClanID);
            //sqb.param("name", clan.Name);
            //sqb.param("level", clan.Level);
            //sqb.param("leaderId", clan.LeaderID);
            //sqb.param("leaderName", clan.ClanMasterName);
            //sqb.param("crestId", clan.CrestID);
            //sqb.param("crestBigId", clan.LargeCrestID);
            //sqb.param("castleId", clan.CastleID);
            //sqb.param("agitId", clan.HideoutID);
            //sqb.param("fortId", clan.FortressID);
            //sqb.param("dominionId", clan.JoinDominionWarID);
            //sqb.param("allyId", clan.AllianceID);
            //sqb.sql_insert(false);

            //talker.updateDb();
        }

        private void ValidateLevelUp1(L2Player talker, int sp, int id, long count, byte lvl)
        {
            if (talker.SP >= sp && talker.hasItem(id, count))
            {
                talker.addExpSp(0, -sp, false);
                talker.takeItem(id, count);
                talker.broadcastSkillUse(@EffectSkill1);
                talker.Clan.LevelUp(lvl);
            }
            else
                talker.ShowHtm("pl016.htm", myself);
        }

        private void ValidateLevelUp2(L2Player talker, int rp, byte memCount, byte lvl, int id = 0, long count = 0, bool dominionCheck = false)
        {
            if (talker.Clan.ClanNameValue >= rp)
            {
                if (talker.Clan.getClanMembers().Count >= memCount)
                {
                    bool yup = false;
                    if (id > 0)
                    {
                        if (talker.hasItem(id, count))
                        {
                            yup = true;
                            talker.takeItem(id, count);
                        }
                        else
                            talker.ShowHtm("pl_err_not_enough_items.htm", myself);
                    }
                    else
                        yup = true;

                    if (dominionCheck)
                    {
                        if (talker.Clan.DominionLord())
                            yup = true;
                        else
                        {
                            yup = false;
                            talker.ShowHtm("pl_err_not_lord_yet.htm", myself);
                        }
                    }

                    if (yup)
                    {
                        talker.Clan.UpdatePledgeNameValue(rp);
                        talker.broadcastSkillUse(@EffectSkill1);
                        talker.Clan.LevelUp(lvl);
                    }
                }
                else
                    talker.ShowHtm("pl_err_total_member.htm", myself);
            }
            else
                talker.ShowHtm("pl_err_fame.htm", myself);
        }
    }
}