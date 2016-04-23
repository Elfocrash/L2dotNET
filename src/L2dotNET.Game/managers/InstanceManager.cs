using System.Collections.Generic;
using L2dotNET.Game.world;
using L2dotNET.Game.world.instances;
using L2dotNET.Game.world.instances.data;

namespace L2dotNET.Game.managers
{
    class InstanceManager
    {
        private static InstanceManager m = new InstanceManager();

        public static InstanceManager getInstance()
        {
            return m;
        }

        private readonly SortedList<int, InstanceTemplate> _instanceTypes = new SortedList<int, InstanceTemplate>();

        public InstanceManager()
        {
            _instanceTypes.Add(1, new _test_instance());
        }

        public void create(int typeId, L2Player player)
        {
            if (!_instanceTypes.ContainsKey(typeId))
            {
                player.sendMessage("Instance type #"+typeId+" is not registered");
                return;
            }

            L2Instance i = new L2Instance(_instanceTypes[typeId]);
            if (i.createFailed(player))
                return;

            i.create(player);
        }
    }
}
