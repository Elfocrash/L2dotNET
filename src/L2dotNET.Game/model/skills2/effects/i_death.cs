using System;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.Model.skills2.effects
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

        public override TEffectResult onStart(L2Character caster, world.L2Character target)
        {
            if (new Random().Next(100) < rate)
                target.sendMessage("i_death " + death_type + " done on you");

            return nothing;
        }
    }
}