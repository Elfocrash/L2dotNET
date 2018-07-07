using System;
using System.Linq;
using System.Threading;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Models.Player;

namespace L2dotNET.Models.Stats
{
    public class CharacterStat
    {
        private readonly L2Character _character;
        private readonly Calculator[] _calculators;

        public CharacterStat(L2Character character)
        {
            _calculators = character is L2Player ? Calculator.GetCalculatorsForStats() : null;
            _character = character;
        }

        public int Str => (int) CalculateStat(CharacterStatId.StatStr, _character.Template.BaseStr, null);
        public int Dex => (int)CalculateStat(CharacterStatId.StatDex, _character.Template.BaseDex, null);
        public int Con => (int)CalculateStat(CharacterStatId.StatCon, _character.Template.BaseCon, null);
        public int Int => (int)CalculateStat(CharacterStatId.StatInt, _character.Template.BaseInt, null);
        public int Men => (int)CalculateStat(CharacterStatId.StatMen, _character.Template.BaseMen, null);
        public int Wit => (int)CalculateStat(CharacterStatId.StatWit, _character.Template.BaseWit, null);
        public int EvasionRate(L2Character target) => (int)CalculateStat(CharacterStatId.EvasionRate, 0, target);
        public int Accuracy => (int)CalculateStat(CharacterStatId.AccuracyCombat, 0, null);
        public int MaxHp => (int)CalculateStat(CharacterStatId.MaxHp, _character.Template.GetBaseMaxHp(_character.Level), null);
        public int MaxMp => (int)CalculateStat(CharacterStatId.MaxMp, _character.Template.GetBaseMaxMp(_character.Level), null);

        //I will create a new PlayerStat later. I know this is shit sorry
        public int MaxCp => _character is L2Player player ? (int)CalculateStat(CharacterStatId.MaxCp, player.Template.GetBaseMaxCp(player.Level), null) : 0;

        public int CriticalHit(L2Character target, object skill = null) =>
            Math.Min((int) CalculateStat(CharacterStatId.CriticalRate, _character.Template.BaseCritRate, target), 500);

        public int MCriticalHit(L2Character target, object skill = null) =>
            (int) CalculateStat(CharacterStatId.McriticalRate, 8, target);

        public int MAttack(L2Character target, object skill = null)
        {
            double attack = _character.Template.BaseMAtk; //TODO add champion stuff here

            return (int) CalculateStat(CharacterStatId.MagicAttack, attack, target);
        }

        public int MAttackSpeed => (int) CalculateStat(CharacterStatId.MagicAttackSpeed, 333.0, null);

        public int MDefence(L2Character target, object skill = null) =>
            (int) CalculateStat(CharacterStatId.MagicDefence, _character.Template.BaseMDef, target);

        public int PAttack(L2Character target) =>
            (int) CalculateStat(CharacterStatId.PowerAttack, _character.Template.BasePAtk, target);

        public int PAttackSpeed => (int)CalculateStat(CharacterStatId.PowerAttackSpeed, _character.Template.BasePAtkSpd, null);

        public int PDefence(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PowerDefence, _character.Template.BasePDef, target);

        public int PAttackAnimals(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PatkAnimals, 1, target);

        public int PAttackDragons(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PatkDragons, 1, target);

        public int PAttackInsects(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PatkInsects, 1, target);

        public int PAttackMonsters(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PatkMonsters, 1, target);

        public int PAttackPlants(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PatkPlants, 1, target);

        public int PAttackGiants(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PatkGiants, 1, target);

        public int PAttackMagicCreatures(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PatkMcreatures, 1, target);

        public int PDefenceAnimals(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PdefAnimals, 1, target);

        public int PDefenceDragons(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PdefDragons, 1, target);

        public int PDefenceInsects(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PdefInsects, 1, target);

        public int PDefenceMonsters(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PdefMonsters, 1, target);

        public int PDefencePlants(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PdefPlants, 1, target);

        public int PDefenceGiants(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PdefGiants, 1, target);

        public int PDefenceMagicCreatures(L2Character target) =>
            (int)CalculateStat(CharacterStatId.PdefMcreatures, 1, target);

        public int PhysicalAttackRange => 40;

