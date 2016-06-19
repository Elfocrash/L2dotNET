using Newtonsoft.Json;

namespace L2dotNET.GameService.Config
{
    public class Login

    {
        ///<summary>LoginServer setting.</summary>
        [JsonProperty(PropertyName = "LoginServer")]
        public LoginServer LoginServer { get; set; }

        ///<summary>Database informations.</summary>
        [JsonProperty(PropertyName = "LoginDatabase")]
        public LoginDatabase LoginDatabase { get; set; }

        ///<summary>Security.</summary>
        [JsonProperty(PropertyName = "Security")]
        public Security Security { get; set; }

        ///<summary>Test server, shoudnt be touched in live server.</summary>
        [JsonProperty(PropertyName = "LoginDeveloperConfig")]
        public LoginDeveloperConfig LoginDeveloperConfig { get; set; }
    }

    ///<summary>LoginServer.</summary>
    public class LoginServer
    {
        ///<summary>This is transmitted to the clients connecting from an external network, so it has to be a public IP or resolvable hostname.</summary>
        [JsonProperty(PropertyName = "ExternalHostname")]
        public string ExternalHostname { get; set; }

        ///<summary>This is transmitted to the client from the same network, so it has to be a local IP or resolvable hostname.</summary>
        [JsonProperty(PropertyName = "InternalHostname")]
        public string InternalHostname { get; set; }

        ///<summary>Bind ip of the LoginServer, use * to bind on all available IPs.</summary>
        [JsonProperty(PropertyName = "LoginserverHostname")]
        public string LoginserverHostname { get; set; }
        [JsonProperty(PropertyName = "LoginserverPort")]
        public int LoginserverPort { get; set; }

        ///<summary>How many times you can provide an invalid account/pass before the IP gets banned.</summary>
        [JsonProperty(PropertyName = "LoginTryBeforeBan")]
        public int LoginTryBeforeBan { get; set; }

        ///<summary>Time you won't be able to login back again after LoginTryBeforeBan tries to login. Provide a value in seconds. Default 10min. (600).</summary>
        [JsonProperty(PropertyName = "LoginBlockAfterBan")]
        public int LoginBlockAfterBan { get; set; }

        ///<summary>The address on which login will listen for GameServers, use * to bind on all available IPs.</summary>
        [JsonProperty(PropertyName = "LoginHostname")]
        public string LoginHostname { get; set; }

        ///<summary>The port on which login will listen for GameServers.</summary>
        [JsonProperty(PropertyName = "LoginPort")]
        public int LoginPort { get; set; }

        ///<summary>If set to true any GameServer can register on your login's free slots.</summary>
        [JsonProperty(PropertyName = "AcceptNewGameServer")]
        public bool AcceptNewGameServer { get; set; }

        ///<summary>If false, the licence (after the login) will not be shown.</summary>
        ///<summary>It is highly recomended for Account Security to leave this option as default (True).</summary>
        [JsonProperty(PropertyName = "ShowLicence")]
        public bool ShowLicence { get; set; }
    }

    ///<summary>Database informations.</summary>
    public class LoginDatabase
    {
        [JsonProperty(PropertyName = "URL")]
        public string URL { get; set; }

        [JsonProperty(PropertyName = "Login")]
        public string Login { get; set; }
        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "MaximumDbConnections")]
        public int MaximumDbConnections { get; set; }

        ///<summary>Usable values: "true" - "false", use this option to choose whether accounts will be created automatically or not.</summary>
        [JsonProperty(PropertyName = "AutoCreateAccounts")]
        public bool AutoCreateAccounts { get; set; }
    }

    ///<summary>Security.</summary>
    public class Security
    {
        ///<summary>Log all events from loginserver (account creation, failed/success login, etc).</summary>
        [JsonProperty(PropertyName = "LogLoginController")]
        public bool LogLoginController { get; set; }

        ///<summary>FloodProtection. time in ms.</summary>
        [JsonProperty(PropertyName = "EnableFloodProtection")]
        public bool EnableFloodProtection { get; set; }

        [JsonProperty(PropertyName = "FastConnectionLimit")]
        public int FastConnectionLimit { get; set; }
        [JsonProperty(PropertyName = "NormalConnectionTime")]
        public int NormalConnectionTime { get; set; }
        [JsonProperty(PropertyName = "FastConnectionTime")]
        public int FastConnectionTime { get; set; }
        [JsonProperty(PropertyName = "MaxConnectionPerIP")]
        public int MaxConnectionPerIP { get; set; }
    }

    ///<summary>Test server, shoudnt be touched in live server.</summary>
    public class LoginDeveloperConfig
    {
        [JsonProperty(PropertyName = "Debug")]
        public bool Debug { get; set; }
        [JsonProperty(PropertyName = "Developer")]
        public bool Developer { get; set; }
        [JsonProperty(PropertyName = "PacketHandlerDebug")]
        public bool PacketHandlerDebug { get; set; }
    }
}