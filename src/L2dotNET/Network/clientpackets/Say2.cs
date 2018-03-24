using System.Linq;
using L2dotNET.Models.player;
using L2dotNET.Models.player.basic;
using L2dotNET.Network.serverpackets;
using L2dotNET.world;

namespace L2dotNET.Network.clientpackets
{
    class Say2 : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _text;
        private readonly SayIDList _type;
        private readonly string _target;

        public Say2(Packet packet, GameClient client)
        {
            _client = client;
            _text = packet.ReadString();
            int typeId = packet.ReadInt();

            if ((typeId < 0) || (typeId >= SayId.MaxId))
                typeId = 0;

            _type = SayId.getType((byte)typeId);

            if (_type == SayIDList.CHAT_TELL)
                _target = packet.ReadString();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            CreatureSay cs = new CreatureSay(player.ObjId, _type, player.Name, _text);

            switch (_type)
            {
                case SayIDList.CHAT_NORMAL:
                    foreach (L2Player target in L2World.Instance.GetPlayers().Where(target => player.IsInsideRadius(target, 1250, true, false) && (player != target)))
                        target.SendPacket(cs);

                    player.SendPacket(cs);
                    break;
                case SayIDList.CHAT_SHOUT:
                    //L2World.Instance.BroadcastToRegion(player.X, player.Y, cs);
                    break;
                case SayIDList.CHAT_TELL:
                    {
                        L2Player target;
                        if (player.Name.Equals(_target))
                            target = player;
                        //else
                        //    target = L2World.Instance.GetPlayer(_target);

                        //if (target == null)
                        //{
                        //    SystemMessage sm = new SystemMessage(SystemMessage.SystemMessageId.S1_IS_NOT_ONLINE);
                        //    sm.AddString(_target);
                        //    player.sendPacket(sm);

                        //    player.sendActionFailed();
                        //    return;
                        //}
                        //else
                        //{
                        //    if (target.WhieperBlock)
                        //    {
                        //        player.sendSystemMessage(SystemMessage.SystemMessageId.THE_PERSON_IS_IN_MESSAGE_REFUSAL_MODE);
                        //        player.sendActionFailed();
                        //        return;
                        //    }
                        //    else
                        //    {
                        //        player.sendPacket(new CreatureSay(player.ObjID, Type, $"->"{target.Name}", _text));
                        //        target.sendPacket(cs);
                        //    }
                        //}
                    }
                    break;
                case SayIDList.CHAT_PARTY:
                    player.Party?.BroadcastToMembers(cs);
                    break;
                case SayIDList.CHAT_MARKET:
                    L2World.Instance.GetPlayers().ForEach(p => p.SendPacket(cs));
                    break;
                case SayIDList.CHAT_HERO:
                    {
                        if (player.Heroic == 1)
                            L2World.Instance.GetPlayers().ForEach(p => p.SendPacket(cs));
                        else
                            player.SendActionFailed();
                    }

                    break;
            }
        }
    }
}