using System.Collections.Generic;
using System.Linq;
using log4net;

namespace L2dotNET.GameService.Model.items.cursed
{
    class CursedWeapons
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CursedWeapons));
        private static readonly CursedWeapons instance = new CursedWeapons();

        public static CursedWeapons getInstance()
        {
            return instance;
        }

        private readonly SortedList<int, CursedWeapon> items = new SortedList<int, CursedWeapon>();

        public CursedWeapons()
        {
            items.Add(8190, null);
            items.Add(8689, null);

            log.Info($"CursedWeapons: Loaded {items.Count} items.");
        }

        public int[] getWeaponIds()
        {
            return items.Keys.ToArray();
        }
    }
}