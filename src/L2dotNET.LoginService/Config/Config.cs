using System.IO;
using Newtonsoft.Json;

namespace L2dotNET.LoginService.Config
{
    public sealed class Config
    {
        public ServerConfig ServerConfig;

        //TODO: Rename server.json to prevent name mismatch from GameServer/server.json
        public void Initialize()
        {
            ServerConfig = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(@"config\server.json"));
        }
    }
}