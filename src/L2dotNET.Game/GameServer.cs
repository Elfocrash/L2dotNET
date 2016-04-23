using System;
using System.Net.Sockets;
using L2dotNET.Game.controllers;
using L2dotNET.Game.crypt;
using L2dotNET.Game.logger;
using L2dotNET.Game.managers;
using L2dotNET.Game.model.events;
using L2dotNET.Game.model.items;
using L2dotNET.Game.model.npcs.ai;
using L2dotNET.Game.model.quests;
using L2dotNET.Game.model.skills2;
using L2dotNET.Game.network;
using L2dotNET.Game.network.loginauth;
using L2dotNET.Game.network.loginauth.send;
using L2dotNET.Game.tables;
using L2dotNET.Game.tables.multisell;
using L2dotNET.Game.world;
using L2dotNET.Game.geo;
using Ninject;
using L2dotNET.Services.Contracts;
using L2dotNET.Models;

namespace L2dotNET.Game
{
    class GameServer
    {
        protected TcpListener _listener;

        public GameServer()
        {
           
        }

        public static IKernel Kernel { get; set; }

        public void Start()
        {
            Console.Title = "L2dotNET Gameserver";

            CLogger.form();
            Cfg.init("all");

            CharTemplateTable.Instance.Initialize();

            NetworkBlock.getInstance();
            GameTime.getInstance();

            IdFactory.Instance.Initialize();

            L2World.getInstance();
            // MapRegionTable.getInstance();
            ZoneTable.getInstance();

            NpcTable.getInstance();
            NpcData.getInstance();
            //  SpawnTable.getInstance();
            StaticObjTable.getInstance().read();
            StructureTable.getInstance().read();
            //  TSkillTable.getInstance();
            ItemTable.getInstance();
            ItemHandler.getInstance();
            MultiSell.getInstance();
            Capsule.getInstance();
            RecipeTable.getInstance();

            MonsterRace.getInstance();

            AIManager.getInstance();


            BlowFishKeygen.genKey();
            CLogger.info("generated 20 blowfish keys");

            AdminAccess.getInstance();

            QuestManager.getInstance();

            AnnounceManager.getInstance();

            AllianceTable.getInstance();
            ClanTable.getInstance();

            CLogger.info("NpcServer: ");
            StaticObjTable.getInstance().Spawn();
            MonsterRace.getInstance().Spawn();
            //  SpawnTable.getInstance().Spawn();
            StructureTable.getInstance().init();

            HtmCache.getInstance();

            AuthThread.getInstance();

            //   GeoData.getInstance();

            CLogger.extra_info("listening game clients on port " + Cfg.SERVER_PORT);
            _listener = new TcpListener(Cfg.SERVER_PORT);
            _listener.Start();

            TcpClient clientSocket = default(TcpClient);
            while (true)
            {
                clientSocket = _listener.AcceptTcpClient();
                accept(clientSocket);
            }
        }

        private void accept(TcpClient clientSocket)
        {
            ClientManager.getInstance().addClient(clientSocket);
        }
    }
}
