using L2dotNET.Game.controllers;
using L2dotNET.Game.managers;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;
using L2dotNET.Game.world;
using L2dotNET.Game.model.items;

namespace L2dotNET.Game.network.l2recv
{
    class EnterWorld : GameServerNetworkRequest
    {
        public EnterWorld(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int[][] tracert = new int[5][];

        public override void read()
        {
            //readB(32);
            //readD();
            //readD();
            //readD();
            //readD();
            //readB(32);
            //readD();

            //for (int i = 0; i < 5; i++)
            //{
            //    tracert[i] = new int[4];
            //    for (int o = 0; o < 4; o++)
            //        tracert[i][o] = readC();
            //}
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            player.TotalRestore();

            player.sendPacket(new SystemMessage(34));

            AnnounceManager.getInstance().onEnter(player);

            if (player.TelbookLimit > 0)
                player.sendPacket(new ExGetBookMarkInfo(player.TelbookLimit, player.Telbook));

            //навешиваем статы уже одетых предметов
            foreach (L2Item item in player.Inventory.Items.Values)
            {
                if (item._isEquipped == 0)
                    continue;

                item.notifyStats(player);
            }

            player.StartRegeneration();
           // player.sendItemList(false);
            //player.Vitality = 20000;
            player.sendPacket(new FriendList());
            player.sendQuestList();
            player.updateSkillList();
            player.updateReuse();

            if (player.ClanId > 0)
            {
                ClanTable.getInstance().apply(player);
            }

            player.sendPacket(new ExStorageMaxCount(player));
           // player.sendPacket(new ExBasicActionList());
          //  NpcTable.getInstance().spawnNpc("grandmaster_ramos", player.X, player.Y, player.Z, player.Heading);
            player.sendActionFailed();

            GameTime.getInstance().enterWorld(player);

            L2World.getInstance().realiseEntry(player, null, true);
            player.timer();

            L2World.getInstance().getRegion(player.X, player.Y).checkZones(player, true);

            player.sendPacket(new UserInfo(player));
           // player.sendPacket(new UserInfo(player));

            //player.sendPacket(new ShortCutInit(player));
            player.StartAI();

            player.RequestPing();
        }
    }
}
