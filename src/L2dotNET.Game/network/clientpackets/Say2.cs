using L2dotNET.GameService.managers;
using L2dotNET.GameService.model.items;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.world;
using L2dotNET.GameService.model.player.basic;
using System;

namespace L2dotNET.GameService.network.l2recv
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
                    L2World.Instance.BroadcastToRegion(player.X, player.Y, cs);
                    break;
                case SayIDList.CHAT_TELL:
                    {
                        L2Player target;
                        if (player.Name.Equals(_target))
                            target = player;
                        else
                            target = L2World.Instance.GetPlayer(_target);

                        if (target == null)
                        {
                            //$s1 is not currently logged in.
                            SystemMessage sm = new SystemMessage(3);
                            sm.AddString(_target);
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
                    foreach (L2Player p in L2World.Instance.GetAllPlayers())
                    {
                        p.sendPacket(cs);
                    }
                    break;
                case SayIDList.CHAT_HERO:
                    {
                        if (player.Heroic == 1)
                        {
                            foreach (L2Player p in L2World.Instance.GetAllPlayers())
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
