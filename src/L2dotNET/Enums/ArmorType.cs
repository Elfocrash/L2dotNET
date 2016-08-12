using System.Collections.Generic;

namespace L2dotNET.Enums
{
    public class ArmorType : ItemType
    {
        public ArmorTypeId Id { get; set; }
        private readonly string _name;
        public static readonly ArmorType None = new ArmorType(ArmorTypeId.None, "NONE");
        public static readonly ArmorType Light = new ArmorType(ArmorTypeId.Light, "LIGHT");
        public static readonly ArmorType Heavy = new ArmorType(ArmorTypeId.Heavy, "HEAVY");
        public static readonly ArmorType Magic = new ArmorType(ArmorTypeId.Magic, "MAGIC");
        public static readonly ArmorType Pet = new ArmorType(ArmorTypeId.Pet, "PET");
        public static readonly ArmorType Shield = new ArmorType(ArmorTypeId.Shield, "SHIELD");

        private ArmorType(ArmorTypeId id, string name)
        {
            _name = name;
            Id = id;
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
            return 1 << ((int)Id + 16);
        }

        public override string ToString()
        {
            return _name;
        }
    }
}