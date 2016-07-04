using System.Linq;
using log4net;
using L2dotNET.GameService.Model.Npcs.Ai;
using L2dotNET.GameService.Model.Npcs.Decor;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Model.Structures;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;

namespace L2dotNET.GameService.Model.Npcs
{
    class L2HideoutManager : L2Npc
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(L2HideoutManager));
        private readonly Hideout _hideout;
        private readonly AgitManagerAi _ai;

        public L2HideoutManager(HideoutTemplate hideout)
        {
            this._hideout = (Hideout)hideout;
            StructureControlled = true;
            _ai = new AgitManagerAi();
            CurMp = 5000;
        }

        private System.Timers.Timer _regenMpTime;

        public void StartRegenTime()
        {
            if (_regenMpTime == null)
            {
                _regenMpTime = new System.Timers.Timer();
                _regenMpTime.Interval = 2000;
                _regenMpTime.Elapsed += new System.Timers.ElapsedEventHandler(RegenTask);
            }

            _regenMpTime.Enabled = true;
        }

        private void RegenTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            int lvl = _hideout.GetFuncLevel(AgitManagerAi.DecotypeBuff);
            CurMp += _ai.RegenPerSec[lvl];
            if (CurMp >= _ai.RegenMax[lvl])
                CurMp = _ai.RegenMax[lvl];
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            StartRegenTime();
        }

        public override void NotifyAction(L2Player player)
        {
            player.SendPacket(new NpcHtmlMessage(player, _ai.FnHi, ObjId));
        }

        public override void OnTeleportRequest(L2Player player)
        {
            if (_hideout.NoTeleports)
            {
                player.SendPacket(new NpcHtmlMessage(player, _ai.FnTeleportLevelZero, ObjId));
                return;
            }

            int level = _hideout.GetFuncLevel(AgitManagerAi.DecotypeTeleport);
            if (level == 0)
                player.SendPacket(new NpcHtmlMessage(player, _ai.FnFuncDisabled, ObjId));
            else
                NpcData.Instance.RequestTeleportList(this, player, level);
        }

        public override void OnDialog(L2Player player, int ask, int reply)
        {
            player.FolkNpc = this;
            short result;
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
                            if (player.Clan.LeaderId == player.ObjId) //TODO privs
                                player.SendPacket(new NpcHtmlMessage(player, _ai.FnDoor, ObjId));
                            break;
                        case 2: //banish
                            player.SendPacket(new NpcHtmlMessage(player, _ai.FnBanish, ObjId));
                            break;
                        case 3: //functions
                        {
                            NpcHtmlMessage htm = new NpcHtmlMessage(player, _ai.FnDecoFunction, ObjId);
                            htm.Replace("<?HPDepth?>", _hideout.GetFuncDepth(AgitManagerAi.DecotypeHpregen));
                            htm.Replace("<?MPDepth?>", _hideout.GetFuncDepth(AgitManagerAi.DecotypeMpregen));
                            htm.Replace("<?XPDepth?>", _hideout.GetFuncDepth(AgitManagerAi.DecotypeXprestore));
                            player.SendPacket(htm);
                        }
                            break;
                        case 4: // warehouse
                        {
                            NpcHtmlMessage htm = new NpcHtmlMessage(player, _ai.FnWarehouse, ObjId);
                            htm.Replace("<?agit_lease?>", _hideout.RentCost);
                            htm.Replace("<?pay_time?>", _hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                            player.SendPacket(htm);
                        }
                            break;
                        case 5: // manage
                            player.SendPacket(new NpcHtmlMessage(player, _ai.FnManage, ObjId));
                            break;
                        case 7: //use buff
                        {
                            int level = _hideout.GetFuncLevel(AgitManagerAi.DecotypeBuff);
                            if (level == 0)
                            {
                                player.SendPacket(new NpcHtmlMessage(player, _ai.FnFuncDisabled, ObjId));
                            }
                            else
                            {
                                NpcHtmlMessage htm = new NpcHtmlMessage(player, _ai.FnAgitBuff + "_" + level + ".htm", ObjId);
                                htm.Replace("<?MPLeft?>", (int)CurMp);
                                player.SendPacket(htm);
                            }
                        }
                            break;
                        case 12: //use itemcreate
                        {
                            int level = _hideout.GetFuncLevel(AgitManagerAi.DecotypeItem);
                            if (level == 0)
                                player.SendPacket(new NpcHtmlMessage(player, _ai.FnFuncDisabled, ObjId));
                            else
                                NpcData.Instance.Buylist(player, this, (short)level);
                        }
                            break;
                        case 51: // manage regen
                        {
                            NpcHtmlMessage htm = new NpcHtmlMessage(player, _ai.FnManageRegen, ObjId);
                            htm.Replace("<?HPDepth?>", _hideout.GetFuncDepth(AgitManagerAi.DecotypeHpregen));
                            htm.Replace("<?HPCost?>", _hideout.GetCurrentDecoCost(AgitManagerAi.DecotypeHpregen));
                            htm.Replace("<?HPExpire?>", _hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                            htm.Replace("<?HPReset?>", "");

                            htm.Replace("<?MPDepth?>", _hideout.GetFuncDepth(AgitManagerAi.DecotypeMpregen));
                            htm.Replace("<?MPCost?>", _hideout.GetCurrentDecoCost(AgitManagerAi.DecotypeMpregen));
                            htm.Replace("<?MPExpire?>", _hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                            htm.Replace("<?MPReset?>", "");

                            htm.Replace("<?XPDepth?>", _hideout.GetFuncDepth(AgitManagerAi.DecotypeXprestore));
                            htm.Replace("<?XPCost?>", _hideout.GetCurrentDecoCost(AgitManagerAi.DecotypeXprestore));
                            htm.Replace("<?XPExpire?>", _hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                            htm.Replace("<?XPReset?>", "");
                            player.SendPacket(htm);
                        }
                            break;
                        case 52: // manage etc
                        {
                            NpcHtmlMessage htm = new NpcHtmlMessage(player, _ai.FnManageEtc, ObjId);
                            htm.Replace("<?TPDepth?>", _hideout.GetFuncDepth(AgitManagerAi.DecotypeTeleport));
                            htm.Replace("<?TPCost?>", _hideout.GetCurrentDecoCost(AgitManagerAi.DecotypeTeleport));
                            htm.Replace("<?TPExpire?>", _hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                            htm.Replace("<?TPReset?>", "");

                            htm.Replace("<?BFDepth?>", _hideout.GetFuncDepth(AgitManagerAi.DecotypeBuff));
                            htm.Replace("<?BFCost?>", _hideout.GetCurrentDecoCost(AgitManagerAi.DecotypeBuff));
                            htm.Replace("<?BFExpire?>", _hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                            htm.Replace("<?BFReset?>", "");

                            htm.Replace("<?ICDepth?>", _hideout.GetFuncDepth(AgitManagerAi.DecotypeItem));
                            htm.Replace("<?ICCost?>", _hideout.GetCurrentDecoCost(AgitManagerAi.DecotypeItem));
                            htm.Replace("<?ICExpire?>", _hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                            htm.Replace("<?ICReset?>", "");
                            player.SendPacket(htm);
                        }
                            break;
                        case 53: // manage deco
                        {
                            NpcHtmlMessage htm = new NpcHtmlMessage(player, _ai.FnManageDeco, ObjId);
                            htm.Replace("<?7_Depth?>", _hideout.GetFuncDepth(AgitManagerAi.DecotypeCurtain));
                            htm.Replace("<?7_Cost?>", _hideout.GetCurrentDecoCost(AgitManagerAi.DecotypeCurtain));
                            htm.Replace("<?7_Expire?>", _hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                            htm.Replace("<?7_Reset?>", "");

                            htm.Replace("<?11_Depth?>", _hideout.GetFuncDepth(AgitManagerAi.DecotypePlatform));
                            htm.Replace("<?11_Cost?>", _hideout.GetCurrentDecoCost(AgitManagerAi.DecotypePlatform));
                            htm.Replace("<?11_Expire?>", _hideout.PayTime.ToString("yyyy/MM/dd HH:mm"));
                            htm.Replace("<?11_Reset?>", "");
                            player.SendPacket(htm);
                        }
                            break;
                    }

                    break;
                case -203:
                    switch (reply)
                    {
                        case 1: //open doors
                            foreach (L2Door door in _hideout.doors.Where(door => door.Closed != 0))
                            {
                                door.Closed = 0;
                                door.BroadcastUserInfo();
                            }

                            player.SendPacket(new NpcHtmlMessage(player, _ai.FnAfterDoorOpen, ObjId));
                            break;
                        case 2: //close
                            foreach (L2Door door in _hideout.doors.Where(door => door.Closed != 1))
                            {
                                door.Closed = 1;
                                door.BroadcastUserInfo();
                            }

                            player.SendPacket(new NpcHtmlMessage(player, _ai.FnAfterDoorClose, ObjId));
                            break;
                    }

                    break;
                case -208: //buffs
                    result = UseBuff(reply, player);

                    switch (result)
                    {
                        case 5:
                        {
                            NpcHtmlMessage htm = new NpcHtmlMessage(player, _ai.FnNotEnoughMp, ObjId);
                            htm.Replace("<?MPLeft?>", (int)CurMp);
                            player.SendPacket(htm);
                        }
                            break;
                        case 1:
                        case 4:
                        {
                            NpcHtmlMessage htm = new NpcHtmlMessage(player, _ai.FnNeedCoolTime, ObjId);
                            htm.Replace("<?MPLeft?>", (int)CurMp);
                            player.SendPacket(htm);
                        }
                            break;

                        case -1:
                        {
                            NpcHtmlMessage htm = new NpcHtmlMessage(player, _ai.FnAfterBuff, ObjId);
                            htm.Replace("<?MPLeft?>", (int)CurMp);
                            player.SendPacket(htm);
                        }
                            break;
                    }

                    break;
                case -219:
                    switch (reply)
                    {
                        case 1: //banish action
                            _hideout.Banish();
                            player.SendPacket(new NpcHtmlMessage(player, _ai.FnAfterBanish, ObjId));
                            break;
                    }

                    break;
                case -270:
                {
                    string val = reply + "";
                    int lvl;
                    int id;
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

                    NpcHtmlMessage htm = new NpcHtmlMessage(player, "agitdeco__" + id + ".htm", ObjId);
                    htm.Replace("<?AgitDecoCost?>", _hideout.GetDecoCost(id, lvl));
                    htm.Replace("<?AgitDecoEffect?>", _hideout.GetDecoEffect(id, lvl));
                    htm.Replace("<?AgitDecoSubmit?>", reply);
                    player.SendPacket(htm);
                }
                    break;
                case -271:
                {
                    result = 0;
                    switch (reply)
                    {
                        case 1004: //hp 80%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeHpregen, 4);
                            break;
                        case 1006: //hp 120%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeHpregen, 6);
                            break;
                        case 1009: //hp 180%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeHpregen, 9);
                            break;
                        case 1012: //hp 240%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeHpregen, 12);
                            break;
                        case 1015: //hp 300%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeHpregen, 15);
                            break;

                        case 2001: // mp 5%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeMpregen, 1);
                            break;
                        case 2003: //mp 15%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeMpregen, 3);
                            break;
                        case 2006: //mp 30%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeMpregen, 6);
                            break;
                        case 2008: //mp 40%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeMpregen, 8);
                            break;

                        case 4003: // xp 15%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeXprestore, 3);
                            break;
                        case 4005: //xp 25%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeXprestore, 5);
                            break;
                        case 4007: //xp 35%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeXprestore, 7);
                            break;
                        case 4010: //xp 50%
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeXprestore, 10);
                            break;

                        case 5001: // teleport lv 1
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeTeleport, 1);
                            break;
                        case 5002: //teleport lv 2
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeTeleport, 2);
                            break;

                        case 7001:
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeCurtain, 1);
                            break;
                        case 7002:
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeCurtain, 2);
                            break;

                        case 9003: // buff lv 3
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeBuff, 3);
                            break;
                        case 9005: //buff lv 5
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeBuff, 5);
                            break;
                        case 9007: // buff lv 7
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeBuff, 7);
                            break;
                        case 9008: //buff lv 8
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeBuff, 8);
                            break;

                        case 11001: // deco 11 lv 1
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypePlatform, 1);
                            break;
                        case 11002: //deco 11 lv 2
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypePlatform, 2);
                            break;

                        case 12001: // itemcreate lv 1
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeItem, 1);
                            break;
                        case 12002: //itemcreate lv 2
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeItem, 2);
                            break;
                        case 12003: // itemcreate lv 3
                            result = _hideout.MofidyFunc(AgitManagerAi.DecotypeItem, 3);
                            break;
                    }

                    switch (result)
                    {
                        case 1:
                        {
                            NpcHtmlMessage htm = new NpcHtmlMessage(player, _ai.FnDecoAlreadySet, ObjId);
                            htm.Replace("<?AgitDecoEffect?>", "Decoration"); //TODO name
                            player.SendPacket(htm);
                        }
                            break;
                        case 2:
                            player.SendPacket(new NpcHtmlMessage(player, _ai.FnFailtoSetDeco, ObjId));
                            break;
                        case 5:
                            player.SendPacket(new NpcHtmlMessage(player, _ai.FnAfterSetDeco, ObjId));
                            BroadcastHideoutUpdate(player);
                            break;
                    }
                }

                    break;
            }
        }

        private short UseBuff(int reply, L2Player player)
        {
            int id = 0,
                lvl = 1;
            switch (reply) // id * 65536 + level
            {
                case 285540353: //Ускорение Ур.1
                    id = 4357;
                    break;
                case 285540354: //Ускорение Ур.2
                    id = 4357;
                    lvl = 2;
                    break;
                case 284557313: //Легкая Походка Ур.1
                    id = 4342;
                    break;
                case 284557314: //Легкая Походка Lv.2
                    id = 4342;
                    lvl = 2;
                    break;
                case 284622849: //Легкость Ур.1
                    id = 4343;
                    break;
                case 284622851: //Легкость Lv.3
                    id = 4343;
                    lvl = 3;
                    break;
                case 284688385: //Щит Ур.1
                    id = 4344;
                    break;
                case 284688387: //Щит Ур. 3
                    id = 4344;
                    lvl = 3;
                    break;
                case 284819457: //Ментальный Щит Ур.1
                    id = 4346;
                    break;
                case 284819460: //Ментальный Щит Ур.4
                    id = 4346;
                    lvl = 4;
                    break;
                case 284753921: //Могущество Ур.1
                    id = 4345;
                    break;
                case 284753923: //Могущество Ур.3
                    id = 4345;
                    lvl = 3;
                    break;
                case 284884994: //Благословение Тела Ур.2
                    id = 4347;
                    lvl = 2;
                    break;
                case 284884998: //Благословенное Тело Ур.6
                    id = 4347;
                    lvl = 6;
                    break;
                case 285016065: //Магический Барьер Ур.1
                    id = 4349;
                    break;
                case 285016066: //Магический Барьер Ур.2
                    id = 4349;
                    lvl = 2;
                    break;
                case 285081601: //Сопротивление Оглушению Ур.1
                    id = 4350;
                    break;
                case 285081604: //Сопротивление Оглушению Ур.4
                    id = 4350;
                    lvl = 4;
                    break;
                case 284950530: //Благословение Души Ур.2
                    id = 4348;
                    lvl = 2;
                    break;
                case 284950534: //Благословенная Душа Ур.6
                    id = 4348;
                    lvl = 6;
                    break;
                case 285147138: //Концентрация Ур.2
                    id = 4351;
                    lvl = 2;
                    break;
                case 285147142: //Концентрация Ур.6
                    id = 4351;
                    lvl = 6;
                    break;
                case 285212673: //Дух Берсерка Ур.1
                    id = 4352;
                    break;
                case 285212674: //Дух Берсерка Ур.2
                    id = 4352;
                    lvl = 2;
                    break;
                case 285278210: //Благословленный Щит Ур.2
                    id = 4353;
                    lvl = 2;
                    break;
                case 285278214: //Благословенный Щит Ур.6
                    id = 4353;
                    lvl = 6;
                    break;
                case 285605889: //Наведение Ур.1
                    id = 4358;
                    break;
                case 285605891: //Наведение Ур.3
                    id = 4358;
                    lvl = 3;
                    break;
                case 285343745: //Ярость Вампира Ур. 1
                    id = 4354;
                    break;
                case 285343748: //Ярость Вампира Ур.4
                    id = 4354;
                    lvl = 4;
                    break;
                case 285409281: //Проницательность Ур.1
                    id = 4355;
                    break;
                case 285409283: //Проницательность Ур.3
                    id = 4355;
                    lvl = 3;
                    break;
                case 285474817: //Благо Ур.1
                    id = 4356;
                    break;
                case 285474819: //Благо Ур.3
                    id = 4356;
                    lvl = 3;
                    break;
                case 285671425: //Фокусировка Ур.1
                    id = 4359;
                    break;
                case 285671427: //Фокусировка Ур.3
                    id = 4359;
                    lvl = 3;
                    break;
                case 285736961: //Шепот Смерти Ур.1
                    id = 4360;
                    break;
                case 285736963: //Шепот Смерти Ур.3
                    id = 4360;
                    lvl = 3;
                    break;
            }

            if (id == 0)
            {
                Log.Error($"hideout manager has invalid buff request {reply}");
                return 1;
            }

            Skill skill = SkillTable.Instance.Get(id, lvl);

            if (skill == null)
            {
                Log.Error($"hideout manager has null buff skill {id}-{lvl}");
                return 1;
            }

            ChangeTarget(player);
            return (short)CastSkill(skill);

            //  CurMP -= (skill.MpConsume1 + skill.MpConsume2);
            // player.addEffect(this, skill, player, true, false);
            //  return 5;
        }

        public void BroadcastHideoutUpdate(L2Player p)
        {
            p.BroadcastPacket(new AgitDecoInfo(_hideout));
        }

        public override string AsString()
        {
            return "L2HideoutManager:" + Template.NpcId + "; id " + ObjId + "; " + _hideout.ID + " " + _hideout.ownerId;
        }
    }
}