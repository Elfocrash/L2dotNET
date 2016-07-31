namespace L2dotNET.GameService.Plugins
{
    public class PluginContext
    {
        public GameServer Server { get; private set; }
        public PluginManager PluginManager { get; private set; }

        public PluginContext(GameServer server, PluginManager pluginManager)
        {
            Server = server;
            PluginManager = pluginManager;
        }
    }
}