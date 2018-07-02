using System.Linq;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Enums;
using L2dotNET.Templates;
using L2dotNET.Utility;

namespace L2dotNET.Models.Items
{
    public class Armor : ItemTemplate
    {
        public ArmorTypeId ArmorType { get; set; }
        public bool AvoidModify { get; set; }
        public int Pdef { get; set; }
        public int Mdef { get; set; }
        public int MpBonus { get; set; }
        public int ItemSkillId { get; set; }
        public byte ItemSkillLvl { get; set; }

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
                Type1 = (int) ItemType1.WeaponRingEarringNecklace;
                Type2 = (int) ItemType2.Accessory;
            }
            else
            {
                if (ArmorType == Enums.ArmorType.None.Id && BodyPart == BodyPartType.SlotLHand) // retail define shield as NONE
                {
                    ArmorType = Enums.ArmorType.Shield.Id;
                }

                Type1 = (int) ItemType1.ShieldArmor;
                Type2 = (int) ItemType2.ShieldArmor;
            }
        }

        public override int GetItemMask()
        {
            ArmorType orDefault = Enums.ArmorType.Values.FirstOrDefault(x => x.Id == ArmorType);

            return orDefault?.GetMask() ?? 0;
        }
    }
}