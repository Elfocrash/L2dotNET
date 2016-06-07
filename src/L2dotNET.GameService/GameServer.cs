using System;
using System.Net;
using System.Net.Sockets;
using log4net;
using L2dotNET.GameService.Controllers;
using L2dotNET.GameService.Handlers;
using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Npcs.Ai;
using L2dotNET.GameService.Model.Quests;
using L2dotNET.GameService.Network;
using L2dotNET.GameService.Network.LoginAuth;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.Tables.Multisell;
using L2dotNET.GameService.World;
using L2dotNET.Utility;
using Ninject;

namespace L2dotNET.GameService
{
    class GameServer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GameServer));

        protected TcpListener GameServerListener;

        public static IKernel Kernel { get; set; }

        public GameServer() { }

        public void Start()
        {
            Config.Config.Instance.Initialize();

            PreReqValidation.Instance.Initialize();

            CharTemplateTable.Instance.Initialize();

            NetworkBlock.Instance.Initialize();
            GameTime.Instance.Initialize();

            IdFactory.Instance.Initialize();

            L2World.Instance.Initialize();

            MapRegionTable.Instance.Initialize();
            ZoneTable.Instance.Initialize();

            ItemTable.Instance.Initialize();
            ItemHandler.Instance.Initialize();

            NpcTable.Instance.Initialize();
            NpcData.Instance.Initialize();
            //SpawnTable.Instance.Initialize();

            //TSkillTable.Instance.Initialize();

            MultiSell.Instance.Initialize();
            Capsule.Instance.Initialize();
            RecipeTable.Instance.Initialize();

            AIManager.Instance.Initialize();

            BlowFishKeygen.GenerateKeys();

            AdminCommandHandler.Instance.Initialize();

            QuestManager.Instance.Initialize();

            AnnouncementManager.Instance.Initialize();

            AllianceTable.Instance.Initialize();
            ClanTable.Instance.Initialize();

            log.Info("NpcServer: ");
            StaticObjTable.Instance.Initialize();
            //SpawnTable.getInstance().Spawn();
            StructureTable.Instance.Initialize();

            HtmCache.Instance.Initialize();

            AuthThread.Instance.Initialize();

            GameServerListener = new TcpListener(IPAddress.Any, Config.Config.Instance.serverConfig.Port);

            try
            {
                GameServerListener.Start();
            }
            catch (SocketException ex)
            {
                log.Error($"Socket Error: '{ex.SocketErrorCode}'. Message: '{ex.Message}' (Error Code: '{ex.NativeErrorCode}')");
                log.Info($"Press ENTER to exit...");
                Console.Read();
                Environment.Exit(0);
            }

            log.Info($"Listening Gameservers on port {Config.Config.Instance.serverConfig.Port}");

            TcpClient clientSocket;
            while (true)
            {
                clientSocket = GameServerListener.AcceptTcpClient();
                Accept(clientSocket);
            }
        }

        private void Accept(TcpClient clientSocket)
        {
            ClientManager.Instance.addClient(clientSocket);
        }
    }
}