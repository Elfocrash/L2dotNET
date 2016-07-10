using System;
using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Player.Basic
{
    class Formulas
    {
        public static double getPhysDamage(L2Character attacker, L2Character target, double skillPower, bool crt, bool ss)
        {
            double damage = 1; // attacker._stats.p_physical_attack;
            double defence = 1; // target._stats.p_physical_defence;

            if (ss)
            {
                damage *= 2;
            }

            if (skillPower != 0)
            {
                damage += skillPower;
            }

            damage = (70 * damage) / defence;

            if (!(attacker is L2Player))
            {
                return damage;
            }

            L2Item weapon = null; //((L2Player)attacker).Inventory.getWeapon();
            if (weapon == null)
            {
                return damage;
            }

            double mix = double.Parse("1.");
            mix = (damage * mix) - damage;
            damage += new Random().Next((int)-mix, (int)mix);

            return damage;
        }
    }
}