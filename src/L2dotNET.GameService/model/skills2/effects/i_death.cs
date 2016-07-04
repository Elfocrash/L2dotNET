using System;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class i_death : TEffect
    {
        private int death_type;
        private int rate;

        public override void build(string str)
        {
            string[] v = str.Split(' ');
            death_type = int.Parse(v[1]);
            rate = int.Parse(v[2]);
        }

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            if (new Random().Next(100) < rate)
                target.SendMessage("i_death " + death_type + " done on you");

            return nothing;
        }
    }
}