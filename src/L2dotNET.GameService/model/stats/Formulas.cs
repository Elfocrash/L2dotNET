using System;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Stats
{
    class Formulas
    {
        private static readonly Random rnd = new Random();

        public static bool checkMissed(L2Character attacker, L2Character target)
        {
            int delta = (int)(attacker.CharacterStat.getStat(TEffectType.b_accuracy) - target.CharacterStat.getStat(TEffectType.b_evasion));

            double chance;
            if (delta >= 10)
                chance = 980;
            else
            {
                switch (delta)
                {
                    case 9:
                        chance = 975;
                        break;
                    case 8:
                        chance = 970;
                        break;
                    case 7:
                        chance = 965;
                        break;
                    case 6:
                        chance = 960;
                        break;
                    case 5:
                        chance = 955;
                        break;
                    case 4:
                        chance = 945;
                        break;
                    case 3:
                        chance = 935;
                        break;
                    case 2:
                        chance = 925;
                        break;
                    case 1:
                        chance = 915;
                        break;
                    case 0:
                        chance = 905;
                        break;
                    case -1:
                        chance = 890;
                        break;
                    case -2:
                        chance = 875;
                        break;
                    case -3:
                        chance = 860;
                        break;
                    case -4:
                        chance = 845;
                        break;
                    case -5:
                        chance = 830;
                        break;
                    case -6:
                        chance = 815;
                        break;
                    case -7:
                        chance = 800;
                        break;
                    case -8:
                        chance = 785;
                        break;
                    case -9:
                        chance = 770;
                        break;
                    case -10:
                        chance = 755;
                        break;
                    case -11:
                        chance = 735;
                        break;
                    case -12:
                        chance = 715;
                        break;
                    case -13:
                        chance = 695;
                        break;
                    case -14:
                        chance = 675;
                        break;
                    case -15:
                        chance = 655;
                        break;
                    case -16:
                        chance = 625;
                        break;
                    case -17:
                        chance = 595;
                        break;
                    case -18:
                        chance = 565;
                        break;
                    case -19:
                        chance = 535;
                        break;
                    case -20:
                        chance = 505;
                        break;
                    case -21:
                        chance = 455;
                        break;
                    case -22:
                        chance = 405;
                        break;
                    case -23:
                        chance = 355;
                        break;
                    case -24:
                        chance = 305;
                        break;
                    default:
                        chance = 275;
                        break;
                }

                if (!attacker.isInFrontOfTarget())
                {
                    if (attacker.isBehindTarget())
                        chance *= 1.2;
                    else
                        chance *= 1.1;

                    if (chance > 980)
                        chance = 980;
                }
            }

            return chance < rnd.Next(1000);
        }

        public static double checkShieldDef(L2Character attacker, L2Character target)
        {
            double rate = target.CharacterStat.getStat(TEffectType.b_shield_rate);

            if (rnd.Next(100) > rate)
                return 0;

            return target.CharacterStat.getStat(TEffectType.p_physical_shield_defence);
        }

        public static bool checkCrit(L2Character attacker, L2Character target)
        {
            double rate = attacker.CharacterStat.getStat(TEffectType.b_critical_rate);

            if (rnd.Next(1000) <= rate)
                return true;

            return false;
        }

        public static double getPhysHitDamage(L2Character attacker, L2Character target, double sdef)
        {
            double atk = attacker.CharacterStat.getStat(TEffectType.p_physical_attack);
            double def = target.CharacterStat.getStat(TEffectType.p_physical_defense);

            double basedamage = 70 * atk / def;

            basedamage -= sdef;
            if (basedamage < 0)
                basedamage = 0;

            int rnddmg = (int)(basedamage * 0.2);
            basedamage += rnd.Next(-rnddmg, rnddmg);

            return basedamage;
        }

        public static double getPhysSkillHitDamage(L2Character attacker, L2Character target, int power)
        {
            double atk = attacker.CharacterStat.getStat(TEffectType.p_physical_attack);
            double def = target.CharacterStat.getStat(TEffectType.p_physical_defense);

            atk += power;
            double basedamage = 70 * atk / def;

            if (basedamage < 0)
                basedamage = 0;

            return basedamage;
        }
    }
}