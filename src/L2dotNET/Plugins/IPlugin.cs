using L2dotNET.model.player;

namespace L2dotNET.Plugins
{
    public interface IPlugin
    {
        /// <summary>
        ///     This function will be called on plugin initialization.
        /// </summary>
        void OnEnable(PluginContext context);

        /// <summary>
        ///     This function will be called when the plugin will be disabled.s
        /// </summary>
        void OnDisable();

        void OnLogin(L2Player player);
    }
}