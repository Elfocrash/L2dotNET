using L2dotNET.GameService.Model.Items;

namespace L2dotNET.GameService.Managers
{
    public class EnchantScroll
    {
        public EnchantType Type;
        public EnchantTarget Target;
        public ItemTemplate.L2ItemGrade Crystall;
        public byte bonus = 0;

        public EnchantScroll(EnchantType enchantType, EnchantTarget enchantTarget, ItemTemplate.L2ItemGrade enchantCrystall)
        {
            this.Type = enchantType;
            this.Target = enchantTarget;
            this.Crystall = enchantCrystall;
        }

        public EnchantScroll(EnchantType enchantType, EnchantTarget enchantTarget, ItemTemplate.L2ItemGrade enchantCrystall, byte bonus)
        {
            this.Type = enchantType;
            this.Target = enchantTarget;
            this.Crystall = enchantCrystall;
            this.bonus = bonus;
        }
    }
}