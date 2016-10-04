using L2dotNET.model.items;
using L2dotNET.model.player;

namespace L2dotNET.Plugins
{
    public abstract class Plugin : IPlugin
    {
        protected PluginContext Context { get; set; }

        public void OnEnable(PluginContext context)
        {
            Context = context;
            OnEnable();
        }

        protected virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {
        }

        public virtual void OnLogin(L2Player player)
        {
        }

        public void OnItemEquip(L2Player player, L2Item item)
        {
        }
    }
}