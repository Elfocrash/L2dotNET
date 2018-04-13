using System;
using System.Linq;
using L2dotNET.Models.Player;
using L2dotNET.World;

namespace L2dotNET.Models.Stats
{
    public class CharacterStat
    {
        public L2Character Character { get; }

        public long Experience { get; set; }
        public int Sp { get; set; }
        public byte Level { get; set; } = 1;

        public CharacterStat(L2Character character)
        {
            Character = character;
        }

        public int Str => (int) CalculateStat(Stats.StatStr, Character.Template.BaseStr, null);
        public int Dex => (int)CalculateStat(Stats.StatDex, Character.Template.BaseDex, null);
        public int Con => (int)CalculateStat(Stats.StatCon, Character.Template.BaseCon, null);
        public int Int => (int)CalculateStat(Stats.StatInt, Character.Template.BaseInt, null);
        public int Men => (int)CalculateStat(Stats.StatMen, Character.Template.BaseMen, null);
        public int Wit => (int)CalculateStat(Stats.StatWit, Character.Template.BaseWit, null);
        public int EvasionRate(L2Character target) => (int)CalculateStat(Stats.EvasionRate, 0, target);
        public int Accuracy => (int)CalculateStat(Stats.AccuracyCombat, 0, null);
        public int MaxHp => (int)CalculateStat(Stats.MaxHp, Character.Template.BaseHpMax(Character.Level), null);
        public int MaxMp => (int)CalculateStat(Stats.MaxMp, Character.Template.BaseMpMax(Character.Level), null);

        //I will create a new PlayerStat later. I know this is shit sorry
        public int MaxCp => Character is L2Player player ? (int)CalculateStat(Stats.MaxCp, player.Template.BaseCpMax(player.Level), null) : 0;

        public int CriticalHit(L2Character target, object skill = null) =>
            Math.Min((int) CalculateStat(Stats.CriticalRate, Character.Template.BaseCritRate, target), 500);

        public int MCriticalHit(L2Character target, object skill = null) =>
            (int) CalculateStat(Stats.McriticalRate, 8, target);

        public int MAttack(L2Character target, object skill = null)
        {
            var attack = Character.Template.BaseMAtk; //TODO add champion stuff here

            return (int) CalculateStat(Stats.MagicAttack, attack, target);
        }

        public int MAttackSpeed => (int) CalculateStat(Stats.MagicAttackSpeed, 333.0, null);

        public int MDefence(L2Character target, object skill = null) =>
            (int) CalculateStat(Stats.MagicDefence, Character.Template.BaseMDef, target);

        public int PAttack(L2Character target) =>
            (int) CalculateStat(Stats.PowerAttack, Character.Template.BasePAtk, target);

        public int PAttackSpeed => (int)CalculateStat(Stats.PowerAttackSpeed, Character.Template.BasePAtkSpd, null);

        public int PDefence(L2Character target) =>
            (int)CalculateStat(Stats.PowerDefence, Character.Template.BasePDef, target);

        public int PAttackAnimals(L2Character target) =>
            (int)CalculateStat(Stats.PatkAnimals, 1, target);

        public int PAttackDragons(L2Character target) =>
            (int)CalculateStat(Stats.PatkDragons, 1, target);

        public int PAttackInsects(L2Character target) =>
            (int)CalculateStat(Stats.PatkInsects, 1, target);

        public int PAttackMonsters(L2Character target) =>
            (int)CalculateStat(Stats.PatkMonsters, 1, target);

        public int PAttackPlants(L2Character target) =>
            (int)CalculateStat(Stats.PatkPlants, 1, target);

        public int PAttackGiants(L2Character target) =>
            (int)CalculateStat(Stats.PatkGiants, 1, target);

        public int PAttackMagicCreatures(L2Character target) =>
            (int)CalculateStat(Stats.PatkMcreatures, 1, target);

        public int PDefenceAnimals(L2Character target) =>
            (int)CalculateStat(Stats.PdefAnimals, 1, target);

        public int PDefenceDragons(L2Character target) =>
            (int)CalculateStat(Stats.PdefDragons, 1, target);

