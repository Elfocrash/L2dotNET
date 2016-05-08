using System;
using System.Net.Sockets;
using L2dotNET.Game.controllers;
using L2dotNET.Game.crypt;
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
using L2dotNET.Game.Managers;
using System.Net;
using log4net;

namespace L2dotNET.Game
{
    class GameServer
    {
        protected TcpListener _listener;
        private static readonly ILog log = LogManager.GetLogger(typeof(GameServer));

        public GameServer()
        {

        }

        public static IKernel Kernel { get; set; }

        public void Start()
        {
            Console.Title = "L2dotNET GameServer";

            Cfg.init("all");

            CharTemplateTable.Instance.Initialize();

            NetworkBlock.Instance.Initialize();
            GameTime.Instance.Initialize();

            IdFactory.Instance.Initialize();

            L2World.Instance.Initialize();

            MapRegionTable.Instance.Initialize();
            ZoneTable.Instance.Initialize();

            NpcTable.Instance.Initialize();
            NpcData.getInstance();
            //  SpawnTable.getInstance();

            //  TSkillTable.getInstance();
            ItemTable.getInstance();
            ItemHandler.getInstance();
            MultiSell.getInstance();
            Capsule.getInstance();
            RecipeTable.getInstance();

            AIManager.getInstance();

            BlowFishKeygen.GenerateKeys();

            AdminAccess.Instance.Initialize(); ;

            QuestManager.getInstance();

            AnnouncementManager.Instance.Initialize();

            AllianceTable.getInstance();
            ClanTable.getInstance();

            log.Info("NpcServer: ");
            StaticObjTable.Instance.Initialize();
            MonsterRace.Instance.Initialize();
            //  SpawnTable.getInstance().Spawn();
            StructureTable.Instance.Initialize();

           HtmCache.Instance.Initialize();

            AuthThread.Instance.Initialize();

            //   GeoData.getInstance();

            _listener = new TcpListener(IPAddress.Any, Cfg.SERVER_PORT);

            bool isListening = false;
            try
            {
                _listener.Start();
                isListening = true;
            }
            catch (SocketException ex)
            {
                log.Error($"Socket Error: '{ ex.SocketErrorCode }'. Message: '{ ex.Message }' (Error Code: '{ ex.NativeErrorCode }')");
            }

            if (isListening)
            {
                log.Info($"Listening Gameservers on port { Cfg.SERVER_PORT }");

                TcpClient clientSocket = default(TcpClient);
                while (true)
                {
                    clientSocket = _listener.AcceptTcpClient();
                    Accept(clientSocket);
                }
            }
        }

        private void Accept(TcpClient clientSocket)
        {
            ClientManager.Instance.addClient(clientSocket);
        }
    }
}
