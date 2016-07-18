using System.Runtime.Remoting.Contexts;
using System.Threading;
using log4net;
using L2dotNET.GameService.Network.Clientpackets;
using L2dotNET.GameService.Network.Clientpackets.ClanAPI;
using L2dotNET.GameService.Network.Clientpackets.ItemEnchantAPI;
using L2dotNET.GameService.Network.Clientpackets.PartyAPI;
using L2dotNET.GameService.Network.Clientpackets.PetAPI;
using L2dotNET.GameService.Network.Clientpackets.RecipeAPI;
using L2dotNET.GameService.Network.Clientpackets.VehicleAPI;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network
{
    [Synchronization]
    public class PacketHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PacketHandler));

        public static void HandlePacket(Packet packet, GameClient client)
        {
            PacketBase packetBase = null;

            switch (packet.FirstOpcode)
            {
                case 0x00:
                    packetBase = new ProtocolVersion(packet, client);
                    break;
                case 0x08:
                    packetBase = new AuthLogin(packet, client);
                    break;
                case 0x09:
                    packetBase = new Logout(packet, client);
                    break;
                case 0x0b:
                    packetBase = new CharacterCreate(packet, client);
                    break;

                case 0x0d:
                    packetBase = new CharacterSelected(packet, client);
                    break;
                case 0x0e:
                    packetBase = new NewCharacter(packet, client);
                    break;
                case 0x01:
                    packetBase = new MoveBackwardToLocation(packet, client);
                    break;
                case 0x03:
                    packetBase = new EnterWorld(packet, client);
                    break;
                case 0x0f:
                    packetBase = new RequestItemList(packet, client);
                    break;
                case 0x0a:
                    packetBase = new AttackRequest(packet, client);
                    break;
                case 0x11:
                    packetBase = new RequestUnEquipItem(packet, client);
                    break;
                case 0x14:
                    packetBase = new RequestUseItem(packet, client);
                    break;
                case 0x1A:
                    packetBase = new RequestStartTrade(packet, client);
                    break;
                case 0x16:
                    packetBase = new RequestAddTradeItem(packet, client);
                    break;
                case 0x17:
                    packetBase = new RequestTradeDone(packet, client);
                    break;
                case 0x04:
                    packetBase = new RequestAction(packet, client);
                    break;

                case 0x20:
                    packetBase = new RequestLinkHtml(packet, client);
                    break;
                case 0x21:
                    packetBase = new RequestBypassToServer(packet, client);
                    break;
                case 0x26:
                    packetBase = new RequestWithdrawalPledge(packet, client);
                    break;
                case 0x8c:
                    packetBase = new RequestGetItemFromPet(packet, client);
                    break;

                case 0x1b:
                    packetBase = new RequestSocialAction(packet, client);
                    break;
                case 0x1e:
                    packetBase = new RequestSellItem(packet, client);
                    break;
                case 0x2f:
                    packetBase = new RequestMagicSkillUse(packet, client);
                    break;
                case 0x30:
                    packetBase = new Appearing(packet, client);
                    break;
                case 0x3B:
                    packetBase = new RequestWarehouseDeposit(packet, client);
                    break;
                case 0x32:
                    packetBase = new RequestWarehouseWithdraw(packet, client);
                    break;
                case 0x33:
                    packetBase = new RequestShortCutReg(packet, client);
                    break;
                case 0x35:
                    packetBase = new RequestShortCutDel(packet, client);
                    break;
                case 0x1f:
                    packetBase = new RequestBuyItem(packet, client);
                    break;
                case 0x29:
                    packetBase = new RequestJoinParty(packet, client);
                    break;
                case 0x2a:
                    packetBase = new RequestAnswerJoinParty(packet, client);
                    break;
                case 0x2b:
                    packetBase = new RequestWithDrawalParty(packet, client);
                    break;
                case 0x2c:
                    packetBase = new RequestOustPartyMember(packet, client);
                    break;
                case 0x36:
                    packetBase = new CannotMoveAnymore(packet, client);
                    break;
                case 0x37:
                    packetBase = new RequestTargetCanceld(packet, client);
                    break;
                case 0x38:
                    packetBase = new Say2(packet, client);
                    break;
                case 0x42:
                    packetBase = new RequestGetOnVehicle(packet, client);
                    break;
                case 0x43:
                    packetBase = new RequestGetOffVehicle(packet, client);
                    break;
                case 0x44:
                    packetBase = new AnswerTradeRequest(packet, client);
                    break;
                case 0x45:
                    packetBase = new RequestActionUse(packet, client);
                    break;
                case 0x46:
                    packetBase = new RequestRestart(packet, client);
                    break;
                case 0x48:
                    packetBase = new ValidatePosition(packet, client);
                    break;

                case 0x4a:
                    packetBase = new StartRotating(packet, client);
                    break;
                case 0x4b:
                    packetBase = new FinishRotating(packet, client);
                    break;

                case 0x57:
                    packetBase = new RequestShowBoard(packet, client);
                    break;
                case 0x58:
                    packetBase = new RequestEnchantItem(packet, client);
                    break;
                case 0x59:
                    packetBase = new RequestDestroyItem(packet, client);
                    break;
                case 0x64:
                    packetBase = new RequestQuestAbort(packet, client);
                    break;
                case 0x66:
                    packetBase = new RequestPledgeInfo(packet, client);
                    break;
                case 0xcd:
                    packetBase = new RequestShowMiniMap(packet, client);
                    break;
                case 0x6D:
                    packetBase = new RequestSendMsnChatLog(packet, client);
                    break;
                case 0xcf:
                    packetBase = new RequestRecordInfo(packet, client);
                    break;
                case 0x73:
                    packetBase = new RequestAcquireSkillInfo(packet, client);
                    break;
                case 0x74:
                    packetBase = new SendBypassBuildCmd(packet, client);
                    break;
                case 0x75:
                    packetBase = new RequestMoveToLocationInVehicle(packet, client);
                    break;

                case 0x7C:
                    packetBase = new RequestAcquireSkill(packet, client);
                    break;
                case 0x7D:
                    packetBase = new RequestRestartPoint(packet, client);
                    break;
                case 0x80:
                    packetBase = new RequestPartyMatchList(packet, client);
                    break;

                case 0x85:
                    packetBase = new RequestTutorialLinkHtml(packet, client);
                    break;
                case 0x86:
                    packetBase = new RequestTutorialPassCmdToServer(packet, client);
                    break;
                //  case 0x87:
                //      msg = new RequestTutorialQuestionMark();
                //     break;

                case 0x93:
                    packetBase = new RequestChangePetName(packet, client);
                    break;
                case 0x94:
                    packetBase = new RequestPetUseItem(packet, client);
                    break;
                case 0x95:
                    packetBase = new RequestGiveItemToPet(packet, client);
                    break;

                case 0xB0:
                    packetBase = new MultiSellChoose(packet, client);
                    break;
                case 0xB1:
                    packetBase = new NetPingResponse(packet, client);
                    break;
                case 0xaa:
                    packetBase = new BypassUserCmd(packet, client);
                    break;
                case 0xB5:
                    packetBase = new RequestRecipeBookOpen(packet, client);
                    break;
                case 0xB6:
                    packetBase = new RequestRecipeBookDestroy(packet, client);
                    break;
                case 0xB7:
                    packetBase = new RequestRecipeItemMakeInfo(packet, client);
                    break;
                case 0xB8:
                    packetBase = new RequestRecipeItemMakeSelf(packet, client);
                    break;
                case 0xC1:
                    packetBase = new ObserverReturn(packet, client);
                    break;
                case 0xC7:
                    packetBase = new RequestWearItem(packet, client);
                    break;
                case 0xD0:
                    switch (packet.SecondOpcode)
                    {
                        case 0x08:
                            packetBase = new RequestManorList(packet, client);
                            break;
                        case 0x11:
                            packetBase = new RequestExSetPledgeCrestLarge(packet, client);
                            break;

                        case 0x05:
                            packetBase = new RequestAutoSoulShot(packet, client);
                            break;

                        case 0x1d:
                            packetBase = new RequestPledgeMemberInfo(packet, client);
                            break;
                        //case 0x24:
                        //    packetBase = new RequestSaveInventoryOrder(packet, client);
                        //    break;

                        case 0x22:
                            packetBase = new RequestCursedWeaponList(packet, client);
                            break;

                        //case 0x4B:
                        //    packetBase = new RequestDispel(packet, client);
                        //    break;
                        //case 0x4C:
                        //    packetBase = new RequestExTryToPutEnchantTargetItem(packet, client);
                        //    break;
                        //case 0x4D:
                        //    packetBase = new RequestExTryToPutEnchantSupportItem(packet, client);
                        //    break;
                        //case 0x4E:
                        //    packetBase = new RequestExCancelEnchantItem(packet, client);
                        //    break;
                        //case 0x58:
                        //    packetBase = new RequestDominionInfo(packet, client);
                        //    break;
                        //case 0x76:
                        //    packetBase = new RequestBuySellUiClose(packet, client);
                        //    break;

                        //case 0x78:
                        //    packetBase = new RequestPartyLootModification(packet, client);
                        //    break;
                        //case 0x79:
                        //    packetBase = new AnswerPartyLootModification(packet, client);
                        //    break;
                    }

                    break;
            }

            if (packetBase == null)
            {
                //Log.Info($"{cninfo}");
                //log.Info($"{cninfo}, {cnt}");
                return;
            }

            if (client.IsTerminated)
                return;

            new Thread(packetBase.RunImpl).Start();
        }

        private static void out_debug(byte level, byte[] buff)
        {
            string s = "";
            byte d = 0;

            if (level > 0)
                s = "Header: ";
            for (byte r = 0; r < level; r++)
                s += buff[r].ToString("x2");

            if (level > 0)
                s += "\n";

            for (int a = level; a < buff.Length; a++)
            {
                byte value = buff[a];
                string t = value < 10 ? "0" + value : value.ToString("x2");
                d++;
                s += t + " ";

                if (d != 4)
                    continue;

                d = 0;
                s += "\n";
            }

            Log.Info(s);
        }
    }
}