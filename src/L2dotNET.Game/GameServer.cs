using System;
using System.Net.Sockets;
using L2dotNET.Game.controllers;
using L2dotNET.Game.crypt;
using L2dotNET.Game.db;
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
using L2dotNET.Game.staticf;
using L2dotNET.Game.tables;
using L2dotNET.Game.tables.multisell;
using L2dotNET.Game.world;
using L2dotNET.Game.geo;

namespace L2dotNET.Game
{
    class GameServer
    {
        private static GameServer gs = new GameServer();
        public static GameServer getInstance()
        {
            return gs;
        }

        protected TcpListener _listener;

        public GameServer()
        {
            Console.Title = "ct26_p1 216";

            CLogger.form();
            Cfg.init("all");

         //   DateTime next = DateTime.Now.AddMinutes(4000);

         //   TimeSpan ts = next - DateTime.Now;

         //   Console.WriteLine("hrs " + (int)ts.TotalHours + " total " + (int)(ts.Minutes)+" " + (int)(ts.TotalMinutes%60));
         //   return;


            PClassess.getInstance();
          //  return;
            //shop_conv.test();
           // Console.Write("end");
          //  return;
          //  double x = 100.01;
        //    x += -23;
         //   Console.WriteLine("res " + x);
          //  DateTime time1 = DateTime.Now;   //Точка начала отсчета времени 
        //    Console.ReadKey();               //Пауза до нажатия клавиши
         //   DateTime time2 = DateTime.Now;   //Точка окончания отсчета времени 
         //   long elapsedTicks = time2.Ticks - time1.Ticks;       // подсчитываем число тактов, один такт соответствует 100 наносекундам
         //   Console.WriteLine(elapsedTicks * 1E-7);  // делим на 10^7 для отображения времени в секундах
          //  Console.ReadKey();
            NetworkBlock.getInstance();
            GameTime.getInstance();

            IdFactory.getInstance().init();

            L2World.getInstance();
            MapRegionTable.getInstance();
            ZoneTable.getInstance();

            NpcTable.getInstance();
            NpcData.getInstance();
            SpawnTable.getInstance();
            StaticObjTable.getInstance().read();
            StructureTable.getInstance().read();
            TSkillTable.getInstance();
            ItemTable.getInstance();
            ItemHandler.getInstance();
            MultiSell.getInstance();
            Capsule.getInstance();
            RecipeTable.getInstance();

            MonsterRace.getInstance();
            
            AIManager.getInstance();


            BlowFishKeygen.genKey();
            CLogger.info("generated 20 blowfish keys");

            SQLjec.getInstance();
            ClassIdContainer.init();
            

            
            

            AdminAccess.getInstance();

            QuestManager.getInstance();

            AnnounceManager.getInstance();

            AllianceTable.getInstance();
            ClanTable.getInstance();
            
            CLogger.info("NpcServer: ");
            StaticObjTable.getInstance().Spawn();
            MonsterRace.getInstance().Spawn();
            SpawnTable.getInstance().Spawn();
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
