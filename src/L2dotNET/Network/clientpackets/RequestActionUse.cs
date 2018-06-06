using System;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Models.Npcs.Decor;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Tools;

namespace L2dotNET.Network.clientpackets
{
    class RequestActionUse : PacketBase
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private readonly GameClient _client;
        private readonly int _actionId;
        private readonly bool _ctrlPressed;
        private readonly bool _shiftPressed;

        public RequestActionUse(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _actionId = packet.ReadInt();
            _ctrlPressed = packet.ReadInt() == 1;
            _shiftPressed = packet.ReadByte() == 1;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.Dead || (player.PBlockAct == 1))
            {
                player.SendActionFailed();
                return;
            }

            int socialId = -1;
            switch (_actionId)
            {
                case 0:
                    CheckSit(player);
                    break;
                case 1:
                    player.IsRunning = (byte)(player.IsRunning == 1 ? 0 : 1);

                    player.BroadcastUserInfo();
                    break;

                case 12: // Greeting
                    socialId = 2;
                    break;
                case 13: // Victory
                    socialId = 3;
                    break;
                case 14: // Advance
                    socialId = 4;
                    break;
                case 15: //pet change node
                   // player.Summon?.ChangeNode();
                    break;
                case 16: //pet attack
                    //player.Summon?.Attack();
                    break;
                case 17: //pet stop
                   // player.Summon?.Stop();
                    break;
                case 19: //pet unsummon
                  //  player.Summon?.UnSummon();
                    break;
                case 21: //summon change node
                   // player.Summon?.ChangeNode();
                    break;
                case 23: //summon stop
                   // player.Summon?.Stop();
                    break;
                case 24: // Yes
                    socialId = 6;
                    break;
                case 25: // No
                    socialId = 5;
                    break;
                case 26: // Bow
                    socialId = 7;
                    break;
                case 29: // Unaware
                    socialId = 8;
                    break;
                case 30: // Social Waiting
                    socialId = 9;
                    break;
                case 31: // Laugh
                    socialId = 10;
                    break;
                case 33: // Applaud
                    socialId = 11;
                    break;
                case 34: // Dance
                    socialId = 12;
                    break;
                case 35: // Sorrow
                    socialId = 13;
                    break;
                case 38: //mount\dismount
                    if (player.MountType > 0)
                        player.UnMount();
                    else
                        player.MountPet();
                    break;
                case 52: //summon unsummon
                   // player.Summon?.UnSummon();
                    break;
                case 53: //summon move
                   // player.Summon?.Move();
                    break;
                case 54: //pet stop
                   // player.Summon?.Stop();
                    break;
                case 62: // Charm
                    socialId = 14;
                    break;
                case 66: // Shyness
                    socialId = 15;
                    break;
                case 71: //Update by rocknow
                    socialId = 16;
                    break;
                case 72: //Update by rocknow
                    socialId = 17;
                    break;
                case 73: //Update by rocknow
                    socialId = 18;
                    break;
                case 1093: //Maguen Strike
                    //PetCast(player, 16071, 6618, 1, 7);
                    break;
                case 1094: //Maguen Speed Walk
                    //PetCast(player, 16071, 6681, 1);
                    break;
                case 1095: //Maguen Power Strike
                    //PetCast(player, 16072, 6619, 1, 7);
                    break;
                case 1096: //Elite Maguen Speed Walk
                    //PetCast(player, 16072, 6682, 1);
                    break;
                case 1097: //Maguen Recall
                    //PetCast(player, 16071, 6683, 1);
                    break;
                case 1098: //Maguen Party Recall
                   // PetCast(player, 16072, 6684, 1);
                    break;
                case 5002: //Critical Seduction
                    //PetCast(player, 0, 23168, 1);
                    break;
                default:
                    Log.Info($"unrecognized action # {_actionId}");

                    break;
            }

            if (socialId != -1)
                player.BroadcastPacket(new SocialAction(player.ObjId, socialId));
        }

        private void CheckSit(L2Player player)
        {
            if (player.CantMove() || player.IsSittingInProgress())
            {
                player.SendActionFailed();
                return;
            }

            if (player.IsSitting())
            {
                player.Stand();
                return;
            }

            int staticId = 0;
            if (player.Target is L2Chair)
            {
                L2Chair chair = (L2Chair)player.Target;
                if (!chair.IsUsedAlready)
                {
                    double dis = Calcs.CalculateDistance(player, chair, true);
                    if (dis < 150)
                        staticId = chair.StaticId;
                }

                if (player.Builder == 1)
                {
                    double dis = Calcs.CalculateDistance(player, chair, true);
                    if (dis < 150)
                        staticId = chair.StaticId;
                }

                if (staticId > 0)
                    player.SetChair(chair);
            }

            player.Sit();
        }
    }
}