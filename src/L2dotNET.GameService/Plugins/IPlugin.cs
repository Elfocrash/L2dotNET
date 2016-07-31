using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Plugins
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