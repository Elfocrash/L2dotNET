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
            //ClientPackets.TryAdd(0x1A, typeof(RequestStartTrade)); -Unused? 
            ClientPackets.TryAdd(0x1b, typeof(RequestSocialAction));
            //ClientPackets.TryAdd(0x1c, typeof(ChangeMoveType)); -Set walk/run
            //ClientPackets.TryAdd(0x1d, typeof(ChangeWaitType)); -Set sit/stand
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
            ClientPackets.TryAdd(0x29, typeof(RequestJoinParty));
            ClientPackets.TryAdd(0x2a, typeof(RequestAnswerJoinParty));
            ClientPackets.TryAdd(0x2b, typeof(RequestWithdrawalParty));
            ClientPackets.TryAdd(0x2c, typeof(RequestOustPartyMember));
            ClientPackets.TryAdd(0x2f, typeof(RequestMagicSkillUse));
            ClientPackets.TryAdd(0x30, typeof(Appearing));
            ClientPackets.TryAdd(0x31, typeof(RequestWarehouseDeposit));
            ClientPackets.TryAdd(0x32, typeof(RequestWarehouseWithdraw));
            ClientPackets.TryAdd(0x33, typeof(RequestShortCutReg));
            ClientPackets.TryAdd(0x35, typeof(RequestShortCutDel));
            ClientPackets.TryAdd(0x36, typeof(CannotMoveAnymore));
            ClientPackets.TryAdd(0x37, typeof(RequestTargetCanceld));
            ClientPackets.TryAdd(0x38, typeof(Say2));
            //ClientPackets.TryAdd(0x3B, typeof(RequestWarehouseDeposit)); -This is only 3B in non interlude should be 31
            ClientPackets.TryAdd(0x3F, typeof(RequestSkillList));
            ClientPackets.TryAdd(0x42, typeof(RequestGetOnVehicle));
            ClientPackets.TryAdd(0x43, typeof(RequestGetOffVehicle));
            ClientPackets.TryAdd(0x44, typeof(AnswerTradeRequest));
            ClientPackets.TryAdd(0x45, typeof(RequestActionUse));
            ClientPackets.TryAdd(0x46, typeof(RequestRestart));
            ClientPackets.TryAdd(0x48, typeof(ValidatePosition));
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
            //ClientPackets.TryAdd(0x5b, typeof(SendBypassBuildCmd));
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
            //ClientPackets.TryAdd(0x68, typeof(RequestPledgeCrest));
            //ClientPackets.TryAdd(0x69, typeof(RequestSurrenderPersonally));
            ClientPackets.TryAdd(0x6b, typeof(RequestAcquireSkillInfo));
            ClientPackets.TryAdd(0x6c, typeof(RequestAcquireSkill));
            ClientPackets.TryAdd(0x6d, typeof(RequestRestartPoint));
            ClientPackets.TryAdd(0x73, typeof(RequestAcquireSkillInfo));
            ClientPackets.TryAdd(0x5b, typeof(SendBypassBuildCmd));
            ClientPackets.TryAdd(0x75, typeof(RequestMoveToLocationInVehicle));
            ClientPackets.TryAdd(0x7C, typeof(RequestAcquireSkill));
            ClientPackets.TryAdd(0x7D, typeof(RequestRestartPoint));
            ClientPackets.TryAdd(0x80, typeof(RequestPartyMatchList));
            ClientPackets.TryAdd(0x85, typeof(RequestTutorialLinkHtml));
            ClientPackets.TryAdd(0x86, typeof(RequestTutorialPassCmdToServer));
            ClientPackets.TryAdd(0x8c, typeof(RequestGetItemFromPet));
            ClientPackets.TryAdd(0x93, typeof(RequestChangePetName));
            ClientPackets.TryAdd(0x94, typeof(RequestPetUseItem));
            ClientPackets.TryAdd(0x95, typeof(RequestGiveItemToPet));
            ClientPackets.TryAdd(0x9d, typeof(RequestSkillCoolTime));
            ClientPackets.TryAdd(0xaa, typeof(BypassUserCmd));
            ClientPackets.TryAdd(0xB0, typeof(MultiSellChoose));
            ClientPackets.TryAdd(0xB1, typeof(NetPingResponse));
            ClientPackets.TryAdd(0xB5, typeof(RequestRecipeBookOpen));
            ClientPackets.TryAdd(0xB6, typeof(RequestRecipeBookDestroy));
            ClientPackets.TryAdd(0xB7, typeof(RequestRecipeItemMakeInfo));
            ClientPackets.TryAdd(0xB8, typeof(RequestRecipeItemMakeSelf));
            ClientPackets.TryAdd(0xC1, typeof(ObserverReturn));
            ClientPackets.TryAdd(0xC7, typeof(RequestWearItem));
            ClientPackets.TryAdd(0xcd, typeof(RequestShowMiniMap));
            ClientPackets.TryAdd(0xcf, typeof(RequestRecordInfo));
            ClientPacketsD0.TryAdd(0x05, typeof(RequestAutoSoulShot));
            ClientPacketsD0.TryAdd(0x08, typeof(RequestManorList));
            ClientPacketsD0.TryAdd(0x11, typeof(RequestExSetPledgeCrestLarge));
            ClientPacketsD0.TryAdd(0x1d, typeof(RequestPledgeMemberInfo));
            ClientPacketsD0.TryAdd(0x22, typeof(RequestCursedWeaponList));
            ClientPacketsD0.TryAdd(0x23, typeof(RequestCursedWeaponLocation));
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