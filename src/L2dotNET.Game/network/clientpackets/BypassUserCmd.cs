using System;
using L2dotNET.Game.controllers;
using L2dotNET.Game.logger;
using L2dotNET.Game.model.player.partials;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;
using L2dotNET.Game.world;
using L2dotNET.Game.world.instances;
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

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            switch (_command)
            {
                case 0: // [loc]
                    {
                        int regId = MapRegionTable.getInstance().getRegionSysId(player.X, player.Y);
                        if (regId > 0)
                            player.sendPacket(new SystemMessage(regId).addNumber(player.X).addNumber(player.Y).addNumber(player.Z));
                        else
                            player.sendPacket(new SystemMessage(2361).addString("Nowhere"));

                        int x = (player.X >> 15) + 9 + 8;
                        int y = (player.Y >> 15) + 10 + 11;
                        player.sendMessage("l2loc: " + x + "_" + y);
                        // player.sendMessage("geotype " + GeoData.getInstance().nGetType(player.X, player.Y));
                        // player.sendMessage("geoheight " + GeoData.getInstance().getHeight(player.X, player.Y, player.Z));
                        
                    }
                    break;
                case 77: // [time]
                    GameTime.getInstance().ShowInfo(player);
                    break;
                case 114: // [instancezone]
                    {
                        if (player.InstanceReuse.Count == 0)
                            player.sendSystemMessage(2229); //There is no instance zone under a time limit.
                        else
                        {
                            byte x = 0;
                            foreach (db_InstanceReuse db in player.InstanceReuse.Values)
                            {
                                //$s1 will be available for re-use after $s2 hour(s) $s3 minute(s).
                                DateTime dt = DateTime.Now;
                                TimeSpan ts = db.dt - dt;
                                if (ts.TotalMilliseconds < 0)
                                    continue;

                                SystemMessage sm = new SystemMessage(2230);
                                sm.addInstanceName(db.id);
                                sm.addNumber((int)ts.TotalHours);
                                sm.addNumber((int)(ts.Minutes));
                                player.sendPacket(sm);
                                x++;
                            }

                            if (x == 0)
                                player.sendSystemMessage(2229); //There is no instance zone under a time limit.
                        }
                    }
                    break;

                default:
                    player.sendMessage("cmd alias " + _command);
                    break;
            }
        }
    }
}
