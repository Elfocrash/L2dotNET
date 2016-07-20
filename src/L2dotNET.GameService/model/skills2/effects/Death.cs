using System;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class Death : Effect
    {
        private int _deathType;
        private int _rate;

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            _deathType = int.Parse(v[1]);
            _rate = int.Parse(v[2]);
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            if (new Random().Next(100) < _rate)
                target.SendMessage($"i_death {_deathType} done on you");

            return Nothing;
        }
    }
}