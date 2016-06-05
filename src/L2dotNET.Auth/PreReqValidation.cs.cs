using System;
using log4net;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.LoginService
{
    public class PreReqValidation
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PreReqValidation));

        [Inject]
        public ICheckService checkService
        {
            get { return LoginServer.Kernel.Get<ICheckService>(); }
        }

        private static volatile PreReqValidation instance;
        private static readonly object syncRoot = new object();

        public static PreReqValidation Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new PreReqValidation();
                        }
                    }
                }

                return instance;
            }
        }

        public PreReqValidation() { }

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