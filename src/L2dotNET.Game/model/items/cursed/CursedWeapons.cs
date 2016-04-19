using System.Collections.Generic;
using System.Linq;
using L2dotNET.Game.logger;

namespace L2dotNET.Game.model.items.cursed
{
    class CursedWeapons
    {
        private static CursedWeapons instance = new CursedWeapons();
        public static CursedWeapons getInstance()
        {
            return instance;
        }

        private SortedList<int, CursedWeapon> items = new SortedList<int, CursedWeapon>();

        public CursedWeapons()
        {
            items.Add(8190, null);
            items.Add(8689, null);

            CLogger.info("CursedWeapons: Loaded "+items.Count+" items.");
        }

        public int[] getWeaponIds()
        {
            return items.Keys.ToArray();
        }

    }
}