        public int ShieldDefence => (int) CalculateStat(CharacterStatId.ShieldDefence, 0, null);

        public int MpConsume(object skill) => 1;

        public int MpInitialConsume(object skill) => 1;

        public int AttackElementValue(byte attackAttribute)
        {
            switch (attackAttribute)
            {
                case 1: // wind
                    return (int) CalculateStat(CharacterStatId.WindPower, 0, null);
                case 2: // fire
                    return (int)CalculateStat(CharacterStatId.FirePower, 0, null);
                case 3: // water
                    return (int)CalculateStat(CharacterStatId.WaterPower, 0, null);
                case 4: // earth
                    return (int)CalculateStat(CharacterStatId.EarthPower, 0, null);
                case 5: // holy
                    return (int)CalculateStat(CharacterStatId.HolyPower, 0, null);
                case 6: // dark
                    return (int)CalculateStat(CharacterStatId.DarkPower, 0, null);
                default:
                    return 0;
            }
        }

        public int DefenceElementValue(byte defenceAttribute)
        {
            switch (defenceAttribute)
            {
                case 1: // wind
                    return (int)CalculateStat(CharacterStatId.WindRes, 0, null);
                case 2: // fire
                    return (int)CalculateStat(CharacterStatId.FireRes, 0, null);
                case 3: // water
                    return (int)CalculateStat(CharacterStatId.WaterRes, 0, null);
                case 4: // earth
                    return (int)CalculateStat(CharacterStatId.EarthRes, 0, null);
                case 5: // holy
                    return (int)CalculateStat(CharacterStatId.HolyRes, 0, null);
                case 6: // dark
                    return (int)CalculateStat(CharacterStatId.DarkRes, 0, null);
                default:
                    return 0;
            }
        }

        public int BaseRunSpeed => _character.Template.BaseRunSpd;

        public int BaseWalkSpeed => _character.Template.BaseWalkSpd;

        public virtual int BaseMoveSpeed => _character.IsRunning == 1 ? BaseRunSpeed : BaseWalkSpeed;

        public float MoveSpeed => (float) CalculateStat(CharacterStatId.RunSpeed, BaseMoveSpeed, null);

        public int RunSpeed => (int) CalculateStat(CharacterStatId.RunSpeed, BaseRunSpeed, null);

        public int WalkSpeed => (int) CalculateStat(CharacterStatId.RunSpeed, BaseWalkSpeed, null);

        public float MovementSpeedMultiplayer => MoveSpeed / BaseMoveSpeed;

        public float AttackSpeedMultiplier => (float) ((1.1) * PAttackSpeed / _character.Template.BasePAtkSpd);

        public double CalculateStat(CharacterStatId stat, double initial, L2Character target)
        {
            if (_calculators == null)
            {
                return initial;
            }

            Calculator calculator = _calculators[(int) stat];
            if (calculator == null || calculator.Size == 0)
            {
                return initial;
            }

            StatFunctionEnvironment statFuncEnv = new StatFunctionEnvironment
            {
                Character = _character,
                Target = target,
                Value = initial
            };

            calculator.Calculate(statFuncEnv);

            if (statFuncEnv.Value < 1)
            {
                if(stat == CharacterStatId.MaxHp || stat == CharacterStatId.MaxMp || stat == CharacterStatId.MaxCp || stat == CharacterStatId.MagicDefence || stat == CharacterStatId.PowerDefence || stat == CharacterStatId.PowerAttack
                   || stat == CharacterStatId.MagicAttack || stat == CharacterStatId.PowerAttackSpeed || stat == CharacterStatId.MagicAttackSpeed || stat == CharacterStatId.ShieldDefence || stat == CharacterStatId.StatCon
                   || stat == CharacterStatId.StatDex || stat == CharacterStatId.StatInt || stat == CharacterStatId.StatMen || stat == CharacterStatId.StatStr || stat == CharacterStatId.StatWit)
                {
                    return 1;
                }
            }

            return statFuncEnv.Value;
        }

        public void AddStatFunction(StatFunction func)
        {
            if (func == null || _calculators == null)
            {
                return;
            }

            int statId = (int) func.Stat;

            Interlocked.CompareExchange(ref _calculators[statId], new Calculator(), null);

            _calculators[statId].AddFunc(func);
        }
    }
}