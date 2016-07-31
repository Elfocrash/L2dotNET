using System;
using System.Collections.Generic;

namespace L2dotNET.Enums
{
    public class WeaponType : ItemType
    {
        public WeaponTypeId Id { get; set; }
        private readonly string _name;

        public static readonly WeaponType None = new WeaponType(WeaponTypeId.None, "None");
        public static readonly WeaponType Sword = new WeaponType(WeaponTypeId.Sword, "Sword");
        public static readonly WeaponType Blunt = new WeaponType(WeaponTypeId.Blunt, "Blunt");
        public static readonly WeaponType Dagger = new WeaponType(WeaponTypeId.Dagger, "Dagger");
        public static readonly WeaponType Bow = new WeaponType(WeaponTypeId.Bow, "Bow");
        public static readonly WeaponType Pole = new WeaponType(WeaponTypeId.Pole, "Pole");
        public static readonly WeaponType Etc = new WeaponType(WeaponTypeId.Etc, "Etc");
        public static readonly WeaponType Fist = new WeaponType(WeaponTypeId.Fist, "Fist");
        public static readonly WeaponType Dual = new WeaponType(WeaponTypeId.Dual, "DualSword");
        public static readonly WeaponType DualFist = new WeaponType(WeaponTypeId.Dualfist, "DualFist");
        public static readonly WeaponType BigSword = new WeaponType(WeaponTypeId.Bigsword, "BigSword");
        public static readonly WeaponType Pet = new WeaponType(WeaponTypeId.Pet, "Pet");
        public static readonly WeaponType Rod = new WeaponType(WeaponTypeId.Fishingrod, "Rod");
        public static readonly WeaponType BigBlunt = new WeaponType(WeaponTypeId.Bigblunt, "BigBlunt");

        private WeaponType(WeaponTypeId id, string name)
        {
            Id = id;
            _name = name;
        }

        public static IEnumerable<WeaponType> Values
        {
            get
            {
                yield return None;
                yield return Sword;
                yield return Blunt;
                yield return Dagger;
                yield return Bow;
                yield return Pole;
                yield return Etc;
                yield return Fist;
                yield return Dual;
                yield return DualFist;
                yield return BigSword;
                yield return Pet;
                yield return Rod;
                yield return BigBlunt;
            }
        }

        public override string ToString()
        {
            return _name;
        }

        public int GetMask()
        {
            return 1 << (int)Id;
        }

    }
}