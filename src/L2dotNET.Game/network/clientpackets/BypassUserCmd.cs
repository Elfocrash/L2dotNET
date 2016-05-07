using System;
using L2dotNET.Game.controllers;
using L2dotNET.Game.model.player.partials;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;
using L2dotNET.Game.world;
using L2dotNET.Game.geo;

namespace L2dotNET.Game.network.l2recv
{
    class BypassUserCmd : GameServerNetworkRequest
    {
        public BypassUserCmd(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _command;
        public override void read()
        {
            _command = readD();
        }

        private enum RequestType
        {
            loc = 0
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            switch (_command)
            {
                case 0: // [loc]
                    int regId = 0;//MapRegionTable.getInstance().getRegionSysId(player.X, player.Y);
                    if (regId > 0)
                        player.sendPacket(new SystemMessage(regId).AddNumber(player.X).AddNumber(player.Y).AddNumber(player.Z));
                    else
                        player.sendPacket(new SystemMessage(2361).AddString("Nowhere"));

                    int x = (player.X >> 15) + 9 + 8;
                    int y = (player.Y >> 15) + 10 + 11;
                    player.sendMessage($"Current loc is X:{player.X} Y:{player.Y} Z:{player.Z}");
                    break;
                case 52: // /unstuck
                    player.sendMessage("Unstuck not implemented yet.");
                    break;
                case 62: // /dismount
                    player.sendMessage("Dismount not implemented yet.");
                    break;
                case 77: // [time]
                    GameTime.Instance.ShowInfo(player);
                    break;
                default:
                    player.sendMessage("cmd alias " + _command);
                    break;
            }
        }
    }
}
