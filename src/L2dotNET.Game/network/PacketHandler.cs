using L2dotNET.GameService.network.l2recv;
using log4net;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace L2dotNET.GameService.network
{
    [Synchronization]
    public class PacketHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PacketHandler));

        private static int cnt;
        public static void handlePacket(GameClient client, byte[] buff)
        {
            byte id = buff[0];
            string cninfo = "handlepacket: request " + id.ToString("x2") + " size " + buff.Length;

            string str = "header: " + buff[0].ToString("x2") + "\n";
            foreach (byte b in buff)
                str += b.ToString("x2") + " ";

            log.Info(str);
            GameServerNetworkRequest msg = null;
            switch (id)
            {
                case 0x00:
                    msg = new ProtocolVersion(client, buff);
                    break;
                case 0x08:
                    msg = new AuthLogin(client, buff);
                    break;
                case 0x09:
                    msg = new Logout(client, buff);
                    break;
                case 0x0b:
                    msg = new CharacterCreate(client, buff);
                    break;
                case 0x0c:
                    msg = new CharacterDelete(client, buff);
                    break;
                case 0x0d:
                    msg = new CharacterSelected(client, buff);
                    break;
                case 0x0e:
                    msg = new NewCharacter(client, buff);
                    break;
                //case 0x62:
                //    msg = new CharacterRestore(client, buff);
                //    break;
                //case 0x68:
                //    msg = new RequestPledgeCrest(client, buff);
                //    break;

                //case 0x0c:
                //    msg = new CharacterCreate(client, buff);
                //    break;
                //case 0x00:
                //    msg = new ProtocolVersion(client, buff);
                //    break;
                //case 0x09:
                //    msg = new RequestSetPledgeCrest(client, buff);
                //    break;
                case 0x01:
                    msg = new MoveBackwardToLocation(client, buff);
                    break;
                case 0x03:
                    msg = new EnterWorld(client, buff);
                    break;
                case 0x0f:
                    msg = new RequestItemList(client, buff);
                    break;
                case 0x0a:
                    msg = new AttackRequest(client, buff);
                    break;
                case 0x11:
                    msg = new RequestUnEquipItem(client, buff);
                    break;
                case 0x14:
                    msg = new RequestUseItem(client, buff);
                    break;
                case 0x1A:
                    msg = new RequestStartTrade(client, buff);
                    break;
                case 0x16:
                    msg = new RequestAddTradeItem(client, buff);
                    break;
                case 0x17:
                    msg = new RequestTradeDone(client, buff);
                    break;
                case 0x04:
                    msg = new RequestAction(client, buff);
                    break;

                case 0x20:
                    msg = new RequestLinkHtml(client, buff);
                    break;
                case 0x21:
                    msg = new RequestBypassToServer(client, buff);
                    break;
                case 0x26:
                    msg = new RequestWithdrawalPledge(client, buff);
                    break;
                case 0x8c:
                    msg = new RequestGetItemFromPet(client, buff);
                    break;

                case 0x1b:
                    msg = new RequestSocialAction(client, buff);
                    break;
                case 0x1e:
                    msg = new RequestSellItem(client, buff);
                    break;
                case 0x2f:
                    msg = new RequestMagicSkillUse(client, buff);
                    break;
                case 0x30:
                    msg = new Appearing(client, buff);
                    break;
                case 0x3B:
                    msg = new RequestWarehouseDeposit(client, buff);
                    break;
                case 0x32:
                    msg = new RequestWarehouseWithdraw(client, buff);
                    break;
                case 0x33:
                    msg = new RequestShortCutReg(client, buff);
                    break;
                case 0x35:
                    msg = new RequestShortCutDel(client, buff);
                    break;
                case 0x1f:
                    msg = new RequestBuyItem(client, buff);
                    break;
                case 0x29:
                    msg = new RequestJoinParty(client, buff);
                    break;
                case 0x2a:
                    msg = new RequestAnswerJoinParty(client, buff);
                    break;
                case 0x2b:
                    msg = new RequestWithDrawalParty(client, buff);
                    break;
                case 0x2c:
                    msg = new RequestOustPartyMember(client, buff);
                    break;
                case 0x36:
                    msg = new CannotMoveAnymore(client, buff);
                    break;
                case 0x37:
                    msg = new RequestTargetCanceld(client, buff);
                    break;
                case 0x38:
                    msg = new Say2(client, buff);
                    break;
                case 0x42:
                    msg = new RequestGetOnVehicle(client, buff);
                    break;
                case 0x43:
                    msg = new RequestGetOffVehicle(client, buff);
                    break;
                case 0x44:
                    msg = new AnswerTradeRequest(client, buff);
                    break;
                case 0x45:
                    msg = new RequestActionUse(client, buff);
                    break;
                case 0x46:
                    msg = new RequestRestart(client, buff);
                    break;
                case 0x48:
                    msg = new ValidatePosition(client, buff);
                    break;

                case 0x4a:
                    msg = new StartRotating(client, buff);
                    break;
                case 0x4b:
                    msg = new FinishRotating(client, buff);
                    break;

                case 0x57:
                    msg = new RequestShowBoard(client, buff);
                    break;
                case 0x58:
                    msg = new RequestEnchantItem(client, buff);
                    break;
                case 0x59:
                    msg = new RequestDestroyItem(client, buff);
                    break;
                case 0x64:
                    msg = new RequestQuestAbort(client, buff);
                    break;
                case 0x66:
                    msg = new RequestPledgeInfo(client, buff);
                    break;
                case 0xcd:
                    msg = new RequestShowMiniMap(client, buff);
                    break;
                case 0x6D:
                    msg = new RequestSendMsnChatLog(client, buff);
                    break;
                case 0xcf:
                    msg = new RequestRecordInfo(client, buff);
                    break;
                case 0x73:
                    msg = new RequestAcquireSkillInfo(client, buff);
                    break;
                case 0x74:
                    msg = new SendBypassBuildCmd(client, buff);
                    break;
                case 0x75:
                    msg = new RequestMoveToLocationInVehicle(client, buff);
                    break;

                case 0x7C:
                    msg = new RequestAcquireSkill(client, buff);
                    break;
                case 0x7D:
                    msg = new RequestRestartPoint(client, buff);
                    break;
                case 0x80:
                    msg = new RequestPartyMatchList(client, buff);
                    break;

                case 0x85:
                    msg = new RequestTutorialLinkHtml(client, buff);
                    break;
                case 0x86:
                    msg = new RequestTutorialPassCmdToServer(client, buff);
                    break;
                //  case 0x87:
                //      msg = new RequestTutorialQuestionMark();
                //     break;

                case 0x93:
                    msg = new RequestChangePetName(client, buff);
                    break;
                case 0x94:
                    msg = new RequestPetUseItem(client, buff);
                    break;
                case 0x95:
                    msg = new RequestGiveItemToPet(client, buff);
                    break;

                case 0xB0:
                    msg = new MultiSellChoose(client, buff);
                    break;
                case 0xB1:
                    msg = new NetPingResponse(client, buff);
                    break;
                case 0xaa:
                    msg = new BypassUserCmd(client, buff);
                    break;
                case 0xB5:
                    msg = new RequestRecipeBookOpen(client, buff);
                    break;
                case 0xB6:
                    msg = new RequestRecipeBookDestroy(client, buff);
                    break;
                case 0xB7:
                    msg = new RequestRecipeItemMakeInfo(client, buff);
                    break;
                case 0xB8:
                    msg = new RequestRecipeItemMakeSelf(client, buff);
                    break;
                case 0xC1:
                    msg = new ObserverReturn(client, buff);
                    break;
                case 0xC7:
                    msg = new RequestWearItem(client, buff);
                    break;
                case 0xD0:
                    byte id2 = buff[1];
                    cninfo = "handlepacket: request unk id2 " + id2.ToString("x2") + " size " + buff.Length;
                    switch (id2)
                    {
                        case 8:
                            msg = new RequestManorList(client, buff);
                            break;
                        case 0x11:
                            msg = new RequestExSetPledgeCrestLarge(client, buff);
                            break;

                        case 5:
                            msg = new RequestAutoSoulShot(client, buff);
                            break;

                        case 0x16:
                            msg = new RequestPledgeMemberInfo(client, buff);
                            break;


                        case 0x1E:
                            msg = new RequestExRqItemLink(client, buff);
                            break;
                        case 0x24:
                            msg = new RequestSaveInventoryOrder(client, buff);
                            break;

                        case 0x22:
                            msg = new RequestCursedWeaponList(client, buff);
                            break;

                        case 0x4B:
                            msg = new RequestDispel(client, buff);
                            break;
                        case 0x4C:
                            msg = new RequestExTryToPutEnchantTargetItem(client, buff);
                            break;
                        case 0x4D:
                            msg = new RequestExTryToPutEnchantSupportItem(client, buff);
                            break;
                        case 0x4E:
                            msg = new RequestExCancelEnchantItem(client, buff);
                            break;
                        case 0x58:
                            msg = new RequestDominionInfo(client, buff);
                            break;
                        case 0x76:
                            msg = new RequestBuySellUIClose(client, buff);
                            break;

                        case 0x78:
                            msg = new RequestPartyLootModification(client, buff);
                            break;
                        case 0x79:
                            msg = new AnswerPartyLootModification(client, buff);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            if (msg == null)
            {
                log.Info($"{ cninfo }, { cnt }");
                return;
            }

            if (msg.Client.IsTerminated)
                return;

            new Thread(new ThreadStart(msg.run)).Start();
        }

        private static void out_debug(byte level, byte[] buff)
        {
            string s = "";
            byte d = 0;

            if (level > 0)
                s = "Header: ";
            for (byte r = 0; r < level; r++)
            {
                s += buff[r].ToString("x2");
            }

            if (level > 0)
                s += "\n";

            for (int a = level; a < buff.Length; a++)
            {
                byte value = buff[a];
                string t = value < 10 ? "0" + value : value.ToString("x2");
                d++;
                s += t + " ";

                if (d == 4)
                {
                    d = 0;
                    s += "\n";
                }
            }

            log.Info(s);
        }
    }
}
