namespace L2dotNET.Enums
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
}