using L2dotNET.GameService.controllers;
using L2dotNET.GameService.managers;
using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tables;
using L2dotNET.GameService.world;
using L2dotNET.GameService.model.items;
using L2dotNET.GameService.Managers;

namespace L2dotNET.GameService.network.l2recv
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

            player.sendPacket(new SystemMessage(SystemMessage.SystemMessageId.WELCOME_TO_LINEAGE));

            AnnouncementManager.Instance.OnEnter(player);

            foreach (L2Item item in player.Inventory.Items.Values)
            {
                if (item._isEquipped == 0)
                    continue;

                item.notifyStats(player);
            }

            player.StartRegeneration();
           // player.sendItemList(false);
            player.sendPacket(new FriendList());
            player.sendQuestList();
            player.updateSkillList();
            player.updateReuse();

            if (player.ClanId > 0)
            {
                ClanTable.Instance.Apply(player);
            }

            player.sendPacket(new ExStorageMaxCount(player));
           // player.sendPacket(new ExBasicActionList());
          //  NpcTable.getInstance().spawnNpc("grandmaster_ramos", player.X, player.Y, player.Z, player.Heading);
            player.sendActionFailed();

            GameTime.Instance.EnterWorld(player);
            
            player.timer();

            player.SpawnMe();
            //L2WorldRegion worldRegion = L2World.Instance.GetRegion(player.X, player.Y);
            //player.SetRegion(worldRegion);
            //player.getKnowns(500, 500, false);

            player.sendPacket(new UserInfo(player));
            player.sendPacket(new UserInfo(player));

            //player.sendPacket(new ShortCutInit(player));
            player.StartAI();

            player.RequestPing();
        }
    }
}
