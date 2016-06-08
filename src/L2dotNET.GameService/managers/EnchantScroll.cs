using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Managers
{
    public class EnchantScroll
    {
        public EnchantType Type;
        public EnchantTarget Target;
        public ItemTemplate.L2ItemGrade Crystall;
        public byte bonus;

        public EnchantScroll(EnchantType enchantType, EnchantTarget enchantTarget, ItemTemplate.L2ItemGrade enchantCrystall)
        {
            Type = enchantType;
            Target = enchantTarget;
            Crystall = enchantCrystall;
        }

        public EnchantScroll(EnchantType enchantType, EnchantTarget enchantTarget, ItemTemplate.L2ItemGrade enchantCrystall, byte bonus)
        {
            Type = enchantType;
            Target = enchantTarget;
            Crystall = enchantCrystall;
            this.bonus = bonus;
        }
    }
}