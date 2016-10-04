using L2dotNET.model.items;
using L2dotNET.model.player;

namespace L2dotNET.Plugins
{
    public interface IPlugin
    {
        /// <summary>
        ///     This method will be called on plugin initialization.
        /// </summary>
        void OnEnable(PluginContext context);

        /// <summary>
        ///     This method will be called when the plugin will be disabled.
        /// </summary>
        void OnDisable();

        /// <summary>
        ///     This method will be called when the player logs in.
        /// </summary>
        /// <param name="player"></param>
        void OnLogin(L2Player player);

        /// <summary>
        ///     This method will be called when a player equips an item.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="item"></param>
        void OnItemEquip(L2Player player, L2Item item);
    }
}