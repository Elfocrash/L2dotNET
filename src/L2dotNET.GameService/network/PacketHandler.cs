using System;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Contexts;
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
        private static readonly ConcurrentDictionary<byte, Type> ClientPackets = new ConcurrentDictionary<byte, Type>();

        private static readonly ConcurrentDictionary<short, Type> ClientPacketsD0 = new ConcurrentDictionary<short, Type>();

        static PacketHandler()
        {
            ClientPackets.TryAdd(0x00, typeof(ProtocolVersion));
            ClientPackets.TryAdd(0x08, typeof(AuthLogin));
            ClientPackets.TryAdd(0x09, typeof(Logout));
            ClientPackets.TryAdd(0x0b, typeof(CharacterCreate));
            ClientPackets.TryAdd(0x0d, typeof(CharacterSelected));
            ClientPackets.TryAdd(0x0e, typeof(NewCharacter));
            ClientPackets.TryAdd(0x01, typeof(MoveBackwardToLocation));
            ClientPackets.TryAdd(0x03, typeof(EnterWorld));
            ClientPackets.TryAdd(0x0f, typeof(RequestItemList));
            ClientPackets.TryAdd(0x0a, typeof(AttackRequest));
            ClientPackets.TryAdd(0x11, typeof(RequestUnEquipItem));
            ClientPackets.TryAdd(0x14, typeof(RequestUseItem));
            ClientPackets.TryAdd(0x1A, typeof(RequestStartTrade));
            ClientPackets.TryAdd(0x16, typeof(RequestAddTradeItem));
            ClientPackets.TryAdd(0x17, typeof(RequestTradeDone));
            ClientPackets.TryAdd(0x04, typeof(RequestAction));
            ClientPackets.TryAdd(0x20, typeof(RequestLinkHtml));
            ClientPackets.TryAdd(0x20, typeof(RequestLinkHtml));
            ClientPackets.TryAdd(0x21, typeof(RequestBypassToServer));
            ClientPackets.TryAdd(0x26, typeof(RequestWithdrawalPledge));
            ClientPackets.TryAdd(0x8c, typeof(RequestGetItemFromPet));
            ClientPackets.TryAdd(0x1b, typeof(RequestSocialAction));
            ClientPackets.TryAdd(0x1e, typeof(RequestSellItem));
            ClientPackets.TryAdd(0x2f, typeof(RequestMagicSkillUse));
            ClientPackets.TryAdd(0x30, typeof(Appearing));
            ClientPackets.TryAdd(0x3B, typeof(RequestWarehouseDeposit));
            ClientPackets.TryAdd(0x32, typeof(RequestWarehouseWithdraw));
            ClientPackets.TryAdd(0x33, typeof(RequestShortCutReg));
            ClientPackets.TryAdd(0x35, typeof(RequestShortCutDel));
            ClientPackets.TryAdd(0x1f, typeof(RequestBuyItem));
            ClientPackets.TryAdd(0x29, typeof(RequestJoinParty));
            ClientPackets.TryAdd(0x2a, typeof(RequestAnswerJoinParty));
            ClientPackets.TryAdd(0x2b, typeof(RequestWithDrawalParty));
            ClientPackets.TryAdd(0x2c, typeof(RequestOustPartyMember));
            ClientPackets.TryAdd(0x36, typeof(CannotMoveAnymore));
            ClientPackets.TryAdd(0x37, typeof(RequestTargetCanceld));
            ClientPackets.TryAdd(0x38, typeof(Say2));
            ClientPackets.TryAdd(0x42, typeof(RequestGetOnVehicle));
            ClientPackets.TryAdd(0x43, typeof(RequestGetOffVehicle));
            ClientPackets.TryAdd(0x44, typeof(AnswerTradeRequest));
            ClientPackets.TryAdd(0x45, typeof(RequestActionUse));
            ClientPackets.TryAdd(0x46, typeof(RequestRestart));
            ClientPackets.TryAdd(0x48, typeof(ValidatePosition));
            ClientPackets.TryAdd(0x4a, typeof(StartRotating));
            ClientPackets.TryAdd(0x4b, typeof(FinishRotating));
            ClientPackets.TryAdd(0x57, typeof(RequestShowBoard));
            ClientPackets.TryAdd(0x58, typeof(RequestEnchantItem));
            ClientPackets.TryAdd(0x59, typeof(RequestDestroyItem));
            ClientPackets.TryAdd(0x64, typeof(RequestQuestAbort));
            ClientPackets.TryAdd(0x66, typeof(RequestPledgeInfo));
            ClientPackets.TryAdd(0xcd, typeof(RequestShowMiniMap));
            ClientPackets.TryAdd(0x6D, typeof(RequestSendMsnChatLog));
            ClientPackets.TryAdd(0xcf, typeof(RequestRecordInfo));
            ClientPackets.TryAdd(0x73, typeof(RequestAcquireSkillInfo));
            ClientPackets.TryAdd(0x74, typeof(SendBypassBuildCmd));
            ClientPackets.TryAdd(0x75, typeof(RequestMoveToLocationInVehicle));
            ClientPackets.TryAdd(0x7C, typeof(RequestAcquireSkill));
            ClientPackets.TryAdd(0x7D, typeof(RequestRestartPoint));
            ClientPackets.TryAdd(0x80, typeof(RequestPartyMatchList));
            ClientPackets.TryAdd(0x85, typeof(RequestTutorialLinkHtml));
            ClientPackets.TryAdd(0x86, typeof(RequestTutorialPassCmdToServer));
            //ClientPackets.TryAdd(0x87, typeof(RequestTutorialQuestionMark));
            ClientPackets.TryAdd(0x93, typeof(RequestChangePetName));
            ClientPackets.TryAdd(0x94, typeof(RequestPetUseItem));
            ClientPackets.TryAdd(0x95, typeof(RequestGiveItemToPet));
            ClientPackets.TryAdd(0xB0, typeof(MultiSellChoose));
            ClientPackets.TryAdd(0xB1, typeof(NetPingResponse));
            ClientPackets.TryAdd(0xaa, typeof(BypassUserCmd));
            ClientPackets.TryAdd(0xB5, typeof(RequestRecipeBookOpen));
            ClientPackets.TryAdd(0xB6, typeof(RequestRecipeBookDestroy));
            ClientPackets.TryAdd(0xB7, typeof(RequestRecipeItemMakeInfo));
            ClientPackets.TryAdd(0xB8, typeof(RequestRecipeItemMakeSelf));
            ClientPackets.TryAdd(0xC1, typeof(ObserverReturn));
            ClientPackets.TryAdd(0xC7, typeof(RequestWearItem));
            ClientPacketsD0.TryAdd(0x08, typeof(RequestManorList));
            ClientPacketsD0.TryAdd(0x11, typeof(RequestExSetPledgeCrestLarge));
            ClientPacketsD0.TryAdd(0x05, typeof(RequestAutoSoulShot));
            ClientPacketsD0.TryAdd(0x1d, typeof(RequestPledgeMemberInfo));
            ClientPacketsD0.TryAdd(0x22, typeof(RequestCursedWeaponList));
            ClientPacketsD0.TryAdd(0x23, typeof(RequestCursedWeaponLocation));
        }

        public static void HandlePacket(Packet packet, GameClient client)
        {
            PacketBase packetBase = null;

            if (packet.FirstOpcode != 0xD0)
            {
                Log.Info($"Received packet with Opcode:{packet.FirstOpcode.ToString("X2")}");
                if (ClientPackets.ContainsKey(packet.FirstOpcode))
                    packetBase = (PacketBase)Activator.CreateInstance(ClientPackets[packet.FirstOpcode], packet, client);
            }
            else
            {
                Log.Info($"Received packet with Opcode 0xD0 and seccond Opcode:{packet.SecondOpcode.ToString("X2")}");
                if (ClientPacketsD0.ContainsKey((short)packet.SecondOpcode))
                    packetBase = (PacketBase)Activator.CreateInstance(ClientPacketsD0[(short)packet.SecondOpcode], packet, client);
            }

            if (client.IsTerminated)
                return;

            packetBase?.RunImpl();
        }
    }
}