namespace L2dotNET.GameService.Enums
{
    public class WeaponType
    {
        private readonly int _mask;
        private readonly int _range;

        private WeaponType(WeaponTypeId id, int range)
        {
            _mask = 1 << ((int)id + 1);
            _range = range;
        }

        public int GetMask()
        {
            return _mask;
        }
    }

    public enum WeaponTypeId
    {
        None = 40,
        Sword = 40,
        Blunt = 40,
        Dagger = 40,
        Bow = 500,
        Pole = 66,
        Etc = 40,
        Fist = 40,
        Dual = 40,
        Dualfist = 40,
        Bigsword = 40,
        Fishingrod = 40,
        Bigblunt = 40,
        Pet = 40
    }
}