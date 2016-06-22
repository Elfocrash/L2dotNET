using System.ComponentModel;
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
        [DefaultValue("localhost")]
        [JsonProperty(PropertyName = "ExternalHostname")]
        public string ExternalHostname { get; set; }

        ///<summary>This is transmitted to the client from the same network, so it has to be a local IP or resolvable hostname.</summary>
        [DefaultValue("localhost")]
        [JsonProperty(PropertyName = "InternalHostname")]
        public string InternalHostname { get; set; }

        ///<summary>Bind ip of the LoginServer, use * to bind on all available IPs.</summary>
        [DefaultValue("*")]
        [JsonProperty(PropertyName = "LoginserverHostname")]
        public string LoginserverHostname { get; set; }

        [DefaultValue("9013")]
        [JsonProperty(PropertyName = "LoginserverPort")]
        public int LoginserverPort { get; set; }

        ///<summary>How many times you can provide an invalid account/pass before the IP gets banned.</summary>
        [DefaultValue(10)]
        [JsonProperty(PropertyName = "LoginTryBeforeBan")]
        public int LoginTryBeforeBan { get; set; }

        ///<summary>Time you won't be able to login back again after LoginTryBeforeBan tries to login. Provide a value in seconds. Default 10min. (600).</summary>
        [DefaultValue(600)]
        [JsonProperty(PropertyName = "LoginBlockAfterBan")]
        public int LoginBlockAfterBan { get; set; }

        ///<summary>The address on which login will listen for GameServers, use * to bind on all available IPs.</summary>
        [DefaultValue("*")]
        [JsonProperty(PropertyName = "LoginHostname")]
        public string LoginHostname { get; set; }

        ///<summary>The port on which login will listen for GameServers.</summary>
        [DefaultValue(2106)]
        [JsonProperty(PropertyName = "LoginPort")]
        public int LoginPort { get; set; }

        ///<summary>If false, the licence (after the login) will not be shown.</summary>
        ///<summary>It is highly recomended for Account Security to leave this option as default (True).</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "ShowLicence")]
        public bool ShowLicence { get; set; }

        ///<summary>If set to true any GameServer can register on your login's free slots.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AcceptNewGameServer")]
        public bool AcceptNewGameServer { get; set; }

        [DefaultValue(0)]
        [JsonProperty(PropertyName = "RequestServerID")]
        public int RequestServerID { get; set; }

        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AcceptAlternateID")]
        public bool AcceptAlternateID { get; set; }
    }

    ///<summary>Database informations.</summary>
    public class LoginDatabase
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "URL")]
        public string URL { get; set; }

        [DefaultValue("root")]
        [JsonProperty(PropertyName = "Login")]
        public string Login { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }

        [DefaultValue(10)]
        [JsonProperty(PropertyName = "MaximumDbConnections")]
        public int MaximumDbConnections { get; set; }

        [DefaultValue(0)]
        [JsonProperty(PropertyName = "MaximumDbIdleTime")]
        public int MaximumDbIdleTime { get; set; }

        ///<summary>Usable values: "true" - "false", use this option to choose whether accounts will be created automatically or not.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "AutoCreateAccounts")]
        public bool AutoCreateAccounts { get; set; }
    }

    ///<summary>Security.</summary>
    public class Security
    {
        ///<summary>Log all events from loginserver (account creation, failed/success login, etc).</summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "LogLoginController")]
        public bool LogLoginController { get; set; }

        ///<summary>FloodProtection. time in ms.</summary>
        [DefaultValue(true)]
        [JsonProperty(PropertyName = "EnableFloodProtection")]
        public bool EnableFloodProtection { get; set; }

        [DefaultValue(15)]
        [JsonProperty(PropertyName = "FastConnectionLimit")]
        public int FastConnectionLimit { get; set; }

        [DefaultValue(700)]
        [JsonProperty(PropertyName = "NormalConnectionTime")]
        public int NormalConnectionTime { get; set; }

        [DefaultValue(15)]
        [JsonProperty(PropertyName = "FastConnectionTime")]
        public int FastConnectionTime { get; set; }

        [DefaultValue(50)]
        [JsonProperty(PropertyName = "MaxConnectionPerIP")]
        public int MaxConnectionPerIP { get; set; }
    }

    ///<summary>Test server, shoudnt be touched in live server.</summary>
    public class LoginDeveloperConfig
    {
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "Debug")]
        public bool Debug { get; set; }

        [DefaultValue(false)]
        [JsonProperty(PropertyName = "Developer")]
        public bool Developer { get; set; }

        [DefaultValue(false)]
        [JsonProperty(PropertyName = "PacketHandlerDebug")]
        public bool PacketHandlerDebug { get; set; }
    }
}