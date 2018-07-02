using System;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.Models.Player;
using L2dotNET.Models.Player.Basic;
using L2dotNET.Network.serverpackets;
using L2dotNET.World;

namespace L2dotNET.Network.clientpackets
{
    class Say2 : PacketBase
    {
        private readonly GameClient _client;
        private readonly string _text;
        private readonly SayIDList _type;
        private readonly string _target;

        public Say2(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
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

        public override async Task RunImpl()
        {
            await Task.Run(() =>
            {
                L2Player player = _client.CurrentPlayer;

                CreatureSay cs = new CreatureSay(player.ObjId, _type, player.Name, _text);

                switch (_type)
                {
                    case SayIDList.CHAT_NORMAL:
                        foreach (L2Player target in L2World.GetPlayers().Where(target => player.IsInsideRadius(target, 1250, true, false) && (player != target)))
                            target.SendPacketAsync(cs);

                        player.SendPacketAsync(cs);
                        break;
                    case SayIDList.CHAT_SHOUT:
                        //L2World.BroadcastToRegion(player.X, player.Y, cs);
                        break;
                    case SayIDList.CHAT_TELL:
                        {
                            L2Player target;
                            if (player.Name.Equals(_target))
                                target = player;
                            //else
                            //    target = L2World.GetPlayer(_target);

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
                        L2World.GetPlayers().ForEach(p => p.SendPacketAsync(cs));
                        break;
                    case SayIDList.CHAT_HERO:
                        {
                            if (player.Heroic == 1)
                                L2World.GetPlayers().ForEach(p => p.SendPacketAsync(cs));
                            else
                                player.SendActionFailedAsync();
                        }

                        break;
                }
            });
        }
    }
}