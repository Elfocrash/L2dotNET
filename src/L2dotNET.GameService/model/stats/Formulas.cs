using System;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Stats
{
    class Formulas
    {
        private static readonly Random Rnd = new Random();

        public static bool CheckMissed(L2Character attacker, L2Character target)
        {
            int delta = (int)(attacker.CharacterStat.GetStat(EffectType.BAccuracy) - target.CharacterStat.GetStat(EffectType.BEvasion));

            double chance;
            if (delta >= 10)
            {
                chance = 980;
            }
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

                if (!attacker.IsInFrontOfTarget())
                {
                    if (attacker.IsBehindTarget())
                    {
                        chance *= 1.2;
                    }
                    else
                    {
                        chance *= 1.1;
                    }

                    if (chance > 980)
                    {
                        chance = 980;
                    }
                }
            }

            return chance < Rnd.Next(1000);
        }

        public static double CheckShieldDef(L2Character attacker, L2Character target)
        {
            double rate = target.CharacterStat.GetStat(EffectType.BShieldRate);

            return Rnd.Next(100) > rate ? 0 : target.CharacterStat.GetStat(EffectType.PPhysicalShieldDefence);
        }

        public static bool CheckCrit(L2Character attacker, L2Character target)
        {
            double rate = attacker.CharacterStat.GetStat(EffectType.BCriticalRate);

            return Rnd.Next(1000) <= rate;
        }

        public static double GetPhysHitDamage(L2Character attacker, L2Character target, double sdef)
        {
            double atk = attacker.CharacterStat.GetStat(EffectType.PPhysicalAttack);
            double def = target.CharacterStat.GetStat(EffectType.PPhysicalDefense);

            double basedamage = (70 * atk) / def;

            basedamage -= sdef;
            if (basedamage < 0)
            {
                basedamage = 0;
            }

            int rnddmg = (int)(basedamage * 0.2);
            basedamage += Rnd.Next(-rnddmg, rnddmg);

            return basedamage;
        }

        public static double GetPhysSkillHitDamage(L2Character attacker, L2Character target, int power)
        {
            double atk = attacker.CharacterStat.GetStat(EffectType.PPhysicalAttack);
            double def = target.CharacterStat.GetStat(EffectType.PPhysicalDefense);

            atk += power;
            double basedamage = (70 * atk) / def;

            if (basedamage < 0)
            {
                basedamage = 0;
            }

            return basedamage;
        }
    }
}