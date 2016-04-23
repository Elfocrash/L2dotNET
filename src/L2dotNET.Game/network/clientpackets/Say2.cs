using L2dotNET.Game.managers;
using L2dotNET.Game.model.items;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.world;
using L2dotNET.Game.model.player.basic;
using System;

namespace L2dotNET.Game.network.l2recv
{
    class Say2 : GameServerNetworkRequest
    {
        public Say2(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private string _text, _target = null;
        private SayIDList Type;
        public override void read()
        {
            _text = readS();
            int typeId = readD();

            if(typeId < 0 || typeId >= SayID.MaxID)
                typeId = 0;

            Type = SayID.getType((byte)typeId);

            if (Type == SayIDList.CHAT_TELL)
                _target = readS();
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            //if (_text.Contains("	Type=1 	ID=") && _text.Contains("	Color=0 	Underline=0 	Title="))
            //{
            //    string tx = _text.Replace("	Type=1 	ID=", "\f");
            //    tx = tx.Split('\f')[1].Split(' ')[0];
            //    int id = int.Parse(tx);
            //    L2Item item = player.getItemByObjId(id);
            //    if (item == null)
            //    {
            //        player.sendMessage("You cant publish this item.");
            //        player.sendActionFailed();
            //        return;
            //    }
            //    else
            //        RqItemManager.getInstance().postItem(item);
            //}

            CreatureSay cs = new CreatureSay(player.ObjID, Type, player.Name, _text);

            switch (Type)
            {
                case SayIDList.CHAT_NORMAL:
                    {
                        char[] arr = _text.ToCharArray();
                        if (arr[0] == '.')
                        {
                            if(PointCmdManager.getInstance().pointed(player, _text))
                                return;
                        }

                        foreach (L2Object o in player.knownObjects.Values)
                        {
                            if (o is L2Player)
                            {
                                if (player.isInsideRadius(o, 1250, true, false))
                                    o.sendPacket(cs);
                            }
                        }

                        player.sendPacket(cs);
                    }
                    break;
                case SayIDList.CHAT_SHOUT:
                    {
                        switch (Cfg.chat_shout)
                        {
                            case Cfg.chatoptions.Default:
                                L2World.getInstance().broadcastToRegion(player.InstanceID, player.X, player.Y, cs);
                                break;
                            case Cfg.chatoptions.Disabled:
                                {
                                    player.sendSystemMessage(346); //Chat disabled.
                                    player.sendActionFailed();
                                    return;
                                }
                            case Cfg.chatoptions.Global:
                                foreach (L2Player p in L2World.getInstance().getAllPlayers())
                                {
                                    p.sendPacket(cs);
                                }
                                break;
                            case Cfg.chatoptions.GMonly:
                                if (player.Builder == 0)
                                {
                                    player.sendMessage("This chat type is restricted for GM characters. You are not supposed to use it.");
                                }
                                else
                                {
                                    foreach (L2Player p in L2World.getInstance().getAllPlayers())
                                    {
                                        p.sendPacket(cs);
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case SayIDList.CHAT_TELL:
                    {
                        L2Player target;
                        if (player.Name.Equals(_target))
                            target = player;
                        else
                            target = L2World.getInstance().getPlayer(_target);

                        if (target == null)
                        {
                            //$s1 is not currently logged in.
                            SystemMessage sm = new SystemMessage(3);
                            sm.addString(_target);
                            player.sendPacket(sm);

                            player.sendActionFailed();
                            return;
                        }
                        else
                        {
                            if (target.WhieperBlock)
                            {
                                //That person is in message refusal mode.
                                player.sendSystemMessage(176);
                                player.sendActionFailed();
                                return;
                            }
                            else
                            {
                                player.sendPacket(new CreatureSay(player.ObjID, Type, "->" + target.Name, _text));
                                target.sendPacket(cs);
                            }
                        }
                    }
                    break;
                case SayIDList.CHAT_PARTY:
                    if (player.Party != null)
                        player.Party.broadcastToMembers(cs);
                    break;
                case SayIDList.CHAT_MARKET:
                    {
                        switch (Cfg.chat_trade)
                        {
                            case Cfg.chatoptions.Default:
                                L2World.getInstance().broadcastToRegion(player.InstanceID, player.X, player.Y, cs);
                                break;
                            case Cfg.chatoptions.Disabled:
                                {
                                    player.sendSystemMessage(346); //Chat disabled.
                                    player.sendActionFailed();
                                    return;
                                }
                            case Cfg.chatoptions.Global:
                                foreach (L2Player p in L2World.getInstance().getAllPlayers())
                                {
                                    p.sendPacket(cs);
                                }
                                break;
                            case Cfg.chatoptions.GMonly:
                                if (player.Builder == 0)
                                {
                                    player.sendMessage("This chat type is restricted for GM characters. You are not supposed to use it.");
                                }
                                else
                                {
                                    foreach (L2Player p in L2World.getInstance().getAllPlayers())
                                    {
                                        p.sendPacket(cs);
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case SayIDList.CHAT_HERO:
                    {
                        if (player.Heroic == 1)
                        {
                            foreach (L2Player p in L2World.getInstance().getAllPlayers())
                                p.sendPacket(cs);
                        }
                        else
                            player.sendActionFailed();
                    }
                    break;
                

            }
        }
    }
}