        public int PDefenceInsects(L2Character target) =>
            (int)CalculateStat(Stats.PdefInsects, 1, target);

        public int PDefenceMonsters(L2Character target) =>
            (int)CalculateStat(Stats.PdefMonsters, 1, target);

        public int PDefencePlants(L2Character target) =>
            (int)CalculateStat(Stats.PdefPlants, 1, target);

        public int PDefenceGiants(L2Character target) =>
            (int)CalculateStat(Stats.PdefGiants, 1, target);

        public int PDefenceMagicCreatures(L2Character target) =>
            (int)CalculateStat(Stats.PdefMcreatures, 1, target);

        public int PhysicalAttackRange => 40;

        public int ShieldDefence => (int) CalculateStat(Stats.ShieldDefence, 0, null);

        public int MpConsume(object skill) => 1;

        public int MpInitialConsume(object skill) => 1;

        public int AttackElementValue(byte attackAttribute)
        {
            switch (attackAttribute)
            {
                case 1: // wind
                    return (int) CalculateStat(Stats.WindPower, 0, null);
                case 2: // fire
                    return (int)CalculateStat(Stats.FirePower, 0, null);
                case 3: // water
                    return (int)CalculateStat(Stats.WaterPower, 0, null);
                case 4: // earth
                    return (int)CalculateStat(Stats.EarthPower, 0, null);
                case 5: // holy
                    return (int)CalculateStat(Stats.HolyPower, 0, null);
                case 6: // dark
                    return (int)CalculateStat(Stats.DarkPower, 0, null);
                default:
                    return 0;
            }
        }

        public int DefenceElementValue(byte defenceAttribute)
        {
            switch (defenceAttribute)
            {
                case 1: // wind
                    return (int)CalculateStat(Stats.WindRes, 0, null);
                case 2: // fire
                    return (int)CalculateStat(Stats.FireRes, 0, null);
                case 3: // water
                    return (int)CalculateStat(Stats.WaterRes, 0, null);
                case 4: // earth
                    return (int)CalculateStat(Stats.EarthRes, 0, null);
                case 5: // holy
                    return (int)CalculateStat(Stats.HolyRes, 0, null);
                case 6: // dark
                    return (int)CalculateStat(Stats.DarkRes, 0, null);
                default:
                    return 0;
            }
        }

        public int BaseRunSpeed => Character.Template.BaseRunSpd;

        public int BaseWalkSpeed => Character.Template.BaseWalkSpd;

        public virtual int BaseMoveSpeed => Character.IsRunning == 1 ? BaseRunSpeed : BaseWalkSpeed;

        public float MoveSpeed => (float) CalculateStat(Stats.RunSpeed, BaseMoveSpeed, null);

        public float MovementSpeedMultiplayer => MoveSpeed / BaseMoveSpeed;

        public float AttackSpeedMultiplier => (float) ((1.1) * PAttackSpeed / Character.Template.BasePAtkSpd);

        public double CalculateStat(Stat stat, double initial, L2Character target)
        {
            if (Character == null || stat == null)
                return initial;

            var statsArray = Stats.Values.ToList().Select(x => x.StatName).ToArray();
            var statId = Array.FindIndex(statsArray, x =>x.Equals(stat.StatName));

            var calculator = Character.Calculators[statId];
            if (calculator == null || calculator.Size == 0)
                return initial;

            var env = new Env
            {
                Character = Character,
                Target = target,
                Value = initial
            };

            calculator.Calculate(env);

            if (env.Value <= 0)
            {
                if(stat == Stats.MaxHp || stat == Stats.MaxMp || stat == Stats.MaxCp || stat == Stats.MagicDefence || stat == Stats.PowerDefence || stat == Stats.PowerAttack
                   || stat == Stats.MagicAttack || stat == Stats.PowerAttackSpeed || stat == Stats.MagicAttackSpeed || stat == Stats.ShieldDefence || stat == Stats.StatCon
                   || stat == Stats.StatDex || stat == Stats.StatInt || stat == Stats.StatMen || stat == Stats.StatStr || stat == Stats.StatWit)
                    env.Value = 1;
            }

            return env.Value;
        }
    }
}