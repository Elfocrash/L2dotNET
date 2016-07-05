using L2dotNET.GameService.Enums;
using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Managers
{
    public class EnchantScroll
    {
        public EnchantType Type;
        public EnchantTarget Target;
        public CrystalTypeId Crystall;
        public byte Bonus;

        public EnchantScroll(EnchantType enchantType, EnchantTarget enchantTarget, CrystalTypeId enchantCrystall)
        {
            Type = enchantType;
            Target = enchantTarget;
            Crystall = enchantCrystall;
        }

        public EnchantScroll(EnchantType enchantType, EnchantTarget enchantTarget, CrystalTypeId enchantCrystall, byte bonus)
        {
            Type = enchantType;
            Target = enchantTarget;
            Crystall = enchantCrystall;
            Bonus = bonus;
        }
    }
}