using System.Collections.Generic;
using L2dotNET.AI.template;
using L2dotNET.model.communities;
using L2dotNET.model.player;
using L2dotNET.model.skills2;
using L2dotNET.Network.serverpackets;
using L2dotNET.tables;
using L2dotNET.Utility;

namespace L2dotNET.AI.npcai
{
    public class GuildMaster : Ai
    {
        private const int ProofOfBlood = 57;
        private const int QProofOfAlliance = 57;
        private const int QProofOfAspiration = 57;
        private const int VowOfBlood = 9910;
        private const int OathOfBlood = 9911;
        private const int PpManageGrowth = 0;
        private const int RoyalGuard = 2;
        private const int Knights = 4;
        private const int EffectSkill1 = 334430209;

        public override void Talked(L2Player talker)
        {
            talker.ShowHtm(GetDialog("fnHi"), Myself);
        }

        public override void TalkedReply(L2Player talker, int ask, int reply)
        {
            switch (ask)
            {
                case -3:
                    L2Clan pledge = talker.Clan;

                    switch (reply)
                    {
                        case 0: //new clan
                            if (talker.Level < 10)
                                talker.ShowHtm("pl002.htm", Myself);
                            else
                            {
                                if ((talker.Clan != null) && (talker.Clan.LeaderId == talker.ObjId))
                                    talker.ShowHtm("pl003.htm", Myself);
                                else
                                {
                                    if ((talker.Clan != null) && (talker.Clan.LeaderId != talker.ObjId))
                                        talker.ShowHtm("pl004.htm", Myself);
                                    else
                                        talker.ShowHtm("pl005.htm", Myself);
                                }
                            }
                            break;
                        case 1: //Повысить
                            if (talker.ClanLeader)
                                talker.ShowHtm("pl013.htm", Myself);
                            else
                                talker.ShowHtm("pl014.htm", Myself);
                            break;
                        case 2: //Распустить
                            if (talker.ClanLeader || talker.HavePledgePower(PpManageGrowth))
                                talker.ShowHtm("pl007.htm", Myself);
                            else
                                talker.ShowHtm("pl008.htm", Myself);
                            break;
                        case 3: //Восстановить
                            if (talker.ClanLeader || talker.HavePledgePower(PpManageGrowth))
                                talker.ShowHtm("pl010.htm", Myself);
                            else
                                talker.ShowHtm("pl011.htm", Myself);
                            break;
                        case 4: //Умения
                            if (talker.ClanLeader)
                                ShowEtcSkillList(talker);
                            else
                                talker.ShowHtm("pl017.htm", Myself);
                            break;
                        case 5: //Управлять Академией
                            talker.ShowHtm("pl_aca_help.htm", Myself);
                            break;
                        case 6: //Управлять Подразделениями Стражей
                            talker.ShowHtm("pl_sub_help.htm", Myself);
                            break;
                        case 7:
                            if (pledge == null)
                                talker.ShowHtm("pl_no_pledgeman.htm", Myself);
                            else
                            {
                                if (talker.ClanLeader || talker.HavePledgePower(PpManageGrowth))
                                {
                                    if (pledge.Level > 5)
                                    {
                                        if (pledge.HasSubPledge(100))
                                        {
                                            if (pledge.HasSubPledge(200))
                                                talker.ShowHtm("pl_err_more_sub.htm", Myself);
                                            else
                                                talker.ShowHtm("pl_create_sub200.htm", Myself);
                                        }
                                        else
                                            talker.ShowHtm("pl_create_sub100.htm", Myself);
                                    }
                                    else
                                        talker.ShowHtm("pl_err_plv.htm", Myself);
                                }
                                else
                                    talker.ShowHtm("pl_err_master.htm", Myself);
                            }
                            break;
                        case 8:
                            if (pledge == null)
                                talker.ShowHtm("pl_no_pledgeman.htm", Myself);
                            else
                            {
                                if (talker.ClanLeader || talker.HavePledgePower(PpManageGrowth))
                                {
                                    if (pledge.Level > 5)
                                    {
                                        if (pledge.HasSubPledge(100))
                                            talker.ShowHtm("pl_submaster.htm", Myself);
                                        else
                                            talker.ShowHtm("pl_err_more_sm.htm", Myself);
                                    }
                                    else
                                        talker.ShowHtm("pl_err_plv.htm", Myself);
                                }
                                else
                                    talker.ShowHtm("pl_err_master.htm", Myself);
                            }
                            break;
                        case 9: //Управлять Подразделениями Рыцарей
                            talker.ShowHtm("pl_sub2_help.htm", Myself);
                            break;
                        case 10:
                            if (pledge == null)
                                talker.ShowHtm("pl_no_pledgeman.htm", Myself);
                            else
                            {
                                if (talker.ClanLeader || talker.HavePledgePower(PpManageGrowth))
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
                                                                talker.ShowHtm("pl_err_more_sub2.htm", Myself);
                                                            else
                                                                talker.ShowHtm("pl_create_sub2002.htm", Myself);
                                                        }
                                                        else
                                                            talker.ShowHtm("pl_create_sub2001.htm", Myself);
                                                    }
                                                    else
                                                        talker.ShowHtm("pl_need_high_lv_sub.htm", Myself);
                                                }
                                                else
                                                    talker.ShowHtm("pl_create_sub1002.htm", Myself);
                                            }
                                            else
                                                talker.ShowHtm("pl_create_sub1001.htm", Myself);
                                        }
                                        else
                                            talker.ShowHtm("pl_need_high_lv_sub.htm", Myself);
                                    }
                                    else
                                        talker.ShowHtm("pl_err_plv.htm", Myself);
                                }
                                else
                                    talker.ShowHtm("pl_err_master.htm", Myself);
                            }
                            break;
                        case 11:
                            if (pledge == null)
                                talker.ShowHtm("pl_no_pledgeman.htm", Myself);
                            else
                            {
                                if (talker.ClanLeader || talker.HavePledgePower(PpManageGrowth))
                                {
                                    if (pledge.Level > 6)
                                    {
                                        if (pledge.HasSubPledge(100))
                                        {
                                            if (pledge.HasSubPledge(1001))
                                                talker.ShowHtm("pl_submaster2.htm", Myself);
                                            else
                                                talker.ShowHtm("pl_err_more_sm2.htm", Myself);
                                        }
                                        else
                                            talker.ShowHtm("pl_need_high_lv_sub.htm", Myself);
                                    }
                                    else
                                        talker.ShowHtm("pl_err_plv.htm", Myself);
                                }
                                else
                                    talker.ShowHtm("pl_err_master.htm", Myself);
                            }
                            break;
                        case 12:
                            if (pledge == null)
                                talker.ShowHtm("pl_no_pledgeman.htm", Myself);
                            else
                            {
                                if (talker.ClanLeader || talker.HavePledgePower(PpManageGrowth))
                                {
                                    if (pledge.Level > 4)
                                        talker.ShowHtm("pl_create_aca.htm", Myself);
                                    else
                                        talker.ShowHtm("pl_err_plv.htm", Myself);
                                }
                                else
                                    talker.ShowHtm("pl_err_master.htm", Myself);
                            }
                            break;
                        case 13: //Передать Полномочия Лидера Клана
                            talker.ShowHtm("pl_master.htm", Myself);
                            break;
                        case 14: //Запрос Передачи Полномочий Лидера Клана
                            if (talker.ClanLeader)
                                talker.ShowHtm("pl_transfer_master.htm", Myself);
                            else
                                talker.ShowHtm("pl_err_master.htm", Myself);
                            break;
                        case 15: //Отменить Запрос Передачи Полномочий Лидера Клана
                            if (talker.ClanLeader)
                                talker.ShowHtm("pl_cancel_master.htm", Myself);
                            else
                                talker.ShowHtm("pl_err_master.htm", Myself);
                            break;
                        case 16:
                            if (talker.ClanLeader || talker.HavePledgePower(PpManageGrowth))
                                talker.ShowHtm("pl_rename.htm", Myself);
                            else
                                talker.ShowHtm("pl_err_master.htm", Myself);
                            break;
                        case 17:
                            if (talker.ClanLeader || talker.HavePledgePower(PpManageGrowth))
                                talker.ShowHtm("pl_rename2.htm", Myself);
                            else
                                talker.ShowHtm("pl_err_master.htm", Myself);
                            break;
                        case 100: //main
                            talker.ShowHtm("pl001.htm", Myself);
                            break;
                        case 101:
                            if (talker.ClanLeader)
                                ShowChangePledgeNameUi(talker);
                            else
                                talker.ShowHtm("pl_err_master.htm", Myself);
                            break;
                    }

                    break;
                case -111:
                    if (!talker.ClanLeader || !talker.HavePledgePower(PpManageGrowth))
                    {
                        talker.ShowHtm("pl_err_master.htm", Myself);
                        return;
                    }

                    if (reply < 1000)
                    {
                        if (reply < 100)
                            talker.ShowHtm("pl_err_rename_aca.htm", Myself);
                        else
                        {
                            if ((reply / 100) > RoyalGuard)
                                talker.ShowHtm("pl_err_more_sub.htm", Myself);
                            else
                            {
                                if (talker.Clan.HasSubPledge(reply))
                                {
                                    NpcHtmlMessage htm = new NpcHtmlMessage(talker, $"pl_ch_submaster{reply}.htm", Myself.ObjId);
                                    string mstr = talker.Clan.GetSubpledgeMasterName(reply) ?? FString.GetInstance().Get(1010642);
                                    htm.Replace($"<?{reply}submaster?>", mstr);

                                    talker.SendPacket(htm);
                                }
                                else
                                    talker.ShowHtm("pl_err_more_sm.htm", Myself);
                            }
                        }
                    }
                    else
                    {
                        if (reply < 10000)
                        {
                            if ((reply <= 1000) && ((reply % 1000) == 0))
                                talker.ShowHtm("pl_err_more_sm2.htm", Myself);
                            else
                            {
                                if ((reply % 1000) > (Knights / RoyalGuard))
                                    talker.ShowHtm("pl_err_more_sub2.htm", Myself);
                                else
                                {
                                    if (talker.Clan.HasSubPledge(reply))
                                    {
                                        NpcHtmlMessage htm = new NpcHtmlMessage(talker, $"pl_ch_submaster{reply}.htm", Myself.ObjId);
                                        string mstr = talker.Clan.GetSubpledgeMasterName(reply) ?? FString.GetInstance().Get(1010642);
                                        htm.Replace($"<?{reply}submaster?>", mstr);

                                        talker.SendPacket(htm);
                                    }
                                    else
                                        talker.ShowHtm("pl_err_more_sm2.htm", Myself);
                                }
                            }
                        }
                    }
                    break;
                case -222:
                    if (!talker.ClanLeader || !talker.HavePledgePower(PpManageGrowth))
                    {
                        talker.ShowHtm("pl_err_master.htm", Myself);
                        return;
                    }

                    if (reply == -1)
                    {
                        if (talker.Clan.HasSubPledge(reply))
                            talker.ShowHtm("pl_ch_rename_aca.htm", Myself);
                        else
                            talker.ShowHtm("pl_err_rename_aca.htm", Myself);
                    }
                    else
                    {
                        if (reply < 1000)
                        {
                            if ((reply / 100) > RoyalGuard)
                                talker.ShowHtm("pl_err_more_sub.htm", Myself);
                            else
                            {
                                if (talker.Clan.HasSubPledge(reply))
                                    talker.ShowHtm($"pl_ch_rename{reply}.htm", Myself);
                                else
                                    talker.ShowHtm("pl_err_rename_sub.htm", Myself);
                            }
                        }
                        else
                        {
                            if (reply < 10000)
                            {
                                if ((reply % 1000) > (Knights / RoyalGuard))
                                    talker.ShowHtm("pl_err_more_sub2.htm", Myself);
                                else
                                {
                                    if (talker.Clan.HasSubPledge(reply))
                                        talker.ShowHtm($"pl_ch_rename{reply}.htm", Myself);
                                    else
                                        talker.ShowHtm("pl_err_rename_sub2.htm", Myself);
                                }
                            }
                        }
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
                                    talker.ShowHtm(GetDialog("fnHalfUpgrade"), Myself);
                                    break;
                                case 10:
                                case 11:
                                    talker.ShowHtm(GetDialog("fnFullUpgrade"), Myself);
                                    break;
                                default:
                                    talker.ShowHtm(GetDialog("fnLowSkillLvForUpgrade"), Myself);
                                    break;
                            }

                            break;
                        case 1:
                            if (lvl == 11)
                                talker.ShowHtm(GetDialog("fnFullUpgradeSub"), Myself);
                            else
                                talker.ShowHtm(GetDialog("fnLowSkillLvForUpgradeSub"), Myself);
                            break;
                    }

                    break;
                case -4:
                    if (reply == 0)
                        talker.ShowHtm("al005.htm", Myself);
                    break;
            }
        }

        private void ShowChangePledgeNameUi(L2Player talker)
        {
            talker.SendMessage("ai.ShowChangePledgeNameUI");
        }

        private void ShowEtcSkillList(L2Player talker)
        {
            AcquireSkillsEntry skills = SkillTable.Instance.GetPledgeSkills();
            SortedList<int, AcquireSkill> avail = new SortedList<int, AcquireSkill>();

            int nextLvl = 800;
            foreach (AcquireSkill e in skills.Skills)
            {
                if (e.GetLv > talker.Clan.Level)
                {
                    if (nextLvl > e.GetLv)
                        nextLvl = e.GetLv;
                    continue;
                }

                if (avail.ContainsKey(e.Id))
                    continue;

                if (talker.Clan.MainSkill.ContainsKey(e.Id))
                {
                    if (talker.Clan.MainSkill[e.Id] >= e.Lv)
                        continue;

                    if (avail.ContainsKey(e.Id))
                        continue;

                    avail.Add(e.Id, e);
                    break;
                }

                avail.Add(e.Id, e);
            }

            talker.ActiveSkillTree = avail;
            talker.SendPacket(new AcquireSkillList(AcquireSkillList.SkillType.Clan));
        }

        public override void TalkedBypass(L2Player talker, string bypass)
        {
            if (bypass == "pledge_levelup")
            {
                byte level = talker.ClanLevel;

                switch (level)
                {
                    case 0:
                        ValidateLevelUp1(talker, 20000, Adena, 650000, 1);
                        break;
                    case 1:
                        ValidateLevelUp1(talker, 100000, Adena, 2500000, 2);
                        break;
                    case 2:
                        ValidateLevelUp1(talker, 350000, ProofOfBlood, 1, 3);
                        break;
                    case 3:
                        ValidateLevelUp1(talker, 1000000, QProofOfAlliance, 1, 4);
                        break;
                    case 4:
                        ValidateLevelUp1(talker, 2500000, QProofOfAspiration, 1, 5);
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
                        ValidateLevelUp2(talker, 40000, 120, 9, VowOfBlood, 150);
                        break;
                    case 9:
                        ValidateLevelUp2(talker, 40000, 140, 10, OathOfBlood, 5);
                        break;
                    case 10:
                        ValidateLevelUp2(talker, 75000, 170, 11, OathOfBlood, 5, true);
                        break;
                }
            }
            else
            {
                if (bypass.StartsWithIgnoreCase("create_pledge"))
                    CreateClan(talker, bypass.Substring("create_pledge?pledge_name= ".Length));
            }
        }

        private void CreateClan(L2Player talker, string name)
        {
            L2Clan clan = new L2Clan
            {
                LeaderId = talker.ObjId,
                ClanMasterName = talker.Name,
                Name = name,
                Level = 0,
                ClanId = IdFactory.Instance.NextId()
            };

            clan.AddMember(talker, 0);

            talker.ClanId = clan.ClanId;
            talker.ClanPrivs = L2Clan.CpAll;

            talker.BroadcastUserInfo();
            talker.SendPacket(new PledgeShowMemberListAll(clan, 0));

            talker.ShowHtm("pl006.htm", Myself);

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

        private void ValidateLevelUp1(L2Player talker, int sp, int id, int count, byte lvl)
        {
            if ((talker.Sp >= sp) && talker.HasItem(id, count))
            {
                talker.AddExpSp(0, -sp, false);
                talker.DestroyItemById(id, count);
                talker.BroadcastSkillUse(EffectSkill1);
                talker.Clan.LevelUp(lvl);
            }
            else
                talker.ShowHtm("pl016.htm", Myself);
        }

        private void ValidateLevelUp2(L2Player talker, int rp, byte memCount, byte lvl, int id = 0, int count = 0, bool dominionCheck = false)
        {
            if (talker.Clan.ClanNameValue >= rp)
            {
                if (talker.Clan.GetClanMembers().Count >= memCount)
                {
                    bool yup = false;
                    if (id > 0)
                    {
                        if (talker.HasItem(id, count))
                        {
                            yup = true;
                            talker.DestroyItemById(id, count);
                        }
                        else
                            talker.ShowHtm("pl_err_not_enough_items.htm", Myself);
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
                            talker.ShowHtm("pl_err_not_lord_yet.htm", Myself);
                        }
                    }

                    if (!yup)
                        return;

                    talker.Clan.UpdatePledgeNameValue(rp);
                    talker.BroadcastSkillUse(EffectSkill1);
                    talker.Clan.LevelUp(lvl);
                }
                else
                    talker.ShowHtm("pl_err_total_member.htm", Myself);
            }
            else
                talker.ShowHtm("pl_err_fame.htm", Myself);
        }
    }
}