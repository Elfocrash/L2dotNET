using L2dotNET.Game.network.l2send;
using System;
using L2dotNET.Game.model.player;

namespace L2dotNET.Game.network.l2recv
{
    class RequestShortCutDel : GameServerNetworkRequest
    {
        public RequestShortCutDel(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _slot;
        private int _page;

        public override void read()
        {
            int id = readD();
            _slot = id % 12;
            _page = id / 12;
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            L2Shortcut scx = null;
            foreach (L2Shortcut sc in player._shortcuts)
            {
                if (sc.Slot == _slot && sc.Page == _page)
                {
                    scx = sc;
                    break;
                }
            }

            if (scx == null)
            {
                player.sendActionFailed();
                return;
            }

            lock (player._shortcuts)
            {
                player._shortcuts.Remove(scx);

                //SQL_Block sqb = new SQL_Block("user_shortcuts");
                //sqb.where("ownerId", player.ObjID);
                //sqb.where("classId", player.ActiveClass.id);
                //sqb.where("slot", _slot);
                //sqb.where("page", _page);
                //sqb.sql_delete(false);
            }
            
            player.sendPacket(new ShortCutInit(player));
        }
    }
}
