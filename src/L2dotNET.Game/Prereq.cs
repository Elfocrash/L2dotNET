using L2dotNET.Services.Contracts;
using log4net;
using Ninject;
using System;

namespace L2dotNET.GameService
{
    public class Prereq
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Prereq));

        [Inject]
        public ICheckService checkService { get { return GameServer.Kernel.Get<ICheckService>(); } }

        private static volatile Prereq instance;
        private static object syncRoot = new object();

        public static Prereq Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Prereq();
                        }
                    }
                }

                return instance;
            }
        }

        public Prereq()
        {

        }

        public void Initialize()
        {
            if (!checkService.PreCheckRepository())
            {
                log.Warn($"Some checks have failed. Please correct the errors and try again.");
                log.Info($"Press ENTER to exit...");
                Console.Read();
                Environment.Exit(0);
            }
        }
    }
}
