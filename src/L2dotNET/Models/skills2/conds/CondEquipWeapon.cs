using System.Collections.Generic;
using L2dotNET.model.player;

namespace L2dotNET.model.skills2.conds
{
    class CondEquipWeapon : SkillCond
    {
        private readonly List<string> _allowed = new List<string>();

        public void Add(string mask)
        {
            if (!_allowed.Contains(mask))
                _allowed.Add(mask);
        }

        public override bool CanUse(L2Player player, Skill skill)
        {
            // L2Item item = null; // player.Inventory.getWeapon();

            //if (item != null)
            //    return _allowed.Any(mask => mask.Equals(item.Template.WeaponType.ToString()));

            return false;
        }
    }
}