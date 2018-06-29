using System;
using System.Threading.Tasks;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Services.Contracts;

namespace L2dotNET
{
    public class PreReqValidation : IInitialisable
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

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