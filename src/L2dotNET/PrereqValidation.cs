using System;
using System.Threading.Tasks;
using NLog;

namespace L2dotNET
{
    public class PreReqValidation : IInitialisable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public bool Initialised { get; private set; }

        public PreReqValidation()
        {
        }

        public async Task Initialise()
        {
            if (Initialised)
            {
                return;
            }

            //TODO: Add Check service
            if (true)
            {
                Initialised = true;
                return;
            }

            Log.Warn("Some checks have failed. Please correct the errors and try again.");
            Log.Info("Press ENTER to exit...");
            Console.Read();
            Environment.Exit(0);
        }
    }
}