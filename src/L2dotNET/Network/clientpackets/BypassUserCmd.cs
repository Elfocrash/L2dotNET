using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using L2dotNET.Controllers;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.World;

namespace L2dotNET.Network.clientpackets
{
    class BypassUserCmd : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _command;

        public BypassUserCmd(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _command = packet.ReadInt();
        }

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                switch (_command)
                {
                    case 0: // [loc]
                        int regId = 0; //MapRegionTable.getInstance().getRegionSysId(player.X, player.Y);
                        if (regId > 0)
                            player.SendPacketAsync(new SystemMessage((SystemMessage.SystemMessageId)regId).AddNumber(player.X).AddNumber(player.Y).AddNumber(player.Z));
                        else
                            player.SendPacketAsync(new SystemMessage(SystemMessage.SystemMessageId.NotImplementedYet2361).AddString("Nowhere"));

                        int x = (player.X >> 15) + 9 + 8;
                        int y = (player.Y >> 15) + 10 + 11;
                        player.SendMessageAsync($"Current loc is X:{player.X} Y:{player.Y} Z:{player.Z}");
                        player.BroadcastUserInfoAsync(); //for debug reasons
                        break;
                    case 52: // /unstuck

                        L2WorldRegion worldRegion = L2World.GetRegion(player.X, player.Y);
                        player.SetRegion(worldRegion);
                        List<L2Player> knowns = player.GetKnownPlayers();
                        //player.SpawnMeAsync();
                        player.SendMessageAsync("Unstuck not implemented yet.");
                        //player.knownObjects;
                        break;
                    case 62: // /dismount
                        player.SendMessageAsync("Dismount not implemented yet.");
                        break;
                    case 77: // [time]
                        GameTime.ShowInfoAsync(player);
                        break;
                    default:
                        player.SendMessageAsync($"cmd alias {_command}");
                        break;
                }
            });
        }
    }
}