using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace L2dotNET.LoginService.Config
{
    public sealed class Config : IInitialisable
    {
        public ServerConfig ServerConfig;
        public bool Initialised { get; private set; }

        //TODO: Rename server.json to prevent name mismatch from GameServer/server.json
        public async Task Initialise()
        {
            if (Initialised)
            {
                return;
            }

            ServerConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"config\server.json"));
            Initialised = true;
        }
    }
}