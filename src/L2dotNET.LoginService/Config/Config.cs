using System.IO;
using Newtonsoft.Json;

namespace L2dotNET.LoginService.Config
{
    public sealed class Config : IInitialisable
    {
        public ServerConfig ServerConfig;
        public bool Initialised { get; private set; }

        //TODO: Rename server.json to prevent name mismatch from GameServer/server.json
        public void Initialise()
        {
            if (Initialised)
                return;

            ServerConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"config\server.json"));
            Initialised = true;
        }
    }
}