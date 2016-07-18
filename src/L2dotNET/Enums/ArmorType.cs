using System;
using System.Collections.Generic;

namespace L2dotNET.Enums
{
    public class ArmorType : ItemType
    {
        private readonly int _mask;
        public static readonly ArmorType None = new ArmorType(ArmorTypeId.None, "NONE");
        public static readonly ArmorType Light = new ArmorType(ArmorTypeId.Light, "LIGHT");
        public static readonly ArmorType Heavy = new ArmorType(ArmorTypeId.Heavy, "HEAVY");
        public static readonly ArmorType Magic = new ArmorType(ArmorTypeId.Magic, "MAGIC");
        public static readonly ArmorType Pet = new ArmorType(ArmorTypeId.Pet, "PET");
        public static readonly ArmorType Shield = new ArmorType(ArmorTypeId.Shield, "SHIELD");

        public ArmorTypeId Id { get; set; }
        public string Name { get; set; }

        private ArmorType(ArmorTypeId id, string name)
        {
            Name = name;
            _mask = 1 << ((int)id + Enum.GetNames(typeof(WeaponTypeId)).Length);
        }

        public static IEnumerable<ArmorType> Values
        {
            get
            {
                yield return None;
                yield return Light;
                yield return Heavy;
                yield return Magic;
                yield return Pet;
                yield return Shield;
            }
        }

        public int GetMask()
        {
            return _mask;
        }

        public static explicit operator int(ArmorType v)
        {
            throw new NotImplementedException();
        }
    }
}