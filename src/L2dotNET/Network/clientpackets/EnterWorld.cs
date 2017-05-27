using System.Linq;
using L2dotNET.controllers;
using L2dotNET.managers;
using L2dotNET.model.items;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Plugins;
using L2dotNET.tables;

namespace L2dotNET.Network.clientpackets
{
    class EnterWorld : PacketBase
    {
        private readonly GameClient _client;

        public EnterWorld(Packet packet, GameClient client)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.SetCharLastAccess();
            player.UpdatePlayer(); //Update Char LastAccess

            player.TotalRestore();

            player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.WelcomeToLineage));

            AnnouncementManager.Instance.OnEnter(player);

            foreach (L2Item item in player.Inventory.Items.Where(item => item.IsEquipped != 0))
                item.NotifyStats(player);

            
            // player.sendItemList(false);
            player.SendPacket(new FriendList());
            player.SendQuestList();
            player.UpdateSkillList();
            player.UpdateReuse();

            if (player.ClanId > 0)
                ClanTable.Instance.Apply(player);

            player.SendPacket(new ExStorageMaxCount(player));
            // player.sendPacket(new ExBasicActionList());
            //  NpcTable.getInstance().spawnNpc("grandmaster_ramos", player.X, player.Y, player.Z, player.Heading);
            player.SendActionFailed();

            GameTime.Instance.EnterWorld(player);

            player.Timer();

            player.SpawnMe();
            //L2WorldRegion worldRegion = L2World.Instance.GetRegion(player.X, player.Y);
            //player.SetRegion(worldRegion);
            //player.getKnowns(500, 500, false);


            player.SetupKnows();
            player.SendPacket(new UserInfo(player));

            foreach (Plugin plugin in PluginManager.Instance.Plugins)
                plugin.OnLogin(player);

            //player.sendPacket(new ShortCutInit(player));
            player.StartAi();
            player.CharStatus.StartHpMpRegeneration();
            player.ShowHtm("servnews.htm",player);
            player.BroadcastUserInfo();
        }

        //private int[][] _tracert = new int[5][];
        //public override void Read()
        //{
        //    //readB(32);
        //    //readD();
        //    //readD();
        //    //readD();
        //    //readD();
        //    //readB(32);
        //    //readD();

        //    //for (int i = 0; i < 5; i++)
        //    //{
        //    //    tracert[i] = new int[4];
        //    //    for (int o = 0; o < 4; o++)
        //    //        tracert[i][o] = packet.ReadByte();
        //    //}
        //}
    }
}