using System.Collections.Generic;
using System.Linq;
using log4net;

namespace L2dotNET.Models.items.cursed
{
    class CursedWeapons
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CursedWeapons));
        private static readonly CursedWeapons Instance = new CursedWeapons();

        public static CursedWeapons GetInstance()
        {
            return Instance;
        }

        private readonly SortedList<int, CursedWeapon> _items = new SortedList<int, CursedWeapon>();

        public CursedWeapons()
        {
            _items.Add(8190, null);
            _items.Add(8689, null);

            Log.Info($"CursedWeapons: Loaded {_items.Count} items.");
        }

        public int[] GetWeaponIds()
        {
            return _items.Keys.ToArray();
        }
    }
}