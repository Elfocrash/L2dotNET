using System;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Contexts;
using log4net;
using L2dotNET.Network.clientpackets;
using L2dotNET.Network.clientpackets.ClanAPI;
using L2dotNET.Network.clientpackets.ItemEnchantAPI;
using L2dotNET.Network.clientpackets.PartyAPI;
using L2dotNET.Network.clientpackets.PetAPI;
using L2dotNET.Network.clientpackets.RecipeAPI;
using L2dotNET.Network.clientpackets.VehicleAPI;

namespace L2dotNET.Network
{
    [Synchronization]
    public class GamePacketHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GamePacketHandler));
        private static readonly ConcurrentDictionary<byte, Type> ClientPackets = new ConcurrentDictionary<byte, Type>();

        private static readonly ConcurrentDictionary<short, Type> ClientPacketsD0 = new ConcurrentDictionary<short, Type>();

        static GamePacketHandler()
        {
            //TODO: Review class packets, looks like some is missing and with wrong KeyValue
            ClientPackets.TryAdd(0x00, typeof(ProtocolVersion));
            ClientPackets.TryAdd(0x01, typeof(MoveBackwardToLocation));
            ClientPackets.TryAdd(0x03, typeof(EnterWorld));
            ClientPackets.TryAdd(0x04, typeof(RequestAction));
            ClientPackets.TryAdd(0x08, typeof(AuthLogin));
            ClientPackets.TryAdd(0x09, typeof(Logout));
            ClientPackets.TryAdd(0x0a, typeof(AttackRequest));
            ClientPackets.TryAdd(0x0b, typeof(CharacterCreate));
            ClientPackets.TryAdd(0x0c, typeof(CharacterDelete));
            ClientPackets.TryAdd(0x0d, typeof(CharacterSelected));
            ClientPackets.TryAdd(0x0e, typeof(NewCharacter));
            ClientPackets.TryAdd(0x0f, typeof(RequestItemList));
            ClientPackets.TryAdd(0x11, typeof(RequestUnEquipItem));
            ClientPackets.TryAdd(0x12, typeof(RequestDropItem));
            ClientPackets.TryAdd(0x14, typeof(RequestUseItem));
            ClientPackets.TryAdd(0x15, typeof(RequestStartTrade));
            ClientPackets.TryAdd(0x16, typeof(RequestAddTradeItem));
            ClientPackets.TryAdd(0x17, typeof(RequestTradeDone));
            //ClientPackets.TryAdd(0x1A, typeof(RequestStartTrade)); -RequestStartTrade @ 0x15
            ClientPackets.TryAdd(0x1b, typeof(RequestSocialAction));
            //ClientPackets.TryAdd(0x1c, typeof(ChangeMoveType)); -Set walk/run
            ClientPackets.TryAdd(0x1d, typeof(ChangeWaitType));
            ClientPackets.TryAdd(0x1e, typeof(RequestSellItem));
            ClientPackets.TryAdd(0x1f, typeof(RequestBuyItem));
            ClientPackets.TryAdd(0x20, typeof(RequestLinkHtml));
            ClientPackets.TryAdd(0x21, typeof(RequestBypassToServer));
            //ClientPackets.TryAdd(0x22, typeof(RequestBBSWrite)); 
            //ClientPackets.TryAdd(0x23, typeof(RequestCreatePledge)); 
            //ClientPackets.TryAdd(0x24, typeof(RequestJoinPledge)); 
            //ClientPakcets.TryAdd(0x25, typeof(RequestAnswerJoinPledge)); 
            ClientPackets.TryAdd(0x26, typeof(RequestWithdrawalPledge));
            //ClientPackets.TryAdd(0x27, typeof(RequestOustPledgeMember));
            //ClientPackets.TryAdd(0x28, typeof(RequestDismissPledge));
            ClientPackets.TryAdd(0x29, typeof(RequestJoinParty));
            ClientPackets.TryAdd(0x2a, typeof(RequestAnswerJoinParty));
            ClientPackets.TryAdd(0x2b, typeof(RequestWithdrawalParty));
            ClientPackets.TryAdd(0x2c, typeof(RequestOustPartyMember));
            //ClientPackets.TryAdd(0x2d, typeof(RequestDismissParty));
            ClientPackets.TryAdd(0x2f, typeof(RequestMagicSkillUse));
            ClientPackets.TryAdd(0x30, typeof(Appearing));
            ClientPackets.TryAdd(0x31, typeof(RequestWarehouseDeposit));
            ClientPackets.TryAdd(0x32, typeof(RequestWarehouseWithdraw));
            ClientPackets.TryAdd(0x33, typeof(RequestShortCutReg));
            ClientPackets.TryAdd(0x35, typeof(RequestShortCutDel));
            ClientPackets.TryAdd(0x36, typeof(CannotMoveAnymore));
            ClientPackets.TryAdd(0x37, typeof(RequestTargetCanceld));
            ClientPackets.TryAdd(0x38, typeof(Say2));
            //ClientPackets.TryAdd(0x3c, typeof(RequestPledgeMemberList));
            //ClientPackets.TryAdd(0x3e, typeof(RequestMagicList)); -Possibly unused
            ClientPackets.TryAdd(0x3f, typeof(RequestSkillList));
            //ClientPackets.TryAdd(0x41, typeof(MoveWithDelta)); -Possibly unused
            ClientPackets.TryAdd(0x42, typeof(RequestGetOnVehicle));
            ClientPackets.TryAdd(0x43, typeof(RequestGetOffVehicle));
            ClientPackets.TryAdd(0x44, typeof(AnswerTradeRequest));
            ClientPackets.TryAdd(0x45, typeof(RequestActionUse));
            ClientPackets.TryAdd(0x46, typeof(RequestRestart));
            //ClientPackets.TryAdd(0x47, typeof(RequestSiegeInfo));
            ClientPackets.TryAdd(0x48, typeof(ValidatePosition));
            //ClientPackets.TryAdd(0x49, typeof(RequestSEKCustom));
            ClientPackets.TryAdd(0x4a, typeof(StartRotating));
            ClientPackets.TryAdd(0x4b, typeof(FinishRotating));
            //ClientPackets.TryAdd(0x4d, typeof(RequestStartPledgeWar));
            //ClientPackets.TryAdd(0x4e, typeof(RequestReplyStartPledgeWar));
            //ClientPackets.TryAdd(0x4f, typeof(RequestStopPledgeWar));
            //ClientPackets.TryAdd(0x50, typeof(RequestReplyStopPledgeWar));
            //ClientPackets.TryAdd(0x51, typeof(RequestSurrenderPledgeWar));
            //ClientPackets.TryAdd(0x52, typeof(RequestReplySurrenderPledgeWar));
            ClientPackets.TryAdd(0x53, typeof(RequestSetPledgeCrest));
            //ClientPackets.TryAdd(0x55, typeof(RequestGiveNickName)); -Title
            ClientPackets.TryAdd(0x57, typeof(RequestShowBoard));
            ClientPackets.TryAdd(0x58, typeof(RequestEnchantItem));
            ClientPackets.TryAdd(0x59, typeof(RequestDestroyItem));
            ClientPackets.TryAdd(0x5b, typeof(SendBypassBuildCmd));
            //ClientPackets.TryAdd(0x5c, typeof(MoveToLocationInVehicle));
            //ClientPackets.TryAdd(0x5d, typeof(CanNotMoveAnymoreVehicle));
            //ClientPackets.TryAdd(0x5e, typeof(RequestFriendInvite));
            //ClientPackets.TryAdd(0x5f, typeof(RequestFriendAddReply));
            //ClientPackets.TryAdd(0x60, typeof(RequestFriendInfoList));
            //ClientPackets.TryAdd(0x61, typeof(RequestFriendDel));
            ClientPackets.TryAdd(0x62, typeof(CharacterRestore));
            //ClientPackets.TryAdd(0x63, typeof(RequestQuestList));
            ClientPackets.TryAdd(0x64, typeof(RequestQuestAbort));
            ClientPackets.TryAdd(0x66, typeof(RequestPledgeInfo));
            //ClientPackets.TryAdd(0x67, typeof(RequestPledgeExtendedInfo));
            //ClientPackets.TryAdd(0x68, typeof(RequestPledgeCrest));
            //ClientPackets.TryAdd(0x69, typeof(RequestSurrenderPersonally));
            //ClientPackets.TryAdd(0x6a, typeof(RequestRide));
            ClientPackets.TryAdd(0x6b, typeof(RequestAcquireSkillInfo));
            ClientPackets.TryAdd(0x6c, typeof(RequestAcquireSkill));
            ClientPackets.TryAdd(0x6d, typeof(RequestRestartPoint));
            //ClientPackets.TryAdd(0x6e, typeof(RequestGMCommand));
            //ClientPackets.TryAdd(0x6f, typeof(RequestListPartyWaiting));
            //ClientPackets.TryAdd(0x70, typeof(RequestPartyMatchList));
            //ClientPackets.TryAdd(0x71, typeof(RequestJoinPartyRoom));
            //ClientPackets.TryAdd(0x72, typeof(RequestCrystallizeItem));
            //ClientPackets.TryAdd(0x73, typeof(RequestPrivateStoreSellManageList));
            //ClientPackets.TryAdd(0x74, typeof(SetPrivateStoreSellList));
            //ClientPackets.TryAdd(0x75, typeof(RequestPrivateStoreSellManageCancel));
            //ClientPackets.TryAdd(0x76, typeof(RequestPrivateStoreSellQuit));
            //ClientPackets.TryAdd(0x77, typeof(SetPrivateStoreSellMsg));
            //ClientPackets.TryAdd(0x79, typeof(SendPrivateStoreBuyList));
            //ClientPackets.TryAdd(0x7a, typeof(RequestReviveReply));
            ClientPackets.TryAdd(0x7b, typeof(RequestTutorialLinkHtml));
            ClientPackets.TryAdd(0x7c, typeof(RequestTutorialPassCmdToServer));
            //ClientPackets.TryAdd(0x7d, typeof(RequestTutorialQuestionMarkPressed));
            //ClientPackets.TryAdd(0x7e, typeof(RequestTutorialClientEvent));
            //ClientPackets.TryAdd(0x7f, typeof(RequestPetition));
            //ClientPackets.TryAdd(0x80, typeof(RequestPetitionCancel));
            //ClientPackets.TryAdd(0x81, typeof(RequestGMList));
            //ClientPackets.TryAdd(0x82, typeof(RequestJoinAlly));
            //ClientPackets.TryAdd(0x83, typeof(RequestAnswerJoinAlly));
            //ClientPackets.TryAdd(0x84, typeof(RequestWithdrawAlly));
            //ClientPackets.TryAdd(0x85, typeof(RequestOustAlly));
            //ClientPackets.TryAdd(0x86, typeof(RequestDismissAlly));
            //ClientPackets.TryAdd(0x87, typeof(RequestSetAllyCrest));
            //ClientPackets.TryAdd(0x88, typeof(RequestAllyCrest));
            ClientPackets.TryAdd(0x89, typeof(RequestChangePetName));
            ClientPackets.TryAdd(0x8a, typeof(RequestPetUseItem));
            ClientPackets.TryAdd(0x8b, typeof(RequestGiveItemToPet));
            ClientPackets.TryAdd(0x8c, typeof(RequestGetItemFromPet));
            //ClientPackets.TryAdd(0x8e, typeof(RequestAllyInfo));
            //ClientPackets.TryAdd(0x8f, typeof(RequestPetGetItem));
            //ClientPackets.TryAdd(0x90, typeof(RequestPrivateStoreBuyManageList));
            //ClientPackets.TryAdd(0x91, typeof(SetPrivateStoreBuyList));
            //ClientPackets.TryAdd(0x93, typeof(RequestPrivateStoreBuyManageQuit));
            //ClientPackets.TryAdd(0x94, typeof(SetPrivateStoreBuyMsg));
            //ClientPackets.TryAdd(0x96, typeof(RequestPrivateStoreSellList));
            //ClientPackets.TryAdd(0x97, typeof(SendTimeCheck));
            //ClientPackets.TryAdd(0x98, typeof(RequestStartAllianceWar));
            //ClientPackets.TryAdd(0x99, typeof(ReplyStartAllianceWar));
            //ClientPackets.TryAdd(0x9a, typeof(RequestStopAllianceWar));
            //ClientPackets.TryAdd(0x9b, typeof(ReplyStopAllianceWar));
            //ClientPackets.TryAdd(0x9c, typeof(RequestSurrenderAllianceWar));
            ClientPackets.TryAdd(0x9d, typeof(RequestSkillCoolTime));
            //ClientPackets.TryAdd(0x9e, typeof(RequestPackageSenadbleItemList));
            //ClientPackets.TryAdd(0x9f, typeof(RequestPackageSend));
            //ClientPackets.TryAdd(0xa0, typeof(RequestBlock));
            //ClientPackets.TryAdd(0xa1, typeof(RequestCastleSiegeInfo));
            //ClientPackets.TryAdd(0xa2, typeof(RequestCastleSiegeAttackerList));
            //ClientPackets.TryAdd(0xa3, typeof(RequestCastleSiegeDefenderList));
            //ClientPackets.TryAdd(0xa4, typeof(RequestJoinCastleSiege));
            //ClientPackets.TryAdd(0xa5, typeof(RequestConfirmCastleSiegeWaitingList));
            //ClientPackets.TryAdd(0xa6, typeof(RequestSetCastleSiegeTime));
            //ClientPackets.TryAdd(0xa7, typeof(RequestMultiSellChoose));
            ClientPackets.TryAdd(0xa8, typeof(NetPingResponse));
            //ClientPackets.TryAdd(0xa9, typeof(RequestRemainTime));
            ClientPackets.TryAdd(0xaa, typeof(BypassUserCmd));
            //ClientPackets.TryAdd(0xab, typeof(GMSnoopEnd));
            ClientPackets.TryAdd(0xac, typeof(RequestRecipeBookOpen));
            ClientPackets.TryAdd(0xad, typeof(RequestRecipeBookDestroy));
            ClientPackets.TryAdd(0xae, typeof(RequestRecipeItemMakeInfo));
            ClientPackets.TryAdd(0xaf, typeof(RequestRecipeItemMakeSelf));
            //ClientPackets.TryAdd(0xb0, typeof(RequestRecipeShopManageList));
            //ClientPackets.TryAdd(0xb1, typeof(RequestRecipeShopMessageSet));
            //ClientPackets.TryAdd(0xb2, typeof(RequestRecipeShopListSet));
            //ClientPackets.TryAdd(0xb3, typeof(RequestRecipeShopManageQuit));
            //ClientPackets.TryAdd(0xb4, typeof(RequestRecipeShopManageCancel));
            //ClientPackets.TryAdd(0xb5, typeof(RequestRecipeShopMakeInfo));
            //ClientPackets.TryAdd(0xb6, typeof(RequestRecipeShopMakeDo));
            //ClientPackets.TryAdd(0xb7, typeof(RequestRecipeShopSellList));
            ClientPackets.TryAdd(0xb8, typeof(ObserverReturn));
            //ClientPackets.TryAdd(0xb9, typeof(VoteSociality)); -Evaluate
            //ClientPackets.TryAdd(0xba, typeof(RequestHennaItemList));
            //ClientPackets.TryAdd(0xbb, typeof(RequestHennaItemInfo));
            //ClientPackets.TryAdd(0xbc, typeof(RequestHennaEquip));
            //ClientPackets.TryAdd(0xbd, typeof(RequestHennaUnequipList));
            //ClientPackets.TryAdd(0xbe, typeof(RequestHennaUnequipInfo));
            //ClientPackets.TryAdd(0xbf, typeof(RequestHennaUnequip));
            //ClientPackets.TryAdd(0xc0, typeof(RequestPledgePower));
            //ClientPackets.TryAdd(0xc1, typeof(RequestMakeMacro));
            //ClientPackets.TryAdd(0xc2, typeof(RequestDeleteMacro));
            //ClientPackets.TryAdd(0xc3, typeof(RequestProcureCrop));
            //ClientPackets.TryAdd(0xc4, typeof(RequestBuySeed));
            //ClientPackets.TryAdd(0xc5, typeof(ConfirmDlg));
            //ClientPackets.TryAdd(0xc6, typeof(RequestPreviewItem));
            //ClientPackets.TryAdd(0xc7, typeof(RequestSSQStatus));
            //ClientPackets.TryAdd(0xc8, typeof(PetitionVote));
            //ClientPackets.TryAdd(0xca, typeof(ReplyGameGuardQuery));
            //ClientPackets.TryAdd(0xcc, typeof(RequestSendFriendSay));
            ClientPackets.TryAdd(0xcd, typeof(RequestShowMiniMap));
            ClientPackets.TryAdd(0xce, typeof(RequestSendMsnChatLog));
            ClientPackets.TryAdd(0xcf, typeof(RequestRecordInfo));
            //ClientPacketsD0.TryAdd(0x01, typeof(RequestOustFromPartyRoom));
            //ClientPacketsD0.TryAdd(0x02, typeof(RequestDismissPartyRoom));
            //ClientPacketsD0.TryAdd(0x03, typeof(RequestWithrawPartyRoom));
            //ClientPacketsD0.TryAdd(0x04, typeof(RequestChangePartyLeader));
            ClientPacketsD0.TryAdd(0x05, typeof(RequestAutoSoulShot));
            //ClientPacketsD0.TryAdd(0x06, typeof(RequestExEnchantSkillInfo));
            //ClientPacketsD0.TryAdd(0x07, typeof(RequestExEnchantSkill));
            ClientPacketsD0.TryAdd(0x08, typeof(RequestManorList));
            //ClientPacketsD0.TryAdd(0x09, typeof(RequestProcureCropList));
            //ClientPacketsD0.TryAdd(0x0a, typeof(RequestSetSeed));
            //ClientPacketsD0.TryAdd(0x0b, typeof(RequestSetCrop));
            //ClientPacketsD0.TryAdd(0x0c, typeof(RequestWriteHeroWords));
            //ClientPacketsD0.TryAdd(0x0d, typeof(RequestExAskJoinMPCC));
            //ClientPacketsD0.TryAdd(0x0e, typeof(RequestExAcceptJoinMPCC));
            //ClientPacketsD0.TryAdd(0x0f, typeof(RequestExOustFromMPCC));
            //ClientPacketsD0.TryAdd(0x10, typeof(RequestExPledgeCrestLarge));
            ClientPacketsD0.TryAdd(0x11, typeof(RequestExSetPledgeCrestLarge));
            //ClientPacketsD0.TryAdd(0x12, typeof(RequestOlypmiadObserverEnd));
            //ClientPacketsD0.TryAdd(0x13, typeof(RequestOlympiadMatchList));
            //ClientPacketsD0.TryAdd(0x14, typeof(RequestAskJoinPartyRoom));
            //ClientPacketsD0.TryAdd(0x15, typeof(AnswerJoinPartyRoom));
            //ClientPacketsD0.TryAdd(0x16, typeof(RequestListPartyMatchingWaitingRoom));
            //ClientPacketsD0.TryAdd(0x17, typeof(RequestExitPartyMatchingWaitingRoom));
            //ClientPacketsD0.TryAdd(0x18, typeof(RequestGetBossRecord));
            //ClientPacketsD0.TryAdd(0x19, typeof(RequestPledgeSetAcademyMaster));
            //ClientPacketsD0.TryAdd(0x1a, typeof(RequestPledgePowerGradeList));
            //ClientPacketsD0.TryAdd(0x1b, typeof(RequestPledgeMemberPowerInfo));
            //ClientPacketsD0.TryAdd(0x1c, typeof(RequestPledgeSetMemberPowerGrade));
            ClientPacketsD0.TryAdd(0x1d, typeof(RequestPledgeMemberInfo));
            //ClientPacketsD0.TryAdd(0x1e, typeof(RequestPledgeWarList));
            //ClientPacketsD0.TryAdd(0x1f, typeof(RequestExFishRanking));
            //ClientPacketsD0.TryAdd(0x20, typeof(RequestPCCafeCouponUse));
            ClientPacketsD0.TryAdd(0x22, typeof(RequestCursedWeaponList));
            ClientPacketsD0.TryAdd(0x23, typeof(RequestCursedWeaponLocation));
            //ClientPacketsD0.TryAdd(0x24, typeof(RequestPledgeReorganizeMember));
            //ClientPacketsD0.TryAdd(0x26, typeof(RequestExMPCCShowPartyMembersInfo));
            //ClientPacketsD0.TryAdd(0x27, typeof(RequestDuelStart));
            //ClientPacketsD0.TryAdd(0x28, typeof(RequestDuelAnswerStart));
            //ClientPacketsD0.TryAdd(0x29, typeof(RequestConfirmTargetItem));
            //ClientPacketsD0.TryAdd(0x2a, typeof(RequestConfirmRefinerItem));
            //ClientPacketsD0.TryAdd(0x2b, typeof(RequestConfirmGemStone));
            //ClientPacketsD0.TryAdd(0x2c, typeof(RequestRefine));
            //ClientPacketsD0.TryAdd(0x2d, typeof(RequestConfirmCancelItem));
            //ClientPacketsD0.TryAdd(0x2e, typeof(RequestRefineCancel));
            //ClientPacketsD0.TryAdd(0x2f, typeof(RequestExMagicSkillUseGround));
            //ClientPacketsD0.TryAdd(0x30, typeof(RequestDuelSurrender));
            //ClientPacketsD0.TryAdd(0x31, typeof(RequestExChangeName));
        }

        public static void HandlePacket(Packet packet, GameClient client)
        {
            PacketBase packetBase = null;

            if (packet.FirstOpcode != 0xD0)
            {
                Log.Info($"Received packet with Opcode:{packet.FirstOpcode:X2}");
                if (ClientPackets.ContainsKey(packet.FirstOpcode))
                    packetBase = (PacketBase)Activator.CreateInstance(ClientPackets[packet.FirstOpcode], packet, client);
            }
            else
            {
                Log.Info($"Received packet with Opcode 0xD0 and seccond Opcode:{packet.SecondOpcode:X2}");
                if (ClientPacketsD0.ContainsKey((short)packet.SecondOpcode))
                    packetBase = (PacketBase)Activator.CreateInstance(ClientPacketsD0[(short)packet.SecondOpcode], packet, client);
            }

            if (client.IsTerminated)
                return;

            if (packetBase == null)
                throw new ArgumentNullException(nameof(packetBase), $"Packet with opcode: {packet.FirstOpcode:X2} doesn't exist in the dictionary.");

            packetBase.RunImpl();
        }
    }
}