using System;
using L2dotNET.model.player;
using L2dotNET.Plugins;

namespace TestPlugin
{
    [Plugin(PluginName = "TestPlugin", Description = "A Test plugin", PluginVersion = "1.0", Author = "Elfocrash")]
    public class TestPlugin : Plugin
    {
        protected override void OnEnable()
        {
            Console.Out.WriteLine("Worked???");
        }

        public override void OnLogin(L2Player player)
        {
            player.SendMessage("Worked?");
        }
    }
}
