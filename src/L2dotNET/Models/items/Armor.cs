using System.Linq;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Enums;
using L2dotNET.Templates;
using L2dotNET.Utility;

namespace L2dotNET.Models.Items
{
    public class Armor : ItemTemplate
    {
        public ArmorTypeId Type { get; set; }
        private int AvoidModifier { get; set; }
        private int Pdef { get; set; }
        private int Mdef { get; set; }
        private int MpBonus { get; set; }
        private int HpBonus { get; set; }

        public Armor()
        {
            //TODO: check this
            if (BodyPart == BodyPartType.SlotNeck 
                || BodyPart == BodyPartType.SlotFace 
                || BodyPart == BodyPartType.SlotHair 
                || BodyPart == BodyPartType.SlotHairall
                || (BodyPart & BodyPartType.SlotREar) != 0 
                || (BodyPart & BodyPartType.SlotLFinger) != 0 
                || (BodyPart & BodyPartType.SlotBack) != 0)
            {
                Type1 = (int) BodyPartType.Type1WeaponRingEarringNecklace;
                Type2 = (int) BodyPartType.Type2Accessory;
            }
            else
            {
                if (Type == ArmorType.None.Id && BodyPart == BodyPartType.SlotLHand) // retail define shield as NONE
                    Type = ArmorType.Shield.Id;

                Type1 = (int) BodyPartType.Type1ShieldArmor;
                Type2 = (int) BodyPartType.Type2ShieldArmor;
            }
        }

        public override int GetItemMask()
        {
            ArmorType orDefault = ArmorType.Values.FirstOrDefault(x => x.Id == Type);
            if (orDefault == null)
                return 0;

            int firstOrDefault = orDefault.GetMask();
            return firstOrDefault;
        }
    }
}