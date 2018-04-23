﻿using System;
using System.Linq;
using L2dotNET.Controllers;
using L2dotNET.Managers;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Plugins;
using L2dotNET.Services.Contracts;
using L2dotNET.World;
using Microsoft.Extensions.DependencyInjection;

namespace L2dotNET.Network.clientpackets
{
    class EnterWorld : PacketBase
    {
        private readonly GameClient _client;

        private readonly IPlayerService _playerService;
        private readonly AnnouncementManager _announcementManager;

        public EnterWorld(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _announcementManager = serviceProvider.GetService<AnnouncementManager>();
            _playerService = serviceProvider.GetService<IPlayerService>();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            player.SetCharLastAccess();
            _playerService.UpdatePlayer(player);

            player.TotalRestore();

            player.SendPacket(new SystemMessage(SystemMessage.SystemMessageId.WelcomeToLineage));

            _announcementManager.OnEnter(player);

            foreach (L2Item item in player.Inventory.Items.Where(item => item.IsEquipped != 0))
                item.NotifyStats(player);
            
            // player.sendItemList(false);
            player.SendPacket(new FriendList());
            player.SendQuestList();
            player.UpdateReuse();

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
            L2World.Instance.AddPlayer(player);
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