using L2dotNET.Services.Contracts;
using log4net;
using Ninject;
using System;
using System.Linq;

namespace L2dotNET.GameService.tables
{
    sealed class IdFactory
    {
        [Inject]
        public IServerService serverService { get { return GameServer.Kernel.Get<IServerService>(); } }

        private static readonly ILog log = LogManager.GetLogger(typeof(IdFactory));

        private static volatile IdFactory instance;
        private static object syncRoot = new object();

        public const int ID_MIN = 0x10000000, 
                         ID_MAX = 0x7FFFFFFF;

        private int currentId = 1;

        public static IdFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new IdFactory();
                        }
                    }
                }

                return instance;
            }
        }

        public IdFactory()
        {

        }

        public int nextId()
        {
            currentId++;
            return currentId;
        }

        public void Initialize()
        {
            currentId = serverService.GetPlayersObjectIdList().DefaultIfEmpty(ID_MIN).Max();
            
            log.Info($"idfactory: used ids { currentId }");
        }
    }
}
