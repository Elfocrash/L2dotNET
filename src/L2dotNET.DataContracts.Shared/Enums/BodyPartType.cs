using System;

namespace L2dotNET.DataContracts.Shared.Enums
{
    [Flags]
    public enum BodyPartType : int
    {
        Type1WeaponRingEarringNecklace = 0,
        Type1ShieldArmor = 1,
        Type1ItemQuestitemAdena = 4,

        Type2Weapon = 0,
        Type2ShieldArmor = 1,
        Type2Accessory = 2,
        Type2Quest = 3,
        Type2Money = 4,
        Type2Other = 5,

        SlotNone = 0x0000,
        SlotUnderwear = 0x0001,
        SlotREar = 0x0002,
        SlotLEar = 0x0004,
        SlotLrEar = 0x00006,
        SlotNeck = 0x0008,
        SlotRFinger = 0x0010,
        SlotLFinger = 0x0020,
        SlotLrFinger = 0x0030,
        SlotHead = 0x0040,
        SlotRHand = 0x0080,
        SlotLHand = 0x0100,
        SlotGloves = 0x0200,
        SlotChest = 0x0400,
        SlotLegs = 0x0800,
        SlotFeet = 0x1000,
        SlotBack = 0x2000,
        SlotLrHand = 0x4000,
        SlotFullArmor = 0x8000,
        SlotFace = 0x010000,
        SlotAlldress = 0x020000,
        SlotHair = 0x040000,
        SlotHairall = 0x080000,

        SlotWolf = -100,
        SlotHatchling = -101,
        SlotStrider = -102,
        SlotBabypet = -103,

        SlotAllweapon = SlotLrHand | SlotRHand
    }
}
