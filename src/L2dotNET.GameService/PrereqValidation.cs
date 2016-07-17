using System;
using log4net;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.GameService
{
    public class PreReqValidation
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PreReqValidation));

        [Inject]
        public ICheckService CheckService => GameServer.Kernel.Get<ICheckService>();

        private static volatile PreReqValidation _instance;
        private static readonly object SyncRoot = new object();

        public static PreReqValidation Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new PreReqValidation();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            if (CheckService.PreCheckRepository())
                return;

            Log.Warn("Some checks have failed. Please correct the errors and try again.");
            Log.Info("Press ENTER to exit...");
            Console.Read();
            Environment.Exit(0);
        }
    }
}