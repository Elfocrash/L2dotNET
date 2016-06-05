using log4net;
using L2dotNET.GameService.model.npcs.ai;
using L2dotNET.GameService.model.npcs.decor;
using L2dotNET.GameService.model.skills2;
using L2dotNET.GameService.model.structures;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tables;

namespace L2dotNET.GameService.model.npcs
{
    class L2HideoutManager : L2Citizen
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(L2HideoutManager));
        private readonly Hideout hideout;
        private readonly AgitManagerAI ai;
        public L2HideoutManager(HideoutTemplate hideout)
        {
            this.hideout = (Hideout)hideout;
            structureControlled = true;
            ai = new AgitManagerAI();
            CurMP = 5000;
        }

        private System.Timers.Timer regenMpTime;

        public void StartRegenTime()
        {
            if (regenMpTime == null)
            {
                regenMpTime = new System.Timers.Timer();
                regenMpTime.Interval = 2000;
                regenMpTime.Elapsed += new System.Timers.ElapsedEventHandler(RegenTask);
            }

            regenMpTime.Enabled = true;
        }

        private void RegenTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            int lvl = hideout.GetFuncLevel(AgitManagerAI.decotype_buff);
            CurMP += ai.regenPerSec[lvl];
            if (CurMP >= ai.regenMax[lvl])
                CurMP = ai.regenMax[lvl];
        }

        public override void onSpawn()
        {
            base.onSpawn();
            StartRegenTime();
        }

        public override void NotifyAction(L2Player player)
        {
            player.sendPacket(new NpcHtmlMessage(player, ai.fnHi, ObjID));
        }

        public override void onTeleportRequest(L2Player player)
        {
            if (hideout.NoTeleports)
            {
                player.sendPacket(new NpcHtmlMessage(player, ai.fnTeleportLevelZero, ObjID));
                return;
            }

            int level = hideout.GetFuncLevel(AgitManagerAI.decotype_teleport);
            if (level == 0)
                player.sendPacket(new NpcHtmlMessage(player, ai.fnFuncDisabled, ObjID));
            else
                NpcData.Instance.RequestTeleportList(this, player, level);
        }

        public override void onDialog(L2Player player, int ask, int reply)
        {
            player.FolkNpc = this;
            short result = 0;
            switch (ask)
            {
                case 0:
                    NotifyAction(player);
                    break;
                case -201:
                    switch (reply)
                    {
                        case 0:
                            NotifyAction(player);
                            break;
                        case 1: //doors
                            if (player.Clan.LeaderID == player.ObjID) //TODO privs
                                player.sendPacket(new NpcHtmlMessage(player, ai.fnDoor, ObjID));
                            else
                                player.teleport(hideout.outsideLoc[0], hideout.outsideLoc[1], hideout.outsideLoc[2]);
                            break;
                        case 2: //banish
                            player.sendPacket(new NpcHtmlMessage(player, ai.fnBanish, ObjID));
                            break;
                        case 3: //functions
                            {
                                NpcHtmlMessage htm = new NpcHtmlMessage(player, ai.fnDecoFunction, ObjID);
                                htm.replace("<?HPDepth?>", hideout.GetFuncDepth(AgitManagerAI.decotype_hpregen));
                                htm.replace("<?MPDepth?>", hideout.GetFuncDepth(AgitManagerAI.decotype_mpregen));
                                htm.replace("<?XPDepth?>", hideout.GetFuncDepth(AgitManagerAI.decotype_xprestore));
                                player.sendPacket(htm);
                            }
                            break;
                        case 4: // warehouse
                            {
                                NpcHtmlMessage htm = new NpcHtmlMessage(player, ai.fnWarehouse, ObjID);
                                htm.replace("<?agit_lease?>", hideout.RentCost);
                                htm.replace("<?pay_time?>", hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                                player.sendPacket(htm);
                            }
                            break;
                        case 5: // manage
                            player.sendPacket(new NpcHtmlMessage(player, ai.fnManage, ObjID));
                            break;
                        case 7: //use buff
                            {
                                int level = hideout.GetFuncLevel(AgitManagerAI.decotype_buff);
                                if (level == 0)
                                {
                                    player.sendPacket(new NpcHtmlMessage(player, ai.fnFuncDisabled, ObjID));
                                }
                                else
                                {
                                    NpcHtmlMessage htm = new NpcHtmlMessage(player, ai.fnAgitBuff + "_" + level + ".htm", ObjID);
                                    htm.replace("<?MPLeft?>", (int)CurMP);
                                    player.sendPacket(htm);
                                }
                            }
                            break;
                        case 12: //use itemcreate
                            {
                                int level = hideout.GetFuncLevel(AgitManagerAI.decotype_item);
                                if (level == 0)
                                    player.sendPacket(new NpcHtmlMessage(player, ai.fnFuncDisabled, ObjID));
                                else
                                    NpcData.Instance.Buylist(player, this, (short)level);
                            }
                            break;
                        case 51: // manage regen
                            {
                                NpcHtmlMessage htm = new NpcHtmlMessage(player, ai.fnManageRegen, ObjID);
                                htm.replace("<?HPDepth?>", hideout.GetFuncDepth(AgitManagerAI.decotype_hpregen));
                                htm.replace("<?HPCost?>", hideout.GetCurrentDecoCost(AgitManagerAI.decotype_hpregen));
                                htm.replace("<?HPExpire?>", hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                                htm.replace("<?HPReset?>", "");

                                htm.replace("<?MPDepth?>", hideout.GetFuncDepth(AgitManagerAI.decotype_mpregen));
                                htm.replace("<?MPCost?>", hideout.GetCurrentDecoCost(AgitManagerAI.decotype_mpregen));
                                htm.replace("<?MPExpire?>", hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                                htm.replace("<?MPReset?>", "");

                                htm.replace("<?XPDepth?>", hideout.GetFuncDepth(AgitManagerAI.decotype_xprestore));
                                htm.replace("<?XPCost?>", hideout.GetCurrentDecoCost(AgitManagerAI.decotype_xprestore));
                                htm.replace("<?XPExpire?>", hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                                htm.replace("<?XPReset?>", "");
                                player.sendPacket(htm);
                            }
                            break;
                        case 52: // manage etc
                            {
                                NpcHtmlMessage htm = new NpcHtmlMessage(player, ai.fnManageEtc, ObjID);
                                htm.replace("<?TPDepth?>", hideout.GetFuncDepth(AgitManagerAI.decotype_teleport));
                                htm.replace("<?TPCost?>", hideout.GetCurrentDecoCost(AgitManagerAI.decotype_teleport));
                                htm.replace("<?TPExpire?>", hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                                htm.replace("<?TPReset?>", "");

                                htm.replace("<?BFDepth?>", hideout.GetFuncDepth(AgitManagerAI.decotype_buff));
                                htm.replace("<?BFCost?>", hideout.GetCurrentDecoCost(AgitManagerAI.decotype_buff));
                                htm.replace("<?BFExpire?>", hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                                htm.replace("<?BFReset?>", "");

                                htm.replace("<?ICDepth?>", hideout.GetFuncDepth(AgitManagerAI.decotype_item));
                                htm.replace("<?ICCost?>", hideout.GetCurrentDecoCost(AgitManagerAI.decotype_item));
                                htm.replace("<?ICExpire?>", hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                                htm.replace("<?ICReset?>", "");
                                player.sendPacket(htm);
                            }
                            break;
                        case 53: // manage deco
                            {
                                NpcHtmlMessage htm = new NpcHtmlMessage(player, ai.fnManageDeco, ObjID);
                                htm.replace("<?7_Depth?>", hideout.GetFuncDepth(AgitManagerAI.decotype_curtain));
                                htm.replace("<?7_Cost?>", hideout.GetCurrentDecoCost(AgitManagerAI.decotype_curtain));
                                htm.replace("<?7_Expire?>", hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                                htm.replace("<?7_Reset?>", "");

                                htm.replace("<?11_Depth?>", hideout.GetFuncDepth(AgitManagerAI.decotype_platform));
                                htm.replace("<?11_Cost?>", hideout.GetCurrentDecoCost(AgitManagerAI.decotype_platform));
                                htm.replace("<?11_Expire?>", hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                                htm.replace("<?11_Reset?>", "");
                                player.sendPacket(htm);
                            }
                            break;

                    }
                    break;
                case -203:
                    switch (reply)
                    {
                        case 1: //open doors
                            foreach (L2Door door in hideout.doors)
                            {
                                if (door.Closed == 0)
                                    continue;

                                door.Closed = 0;
                                door.broadcastUserInfo();
                            }

                            player.sendPacket(new NpcHtmlMessage(player, ai.fnAfterDoorOpen, ObjID));
                            break;
                        case 2: //close
                            foreach (L2Door door in hideout.doors)
                            {
                                if (door.Closed == 1)
                                    continue;

                                door.Closed = 1;
                                door.broadcastUserInfo();
                            }

                            player.sendPacket(new NpcHtmlMessage(player, ai.fnAfterDoorClose, ObjID));
                            break;
                    }
                    break;
                case -208: //buffs
                    result = useBuff(reply, player);

                    switch (result)
                    {
                        case 5:
                            {
                                NpcHtmlMessage htm = new NpcHtmlMessage(player, ai.fnNotEnoughMP, ObjID);
                                htm.replace("<?MPLeft?>", (int)CurMP);
                                player.sendPacket(htm);
                            }
                            break;
                        case 1:
                        case 4:
                            {
                                NpcHtmlMessage htm = new NpcHtmlMessage(player, ai.fnNeedCoolTime, ObjID);
                                htm.replace("<?MPLeft?>", (int)CurMP);
                                player.sendPacket(htm);
                            }
                            break;

                        case -1:
                            {
                                NpcHtmlMessage htm = new NpcHtmlMessage(player, ai.fnAfterBuff, ObjID);
                                htm.replace("<?MPLeft?>", (int)CurMP);
                                player.sendPacket(htm);
                            }
                            break;
                    }
                    break;
                case -219:
                    switch (reply)
                    {
                        case 1: //banish action
                            hideout.Banish();
                            player.sendPacket(new NpcHtmlMessage(player, ai.fnAfterBanish, ObjID));
                            break;
                    }
                    break;
                case -270:
                    {
                        string val = reply + "";
                        int lvl = 0;
                        int id = 0;
                        if (val.Length == 5)
                        {
                            id = int.Parse(val.Remove(2));
                            lvl = int.Parse(val.Substring(3));
                        }
                        else
                        {
                            lvl = int.Parse(val.Substring(2));
                            id = int.Parse(val.Remove(1));
                        }

                        NpcHtmlMessage htm = new NpcHtmlMessage(player, "agitdeco__" + id + ".htm", ObjID);
                        htm.replace("<?AgitDecoCost?>", hideout.GetDecoCost(id, lvl));
                        htm.replace("<?AgitDecoEffect?>", hideout.GetDecoEffect(id, lvl));
                        htm.replace("<?AgitDecoSubmit?>", reply);
                        player.sendPacket(htm);
                    }
                    break;
                case -271:
                    {
                        result = 0;
                        switch (reply)
                        {
                            case 1004: //hp 80%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_hpregen, 4);
                                break;
                            case 1006: //hp 120%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_hpregen, 6);
                                break;
                            case 1009: //hp 180%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_hpregen, 9);
                                break;
                            case 1012: //hp 240%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_hpregen, 12);
                                break;
                            case 1015: //hp 300%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_hpregen, 15);
                                break;

                            case 2001:// mp 5%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_mpregen, 1);
                                break;
                            case 2003: //mp 15%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_mpregen, 3);
                                break;
                            case 2006: //mp 30%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_mpregen, 6);
                                break;
                            case 2008: //mp 40%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_mpregen, 8);
                                break;

                            case 4003:// xp 15%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_xprestore, 3);
                                break;
                            case 4005: //xp 25%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_xprestore, 5);
                                break;
                            case 4007: //xp 35%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_xprestore, 7);
                                break;
                            case 4010: //xp 50%
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_xprestore, 10);
                                break;

                            case 5001:// teleport lv 1
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_teleport, 1);
                                break;
                            case 5002: //teleport lv 2
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_teleport, 2);
                                break;

                            case 7001:
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_curtain, 1);
                                break;
                            case 7002:
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_curtain, 2);
                                break;

                            case 9003:// buff lv 3
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_buff, 3);
                                break;
                            case 9005: //buff lv 5
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_buff, 5);
                                break;
                            case 9007:// buff lv 7
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_buff, 7);
                                break;
                            case 9008: //buff lv 8
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_buff, 8);
                                break;

                            case 11001:// deco 11 lv 1
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_platform, 1);
                                break;
                            case 11002: //deco 11 lv 2
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_platform, 2);
                                break;

                            case 12001:// itemcreate lv 1
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_item, 1);
                                break;
                            case 12002: //itemcreate lv 2
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_item, 2);
                                break;
                            case 12003:// itemcreate lv 3
                                result = hideout.MofidyFunc(AgitManagerAI.decotype_item, 3);
                                break;
                        }

                        switch (result)
                        {
                            case 1:
                                {
                                    NpcHtmlMessage htm = new NpcHtmlMessage(player, ai.fnDecoAlreadySet, ObjID);
                                    htm.replace("<?AgitDecoEffect?>", "Decoration"); //TODO name
                                    player.sendPacket(htm);
                                }
                                break;
                            case 2:
                                player.sendPacket(new NpcHtmlMessage(player, ai.fnFailtoSetDeco, ObjID));
                                break;
                            case 5:
                                player.sendPacket(new NpcHtmlMessage(player, ai.fnAfterSetDeco, ObjID));
                                broadcastHideoutUpdate(player);
                                break;
                        }
                    }
                    break;


            }
        }

        private short useBuff(int reply, L2Player player)
        {
            int id = 0, lvl = 1;
            switch (reply) // id * 65536 + level
            {
                case 285540353://Ускорение Ур.1
                    id = 4357;
                    break;
                case 285540354://Ускорение Ур.2
                    id = 4357; lvl = 2;
                    break;
                case 284557313://Легкая Походка Ур.1
                    id = 4342;
                    break;
                case 284557314://Легкая Походка Lv.2
                    id = 4342; lvl = 2;
                    break;
                case 284622849://Легкость Ур.1
                    id = 4343;
                    break;
                case 284622851://Легкость Lv.3
                    id = 4343; lvl = 3;
                    break;
                case 284688385://Щит Ур.1
                    id = 4344;
                    break;
                case 284688387://Щит Ур. 3
                    id = 4344; lvl = 3;
                    break;
                case 284819457://Ментальный Щит Ур.1
                    id = 4346;
                    break;
                case 284819460://Ментальный Щит Ур.4
                    id = 4346; lvl = 4;
                    break;
                case 284753921://Могущество Ур.1
                    id = 4345;
                    break;
                case 284753923://Могущество Ур.3
                    id = 4345; lvl = 3;
                    break;
                case 284884994://Благословение Тела Ур.2
                    id = 4347; lvl = 2;
                    break;
                case 284884998://Благословенное Тело Ур.6
                    id = 4347; lvl = 6;
                    break;
                case 285016065://Магический Барьер Ур.1
                    id = 4349;
                    break;
                case 285016066://Магический Барьер Ур.2
                    id = 4349; lvl = 2;
                    break;
                case 285081601://Сопротивление Оглушению Ур.1
                    id = 4350;
                    break;
                case 285081604://Сопротивление Оглушению Ур.4
                    id = 4350; lvl = 4;
                    break;
                case 284950530://Благословение Души Ур.2
                    id = 4348; lvl = 2;
                    break;
                case 284950534://Благословенная Душа Ур.6
                    id = 4348; lvl = 6;
                    break;
                case 285147138://Концентрация Ур.2
                    id = 4351; lvl = 2;
                    break;
                case 285147142://Концентрация Ур.6
                    id = 4351; lvl = 6;
                    break;
                case 285212673://Дух Берсерка Ур.1
                    id = 4352;
                    break;
                case 285212674://Дух Берсерка Ур.2
                    id = 4352; lvl = 2;
                    break;
                case 285278210://Благословленный Щит Ур.2
                    id = 4353; lvl = 2;
                    break;
                case 285278214://Благословенный Щит Ур.6
                    id = 4353; lvl = 6;
                    break;
                case 285605889://Наведение Ур.1
                    id = 4358;
                    break;
                case 285605891://Наведение Ур.3
                    id = 4358; lvl = 3;
                    break;
                case 285343745://Ярость Вампира Ур. 1
                    id = 4354;
                    break;
                case 285343748://Ярость Вампира Ур.4
                    id = 4354; lvl = 4;
                    break;
                case 285409281://Проницательность Ур.1
                    id = 4355;
                    break;
                case 285409283://Проницательность Ур.3
                    id = 4355; lvl = 3;
                    break;
                case 285474817://Благо Ур.1
                    id = 4356;
                    break;
                case 285474819://Благо Ур.3
                    id = 4356; lvl = 3;
                    break;
                case 285671425://Фокусировка Ур.1
                    id = 4359;
                    break;
                case 285671427://Фокусировка Ур.3
                    id = 4359; lvl = 3;
                    break;
                case 285736961://Шепот Смерти Ур.1
                    id = 4360;
                    break;
                case 285736963://Шепот Смерти Ур.3
                    id = 4360; lvl = 3;
                    break;
            }

            if (id == 0)
            {
                log.Error($"hideout manager has invalid buff request { reply }");
                return 1;
            }

            TSkill skill = TSkillTable.Instance.Get(id, lvl);

            if (skill == null)
            {
                log.Error($"hideout manager has null buff skill { id }-{ lvl }");
                return 1;
            }

            this.ChangeTarget(player);
            return (short)this.castSkill(skill);


            //  CurMP -= (skill.MpConsume1 + skill.MpConsume2);
            // player.addEffect(this, skill, player, true, false);
            //  return 5;
        }

        public void broadcastHideoutUpdate(L2Player p)
        {
            p.broadcastPacket(new AgitDecoInfo(hideout));
        }

        public override string asString()
        {
            return "L2HideoutManager:" + Template.NpcId + "; id " + ObjID + "; " + hideout.ID + " " + hideout.ownerId;
        }
    }
}
